using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace SpanJson.Internal
{
    static partial class JsonHelpers
    {
        #region -- GetUnescapedTextFromUtf8WithCache --

        static readonly AsymmetricKeyHashTable<string> s_utf8StringCache = new(StringReadOnlySpanByteAscymmetricEqualityComparer.Instance);

        /// <summary> <see cref="JsonReader{TSymbol}.ReadUtf8VerbatimNameSpan(out int)"/> or <see cref="JsonReader{TSymbol}.ReadUtf8VerbatimStringSpan(out int)"/> </summary>
        public static string GetUnescapedTextFromUtf8WithCache(in ReadOnlySpan<byte> escapedUtf8Source, int idx)
        {
            if ((uint)idx >= (uint)escapedUtf8Source.Length)
            {
                return TextEncodings.Utf8.GetStringWithCache(escapedUtf8Source);
            }
            else
            {
                if (escapedUtf8Source.IsEmpty) { return string.Empty; }

                if (s_utf8StringCache.TryGetValue(escapedUtf8Source, out var value)) { return value; }

                return GetUnescapedTextFromUtf8Slow(escapedUtf8Source, idx);
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static string GetUnescapedTextFromUtf8Slow(in ReadOnlySpan<byte> escapedUtf8Source, int idx)
        {
            var buffer = escapedUtf8Source.ToArray();
            var value = JsonReaderHelper.GetUnescapedString(escapedUtf8Source, idx);
            s_utf8StringCache.TryAdd(buffer, value);
            return value;
        }

        #endregion

        #region -- GetUnescapedTextFromUtf16WithCache --

        static readonly AsymmetricKeyHashTable<string> s_utf16StringCache = new(StringReadOnlySpanByteAscymmetricEqualityComparer.Instance);

        /// <summary> <see cref="JsonReader{TSymbol}.ReadUtf8VerbatimNameSpan(out int)"/> or <see cref="JsonReader{TSymbol}.ReadUtf8VerbatimStringSpan(out int)"/> </summary>
        public static string GetUnescapedTextFromUtf16WithCache(in ReadOnlySpan<char> escapedUtf16Source, int escapedCharSize)
        {
            if (escapedUtf16Source.IsEmpty) { return string.Empty; }

            var utf16Source = MemoryMarshal.AsBytes(escapedUtf16Source);
            if (0u >= (uint)escapedCharSize)
            {
                return TextEncodings.Utf16.GetStringWithCache(utf16Source);
            }
            else
            {
                if (s_utf16StringCache.TryGetValue(utf16Source, out var value)) { return value; }

                return GetUnescapedTextFromUtf8Slow(escapedUtf16Source, utf16Source, escapedCharSize);
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static string GetUnescapedTextFromUtf8Slow(in ReadOnlySpan<char> escapedUtf16Source, in ReadOnlySpan<byte> utf16Source, int escapedCharSize)
        {
            var buffer = utf16Source.ToArray();
            var value = JsonReader<char>.UnescapeUtf16(escapedUtf16Source, escapedCharSize);
            s_utf16StringCache.TryAdd(buffer, value);
            return value;
        }

        #endregion
    }
}
