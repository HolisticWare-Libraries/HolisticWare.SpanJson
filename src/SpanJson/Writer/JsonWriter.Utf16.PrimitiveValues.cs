﻿namespace SpanJson
{
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.Runtime.CompilerServices;
    using SpanJson.Internal;
    using System.Buffers.Text;
    using System.Runtime.InteropServices;
    using CuteAnt;
    using SpanJson.Helpers;

    partial struct JsonWriter<TSymbol>
    {
        #region -- Signed Number --

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteUtf16SByte(sbyte value)
        {
            WriteUtf16Int64Internal(value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteUtf16Int16(short value)
        {
            WriteUtf16Int64Internal(value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteUtf16Int32(int value)
        {
            WriteUtf16Int64Internal(value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteUtf16Int64(long value)
        {
            WriteUtf16Int64Internal(value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void WriteUtf16Int64Internal(long value)
        {
            if (value < 0)
            {
                ref var pos = ref _pos;
                EnsureUnsafe(pos, 1);

                ref char pinnableAddr = ref Utf16PinnableAddress;
                Unsafe.Add(ref pinnableAddr, pos++) = '-';

                value = unchecked(-value);
            }

            WriteUtf16UInt64Internal((ulong)value);
        }

        #endregion

        #region -- Unsigned Number --

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void WriteUtf16UInt64Internal(ulong value)
        {
            ref var pos = ref _pos;
            if (value < 10ul)
            {
                EnsureUnsafe(pos, 1);

                ref char pinnableAddr0 = ref Utf16PinnableAddress;
                Unsafe.Add(ref pinnableAddr0, pos++) = (char)('0' + value);
                return;
            }

            var digits = FormatterUtils.CountDigits(value);

            EnsureUnsafe(pos, digits);
            ref char pinnableAddr = ref Utf16PinnableAddress;

            for (var i = digits - 1; i >= 0; i--)
            {
                var temp = '0' + value;
                value /= 10ul;
                Unsafe.Add(ref pinnableAddr, pos + i) = (char)(temp - value * 10ul);
            }

            pos += digits;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteUtf16Byte(byte value)
        {
            WriteUtf16UInt64Internal(value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteUtf16UInt16(ushort value)
        {
            WriteUtf16UInt64Internal(value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteUtf16UInt32(uint value)
        {
            WriteUtf16UInt64Internal(value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteUtf16UInt64(ulong value)
        {
            WriteUtf16UInt64Internal(value);
        }

        #endregion

        #region -- Float / Double --

        public void WriteUtf16Single(float value)
        {
            if (!JsonHelpers.IsFinite(value))
            {
                ThrowHelper.ThrowArgumentException_InvalidFloatValueForJson();
            }

            ref var pos = ref _pos;
            EnsureUnsafe(pos, JsonSharedConstant.MaximumFormatSingleLength);

            bool result = TryFormatUtf16Single(value, Utf16FreeSpan, out int written);
            Debug.Assert(result);
            pos += written;
        }

        private static bool TryFormatUtf16Single(float value, Span<char> destination, out int written)
        {
#if NETCOREAPP3_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
            return value.TryFormat(destination, out written, provider: CultureInfo.InvariantCulture);
#else
            const string FormatString = "G9";

            string utf16Text = value.ToString(FormatString, System.Globalization.CultureInfo.InvariantCulture);

            // Copy the value to the destination, if it's large enough.

            if ((uint)utf16Text.Length > (uint)destination.Length)
            {
                written = 0;
                return false;
            }

            utf16Text.AsSpan().CopyTo(destination);
            written = utf16Text.Length;

            return true;
#endif
        }

        public void WriteUtf16Double(double value)
        {
            if (!JsonHelpers.IsFinite(value))
            {
                ThrowHelper.ThrowArgumentException_InvalidDoubleValueForJson();
            }

            ref var pos = ref _pos;
            EnsureUnsafe(pos, JsonSharedConstant.MaximumFormatSingleLength);

            bool result = TryFormatUtf16Double(value, Utf16FreeSpan, out int written);
            Debug.Assert(result);
            pos += written;
        }

        private static bool TryFormatUtf16Double(double value, Span<char> destination, out int written)
        {
#if NETCOREAPP3_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
            return value.TryFormat(destination, out written, provider: CultureInfo.InvariantCulture);
#else
            const string FormatString = "G17";

            string utf16Text = value.ToString(FormatString, CultureInfo.InvariantCulture);

            // Copy the value to the destination, if it's large enough.

            if ((uint)utf16Text.Length > (uint)destination.Length)
            {
                written = 0;
                return false;
            }

            utf16Text.AsSpan().CopyTo(destination);
            written = utf16Text.Length;

            return true;
#endif
        }

        #endregion

        #region -- Decimal --

        public void WriteUtf16Decimal(decimal value)
        {
            ref var pos = ref _pos;
#if NETSTANDARD2_0
            var utf16Text = value.ToString("G", CultureInfo.InvariantCulture);
            var written = utf16Text.Length;
            EnsureUnsafe(pos, written);
            utf16Text.AsSpan().CopyTo(Utf16FreeSpan);
#else
            EnsureUnsafe(pos, JsonSharedConstant.MaximumFormatDecimalLength);
            var result = value.TryFormat(Utf16FreeSpan, out var written, provider: CultureInfo.InvariantCulture);
            Debug.Assert(result);
#endif
            pos += written;
        }

        #endregion

        #region -- Char --

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteUtf16Char(char value)
        {
            WriteUtf16Char(value, JsonEscapeHandling.Default);
        }

        public void WriteUtf16Char(char value, JsonEscapeHandling escapeHandling)
        {
            ref var pos = ref _pos;
            const int size = 8; // 1-6 chars + two JsonUtf16Constant.DoubleQuote
            EnsureUnsafe(pos, size);
            ref char pinnableAddr = ref Utf16PinnableAddress;

            WriteUtf16DoubleQuote(ref pinnableAddr, ref pos);
            if (EscapingHelper.NeedsEscaping(value, escapeHandling))
            {
                EscapingHelper.EscapeChar(escapeHandling, ref pinnableAddr, value, ref pos);
            }
            else
            {
                Unsafe.Add(ref pinnableAddr, pos++) = value;
            }
            WriteUtf16DoubleQuote(ref pinnableAddr, ref pos);
        }

        #endregion

        #region -- DateTime --

        public void WriteUtf16DateTime(DateTime value)
        {
            ref var pos = ref _pos;
            const int dtSize = JsonSharedConstant.MaxDateTimeLength; // Form o + two JsonUtf16Constant.DoubleQuote
            EnsureUnsafe(pos, dtSize);
            ref char pinnableAddr = ref Utf16PinnableAddress;

            WriteUtf16DoubleQuote(ref pinnableAddr, ref pos);

            DateTimeFormatter.WriteDateTimeTrimmed(Utf16FreeSpan, value, out var charsWritten);
            pos += charsWritten;

            WriteUtf16DoubleQuote(ref pinnableAddr, ref pos);
        }

        #endregion

        #region -- DateTimeOffset --

        public void WriteUtf16DateTimeOffset(DateTimeOffset value)
        {
            ref var pos = ref _pos;
            const int dtSize = JsonSharedConstant.MaxDateTimeOffsetLength; // Form o + two JsonUtf16Constant.DoubleQuote
            EnsureUnsafe(pos, dtSize);
            ref char pinnableAddr = ref Utf16PinnableAddress;

            WriteUtf16DoubleQuote(ref pinnableAddr, ref pos);

            DateTimeFormatter.WriteDateTimeOffsetTrimmed(Utf16FreeSpan, value, out var charsWritten);
            pos += charsWritten;

            WriteUtf16DoubleQuote(ref pinnableAddr, ref pos);
        }

        #endregion

        #region -- TimeSpan --

        public void WriteUtf16TimeSpan(TimeSpan value)
        {
            ref var pos = ref _pos;
            const int tsSize = JsonSharedConstant.MaxTimeSpanLength; // Form c + two JsonUtf16Constant.DoubleQuote
            EnsureUnsafe(pos, tsSize);
            ref char pinnableAddr = ref Utf16PinnableAddress;

            WriteUtf16DoubleQuote(ref pinnableAddr, ref pos);
            Span<byte> byteSpan = stackalloc byte[tsSize];
            var result = Utf8Formatter.TryFormat(value, byteSpan, out var bytesWritten);
            Debug.Assert(result);
            ref byte utf8Source = ref MemoryMarshal.GetReference(byteSpan);
            var offset = (IntPtr)0;
            for (var i = 0; i < bytesWritten; i++)
            {
                Unsafe.Add(ref pinnableAddr, pos + i) = (char)Unsafe.AddByteOffset(ref utf8Source, offset + i);
            }

            pos += bytesWritten;
            WriteUtf16DoubleQuote(ref pinnableAddr, ref pos);
        }

        #endregion

        #region -- Guid --

        public void WriteUtf16Guid(Guid value)
        {
            ref var pos = ref _pos;
            const int guidSize = JsonSharedConstant.MaxGuidLength; // Format D + two JsonUtf16Constant.DoubleQuote;
            EnsureUnsafe(pos, guidSize);
            ref char pinnableAddr = ref Utf16PinnableAddress;

            WriteUtf16DoubleQuote(ref pinnableAddr, ref pos);
            new GuidBits(ref value).Write(ref pinnableAddr, ref pos); // len = 36
            WriteUtf16DoubleQuote(ref pinnableAddr, ref pos);
        }

        #endregion

        #region -- CombGuid --

        public void WriteUtf16CombGuid(CombGuid value)
        {
            ref var pos = ref _pos;
            const int guidSize = JsonSharedConstant.MaxGuidLength; // Format D + two JsonUtf16Constant.DoubleQuote;
            EnsureUnsafe(pos, guidSize);
            ref char pinnableAddr = ref Utf16PinnableAddress;

            WriteUtf16DoubleQuote(ref pinnableAddr, ref pos);
#if NET || NETCOREAPP || NETSTANDARD2_1_OR_GREATER
            value.TryFormat(Utf16FreeSpan, CombGuidFormatStringType.Comb32Digits, out int charsWritten);
            Debug.Assert(charsWritten == 32);
            pos += charsWritten;
#else
            value.ToString(CombGuidFormatStringType.Comb32Digits).AsSpan().CopyTo(Utf16FreeSpan);
            pos += 32;
#endif
            WriteUtf16DoubleQuote(ref pinnableAddr, ref pos);
        }

        #endregion

        #region -- Version --

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteUtf16Version(Version value)
        {
            const int versionLength = JsonSharedConstant.MaxVersionLength;
            ref var pos = ref _pos;
            EnsureUnsafe(pos, versionLength);

            ref char pinnableAddr = ref Utf16PinnableAddress;
            WriteUtf16DoubleQuote(ref pinnableAddr, ref pos);
#if NETSTANDARD2_0
            var utf16Text = value.ToString();
            utf16Text.AsSpan().CopyTo(Utf16FreeSpan);
            pos += utf16Text.Length;
#else
            value.TryFormat(Utf16FreeSpan, out var written);
            pos += written;
#endif
            WriteUtf16DoubleQuote(ref pinnableAddr, ref pos);
        }

        #endregion

        #region -- Uri --

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteUtf16Uri(Uri value)
        {
            WriteUtf16String(value.OriginalString); // Uri does not implement ISpanFormattable
        }

        #endregion
    }
}
