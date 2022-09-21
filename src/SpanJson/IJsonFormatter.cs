namespace SpanJson
{
    public interface IJsonFormatter
    {
    }

    public interface ICustomJsonFormatter : IJsonFormatter
    {
        object? Arguments { get; set; }
    }

    public interface ICustomJsonFormatter<T> : IJsonFormatter<T, byte>, IJsonFormatter<T, char>, ICustomJsonFormatter
    {
    }

    public interface IJsonFormatter<T, TSymbol> : IJsonFormatter where TSymbol : struct
    {
        void Serialize(ref JsonWriter<TSymbol> writer,
#nullable disable // T may or may not be nullable depending on the derived type's overload.
            T value,
#nullable restore
            IJsonFormatterResolver<TSymbol> resolver);
        T? Deserialize(ref JsonReader<TSymbol> reader, IJsonFormatterResolver<TSymbol> resolver);
    }
}