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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte[] GetEscapedPropertyNameSection(in ReadOnlySpan<byte> utf8Value, JsonEscapeHandling escapeHandling, JavaScriptEncoder encoder)
        {
            int idx = EscapingHelper.NeedsEscaping(utf8Value, escapeHandling, encoder);

            if (idx != -1)
            {
                return GetEscapedPropertyNameSection(utf8Value, escapeHandling, idx, encoder);
            }
            else
            {
                return GetPropertyNameSection(utf8Value);
            }
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

            if (valueArray is not null)
            {
                ArrayPool<byte>.Shared.Return(valueArray);
            }

            return escapedString;
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

            if (valueArray != null)
            {
                ArrayPool<byte>.Shared.Return(valueArray);
            }

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
