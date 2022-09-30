using System.Dynamic;
using System.Runtime.CompilerServices;
using System.Text;

namespace SpanJson.Dynamic
{
    public abstract class SpanJsonDynamic<TSymbol> : DynamicObject, ISpanJsonDynamicValue<TSymbol> where TSymbol : struct
    {
        protected SpanJsonDynamic(in ReadOnlySpan<TSymbol> span, bool isFloat)
        {
            Symbols = new ArraySegment<TSymbol>(span.ToArray());
            IsFloat = isFloat;
        }

#if !NETSTANDARD2_0
        protected SpanJsonDynamic(in ArraySegment<TSymbol> data, bool isFloat)
#else
        protected SpanJsonDynamic(ArraySegment<TSymbol> data, bool isFloat)
#endif
        {
            Symbols = data;
            IsFloat = isFloat;
        }

        [JsonIgnore]
        internal readonly bool IsFloat;

        [JsonIgnore]
        public ArraySegment<TSymbol> Symbols { get; }

        protected abstract BaseDynamicTypeConverter<TSymbol> Converter { get; }

        public virtual bool TryConvert(Type outputType, out object? result)
        {
            return Converter.TryConvertTo(outputType, Symbols, out result);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string ToString()
        {
            if (SymbolHelper<TSymbol>.IsUtf8)
            {
                var jsonRaw = Symbols;
                var temp = jsonRaw.Array;
                var bytes = Unsafe.As<byte[]>(temp);
                return Encoding.UTF8.GetString(bytes!, jsonRaw.Offset, jsonRaw.Count);
            }

            if (SymbolHelper<TSymbol>.IsUtf16)
            {
                var jsonRaw = Symbols;
                var temp = jsonRaw.Array;
                var chars = Unsafe.As<char[]>(temp);
                return new string(chars!, jsonRaw.Offset, jsonRaw.Count);
            }

            throw ThrowHelper.GetNotSupportedException();
        }

        public virtual string ToJsonValue() => ToString();

        public override bool TryConvert(ConvertBinder binder, out object? result)
        {
            return TryConvert(binder.ReturnType, out result);
        }
    }
}