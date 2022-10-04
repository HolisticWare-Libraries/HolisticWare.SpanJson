namespace SpanJson.Resolvers
{
    public sealed class IncludeNullsCobolCaseResolver<TSymbol> : ResolverBase<TSymbol, IncludeNullsCobolCaseResolver<TSymbol>> where TSymbol : struct
    {
        public IncludeNullsCobolCaseResolver()
            : base(new SpanJsonOptions(NullOptions.IncludeNulls, EnumOptions.String, JsonNamingPolicy.CobolCase, JsonNamingPolicy.CobolCase, JsonNamingPolicy.CobolCase))
        {
        }
    }
}