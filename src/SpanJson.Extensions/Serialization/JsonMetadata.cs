using System.Collections.Concurrent;
using System.Dynamic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Xml;
using System.Xml.Linq;
using CuteAnt;
using SpanJson.Helpers;
using SpanJson.Linq;
using SpanJson.Resolvers;

namespace SpanJson.Serialization;

public static class JsonMetadata
{
    static class PolymorphicContainer<T>
    {
        public static readonly bool IsPolymorphic;

        static PolymorphicContainer()
        {
            IsPolymorphic = IsPolymorphic(typeof(T));
        }
    }

    private static readonly ConcurrentDictionary<Type, bool> s_polymorphicTypeCache;

    static JsonMetadata()
    {
        s_polymorphicTypeCache = new();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsPolymorphic<T>() => PolymorphicContainer<T>.IsPolymorphic;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsPolymorphic(Type type)
    {
        if (s_polymorphicTypeCache.TryGetValue(type, out var result)) { return result; }

        return IsPolymorphicImpl(type, parentType: null, memberInfo: null, parentTypes: new HashSet<Type>());
    }

    private static bool IsPolymorphicInternal(Type type, Type parentType, MemberInfo memberInfo, HashSet<Type> parentTypes)
    {
        if (s_polymorphicTypeCache.TryGetValue(type, out var result)) { return result; }

        return IsPolymorphicImpl(type, parentType, memberInfo, parentTypes);
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static bool IsPolymorphicImpl(Type type, Type? parentType, MemberInfo? memberInfo, HashSet<Type> parentTypes)
    {
        static Type GetUnderlyingTypeLocal(Type t, Type? pt, MemberInfo? mi)
        {
            Type? underlyingType = null;
            var classType = JsonClassInfo.GetClassType(t);
            switch (classType)
            {
                case ClassType.Enumerable:
                case ClassType.Dictionary:
                case ClassType.IDictionaryConstructible:
                    underlyingType = JsonClassInfo.GetElementType(t, pt, mi);
                    break;
            }
            if (underlyingType is null) { underlyingType = t; }

            if (underlyingType.IsGenericType && underlyingType.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                underlyingType = Nullable.GetUnderlyingType(underlyingType)!;
            }

            return underlyingType;
        }

        var result = false;

        Type implementedType = GetUnderlyingTypeLocal(type, parentType, memberInfo);

        if (implementedType.HasAttribute<JsonPolymorphismAttribute>(false))
        {
            result = true;
        }
        else if (implementedType == TypeConstants.ObjectType)
        {
            result = true;
        }
        else if (ShouldBeSkipped(implementedType))
        {
            result = false;
        }
        else if (IsSupportedType(implementedType))
        {
            result = false;
        }
        else
        {
            foreach (var item in implementedType.SerializableMembers())
            {
                if (item is FieldInfo fi)
                {
                    if (fi.HasAttribute<JsonPolymorphismAttribute>()) { result = true; break; }

                    var fieldType = GetUnderlyingTypeLocal(fi.FieldType, type, fi);

                    if (ShouldBeSkipped(fieldType)) { continue; }
                    if (IsSupportedType(fieldType)) { continue; }

                    if (fieldType.IsAbstract || fieldType.IsInterface) { result = true; break; }

                    if (fi.FieldType == type) { continue; }
                    if (fieldType == type) { continue; }
                    if (fieldType == implementedType) { continue; }
                    if (parentTypes.Contains(fieldType)) { continue; }
                    parentTypes.Add(type);
                    parentTypes.Add(implementedType);

                    if (IsPolymorphicInternal(fieldType, type, fi, parentTypes)) { result = true; break; }
                }

                if (item is PropertyInfo pi)
                {
                    if (pi.HasAttribute<JsonPolymorphismAttribute>()) { result = true; break; }

                    var propertyType = GetUnderlyingTypeLocal(pi.PropertyType, type, pi);

                    if (ShouldBeSkipped(propertyType)) { continue; }
                    if (IsSupportedType(propertyType)) { continue; }

                    if (propertyType.IsAbstract || propertyType.IsInterface) { result = true; break; }

                    if (pi.PropertyType == type) { continue; }
                    if (propertyType == type) { continue; }
                    if (propertyType == implementedType) { continue; }
                    if (parentTypes.Contains(propertyType)) { continue; }
                    parentTypes.Add(type);
                    parentTypes.Add(implementedType);

                    if (IsPolymorphicInternal(propertyType, type, pi, parentTypes)) { result = true; break; }
                }
            }
        }

        s_polymorphicTypeCache.TryAdd(type, result);
        if (implementedType != type)
        {
            s_polymorphicTypeCache.TryAdd(implementedType, result);
        }

        return result;
    }

    private static bool ShouldBeSkipped(Type type)
    {
        if (typeof(IDynamicMetaObjectProvider).IsAssignableFrom(type))
        {
            return true;
        }
        else if (typeof(XObject).IsAssignableFrom(type))
        {
            return true;
        }
        else if (typeof(XmlNode).IsAssignableFrom(type))
        {
            return true;
        }
        else if (IsAnonymousType(type))
        {
            return true;
        }
        return false;
    }

    private static bool IsSupportedType(Type objectType)
    {
        if (IsBuiltInType(objectType)) { return true; }

        if (StandardResolvers.GetResolver<char, IncludeNullsOriginalCaseResolver<char>>().IsSupportedType(objectType)) { return true; }

        var converters = JToken.DefaultSerializerSettings.Converters;
        if (converters is not null)
        {
            for (int i = 0; i < converters.Count; i++)
            {
                var converter = converters[i];

                if (converter.CanConvert(objectType))
                {
                    return true;
                }
            }
        }

        return false;
    }

    private static Type[]? _allTypes;
    private static bool IsBuiltInType(Type type)
    {
        _allTypes ??= typeof(ResolverBase).Assembly.GetTypes().Where(a => a.IsPublic).ToArray();
        var allTypes = _allTypes;
        foreach (var candidate in allTypes)
        {
            if (candidate.TryGetTypeOfGenericInterface(typeof(ICustomJsonFormatter<>), out _))
            {
                continue; // if it's a custom formatter, we skip it
            }

            if (candidate.TryGetTypeOfGenericInterface(typeof(IJsonFormatter<,>), out var argumentTypes) && argumentTypes.Length == 2)
            {
                if (argumentTypes[0] == type && (argumentTypes[1] == typeof(byte) || argumentTypes[1] == typeof(char)))
                {
                    return true;
                }
            }
        }

        return false;
    }

    private static bool IsAnonymousType(Type type)
    {
        return type.IsGenericType && type.Name.Contains("AnonymousType")
               && (type.Name.StartsWith("<>", StringComparison.Ordinal) || type.Name.StartsWith("VB$", StringComparison.Ordinal))
               && (type.Attributes & TypeAttributes.NotPublic) == TypeAttributes.NotPublic;
    }
}
