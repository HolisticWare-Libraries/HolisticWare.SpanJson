namespace SpanJson.Resolvers
{
    public sealed class IncludeNullsCamelCaseResolver<TSymbol> : ResolverBase<TSymbol, IncludeNullsCamelCaseResolver<TSymbol>> where TSymbol : struct
    {
        public IncludeNullsCamelCaseResolver()
            : base(new SpanJsonOptions(NullOptions.IncludeNulls, EnumOptions.String, JsonNamingPolicy.CamelCase, JsonNamingPolicy.CamelCase, JsonNamingPolicy.CamelCase))
        {
        }
    }
}