using System.Collections.Concurrent;
using System.Dynamic;
using System.Reflection;
using System.Runtime.CompilerServices;
using CuteAnt;
using SpanJson.Helpers;
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

    private static readonly ConcurrentDictionary<Type, bool> s_polymorphicTypeCache = new ConcurrentDictionary<Type, bool>();

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

        if (typeof(IDynamicMetaObjectProvider).IsAssignableFrom(implementedType))
        {
            result = false;
        }
        else if (implementedType == TypeConstants.ObjectType)
        {
            result = true;
        }
        else if (implementedType.HasAttribute<JsonPolymorphismAttribute>(false))
        {
            result = true;
        }
        // 只判断是否已经注册自定义 Formatter，IncludeNullsOriginalCaseResolver 作为默认的 Resolver，会确保应用程序所用的 custom formatter
        else if (StandardResolvers.GetResolver<char, IncludeNullsOriginalCaseResolver<char>>().IsSupportedType(implementedType))
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

                    if (StandardResolvers.GetResolver<char, IncludeNullsOriginalCaseResolver<char>>().IsSupportedType(fieldType)) { continue; }

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

                    if (StandardResolvers.GetResolver<char, IncludeNullsOriginalCaseResolver<char>>().IsSupportedType(propertyType)) { continue; }

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
}
