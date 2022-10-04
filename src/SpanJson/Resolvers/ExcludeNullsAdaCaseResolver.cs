namespace SpanJson.Resolvers
{
    public sealed class ExcludeNullsAdaCaseResolver<TSymbol> : ResolverBase<TSymbol, ExcludeNullsAdaCaseResolver<TSymbol>> where TSymbol : struct
    {
        public ExcludeNullsAdaCaseResolver()
            : base(new SpanJsonOptions(NullOptions.ExcludeNulls, EnumOptions.String, JsonNamingPolicy.AdaCase, JsonNamingPolicy.AdaCase, JsonNamingPolicy.AdaCase))
        {
        }
    }
}