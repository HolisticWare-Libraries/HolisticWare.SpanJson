﻿namespace SpanJson
{
    using System;
    using System.Buffers;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Text.Encodings.Web;
    using System.Threading;
    using SpanJson.Internal;

    public ref partial struct JsonWriter<TSymbol> where TSymbol : struct
    {
        private ArrayPool<TSymbol>? _arrayPool;

        private TSymbol[] _borrowedBuffer = null!;
        internal byte[] _utf8Buffer = null!;
        private Span<byte> _utf8Span;
        internal char[] _utf16Buffer = null!;
        private Span<char> _utf16Span;
        private int _capacity;

        internal int _pos;
        private int _depth;

        /// <summary>TBD</summary>
        public ref char Utf16PinnableAddress
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ref MemoryMarshal.GetReference(_utf16Span);
        }

        /// <summary>TBD</summary>
        public ref byte Utf8PinnableAddress
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ref MemoryMarshal.GetReference(_utf8Span);
        }

        public int Position => _pos;

        public TSymbol[] Data => _borrowedBuffer;

        public int Capacity => _capacity;

        public int FreeCapacity
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _capacity - _pos;
        }

        /// <summary>Unsafe</summary>
        public ReadOnlySpan<char> Utf16WrittenSpan
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _utf16Span.Slice(0, _pos);
        }

        /// <summary>Unsafe</summary>
        public Span<char> Utf16FreeSpan
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _utf16Span.Slice(_pos);
        }

        /// <summary>Unsafe</summary>
        public ReadOnlySpan<byte> Utf8WrittenSpan
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _utf8Span.Slice(0, _pos);
        }

        /// <summary>Unsafe</summary>
        public Span<byte> Utf8FreeSpan
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _utf8Span.Slice(_pos);
        }

        /// <summary>Constructs a new <see cref="JsonWriter{TSymbol}"/> instance.</summary>
        public JsonWriter(bool useThreadLocalBuffer)
        {
            _pos = 0;
            _depth = 0;

            if (useThreadLocalBuffer)
            {
                _arrayPool = null;
                _borrowedBuffer = InternalMemoryPool<TSymbol>.GetBuffer();
            }
            else
            {
                _arrayPool = ArrayPool<TSymbol>.Shared;
                _borrowedBuffer = _arrayPool.Rent(InternalMemoryPool<TSymbol>.InitialCapacity);
            }
            if (SymbolHelper<TSymbol>.IsUtf8)
            {
                _utf8Span = _utf8Buffer = (_borrowedBuffer as byte[])!;
                _utf16Span = _utf16Buffer = null!;
            }
            else if (SymbolHelper<TSymbol>.IsUtf16)
            {
                _utf16Span = _utf16Buffer = (_borrowedBuffer as char[])!;
                _utf8Span = _utf8Buffer = null!;
            }
            else
            {
                throw ThrowHelper.GetNotSupportedException();
            }
            _capacity = _borrowedBuffer.Length;
        }

        /// <summary>Constructs a new <see cref="JsonWriter{TSymbol}"/> instance.</summary>
        public JsonWriter(int initialCapacity)
        {
            if (((uint)(initialCapacity - 1)) > JsonSharedConstant.TooBigOrNegative) { initialCapacity = InternalMemoryPool<TSymbol>.InitialCapacity; }

            _pos = 0;
            _depth = 0;

            _arrayPool = ArrayPool<TSymbol>.Shared;
            _borrowedBuffer = _arrayPool.Rent(initialCapacity);
            if (SymbolHelper<TSymbol>.IsUtf8)
            {
                _utf8Span = _utf8Buffer = (_borrowedBuffer as byte[])!;
                _utf16Span = _utf16Buffer = null!;
            }
            else if (SymbolHelper<TSymbol>.IsUtf16)
            {
                _utf16Span = _utf16Buffer = (_borrowedBuffer as char[])!;
                _utf8Span = _utf8Buffer = null!;
            }
            else
            {
                throw ThrowHelper.GetNotSupportedException();
            }
            _capacity = _borrowedBuffer.Length;
        }

        /// <summary>缓存不回收</summary>
        internal void Clear()
        {
            _borrowedBuffer = null!;
            _utf8Buffer = null!;
            _utf8Span = default;
            _utf16Buffer = null!;
            _utf16Span = default;
        }

        public void Dispose()
        {
            var toReturn = Interlocked.Exchange(ref _borrowedBuffer!, null);
            if (toReturn is null) { return; }

            var arrayPool = _arrayPool;
            if (arrayPool is not null)
            {
                arrayPool.Return(toReturn);
                _arrayPool = null;
            }

            _utf8Buffer = null!;
            _utf8Span = default;
            _utf16Buffer = null!;
            _utf16Span = default;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Ensure(int sizeHintt)
        {
            var alreadyWritten = _pos;
            if ((uint)sizeHintt >= (uint)(_capacity - alreadyWritten)) { CheckAndResizeBuffer(alreadyWritten, sizeHintt); }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void EnsureUnsafe(int alreadyWritten, int sizeHintt)
        {
            if ((uint)sizeHintt >= (uint)(_capacity - alreadyWritten)) { CheckAndResizeBuffer(alreadyWritten, sizeHintt); }
        }

        public void Advance(int count)
        {
            if ((uint)count > JsonSharedConstant.TooBigOrNegative) { ThrowHelper.ThrowArgumentOutOfRangeException_Nonnegative(ExceptionArgument.count); }

            if (_pos > _capacity - count) { ThrowHelper.ThrowInvalidOperationException_AdvancedTooFar(_capacity); }

            _pos += count;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private void CheckAndResizeBuffer(int alreadyWritten, int sizeHint)
        {
            Debug.Assert(_borrowedBuffer is not null);

            const int MinimumBufferSize = 256;

            //if ((uint)sizeHint > JsonSharedConstant.TooBigOrNegative) ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.sizeHint);
            //if (sizeHint == 0)
            if (unchecked((uint)(sizeHint - 1)) > JsonSharedConstant.TooBigOrNegative)
            {
                sizeHint = MinimumBufferSize;
            }

            int availableSpace = _capacity - alreadyWritten;

            if ((uint)sizeHint > (uint)availableSpace)
            {
                int growBy = Math.Max(sizeHint, _capacity);

                int newSize = checked(_capacity + growBy);

                var oldBuffer = _borrowedBuffer;

                var useThreadLocal = _arrayPool is null ? true : false;
                if (useThreadLocal) { _arrayPool = ArrayPool<TSymbol>.Shared; }

                _borrowedBuffer = _arrayPool!.Rent(newSize);
                if (SymbolHelper<TSymbol>.IsUtf8)
                {
                    _utf8Span = _utf8Buffer = (_borrowedBuffer as byte[])!;
                    _utf16Span = _utf16Buffer = null!;
                }
                else if (SymbolHelper<TSymbol>.IsUtf16)
                {
                    _utf16Span = _utf16Buffer = (_borrowedBuffer as char[])!;
                    _utf8Span = _utf8Buffer = null!;
                }

#if !(NETSTANDARD2_0 || NETCOREAPP2_1)
                Debug.Assert(oldBuffer.Length >= alreadyWritten);
                Debug.Assert(_borrowedBuffer.Length >= alreadyWritten);

#endif
                var previousBuffer = oldBuffer.AsSpan(0, alreadyWritten);
                previousBuffer.CopyTo(_borrowedBuffer);
                //previousBuffer.Clear();

                //BinaryUtil.CopyMemory(oldBuffer, 0, _borrowedBuffer, 0, alreadyWritten);

                _capacity = _borrowedBuffer.Length;

                if (!useThreadLocal)
                {
                    _arrayPool.Return(oldBuffer);
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void IncrementDepth() => _depth++;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DecrementDepth() => _depth--;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AssertDepth()
        {
            if ((uint)_depth > JsonSharedConstant.NestingLimit)
            {
                ThrowHelper.ThrowInvalidOperationException_NestingLimitOfExceeded();
            }
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteEndArray()
        {
            if (SymbolHelper<TSymbol>.IsUtf8)
            {
                WriteUtf8EndArray();
            }
            else if (SymbolHelper<TSymbol>.IsUtf16)
            {
                WriteUtf16EndArray();
            }
            else
            {
                ThrowHelper.ThrowNotSupportedException();
            }
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteBeginArray()
        {
            if (SymbolHelper<TSymbol>.IsUtf8)
            {
                WriteUtf8BeginArray();
            }
            else if (SymbolHelper<TSymbol>.IsUtf16)
            {
                WriteUtf16BeginArray();
            }
            else
            {
                ThrowHelper.ThrowNotSupportedException();
            }
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteBeginObject()
        {
            if (SymbolHelper<TSymbol>.IsUtf8)
            {
                WriteUtf8BeginObject();
            }
            else if (SymbolHelper<TSymbol>.IsUtf16)
            {
                WriteUtf16BeginObject();
            }
            else
            {
                ThrowHelper.ThrowNotSupportedException();
            }
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteEndObject()
        {
            if (SymbolHelper<TSymbol>.IsUtf8)
            {
                WriteUtf8EndObject();
            }
            else if (SymbolHelper<TSymbol>.IsUtf16)
            {
                WriteUtf16EndObject();
            }
            else
            {
                ThrowHelper.ThrowNotSupportedException();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteValueSeparator()
        {
            if (SymbolHelper<TSymbol>.IsUtf8)
            {
                WriteUtf8ValueSeparator();
            }
            else if (SymbolHelper<TSymbol>.IsUtf16)
            {
                WriteUtf16ValueSeparator();
            }
            else
            {
                ThrowHelper.ThrowNotSupportedException();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteNull()
        {
            if (SymbolHelper<TSymbol>.IsUtf8)
            {
                WriteUtf8Null();
            }
            else if (SymbolHelper<TSymbol>.IsUtf16)
            {
                WriteUtf16Null();
            }
            else
            {
                ThrowHelper.ThrowNotSupportedException();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteChar(char value, JsonEscapeHandling escapeHandling)
        {
            if (SymbolHelper<TSymbol>.IsUtf8)
            {
                WriteUtf8Char(value, escapeHandling);
            }
            else if (SymbolHelper<TSymbol>.IsUtf16)
            {
                WriteUtf16Char(value, escapeHandling);
            }
            else
            {
                ThrowHelper.ThrowNotSupportedException();
            }
        }

        /// <summary>The value should already be properly escaped.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteName(in JsonEncodedText name)
        {
            if (SymbolHelper<TSymbol>.IsUtf8)
            {
                WriteUtf8Name(name);
            }
            else if (SymbolHelper<TSymbol>.IsUtf16)
            {
                WriteUtf16Name(name);
            }
            else
            {
                ThrowHelper.ThrowNotSupportedException();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteName(string name)
        {
            if (SymbolHelper<TSymbol>.IsUtf8)
            {
                WriteUtf8Name(name);
            }
            else if (SymbolHelper<TSymbol>.IsUtf16)
            {
                WriteUtf16Name(name);
            }
            else
            {
                ThrowHelper.ThrowNotSupportedException();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteName(in ReadOnlySpan<char> name)
        {
            if (SymbolHelper<TSymbol>.IsUtf8)
            {
                WriteUtf8Name(name);
            }
            else if (SymbolHelper<TSymbol>.IsUtf16)
            {
                WriteUtf16Name(name);
            }
            else
            {
                ThrowHelper.ThrowNotSupportedException();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteName(string name, JsonEscapeHandling escapeHandling, JavaScriptEncoder? encoder = null)
        {
            if (SymbolHelper<TSymbol>.IsUtf8)
            {
                WriteUtf8Name(name, escapeHandling, encoder);
            }
            else if (SymbolHelper<TSymbol>.IsUtf16)
            {
                WriteUtf16Name(name, escapeHandling, encoder);
            }
            else
            {
                ThrowHelper.ThrowNotSupportedException();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteName(in ReadOnlySpan<char> name, JsonEscapeHandling escapeHandling, JavaScriptEncoder? encoder = null)
        {
            if (SymbolHelper<TSymbol>.IsUtf8)
            {
                WriteUtf8Name(name, escapeHandling, encoder);
            }
            else if (SymbolHelper<TSymbol>.IsUtf16)
            {
                WriteUtf16Name(name, escapeHandling, encoder);
            }
            else
            {
                ThrowHelper.ThrowNotSupportedException();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteString(in JsonEncodedText value)
        {
            if (SymbolHelper<TSymbol>.IsUtf8)
            {
                WriteUtf8String(value);
            }
            else if (SymbolHelper<TSymbol>.IsUtf16)
            {
                WriteUtf16String(value);
            }
            else
            {
                ThrowHelper.ThrowNotSupportedException();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteString(string value, JsonEscapeHandling escapeHandling, JavaScriptEncoder? encoder = null)
        {
            if (SymbolHelper<TSymbol>.IsUtf8)
            {
                WriteUtf8String(value, escapeHandling, encoder);
            }
            else if (SymbolHelper<TSymbol>.IsUtf16)
            {
                WriteUtf16String(value, escapeHandling, encoder);
            }
            else
            {
                ThrowHelper.ThrowNotSupportedException();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteString(in ReadOnlySpan<char> value)
        {
            if (SymbolHelper<TSymbol>.IsUtf8)
            {
                WriteUtf8String(value);
            }
            else if (SymbolHelper<TSymbol>.IsUtf16)
            {
                WriteUtf16String(value);
            }
            else
            {
                ThrowHelper.ThrowNotSupportedException();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteString(in ReadOnlySpan<char> value, JsonEscapeHandling escapeHandling, JavaScriptEncoder? encoder = null)
        {
            if (SymbolHelper<TSymbol>.IsUtf8)
            {
                WriteUtf8String(value, escapeHandling, encoder);
            }
            else if (SymbolHelper<TSymbol>.IsUtf16)
            {
                WriteUtf16String(value, escapeHandling, encoder);
            }
            else
            {
                ThrowHelper.ThrowNotSupportedException();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteVerbatim(in ReadOnlySpan<TSymbol> values)
        {
            if (SymbolHelper<TSymbol>.IsUtf8)
            {
                WriteUtf8Verbatim(MemoryMarshal.Cast<TSymbol, byte>(values));
            }
            else if (SymbolHelper<TSymbol>.IsUtf16)
            {
                WriteUtf16Verbatim(MemoryMarshal.Cast<TSymbol, char>(values));
            }
            else
            {
                ThrowHelper.ThrowNotSupportedException();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteNewLine()
        {
            if (SymbolHelper<TSymbol>.IsUtf8)
            {
                WriteUtf8Verbatim(JsonUtf8Constant.NewLine);
            }
            else if (SymbolHelper<TSymbol>.IsUtf16)
            {
                WriteUtf16Verbatim(JsonUtf16Constant.NewLine);
            }
            else
            {
                ThrowHelper.ThrowNotSupportedException();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteIndentation(int count)
        {
            if (SymbolHelper<TSymbol>.IsUtf8)
            {
                WriteUtf8Indentation(count);
            }
            else if (SymbolHelper<TSymbol>.IsUtf16)
            {
                WriteUtf16Indentation(count);
            }
            else
            {
                ThrowHelper.ThrowNotSupportedException();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteDoubleQuote()
        {
            if (SymbolHelper<TSymbol>.IsUtf8)
            {
                WriteUtf8DoubleQuote();
            }
            else if (SymbolHelper<TSymbol>.IsUtf16)
            {
                WriteUtf16DoubleQuote();
            }
            else
            {
                ThrowHelper.ThrowNotSupportedException();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteNameSeparator()
        {
            if (SymbolHelper<TSymbol>.IsUtf8)
            {
                WriteUtf8NameSeparator();
            }
            else if (SymbolHelper<TSymbol>.IsUtf16)
            {
                WriteUtf16NameSeparator();
            }
            else
            {
                ThrowHelper.ThrowNotSupportedException();
            }
        }

        /// <summary>The value should already be properly escaped.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteVerbatimNameSpan(in ReadOnlySpan<TSymbol> values)
        {
            if (SymbolHelper<TSymbol>.IsUtf8)
            {
                WriteUtf8VerbatimNameSpan(MemoryMarshal.Cast<TSymbol, byte>(values));
            }
            else if (SymbolHelper<TSymbol>.IsUtf16)
            {
                WriteUtf16VerbatimNameSpan(MemoryMarshal.Cast<TSymbol, char>(values));
            }
            else
            {
                ThrowHelper.ThrowNotSupportedException();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteCombGuid(CuteAnt.CombGuid value)
        {
            if (SymbolHelper<TSymbol>.IsUtf8)
            {
                WriteUtf8CombGuid(value);
            }
            else if (SymbolHelper<TSymbol>.IsUtf16)
            {
                WriteUtf16CombGuid(value);
            }
            else
            {
                ThrowHelper.ThrowNotSupportedException();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteBase64String(in ReadOnlySpan<byte> bytes)
        {
            if (SymbolHelper<TSymbol>.IsUtf8)
            {
                WriteUtf8Base64String(bytes);
            }
            else if (SymbolHelper<TSymbol>.IsUtf16)
            {
                WriteUtf16Base64String(bytes);
            }
            else
            {
                ThrowHelper.ThrowNotSupportedException();
            }
        }
    }
}
