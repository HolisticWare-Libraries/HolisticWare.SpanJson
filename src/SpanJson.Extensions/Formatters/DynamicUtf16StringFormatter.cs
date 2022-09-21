using System.Buffers;
using System.Runtime.InteropServices;
using SpanJson.Dynamic;
using SpanJson.Internal;
#if NETSTANDARD2_0
using System.Runtime.CompilerServices;
#endif

namespace SpanJson.Formatters
{
    public sealed class DynamicUtf16StringFormatter : DynamicFormatterBase<SpanJsonDynamicUtf16String>
    {
        public static readonly DynamicUtf16StringFormatter Default = new DynamicUtf16StringFormatter();

        public override void Serialize(ref JsonWriter<byte> writer, SpanJsonDynamicUtf16String? value, IJsonFormatterResolver<byte> resolver)
        {
            if (value is null) { writer.WriteUtf8Null(); return; }

            ReadOnlySpan<char> utf16Json = value.Symbols;
            var maxRequired = utf16Json.Length * JsonSharedConstant.MaxExpansionFactorWhileTranscoding;

            byte[]? valueArray = null;

            Span<byte> utf8Json = (uint)maxRequired <= JsonSharedConstant.StackallocByteThresholdU ?
                stackalloc byte[JsonSharedConstant.StackallocByteThreshold] :
                (valueArray = ArrayPool<byte>.Shared.Rent(maxRequired));
            var written = TextEncodings.Utf8.GetBytes(utf16Json, utf8Json);

#if NETSTANDARD2_0
            unsafe
            {
                writer.WriteUtf8Verbatim(new ReadOnlySpan<byte>(Unsafe.AsPointer(ref MemoryMarshal.GetReference(utf8Json)), written));
            }
#else
            writer.WriteUtf8Verbatim(MemoryMarshal.CreateReadOnlySpan(ref MemoryMarshal.GetReference(utf8Json), written));
#endif

            if (valueArray is not null) { ArrayPool<byte>.Shared.Return(valueArray); }
        }

        public override void Serialize(ref JsonWriter<char> writer, SpanJsonDynamicUtf16String? value, IJsonFormatterResolver<char> resolver)
        {
            if (value is null) { writer.WriteUtf16Null(); return; }

            writer.WriteUtf16Verbatim(value.Symbols);
        }
    }
}
