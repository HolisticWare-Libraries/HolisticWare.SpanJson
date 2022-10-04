namespace SpanJson.Resolvers
{
    public sealed class ExcludeNullsMacroCaseResolver<TSymbol> : ResolverBase<TSymbol, ExcludeNullsMacroCaseResolver<TSymbol>> where TSymbol : struct
    {
        public ExcludeNullsMacroCaseResolver()
            : base(new SpanJsonOptions(NullOptions.ExcludeNulls, EnumOptions.String, JsonNamingPolicy.MacroCase, JsonNamingPolicy.MacroCase, JsonNamingPolicy.MacroCase))
        {
        }
    }
}