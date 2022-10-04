namespace SpanJson.Resolvers
{
    public sealed class IncludeNullsKebabCaseResolver<TSymbol> : ResolverBase<TSymbol, IncludeNullsKebabCaseResolver<TSymbol>> where TSymbol : struct
    {
        public IncludeNullsKebabCaseResolver()
            : base(new SpanJsonOptions(NullOptions.IncludeNulls, EnumOptions.String, JsonNamingPolicy.KebabCase, JsonNamingPolicy.KebabCase, JsonNamingPolicy.KebabCase))
        {
        }
    }
}