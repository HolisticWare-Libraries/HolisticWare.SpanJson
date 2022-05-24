﻿// Autogenerated
// ReSharper disable BuiltInTypeReferenceStyle
using System;
using System.Runtime.CompilerServices;
namespace SpanJson
{
    public ref partial struct JsonReader<TSymbol> where TSymbol : struct
    {

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public SByte ReadSByte()
        {
            if (0u >= (uint)(Unsafe.SizeOf<TSymbol>() - JsonSharedConstant.ByteSize))
            {
                return ReadUtf8SByte();
            }

            if (0u >= (uint)(Unsafe.SizeOf<TSymbol>() - JsonSharedConstant.CharSize))
            {
                return ReadUtf16SByte();
            }

            throw ThrowHelper.GetNotSupportedException();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Int16 ReadInt16()
        {
            if (0u >= (uint)(Unsafe.SizeOf<TSymbol>() - JsonSharedConstant.ByteSize))
            {
                return ReadUtf8Int16();
            }

            if (0u >= (uint)(Unsafe.SizeOf<TSymbol>() - JsonSharedConstant.CharSize))
            {
                return ReadUtf16Int16();
            }

            throw ThrowHelper.GetNotSupportedException();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Int32 ReadInt32()
        {
            if (0u >= (uint)(Unsafe.SizeOf<TSymbol>() - JsonSharedConstant.ByteSize))
            {
                return ReadUtf8Int32();
            }

            if (0u >= (uint)(Unsafe.SizeOf<TSymbol>() - JsonSharedConstant.CharSize))
            {
                return ReadUtf16Int32();
            }

            throw ThrowHelper.GetNotSupportedException();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Int64 ReadInt64()
        {
            if (0u >= (uint)(Unsafe.SizeOf<TSymbol>() - JsonSharedConstant.ByteSize))
            {
                return ReadUtf8Int64();
            }

            if (0u >= (uint)(Unsafe.SizeOf<TSymbol>() - JsonSharedConstant.CharSize))
            {
                return ReadUtf16Int64();
            }

            throw ThrowHelper.GetNotSupportedException();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Byte ReadByte()
        {
            if (0u >= (uint)(Unsafe.SizeOf<TSymbol>() - JsonSharedConstant.ByteSize))
            {
                return ReadUtf8Byte();
            }

            if (0u >= (uint)(Unsafe.SizeOf<TSymbol>() - JsonSharedConstant.CharSize))
            {
                return ReadUtf16Byte();
            }

            throw ThrowHelper.GetNotSupportedException();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public UInt16 ReadUInt16()
        {
            if (0u >= (uint)(Unsafe.SizeOf<TSymbol>() - JsonSharedConstant.ByteSize))
            {
                return ReadUtf8UInt16();
            }

            if (0u >= (uint)(Unsafe.SizeOf<TSymbol>() - JsonSharedConstant.CharSize))
            {
                return ReadUtf16UInt16();
            }

            throw ThrowHelper.GetNotSupportedException();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public UInt32 ReadUInt32()
        {
            if (0u >= (uint)(Unsafe.SizeOf<TSymbol>() - JsonSharedConstant.ByteSize))
            {
                return ReadUtf8UInt32();
            }

            if (0u >= (uint)(Unsafe.SizeOf<TSymbol>() - JsonSharedConstant.CharSize))
            {
                return ReadUtf16UInt32();
            }

            throw ThrowHelper.GetNotSupportedException();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public UInt64 ReadUInt64()
        {
            if (0u >= (uint)(Unsafe.SizeOf<TSymbol>() - JsonSharedConstant.ByteSize))
            {
                return ReadUtf8UInt64();
            }

            if (0u >= (uint)(Unsafe.SizeOf<TSymbol>() - JsonSharedConstant.CharSize))
            {
                return ReadUtf16UInt64();
            }

            throw ThrowHelper.GetNotSupportedException();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Single ReadSingle()
        {
            if (0u >= (uint)(Unsafe.SizeOf<TSymbol>() - JsonSharedConstant.ByteSize))
            {
                return ReadUtf8Single();
            }

            if (0u >= (uint)(Unsafe.SizeOf<TSymbol>() - JsonSharedConstant.CharSize))
            {
                return ReadUtf16Single();
            }

            throw ThrowHelper.GetNotSupportedException();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Double ReadDouble()
        {
            if (0u >= (uint)(Unsafe.SizeOf<TSymbol>() - JsonSharedConstant.ByteSize))
            {
                return ReadUtf8Double();
            }

            if (0u >= (uint)(Unsafe.SizeOf<TSymbol>() - JsonSharedConstant.CharSize))
            {
                return ReadUtf16Double();
            }

            throw ThrowHelper.GetNotSupportedException();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Decimal ReadDecimal()
        {
            if (0u >= (uint)(Unsafe.SizeOf<TSymbol>() - JsonSharedConstant.ByteSize))
            {
                return ReadUtf8Decimal();
            }

            if (0u >= (uint)(Unsafe.SizeOf<TSymbol>() - JsonSharedConstant.CharSize))
            {
                return ReadUtf16Decimal();
            }

            throw ThrowHelper.GetNotSupportedException();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Boolean ReadBoolean()
        {
            if (0u >= (uint)(Unsafe.SizeOf<TSymbol>() - JsonSharedConstant.ByteSize))
            {
                return ReadUtf8Boolean();
            }

            if (0u >= (uint)(Unsafe.SizeOf<TSymbol>() - JsonSharedConstant.CharSize))
            {
                return ReadUtf16Boolean();
            }

            throw ThrowHelper.GetNotSupportedException();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Char ReadChar()
        {
            if (0u >= (uint)(Unsafe.SizeOf<TSymbol>() - JsonSharedConstant.ByteSize))
            {
                return ReadUtf8Char();
            }

            if (0u >= (uint)(Unsafe.SizeOf<TSymbol>() - JsonSharedConstant.CharSize))
            {
                return ReadUtf16Char();
            }

            throw ThrowHelper.GetNotSupportedException();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public DateTime ReadDateTime()
        {
            if (0u >= (uint)(Unsafe.SizeOf<TSymbol>() - JsonSharedConstant.ByteSize))
            {
                return ReadUtf8DateTime();
            }

            if (0u >= (uint)(Unsafe.SizeOf<TSymbol>() - JsonSharedConstant.CharSize))
            {
                return ReadUtf16DateTime();
            }

            throw ThrowHelper.GetNotSupportedException();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public DateTimeOffset ReadDateTimeOffset()
        {
            if (0u >= (uint)(Unsafe.SizeOf<TSymbol>() - JsonSharedConstant.ByteSize))
            {
                return ReadUtf8DateTimeOffset();
            }

            if (0u >= (uint)(Unsafe.SizeOf<TSymbol>() - JsonSharedConstant.CharSize))
            {
                return ReadUtf16DateTimeOffset();
            }

            throw ThrowHelper.GetNotSupportedException();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TimeSpan ReadTimeSpan()
        {
            if (0u >= (uint)(Unsafe.SizeOf<TSymbol>() - JsonSharedConstant.ByteSize))
            {
                return ReadUtf8TimeSpan();
            }

            if (0u >= (uint)(Unsafe.SizeOf<TSymbol>() - JsonSharedConstant.CharSize))
            {
                return ReadUtf16TimeSpan();
            }

            throw ThrowHelper.GetNotSupportedException();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Guid ReadGuid()
        {
            if (0u >= (uint)(Unsafe.SizeOf<TSymbol>() - JsonSharedConstant.ByteSize))
            {
                return ReadUtf8Guid();
            }

            if (0u >= (uint)(Unsafe.SizeOf<TSymbol>() - JsonSharedConstant.CharSize))
            {
                return ReadUtf16Guid();
            }

            throw ThrowHelper.GetNotSupportedException();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public String ReadString()
        {
            if (0u >= (uint)(Unsafe.SizeOf<TSymbol>() - JsonSharedConstant.ByteSize))
            {
                return ReadUtf8String();
            }

            if (0u >= (uint)(Unsafe.SizeOf<TSymbol>() - JsonSharedConstant.CharSize))
            {
                return ReadUtf16String();
            }

            throw ThrowHelper.GetNotSupportedException();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Version ReadVersion()
        {
            if (0u >= (uint)(Unsafe.SizeOf<TSymbol>() - JsonSharedConstant.ByteSize))
            {
                return ReadUtf8Version();
            }

            if (0u >= (uint)(Unsafe.SizeOf<TSymbol>() - JsonSharedConstant.CharSize))
            {
                return ReadUtf16Version();
            }

            throw ThrowHelper.GetNotSupportedException();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Uri ReadUri()
        {
            if (0u >= (uint)(Unsafe.SizeOf<TSymbol>() - JsonSharedConstant.ByteSize))
            {
                return ReadUtf8Uri();
            }

            if (0u >= (uint)(Unsafe.SizeOf<TSymbol>() - JsonSharedConstant.CharSize))
            {
                return ReadUtf16Uri();
            }

            throw ThrowHelper.GetNotSupportedException();
        }
    }
}