using System;
using System.ComponentModel;

namespace SpanJson.Dynamic
{
    [TypeConverter(typeof(DynamicTypeConverter))]
    public sealed class SpanJsonDynamicUtf16String : SpanJsonDynamicString<char>
    {
        public SpanJsonDynamicUtf16String(in ReadOnlySpan<char> span) : base(span, false) { }
        internal SpanJsonDynamicUtf16String(in ReadOnlySpan<char> span, bool isFloat) : base(span, isFloat) { }

        public SpanJsonDynamicUtf16String(
#if !NETSTANDARD2_0
            in
#endif
            ArraySegment<char> data) : base(data, false) { }

        internal SpanJsonDynamicUtf16String(
#if !NETSTANDARD2_0
            in
#endif
            ArraySegment<char> data, bool isFloat) : base(data, isFloat) { }
    }
}