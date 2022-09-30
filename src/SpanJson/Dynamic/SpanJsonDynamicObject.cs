using System.Diagnostics.CodeAnalysis;
using System.Dynamic;

namespace SpanJson.Dynamic
{
    public sealed class SpanJsonDynamicObject : DynamicObject
    {
        private readonly Dictionary<string, object?> _dictionary;
        private object? _rawJson;
        private readonly bool _isUtf16;

        internal SpanJsonDynamicObject(Dictionary<string, object?> dictionary)
        {
            _dictionary = dictionary;
        }

        internal SpanJsonDynamicObject(Dictionary<string, object?> dictionary, object rawJson, bool isUtf16)
        {
            _dictionary = dictionary;
            _rawJson = rawJson;
            _isUtf16 = isUtf16;
        }

        [JsonIgnore]
        internal bool HasRaw => _rawJson is not null;
        [JsonIgnore]
        internal bool IsUtf16 => _isUtf16;
        [JsonIgnore]
        internal ArraySegment<char> Utf16Raw => _rawJson is not null ? (ArraySegment<char>)_rawJson : default;
        [JsonIgnore]
        internal ArraySegment<byte> Utf8Raw => _rawJson is not null ? (ArraySegment<byte>)_rawJson : default;

        /// <summary>Gets or sets the <see cref="object"/> with the specified name.</summary>
        /// <value>The <see cref="object"/>.</value>
        /// <param name="name">The name.</param>
        /// <returns>Value from the property.</returns>
        public object? this[string name]
        {
            get
            {
                if (_dictionary.TryGetValue(name, out object? result))
                {
                    return result;
                }

                return null;
            }
        }

        public override string ToString()
        {
            return $"{{{string.Join(",", _dictionary.Select(a => $"\"{a.Key}\":{a.Value.ToJsonValue()}"))}}}";
        }

        public override bool TryGetMember(GetMemberBinder binder, out object? result)
        {
            return _dictionary.TryGetValue(binder.Name, out result);
        }

        public override bool TryConvert(ConvertBinder binder, [NotNullWhen(true)] out object? result)
        {
            if (typeof(IDictionary<string, object>).IsAssignableFrom(binder.ReturnType))
            {
                result = _dictionary;
                return true;
            }

            return base.TryConvert(binder, out result);
        }

        public override IEnumerable<string> GetDynamicMemberNames()
        {
            return _dictionary.Keys;
        }
    }
}