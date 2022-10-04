namespace SpanJson.Resolvers
{
    public sealed class ExcludeNullsCobolCaseResolver<TSymbol> : ResolverBase<TSymbol, ExcludeNullsCobolCaseResolver<TSymbol>> where TSymbol : struct
    {
        public ExcludeNullsCobolCaseResolver()
            : base(new SpanJsonOptions(NullOptions.ExcludeNulls, EnumOptions.String, JsonNamingPolicy.CobolCase, JsonNamingPolicy.CobolCase, JsonNamingPolicy.CobolCase))
        {
        }
    }
}