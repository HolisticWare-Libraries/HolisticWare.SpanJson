// Largely based on https://github.com/neuecc/Utf8Json/blob/master/src/Utf8Json/Internal/GuidBits.cs

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace SpanJson.Internal
{
    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    internal struct GuidBits
    {
        [FieldOffset(0)]
        public readonly Guid Value;

        [FieldOffset(0)]
        public readonly byte Byte0;
        [FieldOffset(1)]
        public readonly byte Byte1;
        [FieldOffset(2)]
        public readonly byte Byte2;
        [FieldOffset(3)]
        public readonly byte Byte3;
        [FieldOffset(4)]
        public readonly byte Byte4;
        [FieldOffset(5)]
        public readonly byte Byte5;
        [FieldOffset(6)]
        public readonly byte Byte6;
        [FieldOffset(7)]
        public readonly byte Byte7;
        [FieldOffset(8)]
        public readonly byte Byte8;
        [FieldOffset(9)]
        public readonly byte Byte9;
        [FieldOffset(10)]
        public readonly byte Byte10;
        [FieldOffset(11)]
        public readonly byte Byte11;
        [FieldOffset(12)]
        public readonly byte Byte12;
        [FieldOffset(13)]
        public readonly byte Byte13;
        [FieldOffset(14)]
        public readonly byte Byte14;
        [FieldOffset(15)]
        public readonly byte Byte15;

        public GuidBits(ref Guid value)
        {
            this = default(GuidBits);
            this.Value = value;
        }

        // 4-pattern, lower/upper and '-' or no
        public GuidBits(ref ArraySegment<byte> utf8string)
        {
            this = default(GuidBits);

            var array = utf8string.Array;
            var offset = utf8string.Offset;

            // 32
            if (utf8string.Count == 32)
            {
                if (BitConverter.IsLittleEndian)
                {
                    this.Byte0 = Parse(array, offset + 6);
                    this.Byte1 = Parse(array, offset + 4);
                    this.Byte2 = Parse(array, offset + 2);
                    this.Byte3 = Parse(array, offset + 0);

                    this.Byte4 = Parse(array, offset + 10);
                    this.Byte5 = Parse(array, offset + 8);

                    this.Byte6 = Parse(array, offset + 14);
                    this.Byte7 = Parse(array, offset + 12);
                }
                else
                {
                    this.Byte0 = Parse(array, offset + 0);
                    this.Byte1 = Parse(array, offset + 2);
                    this.Byte2 = Parse(array, offset + 4);
                    this.Byte3 = Parse(array, offset + 6);

                    this.Byte4 = Parse(array, offset + 8);
                    this.Byte5 = Parse(array, offset + 10);

                    this.Byte6 = Parse(array, offset + 12);
                    this.Byte7 = Parse(array, offset + 14);
                }
                this.Byte8 = Parse(array, offset + 16);
                this.Byte9 = Parse(array, offset + 18);

                this.Byte10 = Parse(array, offset + 20);
                this.Byte11 = Parse(array, offset + 22);
                this.Byte12 = Parse(array, offset + 24);
                this.Byte13 = Parse(array, offset + 26);
                this.Byte14 = Parse(array, offset + 28);
                this.Byte15 = Parse(array, offset + 30);
                return;
            }
            else if (utf8string.Count == 36)
            {
                // '-' => 45
                if (BitConverter.IsLittleEndian)
                {
                    this.Byte0 = Parse(array, offset + 6);
                    this.Byte1 = Parse(array, offset + 4);
                    this.Byte2 = Parse(array, offset + 2);
                    this.Byte3 = Parse(array, offset + 0);

                    if (array[offset + 8] != '-') goto ERROR;

                    this.Byte4 = Parse(array, offset + 11);
                    this.Byte5 = Parse(array, offset + 9);

                    if (array[offset + 13] != '-') goto ERROR;

                    this.Byte6 = Parse(array, offset + 16);
                    this.Byte7 = Parse(array, offset + 14);
                }
                else
                {
                    this.Byte0 = Parse(array, offset + 0);
                    this.Byte1 = Parse(array, offset + 2);
                    this.Byte2 = Parse(array, offset + 4);
                    this.Byte3 = Parse(array, offset + 6);

                    if (array[offset + 8] != '-') goto ERROR;

                    this.Byte4 = Parse(array, offset + 9);
                    this.Byte5 = Parse(array, offset + 11);

                    if (array[offset + 13] != '-') goto ERROR;

                    this.Byte6 = Parse(array, offset + 14);
                    this.Byte7 = Parse(array, offset + 16);
                }

                if (array[offset + 18] != '-') goto ERROR;

                this.Byte8 = Parse(array, offset + 19);
                this.Byte9 = Parse(array, offset + 21);

                if (array[offset + 23] != '-') goto ERROR;

                this.Byte10 = Parse(array, offset + 24);
                this.Byte11 = Parse(array, offset + 26);
                this.Byte12 = Parse(array, offset + 28);
                this.Byte13 = Parse(array, offset + 30);
                this.Byte14 = Parse(array, offset + 32);
                this.Byte15 = Parse(array, offset + 34);
                return;
            }

        ERROR:
            ThrowHelper.ThrowArgumentException_Guid_Pattern();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static byte Parse(byte[] bytes, int highOffset)
        {
            return unchecked((byte)(SwitchParse(bytes[highOffset]) * 16 + SwitchParse(bytes[highOffset + 1])));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static byte SwitchParse(byte b)
        {
            // '0'(48) ~ '9'(57) => -48
            // 'A'(65) ~ 'F'(70) => -55
            // 'a'(97) ~ 'f'(102) => -87
            switch (b)
            {
                case 48:
                case 49:
                case 50:
                case 51:
                case 52:
                case 53:
                case 54:
                case 55:
                case 56:
                case 57:
                    return unchecked((byte)((b - 48)));
                case 65:
                case 66:
                case 67:
                case 68:
                case 69:
                case 70:
                    return unchecked((byte)((b - 55)));
                case 97:
                case 98:
                case 99:
                case 100:
                case 101:
                case 102:
                    return unchecked((byte)((b - 87)));
                case 0:
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                case 6:
                case 7:
                case 8:
                case 9:
                case 10:
                case 11:
                case 12:
                case 13:
                case 14:
                case 15:
                case 16:
                case 17:
                case 18:
                case 19:
                case 20:
                case 21:
                case 22:
                case 23:
                case 24:
                case 25:
                case 26:
                case 27:
                case 28:
                case 29:
                case 30:
                case 31:
                case 32:
                case 33:
                case 34:
                case 35:
                case 36:
                case 37:
                case 38:
                case 39:
                case 40:
                case 41:
                case 42:
                case 43:
                case 44:
                case 45:
                case 46:
                case 47:
                case 58:
                case 59:
                case 60:
                case 61:
                case 62:
                case 63:
                case 64:
                case 71:
                case 72:
                case 73:
                case 74:
                case 75:
                case 76:
                case 77:
                case 78:
                case 79:
                case 80:
                case 81:
                case 82:
                case 83:
                case 84:
                case 85:
                case 86:
                case 87:
                case 88:
                case 89:
                case 90:
                case 91:
                case 92:
                case 93:
                case 94:
                case 95:
                case 96:
                default:
                    throw ThrowHelper.GetArgumentException_Guid_Pattern();
            }
        }

        // 4(x2) - 2(x2) - 2(x2) - 2(x2) - 6(x2)
        public void Write(ref byte destination, ref int pos)
        {
            var high = JsonHelpers.ByteToHexStringHigh;
            var low = JsonHelpers.ByteToHexStringLow;

            var offset = (IntPtr)pos;
            if (BitConverter.IsLittleEndian)
            {
                // int(_a)
                Unsafe.AddByteOffset(ref destination, offset) = high[Byte3];
                Unsafe.AddByteOffset(ref destination, offset + 1) = low[Byte3];
                Unsafe.AddByteOffset(ref destination, offset + 2) = high[Byte2];
                Unsafe.AddByteOffset(ref destination, offset + 3) = low[Byte2];
                Unsafe.AddByteOffset(ref destination, offset + 4) = high[Byte1];
                Unsafe.AddByteOffset(ref destination, offset + 5) = low[Byte1];
                Unsafe.AddByteOffset(ref destination, offset + 6) = high[Byte0];
                Unsafe.AddByteOffset(ref destination, offset + 7) = low[Byte0];

                offset += 8;
                Unsafe.AddByteOffset(ref destination, offset) = (byte)'-';       // 8

                // short(_b)
                Unsafe.AddByteOffset(ref destination, offset + 1) = high[Byte5]; // 9
                Unsafe.AddByteOffset(ref destination, offset + 2) = low[Byte5];  // 10
                Unsafe.AddByteOffset(ref destination, offset + 3) = high[Byte4]; // 11
                Unsafe.AddByteOffset(ref destination, offset + 4) = low[Byte4];  // 12

                Unsafe.AddByteOffset(ref destination, offset + 5) = (byte)'-';   // 13

                // short(_c)
                Unsafe.AddByteOffset(ref destination, offset + 6) = high[Byte7]; // 14
                Unsafe.AddByteOffset(ref destination, offset + 7) = low[Byte7];  // 15

                offset += 8;

                Unsafe.AddByteOffset(ref destination, offset) = high[Byte6];     // 16
                Unsafe.AddByteOffset(ref destination, offset + 1) = low[Byte6];  // 17
            }
            else
            {
                Unsafe.AddByteOffset(ref destination, offset) = high[Byte0];
                Unsafe.AddByteOffset(ref destination, offset + 1) = low[Byte0];
                Unsafe.AddByteOffset(ref destination, offset + 2) = high[Byte1];
                Unsafe.AddByteOffset(ref destination, offset + 3) = low[Byte1];
                Unsafe.AddByteOffset(ref destination, offset + 4) = high[Byte2];
                Unsafe.AddByteOffset(ref destination, offset + 5) = low[Byte2];
                Unsafe.AddByteOffset(ref destination, offset + 6) = high[Byte3];
                Unsafe.AddByteOffset(ref destination, offset + 7) = low[Byte3];

                offset += 8;
                Unsafe.AddByteOffset(ref destination, offset) = (byte)'-';       // 8

                Unsafe.AddByteOffset(ref destination, offset + 1) = high[Byte4]; // 9
                Unsafe.AddByteOffset(ref destination, offset + 2) = low[Byte4];  // 10
                Unsafe.AddByteOffset(ref destination, offset + 3) = high[Byte5]; // 11
                Unsafe.AddByteOffset(ref destination, offset + 4) = low[Byte5];  // 12

                Unsafe.AddByteOffset(ref destination, offset + 5) = (byte)'-';   // 13

                Unsafe.AddByteOffset(ref destination, offset + 6) = high[Byte6]; // 14
                Unsafe.AddByteOffset(ref destination, offset + 7) = low[Byte6];  // 15

                offset += 8;

                Unsafe.AddByteOffset(ref destination, offset) = high[Byte7];     // 16
                Unsafe.AddByteOffset(ref destination, offset + 1) = low[Byte7];  // 17
            }

            Unsafe.AddByteOffset(ref destination, offset + 2) = (byte)'-';       // 18

            Unsafe.AddByteOffset(ref destination, offset + 3) = high[Byte8];     // 19
            Unsafe.AddByteOffset(ref destination, offset + 4) = low[Byte8];      // 20
            Unsafe.AddByteOffset(ref destination, offset + 5) = high[Byte9];     // 21
            Unsafe.AddByteOffset(ref destination, offset + 6) = low[Byte9];      // 22

            Unsafe.AddByteOffset(ref destination, offset + 7) = (byte)'-';       // 23

            offset += 8;

            Unsafe.AddByteOffset(ref destination, offset) = high[Byte10];        // 24
            Unsafe.AddByteOffset(ref destination, offset + 1) = low[Byte10];     // 25
            Unsafe.AddByteOffset(ref destination, offset + 2) = high[Byte11];    // 26
            Unsafe.AddByteOffset(ref destination, offset + 3) = low[Byte11];     // 27
            Unsafe.AddByteOffset(ref destination, offset + 4) = high[Byte12];    // 28
            Unsafe.AddByteOffset(ref destination, offset + 5) = low[Byte12];     // 29
            Unsafe.AddByteOffset(ref destination, offset + 6) = high[Byte13];    // 30
            Unsafe.AddByteOffset(ref destination, offset + 7) = low[Byte13];     // 31

            offset += 8;

            Unsafe.AddByteOffset(ref destination, offset) = high[Byte14];        // 32
            Unsafe.AddByteOffset(ref destination, offset + 1) = low[Byte14];     // 33
            Unsafe.AddByteOffset(ref destination, offset + 2) = high[Byte15];    // 34
            Unsafe.AddByteOffset(ref destination, offset + 3) = low[Byte15];     // 35

            pos += 36;
        }

        public void Write(ref char destination, ref int pos)
        {
            nint offset = pos;
            ref char current = ref Add(ref destination, offset);

            var high = JsonHelpers.CharToHexStringHigh;
            var low = JsonHelpers.CharToHexStringLow;

            if (BitConverter.IsLittleEndian)
            {
                // int(_a)
                current = high[Byte3];             // 0
                Add(ref current, 1) = low[Byte3];  // 1
                Add(ref current, 2) = high[Byte2]; // 2
                Add(ref current, 3) = low[Byte2];  // 3
                offset += 4; current = ref Add(ref destination, offset);
                current = high[Byte1]; // 4
                Add(ref current, 1) = low[Byte1];  // 5
                Add(ref current, 2) = high[Byte0]; // 6
                Add(ref current, 3) = low[Byte0];  // 7

                offset += 4; current = ref Add(ref destination, offset);
                current = '-';                     // 8

                // short(_b)
                Add(ref current, 1) = high[Byte5]; // 9
                Add(ref current, 2) = low[Byte5];  // 10
                Add(ref current, 3) = high[Byte4]; // 11
                offset += 4; current = ref Add(ref destination, offset);
                current = low[Byte4];              // 12

                Add(ref current, 1) = '-';         // 13

                // short(_c)
                Add(ref current, 2) = high[Byte7]; // 14
                Add(ref current, 3) = low[Byte7];  // 15
                offset += 4; current = ref Add(ref destination, offset);
                current = high[Byte6];             // 16
                Add(ref current, 1) = low[Byte6];  // 17
            }
            else
            {
                current = high[Byte0];             // 0
                Add(ref current, 1) = low[Byte0];  // 1
                Add(ref current, 2) = high[Byte1]; // 2
                Add(ref current, 3) = low[Byte1];  // 3
                offset += 4; current = ref Add(ref destination, offset);
                current = high[Byte2]; // 4
                Add(ref current, 1) = low[Byte2];  // 5
                Add(ref current, 2) = high[Byte3]; // 6
                Add(ref current, 3) = low[Byte3];  // 7

                offset += 4; current = ref Add(ref destination, offset);
                current = '-';                     // 8

                Add(ref current, 1) = high[Byte4]; // 9
                Add(ref current, 2) = low[Byte4];  // 10
                Add(ref current, 3) = high[Byte5]; // 11
                offset += 4; current = ref Add(ref destination, offset);
                current = low[Byte5];              // 12

                Add(ref current, 1) = '-';         // 13

                Add(ref current, 2) = high[Byte6]; // 14
                Add(ref current, 3) = low[Byte6];  // 15
                offset += 4; current = ref Add(ref destination, offset);
                current = high[Byte7];             // 16
                Add(ref current, 1) = low[Byte7];  // 17
            }

            Add(ref current, 2) = '-';             // 18

            Add(ref current, 3) = high[Byte8];     // 19
            offset += 4; current = ref Add(ref destination, offset);
            current = low[Byte8];                  // 20
            Add(ref current, 1) = high[Byte9];     // 21
            Add(ref current, 2) = low[Byte9];      // 22

            Add(ref current, 3) = '-';             // 23

            offset += 4; current = ref Add(ref destination, offset);
            current = high[Byte10];                // 24
            Add(ref current, 1) = low[Byte10];     // 25
            Add(ref current, 2) = high[Byte11];    // 26
            Add(ref current, 3) = low[Byte11];     // 27
            offset += 4; current = ref Add(ref destination, offset);
            current = high[Byte12];                // 28
            Add(ref current, 1) = low[Byte12];     // 29
            Add(ref current, 2) = high[Byte13];    // 30
            Add(ref current, 3) = low[Byte13];     // 31
            offset += 4; current = ref Add(ref destination, offset);
            current = high[Byte14];                // 32
            Add(ref current, 1) = low[Byte14];     // 33
            Add(ref current, 2) = high[Byte15];    // 34
            Add(ref current, 3) = low[Byte15];     // 35

            pos += 36;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static ref char Add(ref char source, nint elementOffset)
            => ref Unsafe.Add(ref source, (IntPtr)elementOffset);
    }
}