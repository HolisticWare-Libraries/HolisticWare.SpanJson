namespace SpanJson.Helpers
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using SpanJson.Internal;

    public static partial class DateTimeFormatter
    {
        public static bool TryFormat(DateTimeOffset value, Span<byte> destination, out int bytesWritten)
        {
            const uint MinimumBytesNeeded = JsonSharedConstant.MaximumFormatDateTimeOffsetLength;
            if ((uint)destination.Length < MinimumBytesNeeded)
            {
                bytesWritten = 0;
                return false;
            }

            ref var b = ref MemoryMarshal.GetReference(destination);

            WriteDateAndTime(value.DateTime, ref b);

            if (value.Offset == TimeSpan.Zero)
            {
                bytesWritten = 28;
                Unsafe.AddByteOffset(ref b, (IntPtr)27) = JsonUtf8Constant.UtcOffsetToken;
            }
            else
            {
                bytesWritten = JsonSharedConstant.MaximumFormatDateTimeOffsetLength;
                WriteTimeZone(value.Offset, ref b);
            }

            return true;
        }

        public static bool TryFormat(DateTime value, Span<byte> destination, out int bytesWritten)
        {
            const uint MinimumBytesNeeded = JsonSharedConstant.MaximumFormatDateTimeOffsetLength;

            if ((uint)destination.Length < MinimumBytesNeeded)
            {
                bytesWritten = 0;
                return false;
            }

            bytesWritten = JsonSharedConstant.MaximumFormatDateTimeLength;

            ref var b = ref MemoryMarshal.GetReference(destination);

            WriteDateAndTime(value, ref b);

            var kind = value.Kind;
            if (kind == DateTimeKind.Local)
            {
                bytesWritten = JsonSharedConstant.MaximumFormatDateTimeOffsetLength;
                WriteTimeZone(TimeZoneInfo.Local.GetUtcOffset(value), ref b);
            }
            else if (kind == DateTimeKind.Utc)
            {
                bytesWritten = 28;
                Unsafe.AddByteOffset(ref b, (IntPtr)27) = JsonUtf8Constant.UtcOffsetToken;
            }

            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void WriteDateAndTime(DateTime value, ref byte b)
        {
            IntPtr offset = (IntPtr)0;
            WriteFourDigits((uint)value.Year, ref b, offset);
            Unsafe.AddByteOffset(ref b, offset + 4) = JsonUtf8Constant.Minus;

            WriteTwoDigits((uint)value.Month, ref b, offset + 5);
            Unsafe.AddByteOffset(ref b, offset + 7) = JsonUtf8Constant.Minus;

            WriteTwoDigits((uint)value.Day, ref b, offset + 8);
            Unsafe.AddByteOffset(ref b, offset + 10) = JsonUtf8Constant.TimePrefix;

            WriteTwoDigits((uint)value.Hour, ref b, offset + 11);
            Unsafe.AddByteOffset(ref b, offset + 13) = JsonUtf8Constant.Colon;

            WriteTwoDigits((uint)value.Minute, ref b, offset + 14);
            Unsafe.AddByteOffset(ref b, offset + 16) = JsonUtf8Constant.Colon;

            WriteTwoDigits((uint)value.Second, ref b, offset + 17);
            Unsafe.AddByteOffset(ref b, offset + 19) = JsonUtf8Constant.Period;

            WriteDigits((uint)((ulong)value.Ticks % (ulong)TimeSpan.TicksPerSecond), ref b, offset + 20);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void WriteTimeZone(TimeSpan offset, ref byte b)
        {
            byte sign;
            if (offset < default(TimeSpan) /* a "const" version of TimeSpan.Zero */)
            {
                sign = JsonUtf8Constant.Minus;
                offset = TimeSpan.FromTicks(-offset.Ticks);
            }
            else
            {
                sign = JsonUtf8Constant.Plus;
            }

            // Writing the value backward allows the JIT to optimize by
            // performing a single bounds check against buffer.

            IntPtr byteOffset = (IntPtr)27;
            Unsafe.AddByteOffset(ref b, byteOffset) = sign; // 27
            WriteTwoDigits((uint)offset.Hours, ref b, byteOffset + 1); // 28
            Unsafe.AddByteOffset(ref b, byteOffset + 3) = JsonUtf8Constant.Colon; // 30
            WriteTwoDigits((uint)offset.Minutes, ref b, byteOffset + 4); // 31
        }

        /// <summary>Writes a value [ 0000 .. 9999 ] to the buffer starting at the specified offset.
        /// This method performs best when the starting index is a constant literal.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void WriteFourDigits(uint value, ref byte b, IntPtr offset)
        {
            Debug.Assert(/*0 <= value && */value <= 9999);

            var temp = '0' + value;
            value /= 10u;
            Unsafe.AddByteOffset(ref b, offset + 3) = (byte)(temp - value * 10u);

            temp = '0' + value;
            value /= 10u;
            Unsafe.AddByteOffset(ref b, offset + 2) = (byte)(temp - value * 10u);

            temp = '0' + value;
            value /= 10u;
            Unsafe.AddByteOffset(ref b, offset + 1) = (byte)(temp - value * 10u);

            Unsafe.AddByteOffset(ref b, offset) = (byte)('0' + value);
        }

        /// <summary>Writes a value [ 00 .. 99 ] to the buffer starting at the specified offset.
        /// This method performs best when the starting index is a constant literal.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void WriteTwoDigits(uint value, ref byte b, IntPtr offset)
        {
            Debug.Assert(/*0 <= value && */value <= 99);

            var temp = '0' + value;
            value /= 10u;
            Unsafe.AddByteOffset(ref b, offset + 1) = (byte)(temp - value * 10u);
            Unsafe.AddByteOffset(ref b, offset) = (byte)('0' + value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void WriteDigits(uint value, ref byte b, IntPtr offset)
        {
            // We can mutate the 'value' parameter since it's a copy-by-value local.
            // It'll be used to represent the value left over after each division by 10.

            for (int i = 6/*buffer.Length - 1*/; i >= 1; i--)
            {
                uint temp = '0' + value;
                value /= 10u;
                Unsafe.AddByteOffset(ref b, offset + i) = (byte)(temp - (value * 10u));
            }

            Debug.Assert(value < 10);
            Unsafe.AddByteOffset(ref b, offset) = (byte)('0' + value);
        }

        // Largely based on https://github.com/dotnet/corefx/blob/8135319caa7e457ed61053ca1418313b88057b51/src/System.Text.Json/src/System/Text/Json/Writer/JsonWriterHelper.Date.cs

        public static void WriteDateTimeTrimmed(Span<byte> buffer, DateTime value, out int bytesWritten)
        {
            Span<byte> tempSpan = stackalloc byte[JsonSharedConstant.MaximumFormatDateTimeOffsetLength];
            bool result = TryFormat(value, tempSpan, out bytesWritten);
            Debug.Assert(result);
            TrimDateTimeOffset(tempSpan.Slice(0, bytesWritten), out bytesWritten);
            tempSpan.Slice(0, bytesWritten).CopyTo(buffer);
        }

        public static void WriteDateTimeTrimmed(ref byte outputSpace, ref int pos, DateTime value)
        {
            Span<byte> tempSpan = stackalloc byte[JsonSharedConstant.MaximumFormatDateTimeOffsetLength];
            bool result = TryFormat(value, tempSpan, out var bytesWritten);
            Debug.Assert(result);
            TrimDateTimeOffset(tempSpan.Slice(0, bytesWritten), out bytesWritten);
            UnsafeMemory.WriteRaw(ref outputSpace, ref MemoryMarshal.GetReference(tempSpan), bytesWritten, ref pos);
        }

        public static void WriteDateTimeOffsetTrimmed(Span<byte> buffer, DateTimeOffset value, out int bytesWritten)
        {
            Span<byte> tempSpan = stackalloc byte[JsonSharedConstant.MaximumFormatDateTimeOffsetLength];
            bool result = TryFormat(value, tempSpan, out bytesWritten);
            Debug.Assert(result);
            TrimDateTimeOffset(tempSpan.Slice(0, bytesWritten), out bytesWritten);
            tempSpan.Slice(0, bytesWritten).CopyTo(buffer);
        }

        public static void WriteDateTimeOffsetTrimmed(ref byte outputSpace, ref int pos, DateTimeOffset value)
        {
            Span<byte> tempSpan = stackalloc byte[JsonSharedConstant.MaximumFormatDateTimeOffsetLength];
            bool result = TryFormat(value, tempSpan, out var bytesWritten);
            Debug.Assert(result);
            TrimDateTimeOffset(tempSpan.Slice(0, bytesWritten), out bytesWritten);
            UnsafeMemory.WriteRaw(ref outputSpace, ref MemoryMarshal.GetReference(tempSpan), bytesWritten, ref pos);
        }

        /// <summary>
        /// Trims roundtrippable DateTime(Offset) input.
        /// If the milliseconds part of the date is zero, we omit the fraction part of the date,
        /// else we write the fraction up to 7 decimal places with no trailing zeros. i.e. the output format is
        /// YYYY-MM-DDThh:mm:ss[.s]TZD where TZD = Z or +-hh:mm.
        /// e.g.
        ///   ---------------------------------
        ///   2017-06-12T05:30:45.768-07:00
        ///   2017-06-12T05:30:45.00768Z           (Z is short for "+00:00" but also distinguishes DateTimeKind.Utc from DateTimeKind.Local)
        ///   2017-06-12T05:30:45                  (interpreted as local time wrt to current time zone)
        /// </summary>
        public static void TrimDateTimeOffset(Span<byte> buffer, out int bytesWritten)
        {
            const int maxDateTimeLength = JsonSharedConstant.MaximumFormatDateTimeLength;

            // Assert buffer is the right length for:
            // YYYY-MM-DDThh:mm:ss.fffffff (JsonConstants.MaximumFormatDateTimeLength)
            // YYYY-MM-DDThh:mm:ss.fffffffZ (JsonConstants.MaximumFormatDateTimeLength + 1)
            // YYYY-MM-DDThh:mm:ss.fffffff(+|-)hh:mm (JsonConstants.MaximumFormatDateTimeOffsetLength)
            Debug.Assert(buffer.Length == maxDateTimeLength ||
                buffer.Length == maxDateTimeLength + 1 ||
                buffer.Length == JsonSharedConstant.MaximumFormatDateTimeOffsetLength);

            // Find the last significant digit.
            int curIndex;
            if (buffer[maxDateTimeLength - 1] == '0')
                if (buffer[maxDateTimeLength - 2] == '0')
                    if (buffer[maxDateTimeLength - 3] == '0')
                        if (buffer[maxDateTimeLength - 4] == '0')
                            if (buffer[maxDateTimeLength - 5] == '0')
                                if (buffer[maxDateTimeLength - 6] == '0')
                                    if (buffer[maxDateTimeLength - 7] == '0')
                                    {
                                        // All decimal places are 0 so we can delete the decimal point too.
                                        curIndex = maxDateTimeLength - 7 - 1;
                                    }
                                    else { curIndex = maxDateTimeLength - 6; }
                                else { curIndex = maxDateTimeLength - 5; }
                            else { curIndex = maxDateTimeLength - 4; }
                        else { curIndex = maxDateTimeLength - 3; }
                    else { curIndex = maxDateTimeLength - 2; }
                else { curIndex = maxDateTimeLength - 1; }
            else
            {
                // There is nothing to trim.
                bytesWritten = buffer.Length;
                return;
            }

            // We are either trimming a DateTimeOffset, or a DateTime with
            // DateTimeKind.Local or DateTimeKind.Utc
            if (buffer.Length == maxDateTimeLength)
            {
                // There is no offset to copy.
                bytesWritten = curIndex;
            }
            else if (buffer.Length == JsonSharedConstant.MaximumFormatDateTimeOffsetLength)
            {
                // We have a non-UTC offset (+|-)hh:mm that are 6 characters to copy.
                buffer[curIndex] = buffer[maxDateTimeLength];
                buffer[curIndex + 1] = buffer[maxDateTimeLength + 1];
                buffer[curIndex + 2] = buffer[maxDateTimeLength + 2];
                buffer[curIndex + 3] = buffer[maxDateTimeLength + 3];
                buffer[curIndex + 4] = buffer[maxDateTimeLength + 4];
                buffer[curIndex + 5] = buffer[maxDateTimeLength + 5];
                bytesWritten = curIndex + 6;
            }
            else
            {
                // There is a single 'Z'. Just write it at the current index.
                Debug.Assert(buffer[maxDateTimeLength] == 'Z');

                buffer[curIndex] = (byte)'Z';
                bytesWritten = curIndex + 1;
            }
        }
    }
}