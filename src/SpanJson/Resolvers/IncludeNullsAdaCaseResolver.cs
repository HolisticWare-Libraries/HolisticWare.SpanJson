namespace SpanJson.Resolvers
{
    public sealed class IncludeNullsAdaCaseResolver<TSymbol> : ResolverBase<TSymbol, IncludeNullsAdaCaseResolver<TSymbol>> where TSymbol : struct
    {
        public IncludeNullsAdaCaseResolver()
            : base(new SpanJsonOptions(NullOptions.IncludeNulls, EnumOptions.String, JsonNamingPolicy.AdaCase, JsonNamingPolicy.AdaCase, JsonNamingPolicy.AdaCase))
        {
        }
    }
}