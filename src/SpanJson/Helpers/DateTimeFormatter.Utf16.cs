namespace SpanJson.Helpers
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public static partial class DateTimeFormatter
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryFormat(DateTimeOffset value, Span<char> destination, out int charsWritten)
        {
            const uint MinimumBytesNeeded = JsonSharedConstant.MaximumFormatDateTimeOffsetLength;
            if ((uint)destination.Length < MinimumBytesNeeded)
            {
                charsWritten = 0;
                return false;
            }

            ref var c = ref MemoryMarshal.GetReference(destination);

            WriteDateAndTime(value.DateTime, ref c);

            if (value.Offset == TimeSpan.Zero)
            {
                charsWritten = 28;
                Unsafe.Add(ref c, 27) = JsonUtf16Constant.UtcOffsetToken;
            }
            else
            {
                charsWritten = JsonSharedConstant.MaximumFormatDateTimeOffsetLength;
                WriteTimeZone(value.Offset, ref c);
            }

            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryFormat(DateTime value, Span<char> destination, out int charsWritten)
        {
            const uint MinimumBytesNeeded = JsonSharedConstant.MaximumFormatDateTimeOffsetLength;
            if ((uint)destination.Length < MinimumBytesNeeded)
            {
                charsWritten = 0;
                return false;
            }

            charsWritten = JsonSharedConstant.MaximumFormatDateTimeLength;

            ref var c = ref MemoryMarshal.GetReference(destination);

            WriteDateAndTime(value, ref c);

            var kind = value.Kind;
            if (kind == DateTimeKind.Local)
            {
                charsWritten = JsonSharedConstant.MaximumFormatDateTimeOffsetLength;
                WriteTimeZone(TimeZoneInfo.Local.GetUtcOffset(value), ref c);
            }
            else if (kind == DateTimeKind.Utc)
            {
                charsWritten = 28;
                Unsafe.Add(ref c, 27) = JsonUtf16Constant.UtcOffsetToken;
            }

            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void WriteDateAndTime(DateTime value, ref char c)
        {
            WriteFourDigits((uint)value.Year, ref c, 0);
            Unsafe.Add(ref c, 4) = JsonUtf16Constant.Minus;

            WriteTwoDigits((uint)value.Month, ref c, 5);
            Unsafe.Add(ref c, 7) = JsonUtf16Constant.Minus;

            WriteTwoDigits((uint)value.Day, ref c, 8);
            Unsafe.Add(ref c, 10) = JsonUtf16Constant.TimePrefix;

            WriteTwoDigits((uint)value.Hour, ref c, 11);
            Unsafe.Add(ref c, 13) = JsonUtf16Constant.Colon;

            WriteTwoDigits((uint)value.Minute, ref c, 14);
            Unsafe.Add(ref c, 16) = JsonUtf16Constant.Colon;

            WriteTwoDigits((uint)value.Second, ref c, 17);
            Unsafe.Add(ref c, 19) = JsonUtf16Constant.Period;

            WriteDigits((uint)((ulong)value.Ticks % (ulong)TimeSpan.TicksPerSecond), ref c, 20);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void WriteTimeZone(TimeSpan offset, ref char c)
        {
            char sign;
            if (offset < default(TimeSpan) /* a "const" version of TimeSpan.Zero */)
            {
                sign = JsonUtf16Constant.Minus;
                offset = TimeSpan.FromTicks(-offset.Ticks);
            }
            else
            {
                sign = JsonUtf16Constant.Plus;
            }

            // Writing the value backward allows the JIT to optimize by
            // performing a single bounds check against buffer.

            Unsafe.Add(ref c, 27) = sign;
            WriteTwoDigits((uint)offset.Hours, ref c, 28);
            Unsafe.Add(ref c, 30) = JsonUtf16Constant.Colon;
            WriteTwoDigits((uint)offset.Minutes, ref c, 31);
        }

        /// <summary>Writes a value [ 0000 .. 9999 ] to the buffer starting at the specified offset.
        /// This method performs best when the starting index is a constant literal.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void WriteFourDigits(uint value, ref char c, int startIndex)
        {
            Debug.Assert(/*0 <= value && */value <= 9999);

            var temp = '0' + value;
            value /= 10u;
            Unsafe.Add(ref c, startIndex + 3) = (char)(temp - value * 10u);

            temp = '0' + value;
            value /= 10u;
            Unsafe.Add(ref c, startIndex + 2) = (char)(temp - value * 10u);

            temp = '0' + value;
            value /= 10u;
            Unsafe.Add(ref c, startIndex + 1) = (char)(temp - value * 10u);

            Unsafe.Add(ref c, startIndex) = (char)('0' + value);
        }

        /// <summary>Writes a value [ 00 .. 99 ] to the buffer starting at the specified offset.
        /// This method performs best when the starting index is a constant literal.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void WriteTwoDigits(uint value, ref char c, int startIndex)
        {
            Debug.Assert(/*0 <= value && */value <= 99);

            var temp = '0' + value;
            value /= 10u;
            Unsafe.Add(ref c, startIndex + 1) = (char)(temp - value * 10u);
            Unsafe.Add(ref c, startIndex) = (char)('0' + value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void WriteDigits(uint value, ref char b, int pos)
        {
            // We can mutate the 'value' parameter since it's a copy-by-value local.
            // It'll be used to represent the value left over after each division by 10.

            for (int i = 6/*buffer.Length - 1*/; i >= 1; i--)
            {
                uint temp = '0' + value;
                value /= 10u;
                Unsafe.Add(ref b, pos + i) = (char)(temp - (value * 10u));
            }

            Debug.Assert(value < 10);
            Unsafe.Add(ref b, pos) = (char)('0' + value);
        }

        // Largely based on https://github.com/dotnet/corefx/blob/8135319caa7e457ed61053ca1418313b88057b51/src/System.Text.Json/src/System/Text/Json/Writer/JsonWriterHelper.Date.cs

        public static void WriteDateTimeTrimmed(Span<char> buffer, DateTime value, out int charsWritten)
        {
            Span<char> tempSpan = stackalloc char[JsonSharedConstant.MaximumFormatDateTimeOffsetLength];
            bool result = TryFormat(value, tempSpan, out charsWritten);
            Debug.Assert(result);
            TrimDateTimeOffset(tempSpan.Slice(0, charsWritten), out charsWritten);
            tempSpan.Slice(0, charsWritten).CopyTo(buffer);
        }

        public static void WriteDateTimeOffsetTrimmed(Span<char> buffer, DateTimeOffset value, out int charsWritten)
        {
            Span<char> tempSpan = stackalloc char[JsonSharedConstant.MaximumFormatDateTimeOffsetLength];
            bool result = TryFormat(value, tempSpan, out charsWritten);
            Debug.Assert(result);
            TrimDateTimeOffset(tempSpan.Slice(0, charsWritten), out charsWritten);
            tempSpan.Slice(0, charsWritten).CopyTo(buffer);
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
        public static void TrimDateTimeOffset(Span<char> buffer, out int charsWritten)
        {
            const int maxDateTimeLength = JsonSharedConstant.MaximumFormatDateTimeLength;

            // Assert buffer is the right length for:
            // YYYY-MM-DDThh:mm:ss.fffffff (JsonSharedConstant.MaximumFormatDateTimeLength)
            // YYYY-MM-DDThh:mm:ss.fffffffZ (JsonSharedConstant.MaximumFormatDateTimeLength + 1)
            // YYYY-MM-DDThh:mm:ss.fffffff(+|-)hh:mm (JsonSharedConstant.MaximumFormatDateTimeOffsetLength)
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
                charsWritten = buffer.Length;
                return;
            }

            // We are either trimming a DateTimeOffset, or a DateTime with
            // DateTimeKind.Local or DateTimeKind.Utc
            if (buffer.Length == maxDateTimeLength)
            {
                // There is no offset to copy.
                charsWritten = curIndex;
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
                charsWritten = curIndex + 6;
            }
            else
            {
                // There is a single 'Z'. Just write it at the current index.
                Debug.Assert(buffer[maxDateTimeLength] == 'Z');

                buffer[curIndex] = 'Z';
                charsWritten = curIndex + 1;
            }
        }
    }
}