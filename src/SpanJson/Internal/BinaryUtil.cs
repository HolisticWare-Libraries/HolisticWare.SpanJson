﻿// Largely based on https://github.com/neuecc/Utf8Json/blob/master/src/Utf8Json/Internal/BinaryUtil.cs

using System;
using System.Runtime.CompilerServices;

namespace SpanJson.Internal
{
    internal static class BinaryUtil
    {
        const int ArrayMaxSize = 0x7FFFFFC7; // https://msdn.microsoft.com/en-us/library/system.array

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyMemory(byte[] src, int srcOffset, byte[] dst, int dstOffset, int count)
        {
            Unsafe.CopyBlockUnaligned(ref dst[dstOffset], ref src[srcOffset], unchecked((uint)count));
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyMemory(ref byte src, ref byte dst, int length)
        {
            Unsafe.CopyBlockUnaligned(ref dst, ref src, unchecked((uint)length));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void EnsureCapacity(ref byte[] bytes, int offset, int appendLength)
        {
            var newLength = offset + appendLength;

            // If null(most case fisrt time) fill byte.
            if (bytes is null)
            {
                bytes = new byte[newLength];
                return;
            }

            // like MemoryStream.EnsureCapacity
            var current = bytes.Length;
            if (newLength > current)
            {
                int num = newLength;
                if (num < 256)
                {
                    num = 256;
                    FastResize(ref bytes, num);
                    return;
                }

                if (current == ArrayMaxSize)
                {
                    ThrowHelper.ThrowInvalidOperationException_Reached_MaximumSize();
                }

                var newSize = unchecked((current * 2));
                if ((uint)newSize > JsonSharedConstant.TooBigOrNegative) // overflow
                {
                    num = ArrayMaxSize;
                }
                else
                {
                    if (num < newSize)
                    {
                        num = newSize;
                    }
                }

                FastResize(ref bytes, num);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void FastResize(ref byte[] array, int newSize)
        {
            if ((uint)newSize > JsonSharedConstant.TooBigOrNegative) ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.newSize);

            byte[] array2 = array;
            if (array2 is null)
            {
                array = new byte[newSize];
                return;
            }

            if (array2.Length != newSize)
            {
                byte[] array3 = new byte[newSize];
                var len = (array2.Length > newSize) ? newSize : array2.Length;
                CopyMemory(array2, 0, array3, 0, len);
                array = array3;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte[] FastCloneWithResize(byte[] src, int newSize)
        {
            if ((uint)newSize > JsonSharedConstant.TooBigOrNegative) ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.newSize);
            if (src is null) return new byte[newSize];
            if (src.Length < newSize) ThrowHelper.ThrowArgumentException_Length();

            byte[] dst = new byte[newSize];

            CopyMemory(src, 0, dst, 0, newSize);

            return dst;
        }
    }
}
