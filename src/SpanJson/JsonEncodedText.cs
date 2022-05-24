﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

// Largely based on https://github.com/dotnet/corefx/blob/8135319caa7e457ed61053ca1418313b88057b51/src/System.Text.Json/src/System/Text/Json/JsonEncodedText.cs

namespace SpanJson
{
    using System;
    using System.Buffers;
    using System.Diagnostics;
    using System.Text.Encodings.Web;
    using SpanJson.Internal;

    /// <summary>
    /// Provides a way to transform UTF-8 or UTF-16 encoded text into a form that is suitable for JSON.
    /// </summary>
    /// <remarks>
    /// This can be used to cache and store known strings used for writing JSON ahead of time by pre-encoding them up front.
    /// </remarks>
    public readonly struct JsonEncodedText : IEquatable<JsonEncodedText>
    {
        internal readonly byte[] _utf8Value;
        internal readonly string _value;

        /// <summary>
        /// Returns the UTF-8 encoded representation of the pre-encoded JSON text.
        /// </summary>
        public ReadOnlySpan<byte> EncodedUtf8Bytes => _utf8Value;

        public bool IsEmpty => 0U >= (uint)_utf8Value.Length;

        private JsonEncodedText(byte[] utf8Value)
        {
            Debug.Assert(utf8Value is not null);

            _value = JsonReaderHelper.GetTextFromUtf8(utf8Value);
            _utf8Value = utf8Value;
        }

        private JsonEncodedText(string value)
        {
            Debug.Assert(value is not null);
            
            _value = value;
            _utf8Value = TextEncodings.UTF8NoBOM.GetBytes(value);
        }

        /// <summary>
        /// Encodes the string text value as a JSON string.
        /// </summary>
        /// <param name="value">The value to be transformed as JSON encoded text.</param>
        /// <param name="escapeHandling"></param>
        /// <param name="encoder">The encoder to use when escaping the string, or <see langword="null" /> to use the default encoder.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if value is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown when the specified value is too large or if it contains invalid UTF-16 characters.
        /// </exception>
        public static JsonEncodedText Encode(string value, JsonEscapeHandling escapeHandling = JsonEscapeHandling.Default, JavaScriptEncoder encoder = null)
        {
            if (value is null) { ThrowHelper.ThrowArgumentNullException(ExceptionArgument.value); }

            return Encode(value.AsSpan(), escapeHandling, encoder);
        }

        /// <summary>
        /// Encodes the text value as a JSON string.
        /// </summary>
        /// <param name="value">The value to be transformed as JSON encoded text.</param>
        /// <param name="escapeHandling"></param>
        /// <param name="encoder">The encoder to use when escaping the string, or <see langword="null" /> to use the default encoder.</param>
        /// <exception cref="ArgumentException">
        /// Thrown when the specified value is too large or if it contains invalid UTF-16 characters.
        /// </exception>
        public static JsonEncodedText Encode(in ReadOnlySpan<char> value, JsonEscapeHandling escapeHandling = JsonEscapeHandling.Default, JavaScriptEncoder encoder = null)
        {
            if (value.IsEmpty)
            {
                return new JsonEncodedText(JsonHelpers.Empty<byte>());
            }

            if (escapeHandling == JsonEscapeHandling.EscapeNonAscii)
            {
                return TranscodeAndEncode(value, escapeHandling, encoder);
            }
            else
            {
                return new JsonEncodedText(EscapingHelper.EscapeString(value, escapeHandling, encoder));
            }
        }

        private static JsonEncodedText TranscodeAndEncode(in ReadOnlySpan<char> value, JsonEscapeHandling escapeHandling, JavaScriptEncoder encoder)
        {
            //JsonWriterHelper.ValidateValue(value);

            int expectedByteCount = JsonReaderHelper.GetUtf8ByteCount(value);
            byte[] utf8Bytes = ArrayPool<byte>.Shared.Rent(expectedByteCount);

            JsonEncodedText encodedText;

            // Since GetUtf8ByteCount above already throws on invalid input, the transcoding
            // to UTF-8 is guaranteed to succeed here. Therefore, there's no need for a try-catch-finally block.
            int actualByteCount = JsonReaderHelper.GetUtf8FromText(value, utf8Bytes);
            Debug.Assert(expectedByteCount == actualByteCount);

            encodedText = EncodeHelper(utf8Bytes.AsSpan(0, actualByteCount), escapeHandling, encoder);

            // On the basis that this is user data, go ahead and clear it.
            //utf8Bytes.AsSpan(0, expectedByteCount).Clear();
            ArrayPool<byte>.Shared.Return(utf8Bytes);

            return encodedText;
        }

        /// <summary>
        /// Encodes the UTF-8 text value as a JSON string.
        /// </summary>
        /// <param name="utf8Value">The UTF-8 encoded value to be transformed as JSON encoded text.</param>
        /// <param name="escapeHandling"></param>
        /// <param name="encoder">The encoder to use when escaping the string, or <see langword="null" /> to use the default encoder.</param>
        /// <exception cref="ArgumentException">
        /// Thrown when the specified value is too large or if it contains invalid UTF-8 bytes.
        /// </exception>
        public static JsonEncodedText Encode(in ReadOnlySpan<byte> utf8Value, JsonEscapeHandling escapeHandling = JsonEscapeHandling.Default, JavaScriptEncoder encoder = null)
        {
            if (utf8Value.IsEmpty)
            {
                return new JsonEncodedText(JsonHelpers.Empty<byte>());
            }

            //JsonWriterHelper.ValidateValue(utf8Value);
            return EncodeHelper(utf8Value, escapeHandling, encoder);
        }

        private static JsonEncodedText EncodeHelper(in ReadOnlySpan<byte> utf8Value, JsonEscapeHandling escapeHandling, JavaScriptEncoder encoder)
        {
            int idx = EscapingHelper.NeedsEscaping(utf8Value, escapeHandling, encoder);

            if ((uint)idx > JsonSharedConstant.TooBigOrNegative) // -1
            {
                return new JsonEncodedText(utf8Value.ToArray());
            }
            else
            {
                return new JsonEncodedText(JsonHelpers.EscapeValue(utf8Value, escapeHandling, idx, encoder));
            }
        }

        /// <summary>
        /// Determines whether this instance and another specified <see cref="JsonEncodedText"/> instance have the same value.
        /// </summary>
        /// <remarks>
        /// Default instances of <see cref="JsonEncodedText"/> are treated as equal.
        /// </remarks>
        public bool Equals(JsonEncodedText other)
        {
            if (_value is null)
            {
                return other._value is null;
            }
            else
            {
                return _value.Equals(other._value);
            }
        }

        /// <summary>
        /// Determines whether this instance and a specified object, which must also be a <see cref="JsonEncodedText"/> instance, have the same value.
        /// </summary>
        /// <remarks>
        /// If <paramref name="obj"/> is null, the method returns false.
        /// </remarks>
        public override bool Equals(object obj)
        {
            if (obj is JsonEncodedText encodedText)
            {
                return Equals(encodedText);
            }
            return false;
        }

        /// <summary>
        /// Converts the value of this instance to a <see cref="string"/>.
        /// </summary>
        /// <remarks>
        /// Returns the underlying UTF-16 encoded string.
        /// </remarks>
        /// <remarks>
        /// Returns an empty string on a default instance of <see cref="JsonEncodedText"/>.
        /// </remarks>
        public override string ToString()
            => _value ?? string.Empty;

        /// <summary>
        /// Returns the hash code for this <see cref="JsonEncodedText"/>.
        /// </summary>
        /// <remarks>
        /// Returns 0 on a default instance of <see cref="JsonEncodedText"/>.
        /// </remarks>
        public override int GetHashCode()
            => _value is null ? 0 : _value.GetHashCode();
    }
}
