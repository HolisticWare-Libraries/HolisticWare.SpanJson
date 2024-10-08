﻿using System.Buffers;
using System.Runtime.InteropServices;
using SpanJson.Dynamic;
using SpanJson.Internal;
#if NETSTANDARD2_0
using System.Runtime.CompilerServices;
#endif

namespace SpanJson.Formatters
{
    public sealed class DynamicUtf8NumberFormatter : DynamicFormatterBase<SpanJsonDynamicUtf8Number>
    {
        public static readonly DynamicUtf8NumberFormatter Default = new DynamicUtf8NumberFormatter();

        public override void Serialize(ref JsonWriter<byte> writer, SpanJsonDynamicUtf8Number? value, IJsonFormatterResolver<byte> resolver)
        {
            if (value is null) { writer.WriteUtf8Null(); return; }

            writer.WriteUtf8Verbatim(value.Symbols);
        }

        public override void Serialize(ref JsonWriter<char> writer, SpanJsonDynamicUtf8Number? value, IJsonFormatterResolver<char> resolver)
        {
            if (value is null) { writer.WriteUtf16Null(); return; }

            ReadOnlySpan<byte> utf8Json = value.Symbols;
            var maxRequired = TextEncodings.Utf8.GetCharCount(utf8Json);

            char[]? valueArray = null;

            Span<char> utf16Json = (uint)maxRequired <= JsonSharedConstant.StackallocCharThresholdU ?
                stackalloc char[JsonSharedConstant.StackallocCharThreshold] :
                (valueArray = ArrayPool<char>.Shared.Rent(maxRequired));
            var written = TextEncodings.Utf8.GetChars(utf8Json, utf16Json);

#if NETSTANDARD2_0
            unsafe
            {
                writer.WriteUtf16Verbatim(new ReadOnlySpan<char>(Unsafe.AsPointer(ref MemoryMarshal.GetReference(utf16Json)), written));
            }
#else
            writer.WriteUtf16Verbatim(MemoryMarshal.CreateReadOnlySpan(ref MemoryMarshal.GetReference(utf16Json), written));
#endif

            if (valueArray is not null) { ArrayPool<char>.Shared.Return(valueArray); }
        }
    }
}
