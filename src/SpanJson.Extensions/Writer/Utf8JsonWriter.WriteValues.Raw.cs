// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Buffers;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using SpanJson.Internal;

namespace SpanJson
{
    partial struct Utf8JsonWriter
    {
        /// <summary>
        /// Writes the input as JSON content. It is expected that the input content is a single complete JSON value.
        /// </summary>
        /// <param name="json">The raw JSON content to write.</param>
        /// <param name="skipInputValidation">Whether to validate if the input is an RFC 8259-compliant JSON payload.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="json"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException">Thrown if the length of the input is zero or greater than 715,827,882 (<see cref="int.MaxValue"/> / 3).</exception>
        /// <exception cref="JsonException">
        /// Thrown if <paramref name="skipInputValidation"/> is <see langword="false"/>, and the input
        /// is not a valid, complete, single JSON value according to the JSON RFC (https://tools.ietf.org/html/rfc8259)
        /// or the input JSON exceeds a recursive depth of 64.
        /// </exception>
        /// <remarks>
        /// When writing untrused JSON values, do not set <paramref name="skipInputValidation"/> to <see langword="true"/> as this can result in invalid JSON
        /// being written, and/or the overall payload being written to the writer instance being invalid.
        ///
        /// When using this method, the input content will be written to the writer destination as-is, unless validation fails (when it is enabled).
        ///
        /// The <see cref="JsonWriterOptions.SkipValidation"/> value for the writer instance is honored when using this method.
        ///
        /// The <see cref="JsonWriterOptions.Indented"/> and <see cref="JsonWriterOptions.Encoder"/> values for the writer instance are not applied when using this method.
        /// </remarks>
        public void WriteRawValue(string json, bool skipInputValidation = false)
        {
            if (!_options.SkipValidation)
            {
                ValidateWritingValue();
            }

            if (json is null) { ThrowHelper.ThrowArgumentNullException(ExceptionArgument.json); }

            TranscodeAndWriteRawValue(json.AsSpan(), skipInputValidation);
        }

        /// <summary>
        /// Writes the input as JSON content. It is expected that the input content is a single complete JSON value.
        /// </summary>
        /// <param name="json">The raw JSON content to write.</param>
        /// <param name="skipInputValidation">Whether to validate if the input is an RFC 8259-compliant JSON payload.</param>
        /// <exception cref="ArgumentException">Thrown if the length of the input is zero or greater than 715,827,882 (<see cref="int.MaxValue"/> / 3).</exception>
        /// <exception cref="JsonException">
        /// Thrown if <paramref name="skipInputValidation"/> is <see langword="false"/>, and the input
        /// is not a valid, complete, single JSON value according to the JSON RFC (https://tools.ietf.org/html/rfc8259)
        /// or the input JSON exceeds a recursive depth of 64.
        /// </exception>
        /// <remarks>
        /// When writing untrused JSON values, do not set <paramref name="skipInputValidation"/> to <see langword="true"/> as this can result in invalid JSON
        /// being written, and/or the overall payload being written to the writer instance being invalid.
        ///
        /// When using this method, the input content will be written to the writer destination as-is, unless validation fails (when it is enabled).
        ///
        /// The <see cref="JsonWriterOptions.SkipValidation"/> value for the writer instance is honored when using this method.
        ///
        /// The <see cref="JsonWriterOptions.Indented"/> and <see cref="JsonWriterOptions.Encoder"/> values for the writer instance are not applied when using this method.
        /// </remarks>
        public void WriteRawValue(ReadOnlySpan<char> json, bool skipInputValidation = false)
        {
            if (!_options.SkipValidation)
            {
                ValidateWritingValue();
            }

            TranscodeAndWriteRawValue(json, skipInputValidation);
        }

        /// <summary>
        /// Writes the input as JSON content. It is expected that the input content is a single complete JSON value.
        /// </summary>
        /// <param name="utf8Json">The raw JSON content to write.</param>
        /// <param name="skipInputValidation">Whether to validate if the input is an RFC 8259-compliant JSON payload.</param>
        /// <exception cref="ArgumentException">Thrown if the length of the input is zero or equal to <see cref="int.MaxValue"/>.</exception>
        /// <exception cref="JsonException">
        /// Thrown if <paramref name="skipInputValidation"/> is <see langword="false"/>, and the input
        /// is not a valid, complete, single JSON value according to the JSON RFC (https://tools.ietf.org/html/rfc8259)
        /// or the input JSON exceeds a recursive depth of 64.
        /// </exception>
        /// <remarks>
        /// When writing untrused JSON values, do not set <paramref name="skipInputValidation"/> to <see langword="true"/> as this can result in invalid JSON
        /// being written, and/or the overall payload being written to the writer instance being invalid.
        ///
        /// When using this method, the input content will be written to the writer destination as-is, unless validation fails (when it is enabled).
        ///
        /// The <see cref="JsonWriterOptions.SkipValidation"/> value for the writer instance is honored when using this method.
        ///
        /// The <see cref="JsonWriterOptions.Indented"/> and <see cref="JsonWriterOptions.Encoder"/> values for the writer instance are not applied when using this method.
        /// </remarks>
        public void WriteRawValue(ReadOnlySpan<byte> utf8Json, bool skipInputValidation = false)
        {
            if (!_options.SkipValidation)
            {
                ValidateWritingValue();
            }

            if (utf8Json.Length == int.MaxValue)
            {
                SysJsonThrowHelper.ThrowArgumentException_ValueTooLarge(int.MaxValue);
            }

            WriteRawValueCore(utf8Json, skipInputValidation);
        }

        private void TranscodeAndWriteRawValue(ReadOnlySpan<char> json, bool skipInputValidation)
        {
            if ((uint)json.Length > (uint)JsonSharedConstant.MaxUtf16RawValueLength)
            {
                SysJsonThrowHelper.ThrowArgumentException_ValueTooLarge(json.Length);
            }

            byte[]? tempArray = null;

            // For performance, avoid obtaining actual byte count unless memory usage is higher than the threshold.
            Span<byte> utf8Json = json.Length <= (JsonSharedConstant.ArrayPoolMaxSizeBeforeUsingNormalAlloc / JsonSharedConstant.MaxExpansionFactorWhileTranscoding) ?
                // Use a pooled alloc.
                tempArray = ArrayPool<byte>.Shared.Rent(json.Length * JsonSharedConstant.MaxExpansionFactorWhileTranscoding) :
                // Use a normal alloc since the pool would create a normal alloc anyway based on the threshold (per current implementation)
                // and by using a normal alloc we can avoid the Clear().
                new byte[JsonReaderHelper.GetUtf8ByteCount(json)];

            try
            {
                int actualByteCount = JsonReaderHelper.GetUtf8FromText(json, utf8Json);
                utf8Json = utf8Json.Slice(0, actualByteCount);
                WriteRawValueCore(utf8Json, skipInputValidation);
            }
            finally
            {
                if (tempArray != null)
                {
                    utf8Json.Clear();
                    ArrayPool<byte>.Shared.Return(tempArray);
                }
            }
        }

        private void WriteRawValueCore(ReadOnlySpan<byte> utf8Json, bool skipInputValidation)
        {
            int len = utf8Json.Length;

            if (0u >= (uint)len)
            {
                ThrowHelper2.ThrowArgumentException_ExpectedJsonTokens();
            }

            // In the UTF-16-based entry point methods above, we validate that the payload length <= int.MaxValue /3.
            // The result of this division will be rounded down, so even if every input character needs to be transcoded
            // (with expansion factor of 3), the resulting payload would be less than int.MaxValue,
            // as (int.MaxValue/3) * 3 is less than int.MaxValue.
            Debug.Assert(len < int.MaxValue);

            if (skipInputValidation)
            {
                // Treat all unvalidated raw JSON value writes as string. If the payload is valid, this approach does
                // not affect structural validation since a string token is equivalent to a complete object, array,
                // or other complete JSON tokens when considering structural validation on subsequent writer calls.
                // If the payload is not valid, then we make no guarantees about the structural validation of the final payload.
                _tokenType = JsonTokenType.String;
            }
            else
            {
                // Utilize reader validation.
                Utf8JsonReader reader = new(utf8Json);
                while (reader.Read()) ;
                _tokenType = reader.TokenType;
            }

            // TODO (https://github.com/dotnet/runtime/issues/29293):
            // investigate writing this in chunks, rather than requesting one potentially long, contiguous buffer.
            int maxRequired = len + 1; // Optionally, 1 list separator. We've guarded against integer overflow earlier in the call stack.

            ref var pos = ref _pos;
            EnsureUnsafe(pos, maxRequired);

            ref byte output = ref PinnableAddress;

            if (_currentDepth < 0)
            {
                Unsafe.Add(ref output, pos++) = JsonUtf8Constant.ListSeparator;
            }

            BinaryUtil.CopyMemory(ref MemoryMarshal.GetReference(utf8Json), ref Unsafe.Add(ref output, pos), len);
            pos += len;

            SetFlagToAddListSeparatorBeforeNextItem();
        }
    }
}
