﻿using System;

namespace SpanJson
{
    public static class JsonSharedConstant
    {
        public const int MaxNumberBufferSize = 32;
        public const int MaxVersionLength = 45; // 4 * int + 3 . + 2 double quote
        public const uint NestingLimit = 256;
        public const int MaxDateTimeOffsetLength = 35; // o + 2 double quotes
        public const int MaxDateTimeLength = 35; // o + 2 double quotes
        public const int MaxTimeSpanLength = 27; // c + 2 double quotes
        public const int MaxGuidLength = 42; // d + 2 double quotes

        public const int StackallocByteThreshold = 256;
        internal const uint StackallocByteThresholdU = 256u;
        public const int StackallocCharThreshold = StackallocByteThreshold / 2;
        internal const uint StackallocCharThresholdU = StackallocCharThreshold;

        public const uint TooBigOrNegative = int.MaxValue;

        // \u2028 and \u2029 are considered respectively line and paragraph separators
        // UTF-8 representation for them is E2, 80, A8/A9
        public const byte StartingByteOfNonStandardSeparator = 0xE2;

        public const int SpacesPerIndent = 2;
        public const int MaxWriterDepth = 1_000;
        public const int RemoveFlagsBitMask = 0x7FFFFFFF;

        // In the worst case, an ASCII character represented as a single utf-8 byte could expand 6x when escaped.
        // For example: '+' becomes '\u0043'
        // Escaping surrogate pairs (represented by 3 or 4 utf-8 bytes) would expand to 12 bytes (which is still <= 6x).
        // The same factor applies to utf-16 characters.
        public const int MaxExpansionFactorWhileEscaping = 6;

        // In the worst case, a single UTF-16 character could be expanded to 3 UTF-8 bytes.
        // Only surrogate pairs expand to 4 UTF-8 bytes but that is a transformation of 2 UTF-16 characters goign to 4 UTF-8 bytes (factor of 2).
        // All other UTF-16 characters can be represented by either 1 or 2 UTF-8 bytes.
        public const int MaxExpansionFactorWhileTranscoding = 3;

        // When transcoding from UTF8 -> UTF16, the byte count threshold where we rent from the array pool before performing a normal alloc.
        public const long ArrayPoolMaxSizeBeforeUsingNormalAlloc = 1024 * 1024;

        // The maximum number of characters allowed when writing raw UTF-16 JSON. This is the maximum length that we can guarantee can
        // be safely transcoded to UTF-8 and fit within an integer-length span, given the max expansion factor of a single character (3).
        public const int MaxUtf16RawValueLength = int.MaxValue / MaxExpansionFactorWhileTranscoding;

        public const int MaxEscapedTokenSize = 1_000_000_000;   // Max size for already escaped value.
        public const int MaxUnescapedTokenSize = MaxEscapedTokenSize / MaxExpansionFactorWhileEscaping;  // 166_666_666 bytes
        public const int MaxBase64ValueTokenSize = (MaxEscapedTokenSize >> 2) * 3 / MaxExpansionFactorWhileEscaping;  // 125_000_000 bytes
        public const int MaxCharacterTokenSize = MaxEscapedTokenSize / MaxExpansionFactorWhileEscaping; // 166_666_666 characters

        public const int MaximumFormatBooleanLength = 5;
        public const int MaximumFormatInt64Length = 20;   // 19 + sign (i.e. -9223372036854775808)
        public const int MaximumFormatUInt64Length = 20;  // i.e. 18446744073709551615
        public const int MaximumFormatDoubleLength = 128;  // default (i.e. 'G'), using 128 (rather than say 32) to be future-proof.
        public const int MaximumFormatSingleLength = 128;  // default (i.e. 'G'), using 128 (rather than say 32) to be future-proof.
        public const int MaximumFormatDecimalLength = 31; // default (i.e. 'G')
        public const int MaximumFormatGuidLength = 36;    // default (i.e. 'D'), 8 + 4 + 4 + 4 + 12 + 4 for the hyphens (e.g. 094ffa0a-0442-494d-b452-04003fa755cc)
        public const int MaximumEscapedGuidLength = MaxExpansionFactorWhileEscaping * MaximumFormatGuidLength;
        public const int MaximumFormatDateTimeLength = 27;    // StandardFormat 'O', e.g. 2017-06-12T05:30:45.7680000
        public const int MaximumFormatDateTimeOffsetLength = 33;  // StandardFormat 'O', e.g. 2017-06-12T05:30:45.7680000-07:00
        public const int MaxDateTimeUtcOffsetHours = 14; // The UTC offset portion of a TimeSpan or DateTime can be no more than 14 hours and no less than -14 hours.
        public const int DateTimeNumFractionDigits = 7;  // TimeSpan and DateTime formats allow exactly up to many digits for specifying the fraction after the seconds.
        public const int MaxDateTimeFraction = 9_999_999;  // The largest fraction expressible by TimeSpan and DateTime formats
        public const int DateTimeParseNumFractionDigits = 16; // The maximum number of fraction digits the Json DateTime parser allows
        public const int MaximumDateTimeOffsetParseLength = (MaximumFormatDateTimeOffsetLength +
            (DateTimeParseNumFractionDigits - DateTimeNumFractionDigits)); // Like StandardFormat 'O' for DateTimeOffset, but allowing 9 additional (up to 16) fraction digits.
        public const int MinimumDateTimeParseLength = 10; // YYYY-MM-DD
        public const int MaximumEscapedDateTimeOffsetParseLength = MaxExpansionFactorWhileEscaping * MaximumDateTimeOffsetParseLength;

        public const int MaximumLiteralLength = 5; // Must be able to fit null, true, & false.

        public const int UnicodePlane01StartValue = 0x10000;
        public const int HighSurrogateStartValue = 0xD800;
        public const int HighSurrogateEndValue = 0xDBFF;
        public const int LowSurrogateStartValue = 0xDC00;
        public const int LowSurrogateEndValue = 0xDFFF;
        public const int BitShiftBy10 = 0x400;

        // The maximum number of parameters a constructor can have where it can be considered
        // for a path on deserialization where we don't box the constructor arguments.
        public const int UnboxedParameterCountThreshold = 4;
    }

    public static class JsonUtf8Constant
    {
        public const byte BeginArray = (byte)'[';
        public const byte BeginObject = (byte)'{';
        public const byte DoubleQuote = (byte)'"';
        public const byte EndArray = (byte)']';
        public const byte EndObject = (byte)'}';
        public const byte False = (byte)'f';
        public const byte NameSeparator = (byte)':';
        public const byte Null = (byte)'n';
        public const byte ReverseSolidus = (byte)'\\';
        public const byte Solidus = (byte)'/';
        public const byte String = (byte)'"';
        public const byte True = (byte)'t';
        public const byte ValueSeparator = (byte)',';
        public const byte Space = (byte)' ';
        public const byte Plus = (byte)'+';
        public const byte Hyphen = (byte)'-';
        public const byte UtcOffsetToken = (byte)'Z';
        public const byte TimePrefix = (byte)'T';

        public const byte OpenBrace = (byte)'{';
        public const byte CloseBrace = (byte)'}';
        public const byte OpenBracket = (byte)'[';
        public const byte CloseBracket = (byte)']';
        public const byte CarriageReturn = (byte)'\r';
        public const byte LineFeed = (byte)'\n';
        public const byte Tab = (byte)'\t';
        public const byte ListSeparator = (byte)',';
        public const byte KeyValueSeperator = (byte)':';
        public const byte BackSlash = (byte)'\\';
        public const byte Slash = (byte)'/';
        public const byte BackSpace = (byte)'\b';
        public const byte FormFeed = (byte)'\f';
        public const byte Asterisk = (byte)'*';
        public const byte Colon = (byte)':';
        public const byte Period = (byte)'.';
        public const byte Minus = (byte)'-';

        public static ReadOnlySpan<byte> NewLine => new[] { (byte)'\r', (byte)'\n' };
        public static ReadOnlySpan<byte> NullTerminator => new byte[] { 0 };
        public static ReadOnlySpan<byte> Utf8Bom => new byte[] { 0xEF, 0xBB, 0xBF };
        public static ReadOnlySpan<byte> TrueValue => new byte[] { (byte)'t', (byte)'r', (byte)'u', (byte)'e' };
        public static ReadOnlySpan<byte> FalseValue => new byte[] { (byte)'f', (byte)'a', (byte)'l', (byte)'s', (byte)'e' };
        public static ReadOnlySpan<byte> NullValue => new byte[] { (byte)'n', (byte)'u', (byte)'l', (byte)'l' };

        public static ReadOnlySpan<byte> NaNValue => new byte[] { (byte)'N', (byte)'a', (byte)'N' };
        public static ReadOnlySpan<byte> PositiveInfinityValue => new byte[] { (byte)'I', (byte)'n', (byte)'f', (byte)'i', (byte)'n', (byte)'i', (byte)'t', (byte)'y' };
        public static ReadOnlySpan<byte> NegativeInfinityValue => new byte[] { (byte)'-', (byte)'I', (byte)'n', (byte)'f', (byte)'i', (byte)'n', (byte)'i', (byte)'t', (byte)'y' };

        // Used to search for the end of a number
        public static ReadOnlySpan<byte> Delimiters => new byte[] { ValueSeparator, CloseBrace, CloseBracket, Space, LineFeed, CarriageReturn, Tab, Slash };

        // Explicitly skipping ReverseSolidus since that is handled separately
        public static ReadOnlySpan<byte> EscapableChars => new byte[] { DoubleQuote, (byte)'n', (byte)'r', (byte)'t', Slash, (byte)'u', (byte)'b', (byte)'f' };
    }

    public static class JsonUtf16Constant
    {
        public const char BeginArray = '[';
        public const char BeginObject = '{';
        public const char DoubleQuote = '"';
        public const char EndArray = ']';
        public const char EndObject = '}';
        public const char False = 'f';
        public const char NameSeparator = ':';
        public const char Null = 'n';
        public const char ReverseSolidus = '\\';
        public const char Solidus = '/';
        public const char String = '"';
        public const char True = 't';
        public const char ValueSeparator = ',';
        public const char Space = ' ';
        public const char Plus = '+';
        public const char Hyphen = '-';
        public const char UtcOffsetToken = 'Z';
        public const char TimePrefix = 'T';

        public const char CarriageReturn = '\r';
        public const char LineFeed = '\n';
        public const char Tab = '\t';
        public const char BackSlash = '\\';
        public const char Slash = '/';
        public const char BackSpace = '\b';
        public const char FormFeed = '\f';
        public const char Asterisk = '*';
        public const char Colon = ':';
        public const char Period = '.';
        public const char Minus = '-';

        public static ReadOnlySpan<char> NewLine => new[] { '\r', '\n' };
        public static ReadOnlySpan<char> NullTerminator => new[] { '\0' };
    }
}