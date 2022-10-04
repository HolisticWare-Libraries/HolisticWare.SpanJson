namespace SpanJson.Resolvers
{
    public sealed class ExcludeNullsKebabCaseResolver<TSymbol> : ResolverBase<TSymbol, ExcludeNullsKebabCaseResolver<TSymbol>> where TSymbol : struct
    {
        public ExcludeNullsKebabCaseResolver()
            : base(new SpanJsonOptions(NullOptions.ExcludeNulls, EnumOptions.String, JsonNamingPolicy.KebabCase, JsonNamingPolicy.KebabCase, JsonNamingPolicy.KebabCase))
        {
        }
    }
}