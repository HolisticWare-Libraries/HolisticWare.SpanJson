﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using SpanJson.Helpers;
using SpanJson.Shared.Fixture;
using Xunit;

namespace SpanJson.Tests
{
    public class DateTests
    {
        [Theory]
        [InlineData("2017-06-12T05:30:45.7680000-07:30", 33, 2017, 6, 12, 5, 30, 45, 7680000, true, 7, 30, DateTimeKind.Local)]
        [InlineData("2017-06-12T05:30:45.7680000Z", 28, 2017, 6, 12, 5, 30, 45, 7680000, false, 0, 0, DateTimeKind.Utc)]
        [InlineData("2017-06-12T05:30:45.7680000", 27, 2017, 6, 12, 5, 30, 45, 7680000, false, 0, 0, DateTimeKind.Unspecified)]
        [InlineData("2017-06-12T05:30:45.768+08:00", 29, 2017, 6, 12, 5, 30, 45, 7680000, false, 8, 0, DateTimeKind.Local)]
        [InlineData("2017-06-12T05:30:45.768Z", 24, 2017, 6, 12, 5, 30, 45, 7680000, false, 0, 0, DateTimeKind.Utc)]
        [InlineData("2017-06-12T05:30:45.768", 23, 2017, 6, 12, 5, 30, 45, 7680000, false, 0, 0, DateTimeKind.Unspecified)]
        [InlineData("2017-06-12T05:30:45+01:00", 25, 2017, 6, 12, 5, 30, 45, 0, false, 1, 0, DateTimeKind.Local)]
        [InlineData("2017-06-12T05:30:45Z", 20, 2017, 6, 12, 5, 30, 45, 0, false, 0, 0, DateTimeKind.Utc)]
        [InlineData("2017-06-12T05:30:45", 19, 2017, 6, 12, 5, 30, 45, 0, false, 0, 0, DateTimeKind.Unspecified)]
        [InlineData("2017-06-12T05:30", 16, 2017, 6, 12, 5, 30, 0, 0, false, 0, 0, DateTimeKind.Unspecified)]
        [InlineData("2017-06-12T05", 13, 2017, 6, 12, 5, 0, 0, 0, false, 0, 0, DateTimeKind.Unspecified)]
        [InlineData("2017-06-12T05:30:45.0010+01:00", 30, 2017, 6, 12, 5, 30, 45, 10000, false, 1, 0, DateTimeKind.Local)]
        [InlineData("2017-06-12T05:30:45.0010Z", 25, 2017, 6, 12, 5, 30, 45, 10000, false, 0, 0, DateTimeKind.Utc)]
        [InlineData("2017-06-12T05:30:45.0010", 24, 2017, 6, 12, 5, 30, 45, 10000, false, 0, 0, DateTimeKind.Unspecified)]
        [InlineData("2017-06-12T05:30:45.760738998+01:00", 35, 2017, 6, 12, 5, 30, 45, 7607389, false, 1, 0, DateTimeKind.Local)]
        [InlineData("2017-06-12T05:30:45.760738998Z", 30, 2017, 6, 12, 5, 30, 45, 7607389, false, 0, 0, DateTimeKind.Utc)]
        [InlineData("2017-06-12T05:30:45.760738998", 29, 2017, 6, 12, 5, 30, 45, 7607389, false, 0, 0, DateTimeKind.Unspecified)]
        [InlineData("2017-06-12T24:00:00", 19, 2017, 6, 13, 0, 0, 0, 0, false, 0, 0, DateTimeKind.Unspecified)]
        [InlineData("2017-06-12T24:00", 16, 2017, 6, 13, 0, 0, 0, 0, false, 0, 0, DateTimeKind.Unspecified)]
        [InlineData("2017-06-12T24", 13, 2017, 6, 13, 0, 0, 0, 0, false, 0, 0, DateTimeKind.Unspecified)]        
        public void Parse(string input, int length, int year, int month, int day, int hour, int minute,
            int second, int fraction, bool negative, int offsethours, int offsetminutes, DateTimeKind kind)
        {
            Assert.True(DateTimeParser.TryParseDateTimeOffset(input.AsSpan(), out var dtoValue, out var dtoConsumed));
            Assert.Equal(length, input.Length);
            Assert.Equal(length, dtoConsumed);
            Assert.True(DateTimeParser.TryParseDateTime(input.AsSpan(), out var dtValue, out var dtConsumed));
            Assert.Equal(length, dtConsumed);
            AssertDateTime(dtoValue.DateTime, year, month, day, hour, minute, second, fraction);
#if NETFRAMEWORK
            var offset = TimeSpan.FromTicks((long)Math.Round(new TimeSpan(offsethours, offsetminutes, 0).Ticks * (negative ? -1d : 1d)));
#else
            var offset = new TimeSpan(offsethours, offsetminutes, 0) * (negative ? -1 : 1);
#endif
            switch (kind)
            {
                case DateTimeKind.Local:
                    Assert.Equal(offset, dtoValue.DateTime - dtoValue.UtcDateTime);
                    Assert.Equal(dtValue, dtoValue.LocalDateTime);
                    break;
                case DateTimeKind.Unspecified:
                    Assert.Equal(offset, dtoValue.DateTime - dtoValue.LocalDateTime);
                    Assert.Equal(dtValue, dtoValue.DateTime);
                    break;
                case DateTimeKind.Utc:
                    Assert.Equal(offset, dtoValue.DateTime - dtoValue.UtcDateTime);
                    Assert.Equal(dtValue, dtoValue.UtcDateTime);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(kind), kind, null);
            }

            Assert.True(DateTimeParser.TryParseDateTimeOffset(Encoding.UTF8.GetBytes(input), out var utf8dtoValue, out dtoConsumed));
            Assert.Equal(length, input.Length);
            Assert.Equal(length, dtoConsumed);
            Assert.True(DateTimeParser.TryParseDateTime(Encoding.UTF8.GetBytes(input), out var utf8dtValue, out dtConsumed));;
            Assert.Equal(length, dtConsumed);
            Assert.Equal(dtoValue, utf8dtoValue);
            Assert.Equal(dtValue, utf8dtValue);
        }

        [Theory]
        [InlineData("1970-01-01", 10, 1970, 01, 01)]
        [InlineData("2017-06-12", 10, 2017, 6, 12)]
        [InlineData("2050-01-01", 10, 2050, 01, 01)]
        public void ParseDate(string input, int length, int year, int month, int day)
        {
            Assert.True(DateTimeParser.TryParseDateTimeOffset(input.AsSpan(), out var dtoValue, out var dtoConsumed));
            Assert.Equal(length, input.Length);
            Assert.Equal(length, dtoConsumed);
            Assert.True(DateTimeParser.TryParseDateTime(input.AsSpan(), out var dtValue, out var dtConsumed));
            Assert.Equal(length, dtConsumed);

            Assert.Equal(year, dtoValue.Year);
            Assert.Equal(month, dtoValue.Month);
            Assert.Equal(day, dtValue.Day);
            Assert.Equal(year, dtValue.Year);
            Assert.Equal(month, dtValue.Month);
            Assert.Equal(day, dtValue.Day);

#if (NET || NETCOREAPP2_1_OR_GREATER)
            Assert.True(DateTimeOffset.TryParseExact(input.AsSpan(), "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var bclDtoValue));
            Assert.True(DateTime.TryParseExact(input.AsSpan(), "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var bclDtValue));
#else
            Assert.True(DateTimeOffset.TryParseExact(input, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var bclDtoValue));
            Assert.True(DateTime.TryParseExact(input, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var bclDtValue));
#endif

            Assert.Equal(bclDtValue, dtValue);
            Assert.Equal(bclDtoValue, dtoValue);

            Assert.True(DateTimeParser.TryParseDateTimeOffset(Encoding.UTF8.GetBytes(input), out var utf8dtoValue, out _));
            Assert.Equal(length, input.Length);
            Assert.Equal(length, dtoConsumed);
            Assert.True(DateTimeParser.TryParseDateTime(Encoding.UTF8.GetBytes(input), out var utf8dtValue, out _));

            Assert.Equal(bclDtValue, utf8dtValue);
            Assert.Equal(bclDtoValue, utf8dtoValue);
        }


        /// <summary>
        ///     To make sure the fractions are properly parsed
        /// </summary>
        [Theory]
        [InlineData("2017-06-12T05:30:45.1000000Z")]
        [InlineData("2017-06-12T05:30:45.0100000Z")]
        [InlineData("2017-06-12T05:30:45.0010000Z")]
        [InlineData("2017-06-12T05:30:45.0001000Z")]
        [InlineData("2017-06-12T05:30:45.0000100Z")]
        [InlineData("2017-06-12T05:30:45.0000010Z")]
        [InlineData("2017-06-12T05:30:45.0000001Z")]
        public void AgainstBcl(string input)
        {
            Assert.True(DateTimeParser.TryParseDateTimeOffset(input.AsSpan(), out var dtoValue, out var dtoConsumed));
#if (NET || NETCOREAPP2_1_OR_GREATER)
            var dto = DateTimeOffset.ParseExact(input.AsSpan(), "O", CultureInfo.InvariantCulture);
#else
            var dto = DateTimeOffset.ParseExact(input, "O", CultureInfo.InvariantCulture);
#endif
            Assert.Equal(dto, dtoValue);
        }

        public static IEnumerable<object[]> GenerateValuesForDigitChecking()
        {
            var startDate = new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Utc).Ticks;

            for (int i = 1; i < 13; i++)
            {
                yield return new object[] {new DateTime(startDate + TimeSpan.FromDays(31).Ticks * i)};
            }

            for (int i = 1; i < 31; i++)
            {
                yield return new object[] {new DateTime(startDate + TimeSpan.FromDays(i).Ticks)};
            }

            for (int i = 0; i < 24; i++)
            {
                yield return new object[] {new DateTime(startDate + TimeSpan.FromHours(i).Ticks)};
            }

            for (int i = 0; i < 60; i++)
            {
                yield return new object[] {new DateTime(startDate + TimeSpan.FromMinutes(i).Ticks)};
            }

            for (int i = 0; i < 60; i++)
            {
                yield return new object[] {new DateTime(startDate + TimeSpan.FromSeconds(i).Ticks)};
            }
        }

        [Theory]
        [MemberData(nameof(GenerateValuesForDigitChecking))]
        public void DigitChecking(DateTime dt)
        {
            Span<char> outputChars = stackalloc char[50];
            Assert.True(DateTimeFormatter.TryFormat(dt, outputChars, out var written));
            Assert.True(DateTimeParser.TryParseDateTime(outputChars, out var outputDt, out var consumed));
            Assert.Equal(dt, outputDt);
            Assert.Equal(written, consumed);

            Span<byte> outputBytes = stackalloc byte[50];
            Assert.True(DateTimeFormatter.TryFormat(dt, outputBytes, out written));
            Assert.True(DateTimeParser.TryParseDateTime(outputBytes, out outputDt, out consumed));
            Assert.Equal(dt, outputDt);
            Assert.Equal(written, consumed);

            var dto = new DateTimeOffset(dt);

            Assert.True(DateTimeFormatter.TryFormat(dto, outputChars, out written));
            Assert.True(DateTimeParser.TryParseDateTime(outputChars, out var outputdto, out consumed));
            Assert.Equal(dto, outputdto);
            Assert.Equal(written, consumed);

            Assert.True(DateTimeFormatter.TryFormat(dto, outputBytes, out written));
            Assert.True(DateTimeParser.TryParseDateTime(outputBytes, out outputdto, out consumed));
            Assert.Equal(dto, outputdto);
            Assert.Equal(written, consumed);
        }


        private void AssertDateTime(DateTime dateTime, int year, int month, int day, int hour, int minute,
            int second, int fraction)
        {
            var comparison = new DateTime(year, month, day, hour, minute, second).AddTicks(fraction);
            Assert.Equal(comparison, dateTime);
        }

        [Fact]
        public void SerializeDeserializeDateTime()
        {
            var fixture = new ExpressionTreeFixture();
            var dt = fixture.Create<DateTime>();
            var serialized = JsonSerializer.Generic.Utf16.Serialize(dt);
            var deserialized = JsonSerializer.Generic.Utf16.Deserialize<DateTime>(serialized);
            Assert.Equal(dt, deserialized);
        }

        [Fact]
        public void SerializeDeserializeDateTimeOffset()
        {
            var fixture = new ExpressionTreeFixture();
            var dt = fixture.Create<DateTimeOffset>();
            var serialized = JsonSerializer.Generic.Utf16.Serialize(dt);
            var deserialized = JsonSerializer.Generic.Utf16.Deserialize<DateTimeOffset>(serialized);
            Assert.Equal(dt, deserialized);
        }

        [Theory]
        [InlineData("2017-06-12T05:30:45.768", 2017, 6, 12, 5, 30, 45, 7680000,
            DateTimeKind.Unspecified)]
        [InlineData("2017-06-12T05:30:45.768Z", 2017, 6, 12, 5, 30, 45, 7680000, DateTimeKind.Utc)]
        [InlineData("2017-06-12T05:30:45", 2017, 6, 12, 5, 30, 45, 0, DateTimeKind.Unspecified)]
        [InlineData("2017-06-12T05:30:45Z", 2017, 6, 12, 5, 30, 45, 0, DateTimeKind.Utc)]
        [InlineData("2017-06-12T05:30:45.001", 2017, 6, 12, 5, 30, 45, 10000, DateTimeKind.Unspecified)]
        [InlineData("2017-06-12T05:30:45.001Z", 2017, 6, 12, 5, 30, 45, 10000, DateTimeKind.Utc)]
        [InlineData("2017-06-12T05:30:45.7607389", 2017, 6, 12, 5, 30, 45, 7607389, DateTimeKind.Unspecified)]
        [InlineData("2017-06-12T05:30:45.7607389Z", 2017, 6, 12, 5, 30, 45, 7607389, DateTimeKind.Utc)]
        [InlineData("2017-06-12T05:30:45.0000001Z", 2017, 6, 12, 5, 30, 45, 0000001, DateTimeKind.Utc)]
        [InlineData("2017-06-12T05:30:45.0000001", 2017, 6, 12, 5, 30, 45, 0000001, DateTimeKind.Unspecified)]
        public void FormatDateTime(string comparison, int year, int month, int day, int hour, int minute,
            int second, int fraction, DateTimeKind kind)
        {
            var value = new DateTime(year, month, day, hour, minute, second, kind).AddTicks(fraction);
            Span<char> charSpan = stackalloc char[33];
            Assert.True(DateTimeFormatter.TryFormat(value, charSpan, out var symbolsWritten));
            DateTimeFormatter.TrimDateTimeOffset(charSpan.Slice(0, symbolsWritten), out symbolsWritten);
            Assert.Equal(comparison, charSpan.Slice(0, symbolsWritten).ToString());


            Span<byte> byteSpan = stackalloc byte[33];
            Assert.True(DateTimeFormatter.TryFormat(value, byteSpan, out symbolsWritten));
            DateTimeFormatter.TrimDateTimeOffset(byteSpan.Slice(0, symbolsWritten), out symbolsWritten);
#if (NET || NETCOREAPP2_1_OR_GREATER)
            Assert.Equal(comparison, Encoding.UTF8.GetString(byteSpan.Slice(0, symbolsWritten)));
#else
            Assert.Equal(comparison, Encoding.UTF8.GetString(byteSpan.Slice(0, symbolsWritten).ToArray()));
#endif
        }

        [Theory]
        [InlineData("2017-06-12T05:30:45.768-07:30", 2017, 6, 12, 5, 30, 45, 7680000, true, 7, 30,
            DateTimeKind.Unspecified)]
        [InlineData("2017-06-12T05:30:45.768Z", 2017, 6, 12, 5, 30, 45, 7680000, false, 0, 0, DateTimeKind.Utc)]
        [InlineData("2017-06-12T05:30:45.768Z", 2017, 6, 12, 5, 30, 45, 7680000, false, 0, 0,
            DateTimeKind.Unspecified)]
        [InlineData("2017-06-12T05:30:45.768+08:00", 2017, 6, 12, 5, 30, 45, 7680000, false, 8, 0, DateTimeKind.Unspecified)]
        [InlineData("2017-06-12T05:30:45+01:00", 2017, 6, 12, 5, 30, 45, 0, false, 1, 0, DateTimeKind.Unspecified)]
        [InlineData("2017-06-12T05:30:45Z", 2017, 6, 12, 5, 30, 45, 0, false, 0, 0, DateTimeKind.Utc)]
        [InlineData("2017-06-12T05:30:45Z", 2017, 6, 12, 5, 30, 45, 0, false, 0, 0, DateTimeKind.Unspecified)]
        [InlineData("2017-06-12T05:30:45.001+01:00", 2017, 6, 12, 5, 30, 45, 10000, false, 1, 0, DateTimeKind.Unspecified)]
        [InlineData("2017-06-12T05:30:45.001Z", 2017, 6, 12, 5, 30, 45, 10000, false, 0, 0, DateTimeKind.Utc)]
        [InlineData("2017-06-12T05:30:45.001Z", 2017, 6, 12, 5, 30, 45, 10000, false, 0, 0, DateTimeKind.Unspecified)]
        [InlineData("2017-06-12T05:30:45.7607389+01:00", 2017, 6, 12, 5, 30, 45, 7607389, false, 1, 0, DateTimeKind.Unspecified)]
        [InlineData("2017-06-12T05:30:45.7607389Z", 2017, 6, 12, 5, 30, 45, 7607389, false, 0, 0, DateTimeKind.Utc)]
        [InlineData("2017-06-12T05:30:45.7607389Z", 2017, 6, 12, 5, 30, 45, 7607389, false, 0, 0, DateTimeKind.Unspecified)]
        [InlineData("2017-06-12T05:30:45.0000001Z", 2017, 6, 12, 5, 30, 45, 0000001, false, 0, 0, DateTimeKind.Utc)]
        [InlineData("2017-06-12T05:30:45.0000001Z", 2017, 6, 12, 5, 30, 45, 0000001, false, 0, 0, DateTimeKind.Unspecified)]
        public void FormatDateTimeOffset(string comparison, int year, int month, int day, int hour, int minute,
            int second, int fraction, bool negative, int offsethours, int offsetminutes, DateTimeKind kind)
        {
#if NETFRAMEWORK
            var offset = TimeSpan.FromTicks((long)Math.Round(new TimeSpan(0, offsethours, offsetminutes, 0).Ticks * (negative ? -1d : 1d)));
            var dt = new DateTime(year, month, day, hour, minute, second, kind).AddTicks(fraction);
            var value = new DateTimeOffset(dt, offset);
#else
            var offset = new TimeSpan(0, offsethours, offsetminutes, 0) * (negative ? -1 : 1);
            var dt = new DateTime(year, month, day, hour, minute, second, kind).AddTicks(fraction);
            var value = new DateTimeOffset(dt, offset);
#endif

            Span<char> charSpan = stackalloc char[33];
            Assert.True(DateTimeFormatter.TryFormat(value, charSpan, out var symbolsWritten));
            DateTimeFormatter.TrimDateTimeOffset(charSpan.Slice(0, symbolsWritten), out symbolsWritten);
            Assert.Equal(comparison, charSpan.Slice(0, symbolsWritten).ToString());

            Span<byte> byteSpan = stackalloc byte[33];
            Assert.True(DateTimeFormatter.TryFormat(value, byteSpan, out symbolsWritten));
            DateTimeFormatter.TrimDateTimeOffset(byteSpan.Slice(0, symbolsWritten), out symbolsWritten);
#if (NET || NETCOREAPP2_1_OR_GREATER)
            Assert.Equal(comparison, Encoding.UTF8.GetString(byteSpan.Slice(0, symbolsWritten)));
#else
            Assert.Equal(comparison, Encoding.UTF8.GetString(byteSpan.Slice(0, symbolsWritten).ToArray()));
#endif
        }

        [Fact]
        public void LocalTime()
        {
            var value = DateTime.Now;
            value = value.AddTicks(-(value.Ticks % TimeSpan.TicksPerSecond));
            var offset = TimeZoneInfo.Local.GetUtcOffset(value);
            var output = value.ToString("yyyy-MM-ddTHH:mm:ss");
            var sign = offset.Hours < 0 ? '-' : '+';
            output = output + sign + $"{offset.Hours:D2}:{offset.Minutes:D2}";
            Span<char> charSpan = stackalloc char[33];
            Assert.True(DateTimeFormatter.TryFormat(value, charSpan, out var symbolsWritten));
            DateTimeFormatter.TrimDateTimeOffset(charSpan.Slice(0, symbolsWritten), out symbolsWritten);
            Assert.Equal(output, charSpan.Slice(0, symbolsWritten).ToString());

            Span<byte> byteSpan = stackalloc byte[33];
            Assert.True(DateTimeFormatter.TryFormat(value, byteSpan, out symbolsWritten));
            DateTimeFormatter.TrimDateTimeOffset(byteSpan.Slice(0, symbolsWritten), out symbolsWritten);
#if (NET || NETCOREAPP2_1_OR_GREATER)
            Assert.Equal(output, Encoding.UTF8.GetString(byteSpan.Slice(0, symbolsWritten)));
#else
            Assert.Equal(output, Encoding.UTF8.GetString(byteSpan.Slice(0, symbolsWritten).ToArray()));
#endif
        }
    }
}