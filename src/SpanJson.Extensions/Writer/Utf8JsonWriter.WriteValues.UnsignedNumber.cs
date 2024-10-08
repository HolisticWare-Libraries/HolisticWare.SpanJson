﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Buffers;
using System.Buffers.Text;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace SpanJson
{
    partial struct Utf8JsonWriter
    {
        /// <summary>
        /// Writes the <see cref="uint"/> value (as a JSON number) as an element of a JSON array.
        /// </summary>
        /// <param name="value">The value to write.</param>
        /// <exception cref="InvalidOperationException">
        /// Thrown if this would result in invalid JSON being written (while validation is enabled).
        /// </exception>
        /// <remarks>
        /// Writes the <see cref="uint"/> using the default <see cref="StandardFormat"/> (that is, 'G'), for example: 32767.
        /// </remarks>
        public void WriteNumberValue(uint value)
            => WriteNumberValue((ulong)value);

        /// <summary>
        /// Writes the <see cref="ulong"/> value (as a JSON number) as an element of a JSON array.
        /// </summary>
        /// <param name="value">The value to write.</param>
        /// <exception cref="InvalidOperationException">
        /// Thrown if this would result in invalid JSON being written (while validation is enabled).
        /// </exception>
        /// <remarks>
        /// Writes the <see cref="ulong"/> using the default <see cref="StandardFormat"/> (that is, 'G'), for example: 32767.
        /// </remarks>
        public void WriteNumberValue(ulong value)
        {
            if (!_options.SkipValidation)
            {
                ValidateWritingValue();
            }

            if (_options.Indented)
            {
                WriteNumberValueIndented(value);
            }
            else
            {
                WriteNumberValueMinimized(value);
            }

            SetFlagToAddListSeparatorBeforeNextItem();
            _tokenType = JsonTokenType.Number;
        }

        private void WriteNumberValueMinimized(ulong value)
        {
            int maxRequired = JsonSharedConstant.MaximumFormatUInt64Length + 1; // Optionally, 1 list separator

            ref var pos = ref _pos;
            EnsureUnsafe(pos, maxRequired);

            ref byte output = ref PinnableAddress;

            if ((uint)_currentDepth > JsonSharedConstant.TooBigOrNegative)
            {
                Unsafe.Add(ref output, pos++) = JsonUtf8Constant.ListSeparator;
            }

            bool result = Utf8Formatter.TryFormat(value, FreeSpan, out int bytesWritten);
            Debug.Assert(result);
            pos += bytesWritten;
        }

        private void WriteNumberValueIndented(ulong value)
        {
            int indent = Indentation;
            Debug.Assert(indent <= 2 * JsonSharedConstant.MaxWriterDepth);

            int maxRequired = indent + JsonSharedConstant.MaximumFormatUInt64Length + 1 + JsonWriterHelper.NewLineLength; // Optionally, 1 list separator and 1-2 bytes for new line

            ref var pos = ref _pos;
            EnsureUnsafe(pos, maxRequired);

            ref byte output = ref PinnableAddress;

            if ((uint)_currentDepth > JsonSharedConstant.TooBigOrNegative)
            {
                Unsafe.Add(ref output, pos++) = JsonUtf8Constant.ListSeparator;
            }

            if (_tokenType != JsonTokenType.PropertyName)
            {
                if (_tokenType != JsonTokenType.None)
                {
                    WriteNewLine(ref output, ref pos);
                }
                JsonWriterHelper.WriteIndentation(ref output, indent, ref pos);
            }

            bool result = Utf8Formatter.TryFormat(value, FreeSpan, out int bytesWritten);
            Debug.Assert(result);
            pos += bytesWritten;
        }

        internal void WriteNumberValueAsString(ulong value)
        {
            Span<byte> utf8Number;
            unsafe
            {
                // Cannot create a span directly since it gets assigned to parameter and passed down.
                byte* ptr = stackalloc byte[JsonSharedConstant.MaximumFormatUInt64Length];
                utf8Number = new Span<byte>(ptr, JsonSharedConstant.MaximumFormatUInt64Length);
            }
            bool result = Utf8Formatter.TryFormat(value, utf8Number, out int bytesWritten);
            Debug.Assert(result);
            WriteNumberValueAsStringUnescaped(utf8Number.Slice(0, bytesWritten));
        }
    }
}
