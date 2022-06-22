﻿using System;
using System.Buffers;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using CuteAnt;
using SpanJson.Dynamic;
using SpanJson.Helpers;
using SpanJson.Internal;

namespace SpanJson
{
    public ref partial struct JsonReader<TSymbol> where TSymbol : struct
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void EnsureUtf8InnerBufferCreated()
        {
            if (_utf8Json.NonEmpty()) { return; }
            _utf8Json = new ArraySegment<byte>(_utf8Span.ToArray());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public sbyte ReadUtf8SByte()
        {
            return checked((sbyte)ReadUtf8NumberInt64(ref MemoryMarshal.GetReference(_utf8Span), ref _pos, _length));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public short ReadUtf8Int16()
        {
            return checked((short)ReadUtf8NumberInt64(ref MemoryMarshal.GetReference(_utf8Span), ref _pos, _length));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int ReadUtf8Int32()
        {
            return checked((int)ReadUtf8NumberInt64(ref MemoryMarshal.GetReference(_utf8Span), ref _pos, _length));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public long ReadUtf8Int64()
        {
            return ReadUtf8NumberInt64(ref MemoryMarshal.GetReference(_utf8Span), ref _pos, _length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte ReadUtf8Byte()
        {
            return checked((byte)ReadUtf8NumberUInt64(ref MemoryMarshal.GetReference(_utf8Span), ref _pos, _length));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ushort ReadUtf8UInt16()
        {
            return checked((ushort)ReadUtf8NumberUInt64(ref MemoryMarshal.GetReference(_utf8Span), ref _pos, _length));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public uint ReadUtf8UInt32()
        {
            return checked((uint)ReadUtf8NumberUInt64(ref MemoryMarshal.GetReference(_utf8Span), ref _pos, _length));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ulong ReadUtf8UInt64()
        {
            return ReadUtf8NumberUInt64(ref MemoryMarshal.GetReference(_utf8Span), ref _pos, _length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float ReadUtf8Single()
        {
            var span = ReadUtf8NumberSpan();
            if (!Utf8Parser.TryParse(span, out float value, out var consumed) || span.Length != consumed)
            {
                ThrowHelper.ThrowJsonParserException(JsonParserException.ParserError.InvalidNumberFormat, _pos);
            }

            return value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double ReadUtf8Double()
        {
            var span = ReadUtf8NumberSpan();
            if (!Utf8Parser.TryParse(span, out double value, out var consumed) || span.Length != consumed)
            {
                ThrowHelper.ThrowJsonParserException(JsonParserException.ParserError.InvalidNumberFormat, _pos);
            }

            return value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ReadOnlySpan<byte> ReadUtf8NumberSpan()
        {
            ref var pos = ref _pos;
            ref byte bStart = ref MemoryMarshal.GetReference(_utf8Span);
            SkipWhitespaceUtf8(ref bStart, ref pos, _length);
            if (TryFindEndOfUtf8Number(ref bStart, pos, _length, out var bytesConsumed))
            {
                var result = _utf8Span.Slice(pos, bytesConsumed);
                pos += bytesConsumed;
                return result;
            }

            throw ThrowHelper.GetJsonParserException(JsonParserException.ParserError.EndOfData, pos);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal ArraySegment<byte> ReadUtf8RawNumber()
        {
            ref var pos = ref _pos;
            ref byte bStart = ref MemoryMarshal.GetReference(_utf8Span);
            SkipWhitespaceUtf8(ref bStart, ref pos, _length);
            if (TryFindEndOfUtf8Number(ref bStart, pos, _length, out var bytesConsumed))
            {
                var result = _utf8Json.Slice(pos, bytesConsumed);
                pos += bytesConsumed;
                return result;
            }

            throw ThrowHelper.GetJsonParserException(JsonParserException.ParserError.EndOfData, pos);
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static long ReadUtf8NumberInt64(ref byte bStart, ref int pos, uint length)
        {
            var currentByte = SkipWhitespaceUtf8(ref bStart, ref pos, length);
            if ((uint)pos >= length)
            {
                ThrowHelper.ThrowJsonParserException(JsonParserException.ParserError.EndOfData, pos);
            }

            var neg = false;
            if (currentByte == (byte)'-')
            {
                pos++;
                neg = true;

                if ((uint)pos >= length) // we still need one digit
                {
                    ThrowHelper.ThrowJsonParserException(JsonParserException.ParserError.EndOfData, pos);
                }
            }

            var result = ReadUtf8NumberDigits(ref bStart, ref pos, length);
            return neg ? unchecked(-(long)result) : checked((long)result);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static ulong ReadUtf8NumberUInt64(ref byte bStart, ref int pos, uint length)
        {
            SkipWhitespaceUtf8(ref bStart, ref pos, length);
            if ((uint)pos >= length)
            {
                ThrowHelper.ThrowJsonParserException(JsonParserException.ParserError.EndOfData, pos);
            }

            return ReadUtf8NumberDigits(ref bStart, ref pos, length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static ulong ReadUtf8NumberDigits(ref byte b, ref int pos, uint length)
        {
            uint value;
            var result = Unsafe.AddByteOffset(ref b, (IntPtr)pos) - 48UL;
            if (result > 9ul) // this includes '-'
            {
                ThrowHelper.ThrowJsonParserException(JsonParserException.ParserError.InvalidNumberFormat, pos);
            }

            pos++;
            while ((uint)pos < length && (value = Unsafe.AddByteOffset(ref b, (IntPtr)pos) - 48U) <= 9u)
            {
                result = checked(result * 10ul + value);
                pos++;
            }

            return result;
        }

        public bool ReadUtf8Boolean()
        {
            const uint _LE = (byte)'e';
            const uint _UE = (byte)'E';

            ref var pos = ref _pos;
            ref byte bStart = ref MemoryMarshal.GetReference(_utf8Span);
            SkipWhitespaceUtf8(ref bStart, ref pos, _length);
            if ((uint)pos + 4u <= _length)
            {
                ref var start = ref Unsafe.AddByteOffset(ref bStart, (IntPtr)pos);
                var value = Unsafe.ReadUnaligned<uint>(ref start);
                switch (value)
                {
                    case 0x65757274U: // eurt
                    case 0x65757254U: // eurT
                    case 0x45555254U: // EURT
                        pos += 4;
                        return true;

                    case 0x736C6166U: // slaf
                    case 0x736C6146U: // slaF
                    case 0x534C4146U: // SLAF
                        if ((uint)pos + 5u <= _length)
                        {
                            var lastChar = Unsafe.AddByteOffset(ref bStart, (IntPtr)(pos + 4));
                            if (lastChar == _LE || lastChar == _UE)
                            {
                                pos += 5;
                                return false;
                            }
                        }
                        break;
                }
            }

            throw ThrowHelper.GetJsonParserException(JsonParserException.ParserError.InvalidSymbol, JsonParserException.ValueType.Bool, pos);
        }

        public char ReadUtf8Char()
        {
            ref var pos = ref _pos;
            ref byte bStart = ref MemoryMarshal.GetReference(_utf8Span);
            SkipWhitespaceUtf8(ref bStart, ref pos, _length);
            var span = ReadUtf8StringSpanInternal(ref bStart, ref pos, _length, out _);
            return ReadUtf8CharInternal(span, pos);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static char ReadUtf8CharInternal(in ReadOnlySpan<byte> span, int position)
        {
            if (span.Length == 1)
            {
                return (char)span[0];
            }

            ref byte source = ref MemoryMarshal.GetReference(span);
            var offset = (IntPtr)0;
            uint nValue = Unsafe.AddByteOffset(ref source, offset);
            if (nValue == JsonUtf8Constant.ReverseSolidus)
            {
                nValue = Unsafe.AddByteOffset(ref source, offset + 1);
                switch (nValue)
                {
                    case JsonUtf8Constant.DoubleQuote:
                        return JsonUtf16Constant.DoubleQuote;
                    case JsonUtf8Constant.ReverseSolidus:
                        return JsonUtf16Constant.ReverseSolidus;
                    case JsonUtf8Constant.Solidus:
                        return JsonUtf16Constant.Solidus;
                    case 'b':
                        return '\b';
                    case 'f':
                        return '\f';
                    case 'n':
                        return '\n';
                    case 'r':
                        return '\r';
                    case 't':
                        return '\t';
                    case 'u':
                        if (Utf8Parser.TryParse(span.Slice(2, 4), out int value, out _, 'X'))
                        {
                            return (char)value;
                        }

                        ThrowHelper.ThrowJsonParserException(JsonParserException.ParserError.InvalidSymbol, position);
                        break;
                }
            }

            Span<char> charSpan = stackalloc char[1];
            TextEncodings.Utf8.GetChars(span, charSpan);
            return charSpan[0];
        }

        public DateTime ReadUtf8DateTime()
        {
            ref var pos = ref _pos;
            ref byte bStart = ref MemoryMarshal.GetReference(_utf8Span);
            SkipWhitespaceUtf8(ref bStart, ref pos, _length);
            var span = ReadUtf8StringSpanInternal(ref bStart, ref pos, _length, out var backslashIdx);
            return (uint)backslashIdx > JsonSharedConstant.TooBigOrNegative ? ParseUtf8DateTime(span, pos) : ParseUtf8DateTimeAllocating(span, backslashIdx, pos);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static DateTime ParseUtf8DateTime(in ReadOnlySpan<byte> span, int pos)
        {
            if (JsonHelpers.TryParseAsISO(span, out DateTime value))
            {
                return value;
            }

            throw ThrowHelper.GetJsonParserException(JsonParserException.ParserError.InvalidSymbol, JsonParserException.ValueType.DateTime, pos);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static DateTime ParseUtf8DateTimeAllocating(in ReadOnlySpan<byte> input, int backslashIdx, int pos)
        {
            byte[] unescapedArray = null;
            Span<byte> utf8Unescaped = (uint)input.Length <= JsonSharedConstant.StackallocByteThresholdU ?
                stackalloc byte[JsonSharedConstant.StackallocByteThreshold] :
                (unescapedArray = ArrayPool<byte>.Shared.Rent(input.Length));
            try
            {
                JsonReaderHelper.Unescape(input, utf8Unescaped, backslashIdx, out var written);
                if (JsonHelpers.TryParseAsISO(utf8Unescaped.Slice(0, written), out DateTime value))
                {
                    return value;
                }

                throw ThrowHelper.GetJsonParserException(JsonParserException.ParserError.InvalidSymbol, JsonParserException.ValueType.DateTime, pos);
            }
            finally
            {
                if (unescapedArray is not null) { ArrayPool<byte>.Shared.Return(unescapedArray); }
            }
        }

        public DateTimeOffset ReadUtf8DateTimeOffset()
        {
            ref var pos = ref _pos;
            ref byte bStart = ref MemoryMarshal.GetReference(_utf8Span);
            SkipWhitespaceUtf8(ref bStart, ref pos, _length);
            var span = ReadUtf8StringSpanInternal(ref bStart, ref pos, _length, out var backslashIdx);
            return (uint)backslashIdx > JsonSharedConstant.TooBigOrNegative ? ParseUtf8DateTimeOffset(span, pos) : ParseUtf8DateTimeOffsetAllocating(span, backslashIdx, pos);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static DateTimeOffset ParseUtf8DateTimeOffset(in ReadOnlySpan<byte> span, int pos)
        {
            if (JsonHelpers.TryParseAsISO(span, out DateTimeOffset value))
            {
                return value;
            }

            throw ThrowHelper.GetJsonParserException(JsonParserException.ParserError.InvalidSymbol, JsonParserException.ValueType.DateTimeOffset, pos);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static DateTimeOffset ParseUtf8DateTimeOffsetAllocating(in ReadOnlySpan<byte> input, int backslashIdx, int pos)
        {
            byte[] unescapedArray = null;
            Span<byte> utf8Unescaped = (uint)input.Length <= JsonSharedConstant.StackallocByteThresholdU ?
                stackalloc byte[JsonSharedConstant.StackallocByteThreshold] :
                (unescapedArray = ArrayPool<byte>.Shared.Rent(input.Length));
            try
            {
                JsonReaderHelper.Unescape(input, utf8Unescaped, backslashIdx, out var written);
                if (JsonHelpers.TryParseAsISO(utf8Unescaped.Slice(0, written), out DateTimeOffset value))
                {
                    return value;
                }

                throw ThrowHelper.GetJsonParserException(JsonParserException.ParserError.InvalidSymbol, JsonParserException.ValueType.DateTimeOffset, pos);
            }
            finally
            {
                if (unescapedArray is not null) { ArrayPool<byte>.Shared.Return(unescapedArray); }
            }
        }

        public TimeSpan ReadUtf8TimeSpan()
        {
            ref var pos = ref _pos;
            ref byte bStart = ref MemoryMarshal.GetReference(_utf8Span);
            SkipWhitespaceUtf8(ref bStart, ref pos, _length);
            var span = ReadUtf8StringSpanInternal(ref bStart, ref pos, _length, out var backslashIdx);
            return (uint)backslashIdx > JsonSharedConstant.TooBigOrNegative ? ParseUtf8TimeSpan(span, pos) : ParseUtf8TimeSpanAllocating(span, backslashIdx, pos);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static TimeSpan ParseUtf8TimeSpan(in ReadOnlySpan<byte> span, int pos)
        {
            if (Utf8Parser.TryParse(span, out TimeSpan result, out var bytesConsumed) && span.Length == bytesConsumed)
            {
                return result;
            }

            throw ThrowHelper.GetJsonParserException(JsonParserException.ParserError.InvalidSymbol, JsonParserException.ValueType.TimeSpan, pos);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static TimeSpan ParseUtf8TimeSpanAllocating(in ReadOnlySpan<byte> input, int backslashIdx, int pos)
        {
            byte[] unescapedArray = null;
            Span<byte> utf8Unescaped = (uint)input.Length <= JsonSharedConstant.StackallocByteThresholdU ?
                stackalloc byte[JsonSharedConstant.StackallocByteThreshold] :
                (unescapedArray = ArrayPool<byte>.Shared.Rent(input.Length));
            try
            {
                JsonReaderHelper.Unescape(input, utf8Unescaped, backslashIdx, out var written);
                if (Utf8Parser.TryParse(utf8Unescaped.Slice(0, written), out TimeSpan result, out var bytesConsumed) && written == bytesConsumed)
                {
                    return result;
                }

                throw ThrowHelper.GetJsonParserException(JsonParserException.ParserError.InvalidSymbol, JsonParserException.ValueType.TimeSpan, pos);
            }
            finally
            {
                if (unescapedArray is not null) { ArrayPool<byte>.Shared.Return(unescapedArray); }
            }
        }

        public Guid ReadUtf8Guid()
        {
            ref var pos = ref _pos;
            ref byte bStart = ref MemoryMarshal.GetReference(_utf8Span);
            SkipWhitespaceUtf8(ref bStart, ref pos, _length);
            var span = ReadUtf8StringSpanInternal(ref bStart, ref pos, _length, out var backslashIdx);
            return (uint)backslashIdx > JsonSharedConstant.TooBigOrNegative ? ParseUtf8Guid(span, pos) : ParseUtf8GuidAllocating(span, backslashIdx, pos);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Guid ParseUtf8Guid(in ReadOnlySpan<byte> span, int pos)
        {
            if (Utf8Parser.TryParse(span, out Guid result, out var bytesConsumed) && span.Length == bytesConsumed)
            {
                return result;
            }

            throw ThrowHelper.GetJsonParserException(JsonParserException.ParserError.InvalidSymbol, JsonParserException.ValueType.Guid, pos);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static Guid ParseUtf8GuidAllocating(in ReadOnlySpan<byte> input, int backslashIdx, int pos)
        {
            byte[] unescapedArray = null;
            Span<byte> utf8Unescaped = (uint)input.Length <= JsonSharedConstant.StackallocByteThresholdU ?
                stackalloc byte[JsonSharedConstant.StackallocByteThreshold] :
                (unescapedArray = ArrayPool<byte>.Shared.Rent(input.Length));
            try
            {
                JsonReaderHelper.Unescape(input, utf8Unescaped, backslashIdx, out var written);
                if (Utf8Parser.TryParse(utf8Unescaped.Slice(0, written), out Guid result, out var bytesConsumed) && written == bytesConsumed)
                {
                    return result;
                }

                throw ThrowHelper.GetJsonParserException(JsonParserException.ParserError.InvalidSymbol, JsonParserException.ValueType.Guid, pos);
            }
            finally
            {
                if (unescapedArray is not null) { ArrayPool<byte>.Shared.Return(unescapedArray); }
            }
        }

        public CombGuid ReadUtf8CombGuid()
        {
            ref var pos = ref _pos;
            ref byte bStart = ref MemoryMarshal.GetReference(_utf8Span);
            SkipWhitespaceUtf8(ref bStart, ref pos, _length);
            var span = ReadUtf8StringSpanInternal(ref bStart, ref pos, _length, out var backslashIdx);
            return (uint)backslashIdx > JsonSharedConstant.TooBigOrNegative ? ParseUtf8CombGuid(span, pos) : ParseUtf8CombGuidAllocating(span, backslashIdx, pos);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static CombGuid ParseUtf8CombGuid(in ReadOnlySpan<byte> span, int pos)
        {
#if NETSTANDARD2_0
            if (CombGuid.TryParse(TextEncodings.Utf8.GetString(span), CombGuidSequentialSegmentType.Comb, out CombGuid result))
#else
            if (CombGuid.TryParse(span, CombGuidSequentialSegmentType.Comb, out CombGuid result, out var bytesConsumed) && span.Length == bytesConsumed)
#endif
            {
                return result;
            }

            throw ThrowHelper.GetJsonParserException(JsonParserException.ParserError.InvalidSymbol, JsonParserException.ValueType.CombGuid, pos);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static CombGuid ParseUtf8CombGuidAllocating(in ReadOnlySpan<byte> input, int backslashIdx, int pos)
        {
            byte[] unescapedArray = null;
            Span<byte> utf8Unescaped = (uint)input.Length <= JsonSharedConstant.StackallocByteThresholdU ?
                stackalloc byte[JsonSharedConstant.StackallocByteThreshold] :
                (unescapedArray = ArrayPool<byte>.Shared.Rent(input.Length));
            try
            {
                JsonReaderHelper.Unescape(input, utf8Unescaped, backslashIdx, out var written);
#if NETSTANDARD2_0
                if (CombGuid.TryParse(TextEncodings.Utf8.GetString(utf8Unescaped.Slice(0, written)), CombGuidSequentialSegmentType.Comb, out CombGuid result))
#else
                if (CombGuid.TryParse(utf8Unescaped.Slice(0, written), CombGuidSequentialSegmentType.Comb, out CombGuid result, out var bytesConsumed) && written == bytesConsumed)
#endif
                {
                    return result;
                }

                throw ThrowHelper.GetJsonParserException(JsonParserException.ParserError.InvalidSymbol, JsonParserException.ValueType.CombGuid, pos);
            }
            finally
            {
                if (unescapedArray is not null) { ArrayPool<byte>.Shared.Return(unescapedArray); }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string ReadUtf8EscapedName()
        {
            var span = ReadUtf8VerbatimNameSpan(out int backslashIdx);

            return (uint)backslashIdx > JsonSharedConstant.TooBigOrNegative
                ? TextEncodings.Utf8.GetString(span)
                : JsonReaderHelper.GetUnescapedString(span, backslashIdx);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ReadOnlySpan<byte> ReadUtf8EscapedNameSpan()
        {
            var span = ReadUtf8VerbatimNameSpan(out int backslashIdx);

            return (uint)backslashIdx > JsonSharedConstant.TooBigOrNegative ? span : UnescapeUtf8Bytes(span, backslashIdx);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ReadOnlySpan<byte> ReadUtf8VerbatimNameSpan()
        {
            return ReadUtf8VerbatimNameSpan(out _);
        }

        public ReadOnlySpan<byte> ReadUtf8VerbatimNameSpan(out int backslashIdx)
        {
            ref var pos = ref _pos;
            ref byte bStart = ref MemoryMarshal.GetReference(_utf8Span);
            SkipWhitespaceUtf8(ref bStart, ref pos, _length);
            var span = ReadUtf8StringSpanInternal(ref bStart, ref pos, _length, out backslashIdx);
            var currentByte = SkipWhitespaceUtf8(ref bStart, ref pos, _length);
            pos++;
            if (currentByte != JsonUtf8Constant.NameSeparator)
            {
                ThrowHelper.ThrowJsonParserException(JsonParserException.ParserError.ExpectedDoubleQuote, pos);
            }

            return span;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte[] ReadUtf8BytesFromBase64()
        {
            ref var pos = ref _pos;
            ref byte bStart = ref MemoryMarshal.GetReference(_utf8Span);
            if (ReadUtf8IsNullInternal(ref bStart, ref pos, _length))
            {
                return null;
            }

            byte[] bytes;
            var span = ReadUtf8StringSpanInternal(ref bStart, ref pos, _length, out int backslashIdx);
            var result = (uint)backslashIdx > JsonSharedConstant.TooBigOrNegative
                 ? JsonReaderHelper.TryDecodeBase64(span, out bytes)
                : JsonReaderHelper.TryGetUnescapedBase64Bytes(span, backslashIdx, out bytes);
            return result ? bytes : null;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string ReadUtf8String()
        {
            ref var pos = ref _pos;
            ref byte bStart = ref MemoryMarshal.GetReference(_utf8Span);
            if (ReadUtf8IsNullInternal(ref bStart, ref pos, _length))
            {
                return null;
            }

            var span = ReadUtf8StringSpanInternal(ref bStart, ref pos, _length, out int backslashIdx);

            return (uint)backslashIdx > JsonSharedConstant.TooBigOrNegative
                ? TextEncodings.Utf8.GetString(span)
                : JsonReaderHelper.GetUnescapedString(span, backslashIdx);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ReadOnlySpan<byte> ReadUtf8StringSpan()
        {
            var span = ReadUtf8VerbatimStringSpan(out int backslashIdx);

            return (uint)backslashIdx > JsonSharedConstant.TooBigOrNegative ? span : UnescapeUtf8Bytes(span, backslashIdx);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ReadOnlySpan<byte> ReadUtf8VerbatimStringSpan()
        {
            return ReadUtf8VerbatimStringSpan(out _);
        }

        public ReadOnlySpan<byte> ReadUtf8VerbatimStringSpan(out int backslashIdx)
        {
            ref var pos = ref _pos;
            ref byte bStart = ref MemoryMarshal.GetReference(_utf8Span);
            if (ReadUtf8IsNullInternal(ref bStart, ref pos, _length))
            {
                backslashIdx = -1;
                return default/*JsonUtf8Constant.NullTerminator*/;
            }

            return ReadUtf8StringSpanInternal(ref bStart, ref pos, _length, out backslashIdx);
        }

        private static ReadOnlySpan<byte> UnescapeUtf8Bytes(in ReadOnlySpan<byte> span, int backslashIdx)
        {
            // not necessarily correct, just needs to be a good upper bound
            // this gets slightly too high, as the normal escapes are two bytes, and the \u1234 escapes are 6 bytes, but we only need 4
            var unescapedLength = span.Length;
            Span<byte> result = new byte[unescapedLength];
            JsonReaderHelper.Unescape(span, result, backslashIdx, out var written);
            return result.Slice(0, written);
        }

        private static ReadOnlySpan<byte> ReadUtf8StringSpanInternal(ref byte bStart, ref int pos, uint length, out int backslashIdx)
        {
            if ((uint)pos + 2u <= length)
            {
                ref var stringStart = ref Unsafe.AddByteOffset(ref bStart, (IntPtr)pos++);
                if (stringStart != JsonUtf8Constant.String)
                {
                    ThrowHelper.ThrowJsonParserException(JsonParserException.ParserError.ExpectedDoubleQuote, pos);
                }

                // We should also get info about how many escaped chars exist from here
                if (TryFindEndOfUtf8String(ref stringStart, length - (uint)pos, out int stringLength, out backslashIdx))
                {
#if NETSTANDARD2_0
                    unsafe
                    {
                        var result = new ReadOnlySpan<byte>(Unsafe.AsPointer(ref Unsafe.AddByteOffset(ref stringStart, (IntPtr)1)), stringLength - 1);
                        pos += stringLength; // skip the doublequote too
                        return result;
                    }
#else
                    var result = MemoryMarshal.CreateReadOnlySpan(ref Unsafe.AddByteOffset(ref stringStart, (IntPtr)1), stringLength - 1);
                    pos += stringLength; // skip the doublequote too
                    return result;
#endif
                }
            }

            throw ThrowHelper.GetJsonParserException(JsonParserException.ParserError.ExpectedDoubleQuote, pos);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ReadOnlySpan<byte> ReadUtf8StringSpanWithQuotes()
        {
            ref var pos = ref _pos;
            ref var bStart = ref MemoryMarshal.GetReference(_utf8Span);

            return ReadUtf8StringSpanWithQuotes(ref bStart, ref pos, _length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void ReadUtf8StringSpanWithQuotes(out ArraySegment<byte> result)
        {
            ref var pos = ref _pos;
            ref var bStart = ref MemoryMarshal.GetReference(_utf8Span);

            ReadUtf8StringSpanWithQuotes(ref bStart, ref pos, _length, out result);
        }

        /// <summary>
        ///     Includes the quotes on each end
        /// </summary>
        internal static ReadOnlySpan<byte> ReadUtf8StringSpanWithQuotes(ref byte bStart, ref int pos, uint length)
        {
            if ((uint)pos + 2u <= length)
            {
                ref var stringStart = ref Unsafe.AddByteOffset(ref bStart, (IntPtr)pos++);
                if (stringStart != JsonUtf8Constant.String)
                {
                    ThrowHelper.ThrowJsonParserException(JsonParserException.ParserError.ExpectedDoubleQuote, pos);
                }

                // We should also get info about how many escaped chars exist from here
                if (TryFindEndOfUtf8String(ref stringStart, length - (uint)pos, out int stringLength, out _))
                {
#if NETSTANDARD2_0
                    unsafe
                    {
                        var result = new ReadOnlySpan<byte>(Unsafe.AsPointer(ref stringStart), stringLength + 1);
                        pos += stringLength; // skip the doublequote too
                        return result;
                    }
#else
                    var result = MemoryMarshal.CreateReadOnlySpan(ref stringStart, stringLength + 1);
                    pos += stringLength; // skip the doublequote too
                    return result;
#endif
                }
            }

            throw ThrowHelper.GetJsonParserException(JsonParserException.ParserError.ExpectedDoubleQuote, pos);
        }
        internal void ReadUtf8StringSpanWithQuotes(ref byte bStart, ref int pos, uint length, out ArraySegment<byte> result)
        {
            if ((uint)pos + 2u <= length)
            {
                var offset = pos;
                ref var stringStart = ref Unsafe.AddByteOffset(ref bStart, (IntPtr)pos++);
                if (stringStart != JsonUtf8Constant.String)
                {
                    ThrowHelper.ThrowJsonParserException(JsonParserException.ParserError.ExpectedDoubleQuote, pos);
                }

                // We should also get info about how many escaped chars exist from here
                if (TryFindEndOfUtf8String(ref stringStart, length - (uint)pos, out int stringLength, out _))
                {
                    result = _utf8Json.Slice(offset, stringLength + 1);
                    pos += stringLength; // skip the doublequote too
                    return;
                }
            }

            throw ThrowHelper.GetJsonParserException(JsonParserException.ParserError.ExpectedDoubleQuote, pos);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public decimal ReadUtf8Decimal()
        {
            if (Utf8Parser.TryParse(ReadUtf8NumberSpan(), out decimal result, out _))
            {
                return result;
            }

            throw ThrowHelper.GetJsonParserException(JsonParserException.ParserError.InvalidNumberFormat, _pos);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool ReadUtf8IsNull()
        {
            return ReadUtf8IsNullInternal(ref MemoryMarshal.GetReference(_utf8Span), ref _pos, _length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool ReadUtf8IsNullInternal(ref byte bStart, ref int pos, uint length)
        {
            SkipWhitespaceUtf8(ref bStart, ref pos, length);
            ref var start = ref Unsafe.AddByteOffset(ref bStart, (IntPtr)pos);
            if ((uint)pos + 4u <= length && Unsafe.ReadUnaligned<uint>(ref start) == 0x6C6C756EU /* llun */)
            {
                pos += 4;
                return true;
            }

            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ReadUtf8Null()
        {
            if (!ReadUtf8IsNull())
            {
                ThrowHelper.ThrowJsonParserException(JsonParserException.ParserError.InvalidSymbol, _pos);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static uint SkipWhitespaceUtf8(ref byte bStart, ref int pos, uint length)
        {
            uint currentByte = Unsafe.AddByteOffset(ref bStart, (IntPtr)pos);
            switch (currentByte)
            {
                case JsonUtf8Constant.Space:
                case JsonUtf8Constant.Tab:
                case JsonUtf8Constant.CarriageReturn:
                case JsonUtf8Constant.LineFeed:
                    pos++;
                    break;
                default: return currentByte;
            }
            return SkipWhitespaceUtf8Slow(ref bStart, ref pos, length);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static uint SkipWhitespaceUtf8Slow(ref byte bStart, ref int pos, uint length)
        {
            uint currentByte = 0u;
            while ((uint)pos < length)
            {
                currentByte = Unsafe.AddByteOffset(ref bStart, (IntPtr)pos);
                switch (currentByte)
                {
                    case JsonUtf8Constant.Space:
                    case JsonUtf8Constant.Tab:
                    case JsonUtf8Constant.CarriageReturn:
                    case JsonUtf8Constant.LineFeed:
                        pos++;
                        continue;
                    default:
                        return currentByte;
                }
            }
            return currentByte;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ReadUtf8BeginArrayOrThrow()
        {
            if (!ReadUtf8BeginArray())
            {
                ThrowHelper.ThrowJsonParserException(JsonParserException.ParserError.ExpectedBeginArray, _pos);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool ReadUtf8BeginArray()
        {
            ref var pos = ref _pos;
            ref byte bStart = ref MemoryMarshal.GetReference(_utf8Span);
            var currentByte = SkipWhitespaceUtf8(ref bStart, ref pos, _length);
            if ((uint)pos < _length && currentByte == JsonUtf8Constant.BeginArray)
            {
                pos++;
                return true;
            }

            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryReadUtf8IsEndArrayOrValueSeparator(ref int count)
        {
            ref var pos = ref _pos;
            ref byte bStart = ref MemoryMarshal.GetReference(_utf8Span);
            var currentByte = SkipWhitespaceUtf8(ref bStart, ref pos, _length);
            if ((uint)pos < _length && currentByte == JsonUtf8Constant.EndArray)
            {
                pos++;
                return true;
            }

            if (count++ > 0)
            {
                if ((uint)pos < _length && Unsafe.AddByteOffset(ref bStart, (IntPtr)pos) == JsonUtf8Constant.ValueSeparator)
                {
                    pos++;
                    return false;
                }

                ThrowHelper.ThrowJsonParserException(JsonParserException.ParserError.ExpectedSeparator, pos);
            }

            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool ReadUtf8IsBeginObject()
        {
            ref var pos = ref _pos;
            ref byte bStart = ref MemoryMarshal.GetReference(_utf8Span);
            var currentByte = SkipWhitespaceUtf8(ref bStart, ref pos, _length);
            if (currentByte == JsonUtf8Constant.BeginObject)
            {
                pos++;
                return true;
            }

            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ReadUtf8BeginObjectOrThrow()
        {
            if (!ReadUtf8IsBeginObject())
            {
                ThrowHelper.ThrowJsonParserException(JsonParserException.ParserError.ExpectedBeginObject, _pos);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ReadUtf8EndObjectOrThrow()
        {
            if (!ReadUtf8IsEndObject())
            {
                ThrowHelper.ThrowJsonParserException(JsonParserException.ParserError.ExpectedEndObject, _pos);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ReadUtf8EndArrayOrThrow()
        {
            if (!ReadUtf8IsEndArray())
            {
                ThrowHelper.ThrowJsonParserException(JsonParserException.ParserError.ExpectedEndArray, _pos);
            }
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool ReadUtf8IsEndObject()
        {
            ref var pos = ref _pos;
            ref byte bStart = ref MemoryMarshal.GetReference(_utf8Span);
            var currentByte = SkipWhitespaceUtf8(ref bStart, ref pos, _length);
            if (currentByte == JsonUtf8Constant.EndObject)
            {
                pos++;
                return true;
            }

            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool ReadUtf8IsEndArray()
        {
            ref var pos = ref _pos;
            ref byte bStart = ref MemoryMarshal.GetReference(_utf8Span);
            var currentByte = SkipWhitespaceUtf8(ref bStart, ref pos, _length);
            if (currentByte == JsonUtf8Constant.EndArray)
            {
                pos++;
                return true;
            }

            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryReadUtf8IsEndObjectOrValueSeparator(ref int count)
        {
            ref var pos = ref _pos;
            ref byte bStart = ref MemoryMarshal.GetReference(_utf8Span);
            var currentByte = SkipWhitespaceUtf8(ref bStart, ref pos, _length);
            if ((uint)pos < _length && currentByte == JsonUtf8Constant.EndObject)
            {
                pos++;
                return true;
            }

            if (count++ > 0)
            {
                if (Unsafe.AddByteOffset(ref bStart, (IntPtr)pos) == JsonUtf8Constant.ValueSeparator)
                {
                    pos++;
                    return false;
                }

                ThrowHelper.ThrowJsonParserException(JsonParserException.ParserError.ExpectedSeparator, pos);
            }

            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Version ReadUtf8Version()
        {
            var stringValue = ReadUtf8String();
            if (stringValue is null)
            {
                return default;
            }

            return Version.Parse(stringValue);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Uri ReadUtf8Uri()
        {
            var stringValue = ReadUtf8String();
            if (stringValue is null)
            {
                return default;
            }

            if (Uri.TryCreate(stringValue, UriKind.RelativeOrAbsolute, out Uri value))
            {
                return value;
            }
            throw ThrowHelper.GetJsonParserException(JsonParserException.ParserError.InvalidSymbol, JsonParserException.ValueType.Uri, _pos);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ReadUtf8SymbolOrThrow(byte constant)
        {
            if (!ReadUtf8IsSymbol(constant))
            {
                ThrowHelper.ThrowJsonParserException(JsonParserException.ParserError.InvalidSymbol, _pos);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool ReadUtf8IsSymbol(byte constant)
        {
            ref var pos = ref _pos;
            ref byte bStart = ref MemoryMarshal.GetReference(_utf8Span);
            var currentByte = SkipWhitespaceUtf8(ref bStart, ref pos, _length);
            if ((uint)pos < _length && currentByte == constant)
            {
                pos++;
                return true;
            }

            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SkipNextUtf8Segment()
        {
            SkipNextUtf8Segment(ref MemoryMarshal.GetReference(_utf8Span), ref _pos, _length, 0);
        }

        private static void SkipNextUtf8Segment(ref byte bStart, ref int pos, uint length, int stack)
        {
            while ((uint)pos < length)
            {
                var token = ReadUtf8NextTokenInternal(ref bStart, ref pos, length);
                switch (token)
                {
                    case JsonTokenType.None:
                        return;
                    case JsonTokenType.BeginArray:
                    case JsonTokenType.BeginObject:
                        {
                            pos++;
                            stack++;
                            continue;
                        }
                    case JsonTokenType.EndObject:
                    case JsonTokenType.EndArray:
                        {
                            pos++;
                            if (stack - 1 > 0)
                            {
                                stack--;
                                continue;
                            }

                            return;
                        }
                    case JsonTokenType.Number:
                    case JsonTokenType.String:
                    case JsonTokenType.True:
                    case JsonTokenType.False:
                    case JsonTokenType.Null:
                    case JsonTokenType.ValueSeparator:
                    case JsonTokenType.NameSeparator:
                        {
                            do
                            {
                                SkipNextUtf8ValueInternal(ref bStart, ref pos, length, token);
                                token = ReadUtf8NextTokenInternal(ref bStart, ref pos, length);
                            } while (stack > 0 && (byte)token > 4); // No None or the Begin/End-Array/Object tokens

                            if (stack > 0)
                            {
                                continue;
                            }

                            return;
                        }
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SkipNextUtf8Value(JsonTokenType token)
        {
            SkipNextUtf8ValueInternal(ref MemoryMarshal.GetReference(_utf8Span), ref _pos, _length, token);
        }

        private static void SkipNextUtf8ValueInternal(ref byte bStart, ref int pos, uint length, JsonTokenType token)
        {
            switch (token)
            {
                case JsonTokenType.None:
                    break;
                case JsonTokenType.BeginObject:
                case JsonTokenType.EndObject:
                case JsonTokenType.BeginArray:
                case JsonTokenType.EndArray:
                case JsonTokenType.ValueSeparator:
                case JsonTokenType.NameSeparator:
                    pos++;
                    break;
                case JsonTokenType.Number:
                    {
                        if (TryFindEndOfUtf8Number(ref bStart, pos, length, out var bytesConsumed))
                        {
                            pos += bytesConsumed;
                        }

                        break;
                    }
                case JsonTokenType.String:
                    {
                        if (SkipUtf8String(ref bStart, ref pos, length))
                        {
                            return;
                        }

                        ThrowHelper.ThrowJsonParserException(JsonParserException.ParserError.ExpectedDoubleQuote, pos);
                        break;
                    }
                case JsonTokenType.Null:
                case JsonTokenType.True:
                    pos += 4;
                    break;
                case JsonTokenType.False:
                    pos += 5;
                    break;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool TryFindEndOfUtf8Number(ref byte bStart, int pos, uint length, out int bytesConsumed)
        {
            uint nValue;
            var i = pos;
            for (; (uint)i < length; i++)
            {
                nValue = Unsafe.AddByteOffset(ref bStart, (IntPtr)i);
                if (!IsNumericSymbol(nValue))
                {
                    break;
                }
            }

            if ((uint)i > (uint)pos)
            {
                bytesConsumed = i - pos;
                return true;
            }

            bytesConsumed = default;
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool TryFindEndOfUtf8String(ref byte stringStart, uint length, out int stringLength, out int backslashIdx)
        {
            const uint DoubleQuote = JsonUtf8Constant.String;

            IntPtr offset = (IntPtr)1; // igore '"'
            var idx = SpanHelpers.IndexOfAny(ref Unsafe.AddByteOffset(ref stringStart, offset), JsonUtf8Constant.String, JsonUtf8Constant.ReverseSolidus, (int)length);

            if ((uint)idx > JsonSharedConstant.TooBigOrNegative) // -1
            {
                stringLength = 0;
                backslashIdx = -1;
                return false;
            }

            uint foundByte = Unsafe.AddByteOffset(ref stringStart, offset + idx);
            if (foundByte == DoubleQuote)
            {
                stringLength = idx + 1;
                backslashIdx = -1;
                return true;
            }

            backslashIdx = idx;
            stringLength = idx;
            return TryFindEndOfUtf8StringSlow(ref stringStart, length, ref stringLength);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static bool TryFindEndOfUtf8StringSlow(ref byte stringStart, uint length, ref int stringLength)
        {
            const uint DoubleQuote = JsonUtf8Constant.String;
            const uint BackSlash = JsonUtf8Constant.ReverseSolidus;
            const uint Unicode = (byte)'u';

            IntPtr offset = (IntPtr)1;
            uint currentByte;
            while ((uint)stringLength < length)
            {
                currentByte = Unsafe.AddByteOffset(ref stringStart, offset + stringLength++);
                switch (currentByte)
                {
                    case BackSlash:
                        currentByte = Unsafe.AddByteOffset(ref stringStart, offset + stringLength++);
                        if (currentByte == Unicode)
                        {
                            stringLength += 4;
                        }
                        break;

                    case DoubleQuote:
                        return true;
                }
            }

            return false;
        }

        private static bool SkipUtf8String(ref byte b, ref int pos, uint length)
        {
            ref var stringStart = ref Unsafe.AddByteOffset(ref b, (IntPtr)pos++);
            // We should also get info about how many escaped chars exist from here
            if (TryFindEndOfUtf8String(ref stringStart, length - (uint)pos, out int stringLength, out _))
            {
                pos += stringLength; // skip the doublequote too
                return true;
            }

            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public object ReadUtf8Dynamic()
        {
            return ReadUtf8Dynamic(0);
        }

        public object ReadUtf8Dynamic(int stack)
        {
            ref var pos = ref _pos;
            var nextToken = ReadUtf8NextToken();
            if ((uint)stack > JsonSharedConstant.NestingLimit)
            {
                ThrowHelper.ThrowJsonParserException(JsonParserException.ParserError.NestingTooDeep, pos);
            }

            switch (nextToken)
            {
                case JsonTokenType.Null:
                    ReadUtf8Null();
                    return null;

                case JsonTokenType.False:
                case JsonTokenType.True:
                    return ReadUtf8Boolean();

                case JsonTokenType.Number:
                    if (_utf8Json.NonEmpty())
                    {
                        return new SpanJsonDynamicUtf8Number(ReadUtf8RawNumber());
                    }
                    else
                    {
                        return new SpanJsonDynamicUtf8Number(ReadUtf8NumberSpan());
                    }

                case JsonTokenType.String:
                    if (_utf8Json.NonEmpty())
                    {
                        ReadUtf8StringSpanWithQuotes(ref MemoryMarshal.GetReference(_utf8Span), ref pos, _length, out ArraySegment<byte> result);
                        return new SpanJsonDynamicUtf8String(result);
                    }
                    else
                    {
                        var span = ReadUtf8StringSpanWithQuotes(ref MemoryMarshal.GetReference(_utf8Span), ref pos, _length);
                        return new SpanJsonDynamicUtf8String(span);
                    }

                case JsonTokenType.BeginObject:
                    {
                        var startOffset = pos;
                        pos++;
                        var count = 0;
                        var dictionary = new Dictionary<string, object>(StringComparer.Ordinal);
                        while (!TryReadUtf8IsEndObjectOrValueSeparator(ref count))
                        {
                            var name = ReadUtf8EscapedName();
                            var value = ReadUtf8Dynamic(stack + 1);
                            dictionary[name] = value; // take last one
                        }

                        if (_utf8Json.NonEmpty())
                        {
                            return new SpanJsonDynamicObject(dictionary, _utf8Json.Slice(startOffset, pos - startOffset), false);
                        }
                        else
                        {
                            return new SpanJsonDynamicObject(dictionary);
                        }
                    }
                case JsonTokenType.BeginArray:
                    {
                        var startOffset = pos;
                        pos++;
                        var count = 0;
                        object[] temp = null;
                        try
                        {
                            temp = ArrayPool<object>.Shared.Rent(4);
                            while (!TryReadUtf8IsEndArrayOrValueSeparator(ref count))
                            {
                                if (count == temp.Length)
                                {
                                    FormatterUtils.GrowArray(ref temp);
                                }

                                temp[count - 1] = ReadUtf8Dynamic(stack + 1);
                            }

                            object[] result;
                            if (0u >= (uint)count)
                            {
                                result = JsonHelpers.Empty<object>();
                            }
                            else
                            {
                                result = FormatterUtils.CopyArray(temp, count);
                            }

                            if (_utf8Json.NonEmpty())
                            {
                                var rawJson = _utf8Json.Slice(startOffset, pos - startOffset);
                                return new SpanJsonDynamicArray<TSymbol>(result, Unsafe.As<ArraySegment<byte>, ArraySegment<TSymbol>>(ref rawJson));
                            }
                            else
                            {
                                return new SpanJsonDynamicArray<TSymbol>(result);
                            }
                        }
                        finally
                        {
                            if (temp is not null)
                            {
                                ArrayPool<object>.Shared.Return(temp);
                            }
                        }
                    }
                default:
                    ThrowHelper.ThrowJsonParserException(JsonParserException.ParserError.EndOfData, pos);
                    return default;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public JsonTokenType ReadUtf8NextToken()
        {
            return ReadUtf8NextTokenInternal(ref MemoryMarshal.GetReference(_utf8Span), ref _pos, _length);
        }

        private static JsonTokenType ReadUtf8NextTokenInternal(ref byte bStart, ref int pos, uint length)
        {
            var currentByte = SkipWhitespaceUtf8(ref bStart, ref pos, length);
            if ((uint)pos >= length)
            {
                return JsonTokenType.None;
            }

            switch (currentByte)
            {
                case JsonUtf8Constant.BeginObject:
                    return JsonTokenType.BeginObject;
                case JsonUtf8Constant.EndObject:
                    return JsonTokenType.EndObject;
                case JsonUtf8Constant.BeginArray:
                    return JsonTokenType.BeginArray;
                case JsonUtf8Constant.EndArray:
                    return JsonTokenType.EndArray;
                case JsonUtf8Constant.String:
                    return JsonTokenType.String;
                case JsonUtf8Constant.True:
                    return JsonTokenType.True;
                case JsonUtf8Constant.False:
                    return JsonTokenType.False;
                case JsonUtf8Constant.Null:
                    return JsonTokenType.Null;
                case JsonUtf8Constant.ValueSeparator:
                    return JsonTokenType.ValueSeparator;
                case JsonUtf8Constant.NameSeparator:
                    return JsonTokenType.NameSeparator;
                case '-':
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                case '8':
                case '9':
                case '0':
                    return JsonTokenType.Number;
                default:
                    return JsonTokenType.None;
            }
        }
    }
}