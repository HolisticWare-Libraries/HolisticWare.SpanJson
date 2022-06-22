// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Buffers;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text.Encodings.Web;

namespace SpanJson.Internal
{
    static partial class JsonHelpers
    {
        #region -- GetEncodedText --

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static JsonEncodedText GetEncodedText(string text, JsonEscapeHandling escapeHandling, JavaScriptEncoder encoder = null)
        {
            switch (escapeHandling)
            {
                case JsonEscapeHandling.EscapeNonAscii:
                    return EscapingHelper.NonAscii.GetEncodedText(text, encoder);
                case JsonEscapeHandling.EscapeHtml:
                    return EscapingHelper.Html.GetEncodedText(text);
                case JsonEscapeHandling.Default:
                default:
                    return EscapingHelper.Default.GetEncodedText(text);
            }
        }

        #endregion

        public static byte[] EscapeValue(in ReadOnlySpan<byte> utf8Value, JsonEscapeHandling escapeHandling = JsonEscapeHandling.Default, JavaScriptEncoder encoder = null)
        {
            int idx = EscapingHelper.NeedsEscaping(utf8Value, escapeHandling, encoder);

            if ((uint)idx >= (uint)utf8Value.Length) // -1
            {
                return utf8Value.ToArray();
            }

            return EscapeValue(utf8Value, escapeHandling, idx, encoder);
        }

        public static byte[] EscapeValue(in ReadOnlySpan<byte> utf8Value, JsonEscapeHandling escapeHandling, int firstEscapeIndexVal, JavaScriptEncoder encoder)
        {
            Debug.Assert(int.MaxValue / JsonSharedConstant.MaxExpansionFactorWhileEscaping >= utf8Value.Length);
            Debug.Assert(firstEscapeIndexVal >= 0 && firstEscapeIndexVal < utf8Value.Length);

            byte[] valueArray = null;

            int length = EscapingHelper.GetMaxEscapedLength(utf8Value.Length, firstEscapeIndexVal);

            Span<byte> escapedValue = (uint)length <= JsonSharedConstant.StackallocByteThresholdU ?
                stackalloc byte[JsonSharedConstant.StackallocByteThreshold] :
                (valueArray = ArrayPool<byte>.Shared.Rent(length));

            EscapingHelper.EscapeString(utf8Value, escapedValue, escapeHandling, firstEscapeIndexVal, encoder, out int written);

            byte[] escapedString = escapedValue.Slice(0, written).ToArray();

            if (valueArray is not null) { ArrayPool<byte>.Shared.Return(valueArray); }

            return escapedString;
        }

        public static string EscapeValue(string input, JsonEscapeHandling escapeHandling = JsonEscapeHandling.Default, JavaScriptEncoder encoder = null)
        {
            if (string.IsNullOrEmpty(input)) { return input; }

#if NETSTANDARD2_0
            ReadOnlySpan<char> utf16Value = input.AsSpan();
#else
            ReadOnlySpan<char> utf16Value = input;
#endif
            var firstEscapeIndex = EscapingHelper.NeedsEscaping(utf16Value, escapeHandling, encoder);
            if ((uint)firstEscapeIndex >= (uint)utf16Value.Length) // -1
            {
                return input;
            }

            return EscapeValue(utf16Value, escapeHandling, firstEscapeIndex, encoder);
        }

        public static string EscapeValue(in ReadOnlySpan<char> utf16Value, JsonEscapeHandling escapeHandling = JsonEscapeHandling.Default, JavaScriptEncoder encoder = null)
        {
            if (utf16Value.IsEmpty) { return string.Empty; }

            var firstEscapeIndex = EscapingHelper.NeedsEscaping(utf16Value, escapeHandling, encoder);
            if ((uint)firstEscapeIndex >= (uint)utf16Value.Length) // -1
            {
                return utf16Value.ToString();
            }

            return EscapeValue(utf16Value, escapeHandling, firstEscapeIndex, encoder);
        }

        public static string EscapeValue(in ReadOnlySpan<char> utf16Value, JsonEscapeHandling escapeHandling, int firstEscapeIndexVal, JavaScriptEncoder encoder)
        {
            Debug.Assert(int.MaxValue / JsonSharedConstant.MaxExpansionFactorWhileEscaping >= utf16Value.Length);
            Debug.Assert(firstEscapeIndexVal >= 0 && firstEscapeIndexVal < utf16Value.Length);

            char[] tempArray = null;

            var length = EscapingHelper.GetMaxEscapedLength(utf16Value.Length, firstEscapeIndexVal);

            try
            {
                Span<char> escapedValue = (uint)length <= JsonSharedConstant.StackallocCharThresholdU ?
                    stackalloc char[JsonSharedConstant.StackallocCharThreshold] :
                    (tempArray = ArrayPool<char>.Shared.Rent(length));

                EscapingHelper.EscapeString(utf16Value, escapedValue, escapeHandling, firstEscapeIndexVal, encoder, out int written);

                return escapedValue.Slice(0, written).ToString();
            }
            finally
            {
                if (tempArray is not null) { ArrayPool<char>.Shared.Return(tempArray); }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte[] GetEscapedPropertyNameSection(in ReadOnlySpan<byte> utf8Value, JsonEscapeHandling escapeHandling, JavaScriptEncoder encoder)
        {
            int idx = EscapingHelper.NeedsEscaping(utf8Value, escapeHandling, encoder);

            if ((uint)idx >= (uint)utf8Value.Length)
            {
                return GetPropertyNameSection(utf8Value);
            }

            return GetEscapedPropertyNameSection(utf8Value, escapeHandling, idx, encoder);
        }

        private static byte[] GetEscapedPropertyNameSection(in ReadOnlySpan<byte> utf8Value, JsonEscapeHandling escapeHandling, int firstEscapeIndexVal, JavaScriptEncoder encoder)
        {
            Debug.Assert(int.MaxValue / JsonSharedConstant.MaxExpansionFactorWhileEscaping >= utf8Value.Length);
            Debug.Assert(firstEscapeIndexVal >= 0 && firstEscapeIndexVal < utf8Value.Length);

            byte[] valueArray = null;

            int length = EscapingHelper.GetMaxEscapedLength(utf8Value.Length, firstEscapeIndexVal);

            Span<byte> escapedValue = (uint)length <= JsonSharedConstant.StackallocByteThresholdU ?
                stackalloc byte[JsonSharedConstant.StackallocByteThreshold] :
                (valueArray = ArrayPool<byte>.Shared.Rent(length));

            EscapingHelper.EscapeString(utf8Value, escapedValue, escapeHandling, firstEscapeIndexVal, encoder, out int written);

            byte[] propertySection = GetPropertyNameSection(escapedValue.Slice(0, written));

            if (valueArray is not null) { ArrayPool<byte>.Shared.Return(valueArray); }

            return propertySection;
        }

        private static byte[] GetPropertyNameSection(in ReadOnlySpan<byte> utf8Value)
        {
            int length = utf8Value.Length;
            byte[] propertySection = new byte[length + 3];

            propertySection[0] = JsonUtf8Constant.DoubleQuote;
            utf8Value.CopyTo(propertySection.AsSpan(1, length));
            propertySection[++length] = JsonUtf8Constant.DoubleQuote;
            propertySection[++length] = JsonUtf8Constant.KeyValueSeperator;

            return propertySection;
        }
    }
}
