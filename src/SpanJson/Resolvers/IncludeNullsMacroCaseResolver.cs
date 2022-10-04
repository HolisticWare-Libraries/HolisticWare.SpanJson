namespace SpanJson.Resolvers
{
    public sealed class IncludeNullsMacroCaseResolver<TSymbol> : ResolverBase<TSymbol, IncludeNullsMacroCaseResolver<TSymbol>> where TSymbol : struct
    {
        public IncludeNullsMacroCaseResolver()
            : base(new SpanJsonOptions(NullOptions.IncludeNulls, EnumOptions.String, JsonNamingPolicy.MacroCase, JsonNamingPolicy.MacroCase, JsonNamingPolicy.MacroCase))
        {
        }
    }
}