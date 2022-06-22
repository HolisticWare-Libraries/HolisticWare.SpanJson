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
        public void WriteUtf8String(in JsonEncodedText value)
        {
            ref var pos = ref _pos;
            var utf8Text = value.EncodedUtf8Bytes;
            EnsureUnsafe(pos, utf8Text.Length + 3);

            ref byte pinnableAddr = ref Utf8PinnableAddress;

            WriteUtf8DoubleQuote(ref pinnableAddr, ref pos);
            UnsafeMemory.WriteRaw(ref pinnableAddr, ref MemoryMarshal.GetReference(utf8Text), utf8Text.Length, ref _pos);
            WriteUtf8DoubleQuote(ref pinnableAddr, ref pos);
        }

        public void WriteUtf8String(string value)
        {
            WriteUtf8StringEscapeValue(value.AsSpan(), false);
        }

        public void WriteUtf8String(in ReadOnlySpan<char> value)
        {
            WriteUtf8StringEscapeValue(value, false);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteUtf8String(string value, JsonEscapeHandling escapeHandling, JavaScriptEncoder encoder = null)
        {
            WriteUtf8String(value.AsSpan(), escapeHandling, encoder);
        }

        public void WriteUtf8String(in ReadOnlySpan<char> value, JsonEscapeHandling escapeHandling, JavaScriptEncoder encoder = null)
        {
            switch (escapeHandling)
            {
                case JsonEscapeHandling.EscapeNonAscii:
                    WriteUtf8StringEscapeNonAscii(value, false, encoder);
                    break;

                case JsonEscapeHandling.EscapeHtml:
                    WriteUtf8StringEscapeHtmlValue(value, false);
                    break;

                case JsonEscapeHandling.Default:
                default:
                    WriteUtf8StringEscapeValue(value, false);
                    break;
            }
        }

        /// <summary>We know that for a pure ascii string all characters will fit if there are no escapes
        /// We make sure that initially the buffer is large enough and an additional fully escaped char fits too
        /// After each escape we make sure that all remaining ascii chars and extra fully escaped char fit
        /// For UTF8 encoded bytes we make sure that the 5 for the fully escaped value and 4 for the utf fit
        /// That's all done to make sure we don't have resizing in the fast path (the ascii case).</summary>
        private void WriteUtf8StringEscapeValue(in ReadOnlySpan<char> value, bool withNameSeparator)
        {
            ref var pos = ref _pos;
            var valueLength = value.Length;
            uint nValueLength = (uint)valueLength;
            EnsureUnsafe(pos, TextEncodings.UTF8NoBOM.GetMaxByteCount(valueLength) + 12); // assume that a fully escaped char fits too + 2 double quotes

            ref byte pinnableAddr = ref Utf8PinnableAddress;

            WriteUtf8DoubleQuote(ref pinnableAddr, ref pos);

            var index = 0;
            var from = 0;
            ref char utf16Source = ref MemoryMarshal.GetReference(value);
            while ((uint)index < nValueLength)
            {
                var val = Unsafe.Add(ref utf16Source, index);
                if (EscapingHelper.Default.NeedsEscaping(val))
                {
                    var length = index - from;
                    pos += TextEncodings.Utf8.GetBytes(value.Slice(from, length), Utf8FreeSpan);
                    EscapingHelper.EscapeNextChars(JsonEscapeHandling.Default, ref utf16Source, nValueLength, val, ref pinnableAddr, ref index, ref pos);
                    index++;
                    var remaining = 10 + valueLength - index; // make sure that all characters and an extra 5 for a full escape still fit
                    if ((uint)remaining >= (uint)(_capacity - pos))
                    {
                        CheckAndResizeBuffer(pos, remaining);
                        pinnableAddr = ref Utf8PinnableAddress;
                    }
                    from = index;
                }
                else
                {
                    index++;
                }
            }

            // Still chars to encode
            if ((uint)from < nValueLength)
            {
                pos += TextEncodings.Utf8.GetBytes(value.Slice(from), Utf8FreeSpan);
            }

            WriteUtf8DoubleQuote(ref pinnableAddr, ref pos);

            if (withNameSeparator) { WriteUtf8NameSeparator(ref pinnableAddr, ref pos); }
        }

        private void WriteUtf8StringEscapeHtmlValue(in ReadOnlySpan<char> value, bool withNameSeparator)
        {
            ref var pos = ref _pos;
            var valueLength = value.Length;
            uint nValueLength = (uint)valueLength;
            EnsureUnsafe(pos, TextEncodings.UTF8NoBOM.GetMaxByteCount(valueLength) + 12); // assume that a fully escaped char fits too + 2 double quotes

            ref byte pinnableAddr = ref Utf8PinnableAddress;

            WriteUtf8DoubleQuote(ref pinnableAddr, ref pos);

            var index = 0;
            var from = 0;
            ref char utf16Source = ref MemoryMarshal.GetReference(value);
            while ((uint)index < nValueLength)
            {
                var val = Unsafe.Add(ref utf16Source, index);
                if (EscapingHelper.Html.NeedsEscaping(val))
                {
                    var length = index - from;
                    pos += TextEncodings.Utf8.GetBytes(value.Slice(from, length), Utf8FreeSpan);
                    EscapingHelper.EscapeNextChars(JsonEscapeHandling.EscapeHtml, ref utf16Source, nValueLength, val, ref pinnableAddr, ref index, ref pos);
                    index++;
                    var remaining = 10 + valueLength - index; // make sure that all characters and an extra 5 for a full escape still fit
                    if ((uint)remaining >= (uint)(_capacity - pos))
                    {
                        CheckAndResizeBuffer(pos, remaining);
                        pinnableAddr = ref Utf8PinnableAddress;
                    }
                    from = index;
                }
                else
                {
                    index++;
                }
            }

            // Still chars to encode
            if ((uint)from < nValueLength)
            {
                pos += TextEncodings.Utf8.GetBytes(value.Slice(from), Utf8FreeSpan);
            }

            WriteUtf8DoubleQuote(ref pinnableAddr, ref pos);

            if (withNameSeparator) { WriteUtf8NameSeparator(ref pinnableAddr, ref pos); }
        }

        private void WriteUtf8StringEscapeNonAscii(in ReadOnlySpan<char> value, bool withNameSeparator, JavaScriptEncoder encoder)
        {
            int firstEscapeIndex = EscapingHelper.NonAscii.NeedsEscaping(value, encoder);

            if ((uint)firstEscapeIndex >= (uint)value.Length) // -1
            {
                WriteUtf8StringMinimized(value, withNameSeparator);
            }
            else
            {
                WriteUtf8StringEscapeNonAsciiValue(value, firstEscapeIndex, withNameSeparator, encoder);
            }
        }

        private void WriteUtf8StringEscapeNonAsciiValue(in ReadOnlySpan<char> value, int firstEscapeIndexVal, bool withNameSeparator, JavaScriptEncoder encoder)
        {
            Debug.Assert(int.MaxValue / JsonSharedConstant.MaxExpansionFactorWhileEscaping >= value.Length);
            Debug.Assert(firstEscapeIndexVal >= 0 && firstEscapeIndexVal < value.Length);

            char[] valueArray = null;

            int length = EscapingHelper.GetMaxEscapedLength(value.Length, firstEscapeIndexVal);

            Span<char> escapedValue = (uint)length <= JsonSharedConstant.StackallocCharThresholdU ?
                stackalloc char[JsonSharedConstant.StackallocCharThreshold] :
                (valueArray = ArrayPool<char>.Shared.Rent(length));

            EscapingHelper.NonAscii.EscapeString(value, escapedValue, firstEscapeIndexVal, encoder, out int written);

#if !NETSTANDARD2_0
            WriteUtf8StringMinimized(MemoryMarshal.CreateReadOnlySpan(ref MemoryMarshal.GetReference(escapedValue), written), withNameSeparator);
#else
            unsafe
            {
                WriteUtf8StringMinimized(new ReadOnlySpan<char>(Unsafe.AsPointer(ref MemoryMarshal.GetReference(escapedValue)), written), withNameSeparator);
            }
#endif

            if (valueArray is not null) { ArrayPool<char>.Shared.Return(valueArray); }
        }

        private void WriteUtf8StringMinimized(in ReadOnlySpan<char> escapedValue, bool withNameSeparator)
        {
            Debug.Assert(escapedValue.Length < (int.MaxValue / JsonSharedConstant.MaxExpansionFactorWhileTranscoding) - 3);

            // All ASCII, 2 quotes => escapedValue.Length + 2
            // Optionally, 1 name separator, and up to 3x growth when transcoding
            int maxRequired = (escapedValue.Length * JsonSharedConstant.MaxExpansionFactorWhileTranscoding) + 3;

            ref var pos = ref _pos;
            EnsureUnsafe(pos, maxRequired);

            ref byte output = ref Utf8PinnableAddress;

            WriteUtf8DoubleQuote(ref output, ref pos);

            TranscodeAndWrite(escapedValue, ref output, FreeCapacity, ref pos);

            WriteUtf8DoubleQuote(ref output, ref pos);

            if (withNameSeparator) { WriteUtf8NameSeparator(ref output, ref pos); }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void TranscodeAndWrite(in ReadOnlySpan<char> escapedPropertyName, ref byte output, int freeCapacity, ref int pos)
        {
            OperationStatus status = TextEncodings.Utf16.ToUtf8(escapedPropertyName,
                ref Unsafe.Add(ref output, pos), freeCapacity, out int consumed, out int written);
            Debug.Assert(status == OperationStatus.Done);
            Debug.Assert(consumed == escapedPropertyName.Length);
            pos += written;
        }
    }
}
