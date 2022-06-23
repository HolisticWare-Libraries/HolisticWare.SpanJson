namespace SpanJson;

internal static class SymbolHelper<TSymbol>
{
    public static readonly bool IsUtf8;

    public static readonly bool IsUtf16;

    static SymbolHelper()
    {
        IsUtf8 = typeof(TSymbol) == typeof(byte);
        IsUtf16 = typeof(TSymbol) == typeof(char);
    }
}