namespace SpanJson
{
    using System;
using System.Buffers;
using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Text.Encodings.Web;
    using SpanJson.Internal;

    partial struct JsonWriter<TSymbol>
    {
        public void WriteUtf16String(in JsonEncodedText value)
        {
            ref var pos = ref _pos;
            var utf16Text = value.ToString();
            EnsureUnsafe(pos, utf16Text.Length + 3);

            ref char pinnableAddr = ref Utf16PinnableAddress;
            WriteUtf16DoubleQuote(ref pinnableAddr, ref pos);
            utf16Text.AsSpan().CopyTo(Utf16FreeSpan);
            pos += utf16Text.Length;
            WriteUtf16DoubleQuote(ref pinnableAddr, ref pos);
        }

        public void WriteUtf16String(string? value)
        {
            WriteUtf16StringEscapeValue(value.AsSpan(), false);
        }

        public void WriteUtf16String(in ReadOnlySpan<char> value)
        {
            WriteUtf16StringEscapeValue(value, false);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteUtf16String(string? value, JsonEscapeHandling escapeHandling, JavaScriptEncoder? encoder = null)
        {
            WriteUtf16String(value.AsSpan(), escapeHandling, encoder);
        }

        public void WriteUtf16String(in ReadOnlySpan<char> value, JsonEscapeHandling escapeHandling, JavaScriptEncoder? encoder = null)
        {
            switch (escapeHandling)
            {
                case JsonEscapeHandling.EscapeNonAscii:
                    WriteUtf16StringEscapeNonAscii(value, false, encoder);
                    break;

                case JsonEscapeHandling.EscapeHtml:
                    WriteUtf16StringEscapeHtmlValue(value, false);
                    break;

                case JsonEscapeHandling.Default:
                default:
                    WriteUtf16StringEscapeValue(value, false);
                    break;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void WriteUtf16StringEscapeValue(in ReadOnlySpan<char> value, bool withNameSeparator)
        {
            ref var pos = ref _pos;
            var valueLength = value.Length;
            uint nValueLength = (uint)valueLength;
            EnsureUnsafe(pos, valueLength + 12); // assume that a fully escaped char fits too (5 * 2 + two double quotes)

            ref char pinnableAddr = ref Utf16PinnableAddress;

            WriteUtf16DoubleQuote(ref pinnableAddr, ref pos);

            var consumed = 0;
            ref char utf16Source = ref MemoryMarshal.GetReference(value);
            while ((uint)consumed < nValueLength)
            {
                char val = Unsafe.Add(ref utf16Source, consumed);
                if (EscapingHelper.Default.NeedsEscaping(val))
                {
                    EscapingHelper.EscapeNextChars(JsonEscapeHandling.Default, ref utf16Source, nValueLength, val, ref pinnableAddr, ref consumed, ref pos);
                    var remaining = 10 + valueLength - consumed; // make sure that all characters and an extra 5 for a full escape still fit
                    if ((uint)remaining >= (uint)(_capacity - pos))
                    {
                        CheckAndResizeBuffer(pos, remaining);
                        pinnableAddr = ref Utf16PinnableAddress;
                    }
                }
                else
                {
                    Unsafe.Add(ref pinnableAddr, pos++) = val;
                }
                consumed++;
            }

            WriteUtf16DoubleQuote(ref pinnableAddr, ref pos);

            if (withNameSeparator) { WriteUtf16NameSeparator(ref pinnableAddr, ref pos); }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private void WriteUtf16StringEscapeHtmlValue(in ReadOnlySpan<char> value, bool withNameSeparator)
        {
            ref var pos = ref _pos;
            var valueLength = value.Length;
            uint nValueLength = (uint)valueLength;
            EnsureUnsafe(pos, valueLength + 12); // assume that a fully escaped char fits too (5 * 2 + two double quotes)

            ref char pinnableAddr = ref Utf16PinnableAddress;

            WriteUtf16DoubleQuote(ref pinnableAddr, ref pos);

            var consumed = 0;
            ref char utf16Source = ref MemoryMarshal.GetReference(value);
            while ((uint)consumed < nValueLength)
            {
                char val = Unsafe.Add(ref utf16Source, consumed);
                if (EscapingHelper.Html.NeedsEscaping(val))
                {
                    EscapingHelper.EscapeNextChars(JsonEscapeHandling.EscapeHtml, ref utf16Source, nValueLength, val, ref pinnableAddr, ref consumed, ref pos);
                    var remaining = 10 + valueLength - consumed; // make sure that all characters and an extra 5 for a full escape still fit
                    if ((uint)remaining >= (uint)(_capacity - pos))
                    {
                        CheckAndResizeBuffer(pos, remaining);
                        pinnableAddr = ref Utf16PinnableAddress;
                    }
                }
                else
                {
                    Unsafe.Add(ref pinnableAddr, pos++) = val;
                }
                consumed++;
            }

            WriteUtf16DoubleQuote(ref pinnableAddr, ref pos);

            if (withNameSeparator) { WriteUtf16NameSeparator(ref pinnableAddr, ref pos); }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private void WriteUtf16StringEscapeNonAscii(in ReadOnlySpan<char> value, bool withNameSeparator, JavaScriptEncoder? encoder)
        {
            int firstEscapeIndex = EscapingHelper.NonAscii.NeedsEscaping(value, encoder);

            if ((uint)firstEscapeIndex >= (uint)value.Length) // -1
            {
                WriteUtf16StringMinimized(value, withNameSeparator);
            }
            else
            {
                WriteUtf16StringEscapeNonAsciiValue(value, firstEscapeIndex, withNameSeparator, encoder);
            }
        }

        private void WriteUtf16StringEscapeNonAsciiValue(in ReadOnlySpan<char> value, int firstEscapeIndexVal, bool withNameSeparator, JavaScriptEncoder? encoder)
        {
            Debug.Assert(int.MaxValue / JsonSharedConstant.MaxExpansionFactorWhileEscaping >= value.Length);
            Debug.Assert(firstEscapeIndexVal >= 0 && firstEscapeIndexVal < value.Length);

            char[]? valueArray = null;

            int length = EscapingHelper.GetMaxEscapedLength(value.Length, firstEscapeIndexVal);

            Span<char> escapedValue = (uint)length <= JsonSharedConstant.StackallocCharThresholdU ?
                stackalloc char[JsonSharedConstant.StackallocCharThreshold] :
                (valueArray = ArrayPool<char>.Shared.Rent(length));

            EscapingHelper.NonAscii.EscapeString(value, escapedValue, firstEscapeIndexVal, encoder, out int written);

#if !NETSTANDARD2_0
            WriteUtf16StringMinimized(MemoryMarshal.CreateReadOnlySpan(ref MemoryMarshal.GetReference(escapedValue), written), withNameSeparator);
#else
            unsafe
            {
                WriteUtf16StringMinimized(new ReadOnlySpan<char>(Unsafe.AsPointer(ref MemoryMarshal.GetReference(escapedValue)), written), withNameSeparator);
            }
#endif

            if (valueArray is not null) { ArrayPool<char>.Shared.Return(valueArray); }
        }

        private void WriteUtf16StringMinimized(in ReadOnlySpan<char> escapedValue, bool withNameSeparator)
        {
            ref var pos = ref _pos;
            EnsureUnsafe(pos, escapedValue.Length + 3);

            ref char pinnableAddr = ref Utf16PinnableAddress;
            WriteUtf16DoubleQuote(ref pinnableAddr, ref pos);
            escapedValue.CopyTo(Utf16FreeSpan);
            pos += escapedValue.Length;
            WriteUtf16DoubleQuote(ref pinnableAddr, ref pos);

            if (withNameSeparator) { WriteUtf16NameSeparator(ref pinnableAddr, ref pos); }
        }
    }
}
