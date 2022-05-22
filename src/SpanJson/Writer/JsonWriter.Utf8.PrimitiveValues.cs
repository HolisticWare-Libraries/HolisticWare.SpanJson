﻿namespace SpanJson
{
    using System;
    using System.Buffers.Text;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using CuteAnt;
    using SpanJson.Helpers;
    using SpanJson.Internal;
    using SpanJson.Internal.DoubleConversion;

    partial struct JsonWriter<TSymbol>
    {
        #region -- Signed Number --

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteUtf8SByte(sbyte value)
        {
            WriteUtf8Int64Internal(value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteUtf8Int16(short value)
        {
            WriteUtf8Int64Internal(value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteUtf8Int32(int value)
        {
            WriteUtf8Int64Internal(value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteUtf8Int64(long value)
        {
            WriteUtf8Int64Internal(value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void WriteUtf8Int64Internal(long value)
        {
            if (value < 0L)
            {
                ref var pos = ref _pos;
                EnsureUnsafe(pos, 1);

                ref byte pinnableAddr = ref Utf8PinnableAddress;
                Unsafe.AddByteOffset(ref pinnableAddr, (IntPtr)pos++) = (byte)'-';

                value = unchecked(-value);
            }

            WriteUtf8UInt64Internal((ulong)value);
        }

        #endregion

        #region -- Unsigned Number --

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void WriteUtf8UInt64Internal(ulong value)
        {
            ref var pos = ref _pos;

            if (value < 10ul)
            {
                EnsureUnsafe(pos, 1);

                Unsafe.AddByteOffset(ref Utf8PinnableAddress, (IntPtr)pos++) = (byte)('0' + value);
                return;
            }

            var digits = FormatterUtils.CountDigits(value);

            EnsureUnsafe(pos, digits);
            ref byte pinnableAddr = ref Utf8PinnableAddress;

            var offset = (IntPtr)pos;
            for (var i = digits - 1; i >= 0; i--)
            {
                var temp = '0' + value;
                value /= 10ul;
                Unsafe.AddByteOffset(ref pinnableAddr, offset + i) = (byte)(temp - value * 10ul);
            }

            pos += digits;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteUtf8Byte(byte value)
        {
            WriteUtf8UInt64Internal(value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteUtf8UInt16(ushort value)
        {
            WriteUtf8UInt64Internal(value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteUtf8UInt32(uint value)
        {
            WriteUtf8UInt64Internal(value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteUtf8UInt64(ulong value)
        {
            WriteUtf8UInt64Internal(value);
        }

        #endregion

        #region -- Float / Double --

        public void WriteUtf8Single(float value)
        {
#if NETSTANDARD2_0 || NET471 || NET451
            if (float.IsNaN(value) || float.IsInfinity(value))
#else
            if (!float.IsFinite(value))
#endif
            {
                ThrowHelper.ThrowArgumentException_InvalidFloatValueForJson();
            }

            var buffer = TinyMemoryPool<byte>.GetBuffer();
            var count = DoubleToStringConverter.GetBytes(ref buffer, 0, value);

            ref var pos = ref _pos;
            EnsureUnsafe(pos, count);
            UnsafeMemory.WriteRaw(ref Utf8PinnableAddress, ref buffer[0], count, ref pos);
        }

        public void WriteUtf8Double(double value)
        {
#if NETSTANDARD2_0 || NET471 || NET451
            if (double.IsNaN(value) || double.IsInfinity(value))
#else
            if (!double.IsFinite(value))
#endif
            {
                ThrowHelper.ThrowArgumentException_InvalidDoubleValueForJson();
            }

            var buffer = TinyMemoryPool<byte>.GetBuffer();
            var count = DoubleToStringConverter.GetBytes(ref buffer, 0, value);

            ref var pos = ref _pos;
            EnsureUnsafe(pos, count);
            UnsafeMemory.WriteRaw(ref Utf8PinnableAddress, ref buffer[0], count, ref pos);
        }

        #endregion

        #region -- Decimal --

        public void WriteUtf8Decimal(decimal value)
        {
            ref var pos = ref _pos;
            EnsureUnsafe(pos, JsonSharedConstant.MaximumFormatDecimalLength);
            var result = Utf8Formatter.TryFormat(value, Utf8FreeSpan, out int bytesWritten);
            Debug.Assert(result);
            pos += bytesWritten;
        }

        #endregion

        #region -- Char --

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteUtf8Char(char value)
        {
            WriteUtf8Char(value, JsonEscapeHandling.Default);
        }

        public void WriteUtf8Char(char value, JsonEscapeHandling escapeHandling)
        {
            ref var pos = ref _pos;
            const int size = 8; // 1-6 chars + two JsonUtf8Constant.DoubleQuote
            EnsureUnsafe(pos, size);

            ref byte pinnableAddr = ref Utf8PinnableAddress;

            WriteUtf8DoubleQuote(ref pinnableAddr, ref pos);

            if (EscapingHelper.NeedsEscaping(value, escapeHandling))
            {
                EscapingHelper.EscapeChar(escapeHandling, ref pinnableAddr, value, ref pos);
            }
            else if (value < 0x80)
            {
                Unsafe.AddByteOffset(ref pinnableAddr, (IntPtr)pos++) = (byte)value;
            }
            else
            {
                unsafe
                {
                    fixed (byte* bytesPtr = &Unsafe.AddByteOffset(ref pinnableAddr, (IntPtr)pos))
                    {
                        pos += TextEncodings.UTF8NoBOM.GetBytes(&value, 1, bytesPtr, FreeCapacity);
                    }
                }
            }

            WriteUtf8DoubleQuote(ref pinnableAddr, ref pos);
        }

        #endregion

        #region -- DateTime --

        public void WriteUtf8DateTime(DateTime value)
        {
            ref var pos = ref _pos;
            const int dtSize = JsonSharedConstant.MaxDateTimeLength; // Form o + two JsonUtf8Constant.DoubleQuote
            EnsureUnsafe(pos, dtSize);

            ref byte pinnableAddr = ref Utf8PinnableAddress;

            WriteUtf8DoubleQuote(ref pinnableAddr, ref pos);

            Span<byte> tempSpan = stackalloc byte[JsonSharedConstant.MaximumFormatDateTimeOffsetLength];
            bool result = DateTimeFormatter.TryFormat(value, tempSpan, out int bytesWritten);
            Debug.Assert(result);
            DateTimeFormatter.TrimDateTimeOffset(tempSpan.Slice(0, bytesWritten), out bytesWritten);

            tempSpan.Slice(0, bytesWritten).CopyTo(Utf8FreeSpan);
            pos += bytesWritten;

            WriteUtf8DoubleQuote(ref pinnableAddr, ref pos);
        }

        #endregion

        #region -- DateTimeOffset --

        public void WriteUtf8DateTimeOffset(DateTimeOffset value)
        {
            ref var pos = ref _pos;
            const int dtSize = JsonSharedConstant.MaxDateTimeOffsetLength; // Form o + two JsonUtf8Constant.DoubleQuote
            EnsureUnsafe(pos, dtSize);

            ref byte pinnableAddr = ref Utf8PinnableAddress;

            WriteUtf8DoubleQuote(ref pinnableAddr, ref pos);

            Span<byte> tempSpan = stackalloc byte[JsonSharedConstant.MaximumFormatDateTimeOffsetLength];
            bool result = DateTimeFormatter.TryFormat(value, tempSpan, out int bytesWritten);
            Debug.Assert(result);
            DateTimeFormatter.TrimDateTimeOffset(tempSpan.Slice(0, bytesWritten), out bytesWritten);

            tempSpan.Slice(0, bytesWritten).CopyTo(Utf8FreeSpan);
            pos += bytesWritten;

            WriteUtf8DoubleQuote(ref pinnableAddr, ref pos);
        }

        #endregion

        #region -- TimeSpan --

        public void WriteUtf8TimeSpan(TimeSpan value)
        {
            ref var pos = ref _pos;
            const int tsSize = JsonSharedConstant.MaxTimeSpanLength; // Form o + two JsonUtf8Constant.DoubleQuote
            EnsureUnsafe(pos, tsSize);

            ref byte pinnableAddr = ref Utf8PinnableAddress;

            WriteUtf8DoubleQuote(ref pinnableAddr, ref pos);
            Utf8Formatter.TryFormat(value, Utf8FreeSpan, out var bytesWritten);
            pos += bytesWritten;
            WriteUtf8DoubleQuote(ref pinnableAddr, ref pos);
        }

        #endregion

        #region -- Guid --

        public void WriteUtf8Guid(Guid value)
        {
            ref var pos = ref _pos;
            const int guidSize = JsonSharedConstant.MaxGuidLength; // Format D + two JsonUtf8Constant.DoubleQuote;
            EnsureUnsafe(pos, guidSize);

            ref byte pinnableAddr = ref Utf8PinnableAddress;

            WriteUtf8DoubleQuote(ref pinnableAddr, ref pos);
            new GuidBits(ref value).Write(ref pinnableAddr, ref pos); // len = 36
            WriteUtf8DoubleQuote(ref pinnableAddr, ref pos);
        }

        #endregion

        #region -- CombGuid --

        public void WriteUtf8CombGuid(CombGuid value)
        {
            ref var pos = ref _pos;
            const int guidSize = JsonSharedConstant.MaxGuidLength; // Format D + two JsonUtf8Constant.DoubleQuote;
            EnsureUnsafe(pos, guidSize);

            ref byte pinnableAddr = ref Utf8PinnableAddress;

            WriteUtf8DoubleQuote(ref pinnableAddr, ref pos);
#if NET || NETCOREAPP || NETSTANDARD2_1_OR_GREATER
            value.TryFormat(Utf8FreeSpan, CombGuidFormatStringType.Comb32Digits, out int bytesWritten);
            pos += bytesWritten;
#else
            Span<byte> tempSpan = stackalloc byte[32];
            var bytesWritten = TextEncodings.Utf8.GetBytes(value.ToString(CombGuidFormatStringType.Comb32Digits).AsSpan(), tempSpan);
            Debug.Assert(bytesWritten == tempSpan.Length);
            UnsafeMemory.WriteRawBytes(ref pinnableAddr, ref MemoryMarshal.GetReference(tempSpan), bytesWritten, ref pos);
#endif
            //new GuidBits(ref value).Write(ref pinnableAddr, ref pos); // len = 32
            WriteUtf8DoubleQuote(ref pinnableAddr, ref pos);
        }

        #endregion

        #region -- Version --

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteUtf8Version(Version value)
        {
#if NETSTANDARD2_0 || NET471 || NET451
            WriteUtf8String(value.ToString());
#else
            ref var pos = ref _pos;
            EnsureUnsafe(pos, JsonSharedConstant.MaxVersionLength);

            ref byte pinnableAddr = ref Utf8PinnableAddress;

            WriteUtf8DoubleQuote(ref pinnableAddr, ref pos);
            Span<char> tempSpan = TinyMemoryPool<char>.GetBuffer();
            var result = value.TryFormat(tempSpan, out var charsWritten);
            Debug.Assert(result);
            pos += TextEncodings.UTF8NoBOM.GetBytes(tempSpan.Slice(0, charsWritten), Utf8FreeSpan);
            WriteUtf8DoubleQuote(ref pinnableAddr, ref pos);
#endif
        }

        #endregion

        #region -- Uri --

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteUtf8Uri(Uri value)
        {
            WriteUtf8String(value.OriginalString); // Uri does not implement ISpanFormattable
        }

        #endregion
    }
}
