using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using SpanJson.Internal;

namespace SpanJson
{
    public ref partial struct JsonReader<TSymbol> where TSymbol : struct
    {
        internal ArraySegment<char> _utf16Json;
        internal ArraySegment<byte> _utf8Json;
        internal readonly ReadOnlySpan<char> _utf16Span;
        internal readonly ReadOnlySpan<byte> _utf8Span;
        internal readonly uint _length;

        internal int _pos;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public JsonReader(TSymbol[] input)
        {
            if (input is null) { ThrowHelper.ThrowArgumentNullException(ExceptionArgument.input); }

            _length = (uint)input.Length;
            _pos = 0;

            if (SymbolHelper<TSymbol>.IsUtf8)
            {
                var utf8Json = Unsafe.As<TSymbol[], byte[]>(ref input);
                _utf8Span = new ReadOnlySpan<byte>(utf8Json);
                _utf8Json = new ArraySegment<byte>(utf8Json);
                _utf16Span = null;
                _utf16Json = default;
            }
            else if (SymbolHelper<TSymbol>.IsUtf16)
            {
                var utf16Json = Unsafe.As<TSymbol[], char[]>(ref input);
                _utf16Span = new ReadOnlySpan<char>(utf16Json);
                _utf16Json = new ArraySegment<char>(utf16Json);
                _utf8Json = default;
                _utf8Span = null;
            }
            else
            {
                throw ThrowHelper.GetNotSupportedException();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public JsonReader(ArraySegment<TSymbol> input)
        {
            _length = (uint)input.Count;
            _pos = 0;

            if (SymbolHelper<TSymbol>.IsUtf8)
            {
                var utf8Json = Unsafe.As<ArraySegment<TSymbol>, ArraySegment<byte>>(ref input);
                _utf8Span = new ReadOnlySpan<byte>(utf8Json.Array, utf8Json.Offset, utf8Json.Count);
                _utf8Json = utf8Json;
                _utf16Span = null;
                _utf16Json = default;
            }
            else if (SymbolHelper<TSymbol>.IsUtf16)
            {
                var utf16Json = Unsafe.As<ArraySegment<TSymbol>, ArraySegment<char>>(ref input);
                _utf16Span = new ReadOnlySpan<char>(utf16Json.Array, utf16Json.Offset, utf16Json.Count);
                _utf16Json = utf16Json;
                _utf8Json = default;
                _utf8Span = null;
            }
            else
            {
                throw ThrowHelper.GetNotSupportedException();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public JsonReader(in ReadOnlyMemory<TSymbol> input)
        {
            _length = (uint)input.Length;
            _pos = 0;

            MemoryMarshal.TryGetArray(input, out ArraySegment<TSymbol> tmp);

            if (SymbolHelper<TSymbol>.IsUtf8)
            {
                _utf8Json = Unsafe.As<ArraySegment<TSymbol>, ArraySegment<byte>>(ref tmp);
                _utf8Span = MemoryMarshal.Cast<TSymbol, byte>(input.Span);
                _utf16Json = default;
                _utf16Span = null;
            }
            else if (SymbolHelper<TSymbol>.IsUtf16)
            {
                _utf16Json = Unsafe.As<ArraySegment<TSymbol>, ArraySegment<char>>(ref tmp);
                _utf16Span = MemoryMarshal.Cast<TSymbol, char>(input.Span);
                _utf8Json = default;
                _utf8Span = null;
            }
            else
            {
                throw ThrowHelper.GetNotSupportedException();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public JsonReader(in ReadOnlySpan<TSymbol> input)
        {
            _length = (uint)input.Length;
            _pos = 0;
            _utf16Json = default;
            _utf8Json = default;

            if (SymbolHelper<TSymbol>.IsUtf8)
            {
                _utf8Span = MemoryMarshal.Cast<TSymbol, byte>(input);
                _utf16Span = null;
            }
            else if (SymbolHelper<TSymbol>.IsUtf16)
            {
                _utf16Span = MemoryMarshal.Cast<TSymbol, char>(input);
                _utf8Span = null;
            }
            else
            {
                throw ThrowHelper.GetNotSupportedException();
            }
        }

        public int Position => _pos;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ReadBeginArrayOrThrow()
        {
            if (SymbolHelper<TSymbol>.IsUtf8)
            {
                ReadUtf8BeginArrayOrThrow();
            }
            else if (SymbolHelper<TSymbol>.IsUtf16)
            {
                ReadUtf16BeginArrayOrThrow();
            }
            else
            {
                ThrowHelper.ThrowNotSupportedException();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryReadIsEndArrayOrValueSeparator(ref int count)
        {
            if (SymbolHelper<TSymbol>.IsUtf8)
            {
                return TryReadUtf8IsEndArrayOrValueSeparator(ref count);
            }

            if (SymbolHelper<TSymbol>.IsUtf16)
            {
                return TryReadUtf16IsEndArrayOrValueSeparator(ref count);
            }

            throw ThrowHelper.GetNotSupportedException();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public object? ReadDynamic()
        {
            if (SymbolHelper<TSymbol>.IsUtf8)
            {
                return ReadUtf8Dynamic();
            }

            if (SymbolHelper<TSymbol>.IsUtf16)
            {
                return ReadUtf16Dynamic();
            }

            throw ThrowHelper.GetNotSupportedException();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool ReadIsNull()
        {
            if (SymbolHelper<TSymbol>.IsUtf8)
            {
                return ReadUtf8IsNull();
            }

            if (SymbolHelper<TSymbol>.IsUtf16)
            {
                return ReadUtf16IsNull();
            }

            throw ThrowHelper.GetNotSupportedException();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string ReadEscapedName()
        {
            if (SymbolHelper<TSymbol>.IsUtf8)
            {
                return ReadUtf8EscapedName();
            }

            if (SymbolHelper<TSymbol>.IsUtf16)
            {
                return ReadUtf16EscapedName();
            }

            throw ThrowHelper.GetNotSupportedException();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ReadOnlySpan<TSymbol> ReadEscapedNameSpan()
        {
            if (SymbolHelper<TSymbol>.IsUtf8)
            {
                return MemoryMarshal.Cast<byte, TSymbol>(ReadUtf8EscapedNameSpan());
            }

            if (SymbolHelper<TSymbol>.IsUtf16)
            {
                return MemoryMarshal.Cast<char, TSymbol>(ReadUtf16EscapedNameSpan());
            }

            throw ThrowHelper.GetNotSupportedException();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ReadOnlySpan<TSymbol> ReadVerbatimNameSpan()
        {
            if (SymbolHelper<TSymbol>.IsUtf8)
            {
                //ref var pos = ref _pos;
                //ref byte bStart = ref MemoryMarshal.GetReference(_bytes);
                //SkipWhitespaceUtf8(ref bStart, ref pos, _nLength);
                return MemoryMarshal.Cast<byte, TSymbol>(ReadUtf8VerbatimNameSpan());
            }

            if (SymbolHelper<TSymbol>.IsUtf16)
            {
                //SkipWhitespaceUtf16();
                return MemoryMarshal.Cast<char, TSymbol>(ReadUtf16VerbatimNameSpan());
            }

            throw ThrowHelper.GetNotSupportedException();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryReadIsEndObjectOrValueSeparator(ref int count)
        {
            if (SymbolHelper<TSymbol>.IsUtf8)
            {
                return TryReadUtf8IsEndObjectOrValueSeparator(ref count);
            }

            if (SymbolHelper<TSymbol>.IsUtf16)
            {
                return TryReadUtf16IsEndObjectOrValueSeparator(ref count);
            }

            throw ThrowHelper.GetNotSupportedException();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ReadBeginObjectOrThrow()
        {
            if (SymbolHelper<TSymbol>.IsUtf8)
            {
                ReadUtf8BeginObjectOrThrow();
            }
            else if (SymbolHelper<TSymbol>.IsUtf16)
            {
                ReadUtf16BeginObjectOrThrow();
            }
            else
            {
                ThrowHelper.ThrowNotSupportedException();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ReadEndObjectOrThrow()
        {
            if (SymbolHelper<TSymbol>.IsUtf8)
            {
                ReadUtf8EndObjectOrThrow();
            }
            else if (SymbolHelper<TSymbol>.IsUtf16)
            {
                ReadUtf16EndObjectOrThrow();
            }
            else
            {
                ThrowHelper.ThrowNotSupportedException();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ReadEndArrayOrThrow()
        {
            if (SymbolHelper<TSymbol>.IsUtf8)
            {
                ReadUtf8EndArrayOrThrow();
            }
            else if (SymbolHelper<TSymbol>.IsUtf16)
            {
                ReadUtf16EndArrayOrThrow();
            }
            else
            {
                ThrowHelper.ThrowNotSupportedException();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ReadOnlySpan<TSymbol> ReadStringSpan()
        {
            if (SymbolHelper<TSymbol>.IsUtf8)
            {
                return MemoryMarshal.Cast<byte, TSymbol>(ReadUtf8StringSpan());
            }

            if (SymbolHelper<TSymbol>.IsUtf16)
            {
                return MemoryMarshal.Cast<char, TSymbol>(ReadUtf16StringSpan());
            }

            throw ThrowHelper.GetNotSupportedException();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ReadOnlySpan<TSymbol> ReadVerbatimStringSpan()
        {
            if (SymbolHelper<TSymbol>.IsUtf8)
            {
                return MemoryMarshal.Cast<byte, TSymbol>(ReadUtf8VerbatimStringSpan());
            }

            if (SymbolHelper<TSymbol>.IsUtf16)
            {
                return MemoryMarshal.Cast<char, TSymbol>(ReadUtf16VerbatimStringSpan());
            }

            throw ThrowHelper.GetNotSupportedException();
        }

        /// <summary>Doesn't skip whitespace, just for copying around in a token loop</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ReadOnlySpan<TSymbol> ReadVerbatimStringSpanUnsafe()
        {
            ref var pos = ref _pos;
            if (SymbolHelper<TSymbol>.IsUtf8)
            {
                ref byte bStart = ref MemoryMarshal.GetReference(_utf8Span);
                return MemoryMarshal.Cast<byte, TSymbol>(ReadUtf8StringSpanInternal(ref bStart, ref pos, _length, out _));
            }

            if (SymbolHelper<TSymbol>.IsUtf16)
            {
                ref var cStart = ref MemoryMarshal.GetReference(_utf16Span);
                return MemoryMarshal.Cast<char, TSymbol>(ReadUtf16StringSpanInternal(ref cStart, ref pos, _length, out _));
            }

            throw ThrowHelper.GetNotSupportedException();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SkipNextSegment()
        {
            if (SymbolHelper<TSymbol>.IsUtf8)
            {
                SkipNextUtf8Segment();
            }
            else if (SymbolHelper<TSymbol>.IsUtf16)
            {
                SkipNextUtf16Segment();
            }
            else
            {
                ThrowHelper.ThrowNotSupportedException();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SkipNextValue(JsonTokenType tokenType)
        {
            if (SymbolHelper<TSymbol>.IsUtf8)
            {
                SkipNextUtf8Value(tokenType);
            }
            else if (SymbolHelper<TSymbol>.IsUtf16)
            {
                SkipNextUtf16Value(tokenType);
            }
            else
            {
                ThrowHelper.ThrowNotSupportedException();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public JsonTokenType ReadNextToken()
        {
            if (SymbolHelper<TSymbol>.IsUtf8)
            {
                return ReadUtf8NextToken();
            }

            if (SymbolHelper<TSymbol>.IsUtf16)
            {
                return ReadUtf16NextToken();
            }

            throw ThrowHelper.GetNotSupportedException();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ReadOnlySpan<TSymbol> ReadNumberSpan()
        {
            if (SymbolHelper<TSymbol>.IsUtf8)
            {
                return MemoryMarshal.Cast<byte, TSymbol>(ReadUtf8NumberSpan());
            }

            if (SymbolHelper<TSymbol>.IsUtf16)
            {
                return MemoryMarshal.Cast<char, TSymbol>(ReadUtf16NumberSpan());
            }

            throw ThrowHelper.GetNotSupportedException();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ReadSymbolOrThrow(TSymbol symbol)
        {
            if (SymbolHelper<TSymbol>.IsUtf8)
            {
                ReadUtf8SymbolOrThrow(Unsafe.As<TSymbol, byte>(ref symbol));
            }
            else if (SymbolHelper<TSymbol>.IsUtf16)
            {
                ReadUtf16SymbolOrThrow(Unsafe.As<TSymbol, char>(ref symbol));
            }
            else
            {
                ThrowHelper.ThrowNotSupportedException();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public CuteAnt.CombGuid ReadCombGuid()
        {
            if (SymbolHelper<TSymbol>.IsUtf8)
            {
                return ReadUtf8CombGuid();
            }

            if (SymbolHelper<TSymbol>.IsUtf16)
            {
                return ReadUtf16CombGuid();
            }

            throw ThrowHelper.GetNotSupportedException();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte[]? ReadBytesFromBase64()
        {
            if (SymbolHelper<TSymbol>.IsUtf8)
            {
                return ReadUtf8BytesFromBase64();
            }

            if (SymbolHelper<TSymbol>.IsUtf16)
            {
                return ReadUtf16BytesFromBase64();
            }

            throw ThrowHelper.GetNotSupportedException();
        }
    }
}