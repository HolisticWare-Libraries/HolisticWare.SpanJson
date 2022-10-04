namespace SpanJson.Resolvers
{
    public sealed class IncludeNullsOriginalCaseResolver<TSymbol> : ResolverBase<TSymbol, IncludeNullsOriginalCaseResolver<TSymbol>> where TSymbol : struct
    {
        public IncludeNullsOriginalCaseResolver()
            : base(new SpanJsonOptions(NullOptions.IncludeNulls, EnumOptions.String))
        {
        }
    }
}