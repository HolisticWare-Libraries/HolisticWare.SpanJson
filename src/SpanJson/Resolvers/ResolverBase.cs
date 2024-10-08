﻿using System.Buffers;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Dynamic;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text.Encodings.Web;
using CuteAnt.Reflection;
using SpanJson.Formatters;
using SpanJson.Helpers;
using SpanJson.Internal;

namespace SpanJson.Resolvers
{
    public abstract class ResolverBase
    {
        private static readonly IReadOnlyDictionary<Type, JsonConstructorAttribute> BaseClassJsonConstructorMap = BuildMap();

        protected static readonly ParameterExpression DynamicMetaObjectParameterExpression = Expression.Parameter(typeof(object));

        protected static bool TryGetBaseClassJsonConstructorAttribute(Type type, [MaybeNullWhen(false)] out JsonConstructorAttribute attribute)
        {
            if (BaseClassJsonConstructorMap.TryGetValue(type, out attribute))
            {
                return true;
            }

            if (type.IsGenericType && BaseClassJsonConstructorMap.TryGetValue(type.GetGenericTypeDefinition(), out attribute))
            {
                return true;
            }

            return false;
        }

        private static Dictionary<Type, JsonConstructorAttribute> BuildMap()
        {
            // TODO: what to do with the 8 args constructor with TRest?
            var result = new Dictionary<Type, JsonConstructorAttribute>
            {
                {typeof(KeyValuePair<,>), new JsonConstructorAttribute()},
                {typeof(Tuple<,>), new JsonConstructorAttribute()},
                {typeof(Tuple<,,>), new JsonConstructorAttribute()},
                {typeof(Tuple<,,,>), new JsonConstructorAttribute()},
                {typeof(Tuple<,,,,>), new JsonConstructorAttribute()},
                {typeof(Tuple<,,,,,>), new JsonConstructorAttribute()},
                {typeof(Tuple<,,,,,,>), new JsonConstructorAttribute()},
                {typeof(ValueTuple<,>), new JsonConstructorAttribute()},
                {typeof(ValueTuple<,,>), new JsonConstructorAttribute()},
                {typeof(ValueTuple<,,,>), new JsonConstructorAttribute()},
                {typeof(ValueTuple<,,,,>), new JsonConstructorAttribute()},
                {typeof(ValueTuple<,,,,,>), new JsonConstructorAttribute()},
                {typeof(ValueTuple<,,,,,,>), new JsonConstructorAttribute()}
            };

            return result;
        }

        public static IJsonFormatter GetDefaultOrCreate(Type type)
        {
            return (IJsonFormatter)(type.GetField("Default", BindingFlags.Public | BindingFlags.Static)
                                        ?.GetValue(null) ?? ActivatorUtils.FastCreateInstance(type)); // leave the createinstance here, this helps with recursive types
        }
    }

    public abstract class ResolverBase<TSymbol, TResolver> : ResolverBase, IJsonFormatterResolver<TSymbol, TResolver>
        where TResolver : IJsonFormatterResolver<TSymbol, TResolver>, new() where TSymbol : struct
    {
        private readonly SpanJsonOptions _spanJsonOptions;
        private readonly JsonNamingPolicy? _dictionayKeyPolicy;
        private readonly JsonNamingPolicy? _extensionDataPolicy;
        private readonly JsonNamingPolicy? _jsonPropertyNamingPolicy;

        private readonly JsonEscapeHandling _escapeHandling;
        private readonly JavaScriptEncoder? _encoder;

        private DeserializeDynamicDelegate<TSymbol> _dynamicDeserializer = ReadDynamic;

        // ReSharper disable StaticMemberInGenericType
        private static readonly ConcurrentDictionary<Type, IJsonFormatter> Formatters =
            new();
        // ReSharper restore StaticMemberInGenericType

        protected ResolverBase(SpanJsonOptions spanJsonOptions)
        {
            _spanJsonOptions = spanJsonOptions;
            _escapeHandling = spanJsonOptions.EscapeHandling;
            _encoder = spanJsonOptions.Encoder;
            _dictionayKeyPolicy = spanJsonOptions.DictionaryKeyPolicy;
            _extensionDataPolicy = spanJsonOptions.ExtensionDataNamingPolicy;
            _jsonPropertyNamingPolicy = spanJsonOptions.PropertyNamingPolicy;
        }

        public SpanJsonOptions JsonOptions => _spanJsonOptions;

        public JsonEscapeHandling EscapeHandling => _escapeHandling;
        public JavaScriptEncoder? Encoder => _encoder;

        public virtual DeserializeDynamicDelegate<TSymbol> DynamicDeserializer
        {
            get => _dynamicDeserializer;
            set
            {
                if (value is null) { ThrowHelper.ThrowArgumentNullException(ExceptionArgument.value); }
                _dynamicDeserializer = value;
            }
        }

        public virtual IJsonFormatter GetFormatter(Type type)
        {
            // ReSharper disable ConvertClosureToMethodGroup
            return Formatters.GetOrAdd(type, x => BuildFormatter(x));
            // ReSharper restore ConvertClosureToMethodGroup
        }

        /// <summary>Override a formatter on global scale, additionally we might need to register array versions etc
        /// Only register primitive types here, no arrays etc. this creates weird problems.</summary>
        public static void RegisterGlobalCustomFormatter<T, TFormatter>() where TFormatter : ICustomJsonFormatter<T>
        {
            var type = typeof(T);
            var formatterType = typeof(TFormatter);
            var staticDefaultField = formatterType.GetField("Default", BindingFlags.Static | BindingFlags.Public);
            if (staticDefaultField is null)
            {
                throw new InvalidOperationException($"{formatterType.FullName} must have a public static field 'Default' returning an instance of it.");
            }

            Formatters.AddOrUpdate(type, GetDefaultOrCreate(formatterType), (t, formatter) => GetDefaultOrCreate(formatterType));
        }

        public virtual IJsonFormatter GetFormatter(JsonMemberInfo memberInfo, Type? overrideMemberType = null)
        {
            // ReSharper disable ConvertClosureToMethodGroup
            if (memberInfo.CustomSerializer is not null)
            {
                var formatter = GetDefaultOrCreate(memberInfo.CustomSerializer);
                if (formatter is ICustomJsonFormatter csf && memberInfo.CustomSerializerArguments is not null)
                {
                    csf.Arguments = memberInfo.CustomSerializerArguments;
                }
                return formatter;
            }

            var type = overrideMemberType ?? memberInfo.MemberType;
            return GetFormatter(type);
            // ReSharper restore ConvertClosureToMethodGroup
        }

        public virtual JsonObjectDescription GetDynamicObjectDescription(IDynamicMetaObjectProvider provider)
        {
            var metaObject = provider.GetMetaObject(DynamicMetaObjectParameterExpression);
            var members = metaObject.GetDynamicMemberNames();
            var result = new List<JsonMemberInfo>();
            foreach (var memberInfoName in members)
            {
                var resolvedName = ResolvePropertyName(memberInfoName);
                var escapedName = JsonHelpers.GetEncodedText(resolvedName, _escapeHandling, _encoder);
                result.Add(new JsonMemberInfo(
                    memberInfoName, null, typeof(object), null, resolvedName, escapedName,
                    _spanJsonOptions.NullOption == NullOptions.ExcludeNulls, true, true, null, null));
            }

            return new JsonObjectDescription(null, null, result.ToArray(), null);
        }

        public IJsonFormatter<object?, TSymbol> GetRuntimeFormatter() => RuntimeFormatter<TSymbol, TResolver>.Default;
        public IJsonFormatter<T, TSymbol> GetEnumStringFormatter<T>() where T : struct, Enum
        {
            var type = typeof(T);
            if (type.FirstAttribute<FlagsAttribute>() is not null)
            {
                var enumBaseType = Enum.GetUnderlyingType(type);
                return (IJsonFormatter<T, TSymbol>)GetDefaultOrCreate(typeof(EnumStringFlagsFormatter<,,,>).GetCachedGenericType(type, enumBaseType, typeof(TSymbol), typeof(TResolver)));
            }

            return EnumStringFormatter<T, TSymbol, TResolver>.Default;
        }
        public IJsonFormatter<T, TSymbol> GetEnumIntegerFormatter<T>() where T : struct, Enum
        {
            return EnumIntegerFormatter<T, TSymbol, TResolver>.Default;
        }
        public virtual IJsonFormatter<T, TSymbol> GetFormatter<T>()
        {
            return (IJsonFormatter<T, TSymbol>)GetFormatter(typeof(T));
        }

        public virtual JsonObjectDescription GetObjectDescription<T>()
        {
            return BuildMembers(typeof(T)); // no need to cache that
        }

        protected virtual JsonObjectDescription BuildMembers(Type type)
        {
            var publicMembers = type.SerializableMembers();
            var result = new List<JsonMemberInfo>();
            JsonExtensionMemberInfo? extensionMemberInfo = null;
            var excludeNulls = _spanJsonOptions.NullOption == NullOptions.ExcludeNulls;
            foreach (var memberInfo in publicMembers)
            {
                var memberType = memberInfo is FieldInfo fi ? fi.FieldType :
                    memberInfo is PropertyInfo pi ? pi.PropertyType : null;
                var alias = GetAttributeName(memberInfo);
                var resolvedName = alias ?? ResolvePropertyName(memberInfo.Name);
                var escapedName = JsonHelpers.GetEncodedText(resolvedName, _escapeHandling, _encoder);

                var canRead = true;
                var canWrite = true;
                if (memberInfo is PropertyInfo propertyInfo)
                {
                    canRead = propertyInfo.CanRead;
                    canWrite = propertyInfo.CanWrite;
                }

                if (JsonHelpers.HasExtensionAttribute(memberInfo) && typeof(IDictionary<string, object>).IsAssignableFrom(memberType) && canRead && canWrite)
                {
                    extensionMemberInfo = new JsonExtensionMemberInfo(memberInfo.Name, memberType!, excludeNulls);
                }
                else if (!JsonHelpers.IsIgnored(memberInfo))
                {
                    var customSerializerAttr = memberInfo.FirstAttribute<JsonCustomSerializerAttribute>();
                    var shouldSerialize = type.GetMethod($"ShouldSerialize{memberInfo.Name}");
                    result.Add(new JsonMemberInfo(
                        memberInfo.Name, alias, memberType!, shouldSerialize, resolvedName, escapedName,
                        excludeNulls, canRead, canWrite, customSerializerAttr?.Type, customSerializerAttr?.Arguments));
                }
            }

            TryGetAnnotatedAttributeConstructor(type, out var constructor, out var attribute);
            return new JsonObjectDescription(constructor, attribute, result.ToArray(), extensionMemberInfo);
        }

        protected virtual void TryGetAnnotatedAttributeConstructor(Type type, out ConstructorInfo? constructor, out JsonConstructorAttribute? attribute)
        {
            var declaredConstructors = type.GetTypeInfo().DeclaredConstructors.Where(c => !c.IsStatic && c.IsPublic).ToArray();

            constructor = declaredConstructors
                .FirstOrDefault(a => a.HasAttribute<JsonConstructorAttribute>() || a.HasAttributeNamed("JsonConstructor"));
            if (constructor is not null)
            {
                attribute = constructor.FirstAttribute<JsonConstructorAttribute>();
                if (attribute is null) { attribute = JsonConstructorAttribute.Default; }
                return;
            }

            if (TryGetBaseClassJsonConstructorAttribute(type, out attribute) || type.GetMethod("<Clone>$") is not null)
            {
                // We basically take the one with the most parameters, this needs to match the dictionary // TODO find better method
                constructor = declaredConstructors.OrderByDescending(a => a.GetParameters().Length)
                    .FirstOrDefault();
                return;
            }

            attribute = type.FirstAttribute<JsonConstructorAttribute>(); // 检查动态特性
            if (attribute is not null)
            {
                var parameterNames = attribute.ParameterNames;
                var hasParameterNames = parameterNames is not null && (uint)parameterNames.Length > 0u;
                var parameterTypes = attribute.ParameterTypes;
                var hasParameterTypes = parameterTypes is not null && (uint)parameterTypes.Length > 0u;
                if (hasParameterTypes)
                {
                    constructor = declaredConstructors.FirstOrDefault(a => MatchConstructor(a, parameterTypes!));
                    if (constructor is not null) { return; }
                }
                else if (hasParameterNames)
                {
                    constructor = declaredConstructors.OrderByDescending(a => a.GetParameters().Length).FirstOrDefault(a => a.GetParameters().Length == parameterNames!.Length);
                    if (constructor is not null) { return; }
                }

                // We basically take the one with the most parameters, this needs to match the dictionary // TODO find better method
                constructor = declaredConstructors.OrderByDescending(a => a.GetParameters().Length)
                    .FirstOrDefault();
                return;
            }

            constructor = default;
            attribute = default;
        }

        private static bool MatchConstructor(ConstructorInfo constructor, Type[] givenParameterTypes)
        {
            var parameters = constructor.GetParameters();
            if (parameters.Length != givenParameterTypes.Length) { return false; }

            for (int idx = 0; idx < parameters.Length; idx++)
            {
                if (parameters[idx].ParameterType != givenParameterTypes[idx]) { return false; }
            }
            return true;
        }

        private static string? GetAttributeName(MemberInfo memberInfo)
        {
            var alias = memberInfo.FirstAttribute<JsonPropertyNameAttribute>()?.Name;
            if (alias is null)
            {
                var attrs = memberInfo.GetAllAttributes();
                foreach (var attr in attrs)
                {
                    var attrType = attr.GetType();
                    if (attrType.Name == "JsonPropertyNameAttribute") // System.Text.Json
                    {
                        var nameProperty = attrType.GetInstanceProperties(true).FirstOrDefault(x => x.Name == "Name");
                        if (nameProperty is not null && attr.TryGetPropertyValue(nameProperty, out var pv))
                        {
                            alias = pv as string;
                        }
                        break;
                    }
                    else if (attrType.Name == "JsonPropertyAttribute") // Newtonsoft.Json
                    {
                        var nameProperty = attrType.GetInstanceProperties(true).FirstOrDefault(x => x.Name == "PropertyName");
                        if (nameProperty is not null && attr.TryGetPropertyValue(nameProperty, out var pv))
                        {
                            alias = pv as string;
                        }
                        break;
                    }
                }
            }
            return alias;
        }

        private IJsonFormatter BuildFormatter(Type type)
        {
            var integrated = GetIntegrated(type);
            if (integrated is not null)
            {
                return integrated;
            }

            JsonCustomSerializerAttribute attr;
            if ((attr = type.FirstAttribute<JsonCustomSerializerAttribute>()) is not null)
            {
                var formatter = GetDefaultOrCreate(attr.Type);
                if (formatter is ICustomJsonFormatter csf && attr.Arguments is not null)
                {
                    csf.Arguments = attr.Arguments;
                }
                return formatter;
            }

            // from custom json formatter resolver
            Interlocked.CompareExchange(ref s_isFreezed, Locked, Unlocked);
            var resolvers = Volatile.Read(ref s_resolvers);
            foreach (var item in resolvers)
            {
                var f = item.GetFormatter(type);
                if (f is not null) { return f; }
            }

            if (type == typeof(object))
            {
                return GetDefaultOrCreate(typeof(RuntimeFormatter<TSymbol, TResolver>));
            }

            if (type.IsArray)
            {
                var rank = type.GetArrayRank();
                switch (rank)
                {
                    case 1:
                        return GetDefaultOrCreate(typeof(ArrayFormatter<,,>).GetCachedGenericType(type.GetElementType(),
                            typeof(TSymbol), typeof(TResolver)));
                    case 2:
                        return GetDefaultOrCreate(typeof(TwoDimensionalArrayFormatter<,,>).GetCachedGenericType(type.GetElementType(),
                            typeof(TSymbol), typeof(TResolver)));
                    default:
                        throw new NotSupportedException("Only One- and Two-dimensional arrrays are supported.");
                }
            }

            if (type.IsEnum)
            {
                switch (_spanJsonOptions.EnumOption)
                {
                    case EnumOptions.String:
                        {
                            if (type.FirstAttribute<FlagsAttribute>() is not null)
                            {
                                var enumBaseType = Enum.GetUnderlyingType(type);
                                return GetDefaultOrCreate(typeof(EnumStringFlagsFormatter<,,,>).GetCachedGenericType(type, enumBaseType, typeof(TSymbol), typeof(TResolver)));
                            }

                            return GetDefaultOrCreate(typeof(EnumStringFormatter<,,>).GetCachedGenericType(type, typeof(TSymbol), typeof(TResolver)));
                        }
                    case EnumOptions.Integer:
                        return GetDefaultOrCreate(typeof(EnumIntegerFormatter<,,>).GetCachedGenericType(type, typeof(TSymbol), typeof(TResolver)));
                }
            }

            if (typeof(IDynamicMetaObjectProvider).IsAssignableFrom(type))
            {
                return GetDefaultOrCreate(typeof(DynamicMetaObjectProviderFormatter<,,>).GetCachedGenericType(type, typeof(TSymbol), typeof(TResolver)));
            }

            if (type.TryGetTypeOfGenericInterface(typeof(IDictionary<,>), out var dictArgumentTypes) && HasApplicableCtor(type))
            {
                var writableType = type.IsInterface ? GetFunctorFallBackType(type) : type;
                return GetDefaultOrCreate(typeof(DictionaryFormatter<,,,,,>).GetCachedGenericType(type, writableType, dictArgumentTypes[0], dictArgumentTypes[1],
                    typeof(TSymbol), typeof(TResolver)));
            }

            if (type.TryGetTypeOfGenericInterface(typeof(IReadOnlyDictionary<,>), out var rodictArgumentTypes))
            {
                var writableType = typeof(Dictionary<,>).GetCachedGenericType(rodictArgumentTypes);
                return GetDefaultOrCreate(
                    typeof(DictionaryFormatter<,,,,,>).GetCachedGenericType(type, writableType, rodictArgumentTypes[0], rodictArgumentTypes[1], typeof(TSymbol),
                        typeof(TResolver)));
            }

            if (type.TryGetTypeOfGenericInterface(typeof(IList<>), out var listArgumentTypes) && HasApplicableCtor(type))
            {
                return GetDefaultOrCreate(typeof(ListFormatter<,,,>).GetCachedGenericType(type, listArgumentTypes.Single(), typeof(TSymbol), typeof(TResolver)));
            }

            if (type.TryGetTypeOfGenericInterface(typeof(IEnumerable<>), out var enumArgumentTypes))
            {
                return GetDefaultOrCreate(
                    typeof(EnumerableFormatter<,,,>).GetCachedGenericType(type, enumArgumentTypes.Single(), typeof(TSymbol), typeof(TResolver)));
            }

            if (type.TryGetNullableUnderlyingType(out var underlyingType))
            {
                return GetDefaultOrCreate(typeof(NullableFormatter<,,>).GetCachedGenericType(underlyingType,
                    typeof(TSymbol), typeof(TResolver)));
            }

            // no integrated type, let's build it
            if (type.IsValueType)
            {
                return GetDefaultOrCreate(
                    typeof(ComplexStructFormatter<,,>).GetCachedGenericType(type, typeof(TSymbol), typeof(TResolver)));
            }

            return GetDefaultOrCreate(typeof(ComplexClassFormatter<,,>).GetCachedGenericType(type, typeof(TSymbol), typeof(TResolver)));
        }

        /// <summary>
        /// Either standard ctor or ctor with constructor for proper values
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        protected virtual bool HasApplicableCtor(Type type)
        {
            // ReadOnlyDictionary is kinda broken, it implements IDictionary<T> too, but without any standard ctor
            // Make sure this is using the ReadOnlyDictionaryFormatter
            // ReadOnlyCollection is kinda broken, it implements ICollection<T> too, but without any standard ctor
            // Make sure this is using the EnumerableFormatter
            if (type.IsInterface)
            {
                return true; // late checking with fallback
            }

            return type.GetConstructor(Type.EmptyTypes) is not null;
        }

        private static IJsonFormatter? GetIntegrated(Type type)
        {
            var allTypes = typeof(ResolverBase).Assembly.GetTypes();
            foreach (var candidate in allTypes.Where(a => a.IsPublic))
            {
                if (candidate.TryGetTypeOfGenericInterface(typeof(ICustomJsonFormatter<>), out _))
                {
                    continue; // if it's a custom formatter, we skip it
                }

                if (candidate.TryGetTypeOfGenericInterface(typeof(IJsonFormatter<,>), out var argumentTypes) && argumentTypes.Length == 2)
                {
                    if (argumentTypes[0] == type && argumentTypes[1] == typeof(TSymbol))
                    {
                        // if it has a custom formatter for a base type (i.e. nullable base type, array element, list element)
                        // we need to ignore the integrated types for this
                        if (HasCustomFormatterForRelatedType(type))
                        {
                            continue;
                        }

                        return GetDefaultOrCreate(candidate);
                    }
                }
            }

            return null;
        }

        private static bool HasCustomFormatterForRelatedType(Type type)
        {
            Type? relatedType = Nullable.GetUnderlyingType(type);
            if (relatedType is null && type.IsArray)
            {
                relatedType = type.GetElementType();
            }

            if (relatedType is null && type.TryGetTypeOfGenericInterface(typeof(IList<>), out var argumentTypes) && argumentTypes.Length == 1)
            {
                relatedType = argumentTypes.Single();
            }

            if (relatedType is not null)
            {
                if (Formatters.TryGetValue(relatedType, out var formatter) && formatter is ICustomJsonFormatter)
                {
                    return true;
                }

                if (Nullable.GetUnderlyingType(relatedType) is not null)
                {
                    return HasCustomFormatterForRelatedType(relatedType); // we need to recurse if the related type is again nullable
                }
            }

            return false;
        }

        public virtual Func<T> GetCreateFunctor<T>()
        {
            var type = typeof(T);
            var ci = type.GetConstructor(Type.EmptyTypes);
            if (type.IsInterface || ci is null)
            {
                type = GetFunctorFallBackType(type);
                if (type is null)
                {
                    return () => throw new NotSupportedException($"Can't create {typeof(T).Name}.");
                }
            }

            return Expression.Lambda<Func<T>>(Expression.New(type)).Compile();
        }

        protected virtual Type? GetFunctorFallBackType(Type type)
        {
            if (type.TryGetTypeOfGenericInterface(typeof(IDictionary<,>), out var dictArgumentTypes))
            {
                return typeof(Dictionary<,>).GetCachedGenericType(dictArgumentTypes);
            }

            if (type.TryGetTypeOfGenericInterface(typeof(IReadOnlyDictionary<,>), out var rodictArgumentTypes))
            {
                return typeof(Dictionary<,>).GetCachedGenericType(rodictArgumentTypes);
            }

            if (type.TryGetTypeOfGenericInterface(typeof(IList<>), out var listArgumentTypes))
            {
                return typeof(List<>).GetCachedGenericType(listArgumentTypes);
            }

            if (type.TryGetTypeOfGenericInterface(typeof(ISet<>), out var setArgumentTypes))
            {
                return typeof(HashSet<>).GetCachedGenericType(setArgumentTypes);
            }

            return null;
        }


        public virtual Func<T, TConverted> GetEnumerableConvertFunctor<T, TConverted>()
        {
            var inputType = typeof(T);
            var convertedType = typeof(TConverted);
            if (typeof(T) == typeof(TConverted))
            {
                return arg => Unsafe.As<T, TConverted>(ref arg);
            }
            var paramExpression = Expression.Parameter(inputType, "input");
            if (convertedType.IsAssignableFrom(inputType))
            {
                return Expression.Lambda<Func<T, TConverted>>(Expression.Convert(paramExpression, convertedType), paramExpression).Compile();
            }

            if (IsUnsupportedEnumerable(convertedType))
            {
                return _ => throw new NotSupportedException($"{typeof(TConverted).Name} is not supported.");
            }

            // not a nice way, but I don't find another good way to solve this, without adding either a dependency to immutable collection
            // or another nuget package and plugin code.
            if (convertedType.Namespace == "System.Collections.Immutable")
            {
                var emptyField = convertedType.GetField("Empty", BindingFlags.Public | BindingFlags.Static);
                var addRangeMethod = convertedType.GetMethods(BindingFlags.Public | BindingFlags.Instance).FirstOrDefault(a =>
                    a.Name == "AddRange" && a.GetParameters().Length == 1 && a.GetParameters().Single().ParameterType.IsAssignableFrom(paramExpression.Type));
                if (emptyField is null || addRangeMethod is null)
                {
                    return _ => throw new NotSupportedException($"{typeof(TConverted).Name} has no supported Immutable Collections (Immutable.Empty.AddRange) pattern.");
                }

                return Expression.Lambda<Func<T, TConverted>>(Expression.Call(Expression.Field(null, emptyField), addRangeMethod, paramExpression),
                    paramExpression).Compile();
            }

            if (convertedType.IsInterface)
            {
                convertedType = GetFunctorFallBackType(convertedType);
                if (convertedType is null)
                {
                    return _ => throw new NotSupportedException($"Can't convert {typeof(T).Name} to {typeof(TConverted).Name}.");
                }
            }

            var ci = convertedType.GetConstructors().FirstOrDefault(a =>
                a.GetParameters().Length == 1 && a.GetParameters().Single().ParameterType.IsAssignableFrom(paramExpression.Type));
            if (ci is null)
            {
                return _ => throw new NotSupportedException($"No constructor of {convertedType.Name} accepts {paramExpression.Type.Name}.");
            }

            var lambda = Expression.Lambda<Func<T, TConverted>>(Expression.New(ci, paramExpression), paramExpression);
            return lambda.Compile();
        }

        /// <summary>
        /// Some types are just bad to be deserialized for enumerables
        /// </summary>
        protected virtual bool IsUnsupportedEnumerable(Type type)
        {
            // TODO: Stack/ConcurrentStack require that the order of the elements is reversed on deserialization, block it for now
            if (type.IsGenericType && (type.GetGenericTypeDefinition() == typeof(Stack<>) || type.GetGenericTypeDefinition() == typeof(ConcurrentStack<>)))
            {
                return true;
            }

            return false;
        }

        /// <summary>Resolves the key of the dictionary.</summary>
        /// <param name="dictionaryKey">Key of the dictionary.</param>
        /// <returns>Resolved key of the dictionary.</returns>
        public string ResolveDictionaryKey(string dictionaryKey)
        {
            if (_dictionayKeyPolicy is not null)
            {
                return _dictionayKeyPolicy.ConvertName(dictionaryKey);
            }

            return ResolvePropertyName(dictionaryKey);
        }

        /// <summary>Resolves the name of the extension data.</summary>
        /// <param name="extensionDataName">Name of the extension data.</param>
        /// <returns>Resolved name of the extension data.</returns>
        public string ResolveExtensionDataName(string extensionDataName)
        {
            if (_extensionDataPolicy is not null)
            {
                return _extensionDataPolicy.ConvertName(extensionDataName);
            }

            return extensionDataName;
        }

        /// <summary>Resolves the name of the property.</summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>Resolved name of the property.</returns>
        public string ResolvePropertyName(string propertyName)
        {
            if (_jsonPropertyNamingPolicy is not null)
            {
                return _jsonPropertyNamingPolicy.ConvertName(propertyName);
            }

            return propertyName;
        }

        public JsonEncodedText GetEncodedDictionaryKey(string dictionaryKey)
        {
            return JsonHelpers.GetEncodedText(ResolveDictionaryKey(dictionaryKey), _escapeHandling, _encoder);
        }

        public JsonEncodedText GetEncodedExtensionDataName(string extensionDataName)
        {
            return JsonHelpers.GetEncodedText(ResolveExtensionDataName(extensionDataName), _escapeHandling, _encoder);
        }

        public JsonEncodedText GetEncodedPropertyName(string propertyName)
        {
            return JsonHelpers.GetEncodedText(ResolvePropertyName(propertyName), _escapeHandling, _encoder);
        }

        // CustomJsonFormatterResolver

        private const int Locked = 1;
        private const int Unlocked = 0;
        private static int s_isFreezed = Unlocked;
        private static List<ICustomJsonFormatterResolver> s_resolvers = new();

        /// <summary>Only support for custom formatters.</summary>
        public bool IsSupportedType(Type type)
        {
            if (Formatters.ContainsKey(type)) { return true; }

            var resolvers = Volatile.Read(ref s_resolvers);
            foreach (var item in resolvers)
            {
                if (item.IsSupportedType(type)) { return true; }
            }

            return false;
        }

        public static void RegisterGlobalCustomrResolver(params ICustomJsonFormatterResolver[] resolvers)
        {
            if (resolvers is null || 0u >= (uint)resolvers.Length) { return; }

            if (TryRegisterGlobalCustomrResolver(resolvers)) { return; }
            ThrowHelper.ThrowInvalidOperationException_Register_Resolver_Err();
        }

        public static bool TryRegisterGlobalCustomrResolver(params ICustomJsonFormatterResolver[] resolvers)
        {
            if (resolvers is null || 0u >= (uint)resolvers.Length) { return false; }
            if (Locked == Volatile.Read(ref s_isFreezed)) { return false; }

            List<ICustomJsonFormatterResolver> snapshot, newCache;
            do
            {
                snapshot = Volatile.Read(ref s_resolvers);
                newCache = new List<ICustomJsonFormatterResolver>();
                newCache.AddRange(resolvers);
                if ((uint)snapshot.Count > 0u) { newCache.AddRange(snapshot); }
            } while (!ReferenceEquals(
                Interlocked.CompareExchange(ref s_resolvers, newCache, snapshot), snapshot));
            return true;
        }

        private static object? ReadDynamic(ref JsonReader<TSymbol> reader)
        {
            return reader.ReadDynamic();
        }
    }
}