using System.Reflection;

namespace SpanJson.Resolvers
{
    public class JsonMemberInfo
    {
        public JsonMemberInfo(string memberName, string? alias, Type memberType, MethodInfo? shouldSerialize, string name, in JsonEncodedText escapedName,
            bool excludeNull, bool canRead, bool canWrite, Type? customSerializer, object? customSerializerArguments)
        {
            MemberName = memberName;
            Alias = alias;
            MemberType = memberType;
            ShouldSerialize = shouldSerialize;
            Name = name;
            EscapedName = escapedName;
            ExcludeNull = excludeNull;
            CanRead = canRead;
            CanWrite = canWrite;
            CustomSerializer = customSerializer;
            CustomSerializerArguments = customSerializerArguments;
        }

        public string MemberName { get; }
        public string? Alias { get; set; }
        public Type MemberType { get; }
        public MethodInfo? ShouldSerialize { get; }
        public string Name { get; }
        public JsonEncodedText EscapedName { get; }
        public bool ExcludeNull { get; }

        public Type? CustomSerializer { get; }
        public object? CustomSerializerArguments { get; }

        public bool CanRead { get; }
        public bool CanWrite { get; set; }
    }
}