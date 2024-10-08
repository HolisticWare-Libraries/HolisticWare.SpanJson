﻿
namespace SpanJson.Internal
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    static partial class UnsafeMemory64
    {
        public static void WriteRaw(ref byte destination, ref byte source, int sourceBytesToCopy, ref int idx)
        {
            var nCount = (uint)sourceBytesToCopy;
            switch (nCount)
            {
                case 0u: return;
                case 1u: WriteRaw1(ref destination, ref source, ref idx); return;
                case 2u: WriteRaw2(ref destination, ref source, ref idx); return;
                case 3u: WriteRaw3(ref destination, ref source, ref idx); return;
                case 4u: WriteRaw4(ref destination, ref source, ref idx); return;
                case 5u: WriteRaw5(ref destination, ref source, ref idx); return;
                case 6u: WriteRaw6(ref destination, ref source, ref idx); return;
                case 7u: WriteRaw7(ref destination, ref source, ref idx); return;
                case 8u: WriteRaw8(ref destination, ref source, ref idx); return;
                case 9u: WriteRaw9(ref destination, ref source, ref idx); return;
                case 10u: WriteRaw10(ref destination, ref source, ref idx); return;
                case 11u: WriteRaw11(ref destination, ref source, ref idx); return;
                case 12u: WriteRaw12(ref destination, ref source, ref idx); return;
                case 13u: WriteRaw13(ref destination, ref source, ref idx); return;
                case 14u: WriteRaw14(ref destination, ref source, ref idx); return;
                case 15u: WriteRaw15(ref destination, ref source, ref idx); return;
                case 16u: WriteRaw16(ref destination, ref source, ref idx); return;
                case 17u: WriteRaw17(ref destination, ref source, ref idx); return;
                case 18u: WriteRaw18(ref destination, ref source, ref idx); return;
                case 19u: WriteRaw19(ref destination, ref source, ref idx); return;
                case 20u: WriteRaw20(ref destination, ref source, ref idx); return;
                case 21u: WriteRaw21(ref destination, ref source, ref idx); return;
                case 22u: WriteRaw22(ref destination, ref source, ref idx); return;
                case 23u: WriteRaw23(ref destination, ref source, ref idx); return;
                case 24u: WriteRaw24(ref destination, ref source, ref idx); return;
                case 25u: WriteRaw25(ref destination, ref source, ref idx); return;
                case 26u: WriteRaw26(ref destination, ref source, ref idx); return;
                case 27u: WriteRaw27(ref destination, ref source, ref idx); return;
                case 28u: WriteRaw28(ref destination, ref source, ref idx); return;
                case 29u: WriteRaw29(ref destination, ref source, ref idx); return;
                case 30u: WriteRaw30(ref destination, ref source, ref idx); return;
                case 31u: WriteRaw31(ref destination, ref source, ref idx); return;
                case 32u: WriteRaw32(ref destination, ref source, ref idx); return;
                default: UnsafeMemory.WriteRawBytes(ref destination, ref source, sourceBytesToCopy, ref idx); return;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteRaw2(ref byte destination, ref byte src, ref int idx)
        {
            ref byte dest = ref Unsafe.Add(ref destination, (IntPtr)(ulong)idx);

            Unsafe.As<byte, short>(ref dest) = Unsafe.As<byte, short>(ref src);

            idx += 2;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteRaw3(ref byte destination, ref byte src, ref int idx)
        {
            ref byte dest = ref Unsafe.Add(ref destination, (IntPtr)(ulong)idx);

            Unsafe.As<byte, short>(ref dest) = Unsafe.As<byte, short>(ref src);
            Unsafe.AddByteOffset(ref dest, (IntPtr)2) = Unsafe.AddByteOffset(ref src, (IntPtr)2);

            idx += 3;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteRaw4(ref byte destination, ref byte src, ref int idx)
        {
            ref byte dest = ref Unsafe.Add(ref destination, (IntPtr)(ulong)idx);

            Unsafe.As<byte, int>(ref dest) = Unsafe.As<byte, int>(ref src);

            idx += 4;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteRaw5(ref byte destination, ref byte src, ref int idx)
        {
            ref byte dest = ref Unsafe.Add(ref destination, (IntPtr)(ulong)idx);

            Unsafe.As<byte, int>(ref dest) = Unsafe.As<byte, int>(ref src);
            Unsafe.AddByteOffset(ref dest, (IntPtr)4) = Unsafe.AddByteOffset(ref src, (IntPtr)4);

            idx += 5;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteRaw6(ref byte destination, ref byte src, ref int idx)
        {
            ref byte dest = ref Unsafe.Add(ref destination, (IntPtr)(ulong)idx);

            Unsafe.As<byte, int>(ref dest) = Unsafe.As<byte, int>(ref src);
            Unsafe.As<byte, short>(ref Unsafe.Add(ref dest, 4)) = Unsafe.As<byte, short>(ref Unsafe.Add(ref src, 4));

            idx += 6;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteRaw7(ref byte destination, ref byte src, ref int idx)
        {
            ref byte dest = ref Unsafe.Add(ref destination, (IntPtr)(ulong)idx);

            Unsafe.As<byte, int>(ref dest) = Unsafe.As<byte, int>(ref src);
            Unsafe.As<byte, short>(ref Unsafe.Add(ref dest, 4)) = Unsafe.As<byte, short>(ref Unsafe.Add(ref src, 4));
            Unsafe.AddByteOffset(ref dest, (IntPtr)6) = Unsafe.AddByteOffset(ref src, (IntPtr)6);

            idx += 7;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteRaw8(ref byte destination, ref byte src, ref int idx)
        {
            ref byte dest = ref Unsafe.Add(ref destination, (IntPtr)(ulong)idx);

            Unsafe.As<byte, long>(ref dest) = Unsafe.As<byte, long>(ref src);

            idx += 8;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteRaw9(ref byte destination, ref byte src, ref int idx)
        {
            ref byte dest = ref Unsafe.Add(ref destination, (IntPtr)(ulong)idx);

            Unsafe.As<byte, long>(ref dest) = Unsafe.As<byte, long>(ref src);
            Unsafe.AddByteOffset(ref dest, (IntPtr)8) = Unsafe.AddByteOffset(ref src, (IntPtr)8);

            idx += 9;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteRaw10(ref byte destination, ref byte src, ref int idx)
        {
            ref byte dest = ref Unsafe.Add(ref destination, (IntPtr)(ulong)idx);

            Unsafe.As<byte, long>(ref dest) = Unsafe.As<byte, long>(ref src);
            Unsafe.As<byte, short>(ref Unsafe.Add(ref dest, 8)) = Unsafe.As<byte, short>(ref Unsafe.Add(ref src, 8));

            idx += 10;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteRaw11(ref byte destination, ref byte src, ref int idx)
        {
            ref byte dest = ref Unsafe.Add(ref destination, (IntPtr)(ulong)idx);

            Unsafe.As<byte, long>(ref dest) = Unsafe.As<byte, long>(ref src);
            Unsafe.As<byte, short>(ref Unsafe.Add(ref dest, 8)) = Unsafe.As<byte, short>(ref Unsafe.Add(ref src, 8));
            Unsafe.AddByteOffset(ref dest, (IntPtr)10) = Unsafe.AddByteOffset(ref src, (IntPtr)10);

            idx += 11;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteRaw12(ref byte destination, ref byte src, ref int idx)
        {
            ref byte dest = ref Unsafe.Add(ref destination, (IntPtr)(ulong)idx);

            Unsafe.As<byte, long>(ref dest) = Unsafe.As<byte, long>(ref src);
            Unsafe.As<byte, int>(ref Unsafe.Add(ref dest, 8)) = Unsafe.As<byte, int>(ref Unsafe.Add(ref src, 8));

            idx += 12;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteRaw13(ref byte destination, ref byte src, ref int idx)
        {
            ref byte dest = ref Unsafe.Add(ref destination, (IntPtr)(ulong)idx);

            Unsafe.As<byte, long>(ref dest) = Unsafe.As<byte, long>(ref src);
            Unsafe.As<byte, int>(ref Unsafe.Add(ref dest, 8)) = Unsafe.As<byte, int>(ref Unsafe.Add(ref src, 8));
            Unsafe.AddByteOffset(ref dest, (IntPtr)12) = Unsafe.AddByteOffset(ref src, (IntPtr)12);

            idx += 13;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteRaw14(ref byte destination, ref byte src, ref int idx)
        {
            ref byte dest = ref Unsafe.Add(ref destination, (IntPtr)(ulong)idx);

            Unsafe.As<byte, long>(ref dest) = Unsafe.As<byte, long>(ref src);
            Unsafe.As<byte, int>(ref Unsafe.Add(ref dest, 8)) = Unsafe.As<byte, int>(ref Unsafe.Add(ref src, 8));
            Unsafe.As<byte, short>(ref Unsafe.Add(ref dest, 12)) = Unsafe.As<byte, short>(ref Unsafe.Add(ref src, 12));

            idx += 14;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteRaw15(ref byte destination, ref byte src, ref int idx)
        {
            ref byte dest = ref Unsafe.Add(ref destination, (IntPtr)(ulong)idx);

            Unsafe.As<byte, long>(ref dest) = Unsafe.As<byte, long>(ref src);
            Unsafe.As<byte, int>(ref Unsafe.Add(ref dest, 8)) = Unsafe.As<byte, int>(ref Unsafe.Add(ref src, 8));
            Unsafe.As<byte, short>(ref Unsafe.Add(ref dest, 12)) = Unsafe.As<byte, short>(ref Unsafe.Add(ref src, 12));
            Unsafe.AddByteOffset(ref dest, (IntPtr)14) = Unsafe.AddByteOffset(ref src, (IntPtr)14);

            idx += 15;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteRaw16(ref byte destination, ref byte src, ref int idx)
        {
            ref byte dest = ref Unsafe.Add(ref destination, (IntPtr)(ulong)idx);

            Unsafe.As<byte, long>(ref dest) = Unsafe.As<byte, long>(ref src);
            Unsafe.As<byte, long>(ref Unsafe.Add(ref dest, 8)) = Unsafe.As<byte, long>(ref Unsafe.Add(ref src, 8));

            idx += 16;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteRaw17(ref byte destination, ref byte src, ref int idx)
        {
            ref byte dest = ref Unsafe.Add(ref destination, (IntPtr)(ulong)idx);

            Unsafe.As<byte, long>(ref dest) = Unsafe.As<byte, long>(ref src);
            Unsafe.As<byte, long>(ref Unsafe.Add(ref dest, 8)) = Unsafe.As<byte, long>(ref Unsafe.Add(ref src, 8));
            Unsafe.AddByteOffset(ref dest, (IntPtr)16) = Unsafe.AddByteOffset(ref src, (IntPtr)16);

            idx += 17;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteRaw18(ref byte destination, ref byte src, ref int idx)
        {
            ref byte dest = ref Unsafe.Add(ref destination, (IntPtr)(ulong)idx);

            Unsafe.As<byte, long>(ref dest) = Unsafe.As<byte, long>(ref src);
            Unsafe.As<byte, long>(ref Unsafe.Add(ref dest, 8)) = Unsafe.As<byte, long>(ref Unsafe.Add(ref src, 8));
            Unsafe.As<byte, short>(ref Unsafe.Add(ref dest, 16)) = Unsafe.As<byte, short>(ref Unsafe.Add(ref src, 16));

            idx += 18;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteRaw19(ref byte destination, ref byte src, ref int idx)
        {
            ref byte dest = ref Unsafe.Add(ref destination, (IntPtr)(ulong)idx);

            Unsafe.As<byte, long>(ref dest) = Unsafe.As<byte, long>(ref src);
            Unsafe.As<byte, long>(ref Unsafe.Add(ref dest, 8)) = Unsafe.As<byte, long>(ref Unsafe.Add(ref src, 8));
            Unsafe.As<byte, short>(ref Unsafe.Add(ref dest, 16)) = Unsafe.As<byte, short>(ref Unsafe.Add(ref src, 16));
            Unsafe.AddByteOffset(ref dest, (IntPtr)18) = Unsafe.AddByteOffset(ref src, (IntPtr)18);

            idx += 19;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteRaw20(ref byte destination, ref byte src, ref int idx)
        {
            ref byte dest = ref Unsafe.Add(ref destination, (IntPtr)(ulong)idx);

            Unsafe.As<byte, long>(ref dest) = Unsafe.As<byte, long>(ref src);
            Unsafe.As<byte, long>(ref Unsafe.Add(ref dest, 8)) = Unsafe.As<byte, long>(ref Unsafe.Add(ref src, 8));
            Unsafe.As<byte, int>(ref Unsafe.Add(ref dest, 16)) = Unsafe.As<byte, int>(ref Unsafe.Add(ref src, 16));

            idx += 20;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteRaw21(ref byte destination, ref byte src, ref int idx)
        {
            ref byte dest = ref Unsafe.Add(ref destination, (IntPtr)(ulong)idx);

            Unsafe.As<byte, long>(ref dest) = Unsafe.As<byte, long>(ref src);
            Unsafe.As<byte, long>(ref Unsafe.Add(ref dest, 8)) = Unsafe.As<byte, long>(ref Unsafe.Add(ref src, 8));
            Unsafe.As<byte, int>(ref Unsafe.Add(ref dest, 16)) = Unsafe.As<byte, int>(ref Unsafe.Add(ref src, 16));
            Unsafe.AddByteOffset(ref dest, (IntPtr)20) = Unsafe.AddByteOffset(ref src, (IntPtr)20);

            idx += 21;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteRaw22(ref byte destination, ref byte src, ref int idx)
        {
            ref byte dest = ref Unsafe.Add(ref destination, (IntPtr)(ulong)idx);

            Unsafe.As<byte, long>(ref dest) = Unsafe.As<byte, long>(ref src);
            Unsafe.As<byte, long>(ref Unsafe.Add(ref dest, 8)) = Unsafe.As<byte, long>(ref Unsafe.Add(ref src, 8));
            Unsafe.As<byte, int>(ref Unsafe.Add(ref dest, 16)) = Unsafe.As<byte, int>(ref Unsafe.Add(ref src, 16));
            Unsafe.As<byte, short>(ref Unsafe.Add(ref dest, 20)) = Unsafe.As<byte, short>(ref Unsafe.Add(ref src, 20));

            idx += 22;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteRaw23(ref byte destination, ref byte src, ref int idx)
        {
            ref byte dest = ref Unsafe.Add(ref destination, (IntPtr)(ulong)idx);

            Unsafe.As<byte, long>(ref dest) = Unsafe.As<byte, long>(ref src);
            Unsafe.As<byte, long>(ref Unsafe.Add(ref dest, 8)) = Unsafe.As<byte, long>(ref Unsafe.Add(ref src, 8));
            Unsafe.As<byte, int>(ref Unsafe.Add(ref dest, 16)) = Unsafe.As<byte, int>(ref Unsafe.Add(ref src, 16));
            Unsafe.As<byte, short>(ref Unsafe.Add(ref dest, 20)) = Unsafe.As<byte, short>(ref Unsafe.Add(ref src, 20));
            Unsafe.AddByteOffset(ref dest, (IntPtr)22) = Unsafe.AddByteOffset(ref src, (IntPtr)22);

            idx += 23;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteRaw24(ref byte destination, ref byte src, ref int idx)
        {
            ref byte dest = ref Unsafe.Add(ref destination, (IntPtr)(ulong)idx);

            Unsafe.As<byte, long>(ref dest) = Unsafe.As<byte, long>(ref src);
            Unsafe.As<byte, long>(ref Unsafe.Add(ref dest, 8)) = Unsafe.As<byte, long>(ref Unsafe.Add(ref src, 8));
            Unsafe.As<byte, long>(ref Unsafe.Add(ref dest, 16)) = Unsafe.As<byte, long>(ref Unsafe.Add(ref src, 16));

            idx += 24;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteRaw25(ref byte destination, ref byte src, ref int idx)
        {
            ref byte dest = ref Unsafe.Add(ref destination, (IntPtr)(ulong)idx);

            Unsafe.As<byte, long>(ref dest) = Unsafe.As<byte, long>(ref src);
            Unsafe.As<byte, long>(ref Unsafe.Add(ref dest, 8)) = Unsafe.As<byte, long>(ref Unsafe.Add(ref src, 8));
            Unsafe.As<byte, long>(ref Unsafe.Add(ref dest, 16)) = Unsafe.As<byte, long>(ref Unsafe.Add(ref src, 16));
            Unsafe.AddByteOffset(ref dest, (IntPtr)24) = Unsafe.AddByteOffset(ref src, (IntPtr)24);

            idx += 25;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteRaw26(ref byte destination, ref byte src, ref int idx)
        {
            ref byte dest = ref Unsafe.Add(ref destination, (IntPtr)(ulong)idx);

            Unsafe.As<byte, long>(ref dest) = Unsafe.As<byte, long>(ref src);
            Unsafe.As<byte, long>(ref Unsafe.Add(ref dest, 8)) = Unsafe.As<byte, long>(ref Unsafe.Add(ref src, 8));
            Unsafe.As<byte, long>(ref Unsafe.Add(ref dest, 16)) = Unsafe.As<byte, long>(ref Unsafe.Add(ref src, 16));
            Unsafe.As<byte, short>(ref Unsafe.Add(ref dest, 24)) = Unsafe.As<byte, short>(ref Unsafe.Add(ref src, 24));

            idx += 26;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteRaw27(ref byte destination, ref byte src, ref int idx)
        {
            ref byte dest = ref Unsafe.Add(ref destination, (IntPtr)(ulong)idx);

            Unsafe.As<byte, long>(ref dest) = Unsafe.As<byte, long>(ref src);
            Unsafe.As<byte, long>(ref Unsafe.Add(ref dest, 8)) = Unsafe.As<byte, long>(ref Unsafe.Add(ref src, 8));
            Unsafe.As<byte, long>(ref Unsafe.Add(ref dest, 16)) = Unsafe.As<byte, long>(ref Unsafe.Add(ref src, 16));
            Unsafe.As<byte, short>(ref Unsafe.Add(ref dest, 24)) = Unsafe.As<byte, short>(ref Unsafe.Add(ref src, 24));
            Unsafe.AddByteOffset(ref dest, (IntPtr)26) = Unsafe.AddByteOffset(ref src, (IntPtr)26);

            idx += 27;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteRaw28(ref byte destination, ref byte src, ref int idx)
        {
            ref byte dest = ref Unsafe.Add(ref destination, (IntPtr)(ulong)idx);

            Unsafe.As<byte, long>(ref dest) = Unsafe.As<byte, long>(ref src);
            Unsafe.As<byte, long>(ref Unsafe.Add(ref dest, 8)) = Unsafe.As<byte, long>(ref Unsafe.Add(ref src, 8));
            Unsafe.As<byte, long>(ref Unsafe.Add(ref dest, 16)) = Unsafe.As<byte, long>(ref Unsafe.Add(ref src, 16));
            Unsafe.As<byte, int>(ref Unsafe.Add(ref dest, 24)) = Unsafe.As<byte, int>(ref Unsafe.Add(ref src, 24));

            idx += 28;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteRaw29(ref byte destination, ref byte src, ref int idx)
        {
            ref byte dest = ref Unsafe.Add(ref destination, (IntPtr)(ulong)idx);

            Unsafe.As<byte, long>(ref dest) = Unsafe.As<byte, long>(ref src);
            Unsafe.As<byte, long>(ref Unsafe.Add(ref dest, 8)) = Unsafe.As<byte, long>(ref Unsafe.Add(ref src, 8));
            Unsafe.As<byte, long>(ref Unsafe.Add(ref dest, 16)) = Unsafe.As<byte, long>(ref Unsafe.Add(ref src, 16));
            Unsafe.As<byte, int>(ref Unsafe.Add(ref dest, 24)) = Unsafe.As<byte, int>(ref Unsafe.Add(ref src, 24));
            Unsafe.AddByteOffset(ref dest, (IntPtr)28) = Unsafe.AddByteOffset(ref src, (IntPtr)28);

            idx += 29;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteRaw30(ref byte destination, ref byte src, ref int idx)
        {
            ref byte dest = ref Unsafe.Add(ref destination, (IntPtr)(ulong)idx);

            Unsafe.As<byte, long>(ref dest) = Unsafe.As<byte, long>(ref src);
            Unsafe.As<byte, long>(ref Unsafe.Add(ref dest, 8)) = Unsafe.As<byte, long>(ref Unsafe.Add(ref src, 8));
            Unsafe.As<byte, long>(ref Unsafe.Add(ref dest, 16)) = Unsafe.As<byte, long>(ref Unsafe.Add(ref src, 16));
            Unsafe.As<byte, int>(ref Unsafe.Add(ref dest, 24)) = Unsafe.As<byte, int>(ref Unsafe.Add(ref src, 24));
            Unsafe.As<byte, short>(ref Unsafe.Add(ref dest, 28)) = Unsafe.As<byte, short>(ref Unsafe.Add(ref src, 28));

            idx += 30;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteRaw31(ref byte destination, ref byte src, ref int idx)
        {
            ref byte dest = ref Unsafe.Add(ref destination, (IntPtr)(ulong)idx);

            Unsafe.As<byte, long>(ref dest) = Unsafe.As<byte, long>(ref src);
            Unsafe.As<byte, long>(ref Unsafe.Add(ref dest, 8)) = Unsafe.As<byte, long>(ref Unsafe.Add(ref src, 8));
            Unsafe.As<byte, long>(ref Unsafe.Add(ref dest, 16)) = Unsafe.As<byte, long>(ref Unsafe.Add(ref src, 16));
            Unsafe.As<byte, int>(ref Unsafe.Add(ref dest, 24)) = Unsafe.As<byte, int>(ref Unsafe.Add(ref src, 24));
            Unsafe.As<byte, short>(ref Unsafe.Add(ref dest, 28)) = Unsafe.As<byte, short>(ref Unsafe.Add(ref src, 28));
            Unsafe.AddByteOffset(ref dest, (IntPtr)30) = Unsafe.AddByteOffset(ref src, (IntPtr)30);

            idx += 31;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteRaw32(ref byte destination, ref byte src, ref int idx)
        {
            ref byte dest = ref Unsafe.Add(ref destination, (IntPtr)(ulong)idx);

            Unsafe.As<byte, long>(ref dest) = Unsafe.As<byte, long>(ref src);
            Unsafe.As<byte, long>(ref Unsafe.Add(ref dest, 8)) = Unsafe.As<byte, long>(ref Unsafe.Add(ref src, 8));
            Unsafe.As<byte, long>(ref Unsafe.Add(ref dest, 16)) = Unsafe.As<byte, long>(ref Unsafe.Add(ref src, 16));
            Unsafe.As<byte, long>(ref Unsafe.Add(ref dest, 24)) = Unsafe.As<byte, long>(ref Unsafe.Add(ref src, 24));

            idx += 32;
        }
    }
}
