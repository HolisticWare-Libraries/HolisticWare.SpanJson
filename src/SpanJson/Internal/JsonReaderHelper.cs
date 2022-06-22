using System;
using System.Buffers.Text;
using System.Diagnostics;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using CuteAnt;

// borrowed from https://github.com/dotnet/corefx/blob/8135319caa7e457ed61053ca1418313b88057b51/src/System.Text.Json/src/System/Text/Json/Reader/JsonReaderHelper.cs

namespace SpanJson.Internal
{
    internal static partial class JsonReaderHelper
    {
        public static (int, int) CountNewLines(in ReadOnlySpan<byte> data)
        {
            int lastLineFeedIndex = -1;
            int newLines = 0;
            for (int i = 0; i < data.Length; i++)
            {
                if (data[i] == JsonUtf8Constant.LineFeed)
                {
                    lastLineFeedIndex = i;
                    newLines++;
                }
            }
            return (newLines, lastLineFeedIndex);
        }

        internal static JsonValueKind ToValueKind(this JsonTokenType tokenType)
        {
            switch (tokenType)
            {
                case JsonTokenType.None:
                    return JsonValueKind.Undefined;
                case JsonTokenType.BeginArray:
                    return JsonValueKind.Array;
                case JsonTokenType.BeginObject:
                    return JsonValueKind.Object;
                case JsonTokenType.String:
                    return JsonValueKind.String;
                case JsonTokenType.Number:
                    return JsonValueKind.Number;
                case JsonTokenType.True:
                    return JsonValueKind.True;
                case JsonTokenType.False:
                    return JsonValueKind.False;
                case JsonTokenType.Null:
                    return JsonValueKind.Null;
                default:
                    Debug.Fail($"No mapping for token type {tokenType}");
                    return JsonValueKind.Undefined;
            }
        }

        // Returns true if the TokenType is a primitive "value", i.e. String, Number, True, False, and Null
        // Otherwise, return false.
        public static bool IsTokenTypePrimitive(JsonTokenType tokenType) =>
            (tokenType - JsonTokenType.String) <= (JsonTokenType.Null - JsonTokenType.String);

        // A hex digit is valid if it is in the range: [0..9] | [A..F] | [a..f]
        // Otherwise, return false.
        public static bool IsHexDigit(byte nextByte) => HexConverter.IsHexChar(nextByte);

        // https://tools.ietf.org/html/rfc8259
        // Does the span contain '"', '\',  or any control characters (i.e. 0 to 31)
        // IndexOfAny(34, 92, < 32)
        // Borrowed and modified from SpanHelpers.Byte:
        // https://github.com/dotnet/corefx/blob/fc169cddedb6820aaabbdb8b7bece2a3df0fd1a5/src/Common/src/CoreLib/System/SpanHelpers.Byte.cs#L473-L604
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOfQuoteOrAnyControlOrBackSlash(this in ReadOnlySpan<byte> span)
        {
            return SpanHelpers.IndexOfAnyOrLessThan(
                    ref MemoryMarshal.GetReference(span),
                    JsonUtf8Constant.DoubleQuote,
                    JsonUtf8Constant.BackSlash,
                    lessThan: 32,   // Space ' '
                    span.Length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOfQuoteOrAnyControlOrBackSlash(ref byte searchSpace, int length)
        {
            return SpanHelpers.IndexOfAnyOrLessThan(
                    ref searchSpace,
                    JsonUtf8Constant.DoubleQuote,
                    JsonUtf8Constant.BackSlash,
                    lessThan: 32,   // Space ' '
                    length);
        }

        public static bool TryGetEscapedDateTime(in ReadOnlySpan<byte> source, out DateTime value)
        {
            int backslash = source.IndexOf(JsonUtf8Constant.BackSlash);
            Debug.Assert(backslash != -1);

            Debug.Assert(source.Length <= JsonSharedConstant.MaximumEscapedDateTimeOffsetParseLength);
            Span<byte> sourceUnescaped = stackalloc byte[JsonSharedConstant.MaximumEscapedDateTimeOffsetParseLength];

            Unescape(source, sourceUnescaped, backslash, out int written);
            Debug.Assert(written > 0);

            sourceUnescaped = sourceUnescaped.Slice(0, written);
            Debug.Assert(!sourceUnescaped.IsEmpty);

            if (sourceUnescaped.Length <= JsonSharedConstant.MaximumDateTimeOffsetParseLength
                && JsonHelpers.TryParseAsISO(sourceUnescaped, out DateTime tmp))
            {
                value = tmp;
                return true;
            }

            value = default;
            return false;
        }

        public static bool TryGetEscapedDateTimeOffset(in ReadOnlySpan<byte> source, out DateTimeOffset value)
        {
            int backslash = source.IndexOf(JsonUtf8Constant.BackSlash);
            Debug.Assert(backslash != -1);

            Debug.Assert(source.Length <= JsonSharedConstant.MaximumEscapedDateTimeOffsetParseLength);
            Span<byte> sourceUnescaped = stackalloc byte[JsonSharedConstant.MaximumEscapedDateTimeOffsetParseLength];

            Unescape(source, sourceUnescaped, backslash, out int written);
            Debug.Assert(written > 0);

            sourceUnescaped = sourceUnescaped.Slice(0, written);
            Debug.Assert(!sourceUnescaped.IsEmpty);

            if (sourceUnescaped.Length <= JsonSharedConstant.MaximumDateTimeOffsetParseLength
                && JsonHelpers.TryParseAsISO(sourceUnescaped, out DateTimeOffset tmp))
            {
                value = tmp;
                return true;
            }

            value = default;
            return false;
        }

        public static bool TryGetEscapedGuid(in ReadOnlySpan<byte> source, out Guid value)
        {
            Debug.Assert(source.Length <= JsonSharedConstant.MaximumEscapedGuidLength);

            int idx = source.IndexOf(JsonUtf8Constant.BackSlash);
            Debug.Assert(idx != -1);

            Span<byte> utf8Unescaped = stackalloc byte[JsonSharedConstant.MaximumEscapedGuidLength];

            Unescape(source, utf8Unescaped, idx, out int written);
            Debug.Assert(written > 0);

            utf8Unescaped = utf8Unescaped.Slice(0, written);
            Debug.Assert(!utf8Unescaped.IsEmpty);

            if (utf8Unescaped.Length == JsonSharedConstant.MaximumFormatGuidLength
                && Utf8Parser.TryParse(utf8Unescaped, out Guid tmp, out _, 'D'))
            {
                value = tmp;
                return true;
            }

            value = default;
            return false;
        }

        public static bool TryGetEscapedCombGuid(in ReadOnlySpan<byte> source, out CombGuid value)
        {
            Debug.Assert(source.Length <= JsonSharedConstant.MaximumEscapedGuidLength);

            int idx = source.IndexOf(JsonUtf8Constant.BackSlash);
            Debug.Assert(idx != -1);

            Span<byte> utf8Unescaped = stackalloc byte[source.Length];

            Unescape(source, utf8Unescaped, idx, out int written);
            Debug.Assert(written > 0);

            utf8Unescaped = utf8Unescaped.Slice(0, written);
            Debug.Assert(!utf8Unescaped.IsEmpty);

            if (utf8Unescaped.Length == JsonSharedConstant.MaximumFormatGuidLength &&
#if NETSTANDARD2_0
                CombGuid.TryParse(TextEncodings.Utf8.GetString(utf8Unescaped), CombGuidSequentialSegmentType.Comb, out CombGuid tmp)
#else
                CombGuid.TryParse(utf8Unescaped, CombGuidSequentialSegmentType.Comb, out CombGuid tmp, out _)
#endif
                )
            {
                value = tmp;
                return true;
            }

            value = default;
            return false;
        }

        public static bool TryGetFloatingPointConstant(in ReadOnlySpan<byte> span, out float value)
        {
            if (span.Length == 3)
            {
                if (span.SequenceEqual(JsonUtf8Constant.NaNValue))
                {
                    value = float.NaN;
                    return true;
                }
            }
            else if (span.Length == 8)
            {
                if (span.SequenceEqual(JsonUtf8Constant.PositiveInfinityValue))
                {
                    value = float.PositiveInfinity;
                    return true;
                }
            }
            else if (span.Length == 9)
            {
                if (span.SequenceEqual(JsonUtf8Constant.NegativeInfinityValue))
                {
                    value = float.NegativeInfinity;
                    return true;
                }
            }

            value = 0;
            return false;
        }

        public static bool TryGetFloatingPointConstant(in ReadOnlySpan<byte> span, out double value)
        {
            if (span.Length == 3)
            {
                if (span.SequenceEqual(JsonUtf8Constant.NaNValue))
                {
                    value = double.NaN;
                    return true;
                }
            }
            else if (span.Length == 8)
            {
                if (span.SequenceEqual(JsonUtf8Constant.PositiveInfinityValue))
                {
                    value = double.PositiveInfinity;
                    return true;
                }
            }
            else if (span.Length == 9)
            {
                if (span.SequenceEqual(JsonUtf8Constant.NegativeInfinityValue))
                {
                    value = double.NegativeInfinity;
                    return true;
                }
            }

            value = 0;
            return false;
        }
    }
}
