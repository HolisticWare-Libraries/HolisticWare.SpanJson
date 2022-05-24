// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

// Largely based on https://github.com/dotnet/corefx/blob/8135319caa7e457ed61053ca1418313b88057b51/src/System.Text.Json/src/System/Text/Json/Writer/JsonWriterHelper.Transcoding.cs#L12

#if NET451
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text.Encodings.Web;

namespace SpanJson.Internal
{
    partial class EscapingHelper
    {
        public static partial class NonAscii
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static JsonEncodedText GetEncodedText(string text)
            {
                return s_encodedTextCache.GetOrAdd(text, s => JsonEncodedText.Encode(s, JsonEscapeHandling.EscapeNonAscii));
            }

            public static int NeedsEscaping(in ReadOnlySpan<byte> utf8Source, JavaScriptEncoder encoder = null)
            {
                int idx;

#if !NET451
                if (encoder is not null)
                {
                    idx = encoder.FindFirstCharacterToEncodeUtf8(utf8Source);
                    goto Return;
                }
#endif

                ref byte space = ref MemoryMarshal.GetReference(utf8Source);
                idx = 0;
                uint nlen = (uint)utf8Source.Length;
                while ((uint)idx < nlen)
                {
                    if (NeedsEscaping(Unsafe.Add(ref space, idx))) { goto Return; }
                    idx++;
                }

                idx = -1; // all characters allowed

            Return:
                return idx;
            }

            public static unsafe int NeedsEscaping(in ReadOnlySpan<char> utf16Source, JavaScriptEncoder encoder = null)
            {
                int idx;

#if !NET451
                // Some implementations of JavascriptEncoder.FindFirstCharacterToEncode may not accept
                // null pointers and gaurd against that. Hence, check up-front and fall down to return -1.
                if (encoder is not null && !utf16Source.IsEmpty)
                {
                    fixed (char* ptr = utf16Source)
                    {
                        idx = encoder.FindFirstCharacterToEncode(ptr, utf16Source.Length);
                    }
                    goto Return;
                }
#endif

                ref char space = ref MemoryMarshal.GetReference(utf16Source);
                idx = 0;
                uint nlen = (uint)utf16Source.Length;
                while ((uint)idx < nlen)
                {
                    if (NeedsEscaping(Unsafe.Add(ref space, idx))) { goto Return; }
                    idx++;
                }

                idx = -1; // all characters allowed

            Return:
                return idx;
            }

            public static void EscapeString(in ReadOnlySpan<byte> utf8Source, Span<byte> destination, int indexOfFirstByteToEscape, out int written)
            {
                Debug.Assert(indexOfFirstByteToEscape >= 0 && indexOfFirstByteToEscape < utf8Source.Length);

                utf8Source.Slice(0, indexOfFirstByteToEscape).CopyTo(destination);
                written = indexOfFirstByteToEscape;
                int consumed = indexOfFirstByteToEscape;

                ref byte sourceSpace = ref MemoryMarshal.GetReference(utf8Source);
                ref byte destSpace = ref MemoryMarshal.GetReference(destination);
                uint nlen = (uint)utf8Source.Length;
                while ((uint)consumed < nlen)
                {
                    byte val = Unsafe.Add(ref sourceSpace, consumed);
                    if (NeedsEscaping(val))
                    {
                        if (!EscapeNextBytes(JsonEscapeHandling.EscapeNonAscii, ref sourceSpace, ref consumed, nlen - (uint)consumed, destination, ref destSpace, ref written))
                        {
                            ThrowHelper.ThrowArgumentException_InvalidUTF8(utf8Source, consumed);
                        }
                    }
                    else
                    {
                        destination[written] = val;
                        written++;
                        consumed++;
                    }
                }
            }

            public static void EscapeString(in ReadOnlySpan<char> utf16Source, Span<char> destination, int indexOfFirstByteToEscape, out int written)
            {
                Debug.Assert(indexOfFirstByteToEscape >= 0 && indexOfFirstByteToEscape < utf16Source.Length);

                utf16Source.Slice(0, indexOfFirstByteToEscape).CopyTo(destination);
                written = indexOfFirstByteToEscape;
                int consumed = indexOfFirstByteToEscape;

                ref char sourceSpace = ref MemoryMarshal.GetReference(utf16Source);
                ref char destSpace = ref MemoryMarshal.GetReference(destination);
                uint nlen = (uint)utf16Source.Length;
                while ((uint)consumed < nlen)
                {
                    char val = Unsafe.Add(ref sourceSpace, consumed);
                    if (NeedsEscaping(val))
                    {
                        EscapeNextChars(JsonEscapeHandling.EscapeNonAscii, ref sourceSpace, nlen, val, ref destSpace, ref consumed, ref written);
                    }
                    else
                    {
                        Unsafe.Add(ref destSpace, written++) = val;
                    }
                    consumed++;
                }
            }
        }
    }
}
#endif