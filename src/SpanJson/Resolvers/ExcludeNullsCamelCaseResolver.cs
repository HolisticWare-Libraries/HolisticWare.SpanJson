namespace SpanJson.Resolvers
{
    public sealed class ExcludeNullsCamelCaseResolver<TSymbol> : ResolverBase<TSymbol, ExcludeNullsCamelCaseResolver<TSymbol>> where TSymbol : struct
    {
        public ExcludeNullsCamelCaseResolver()
            : base(new SpanJsonOptions(NullOptions.ExcludeNulls, EnumOptions.String, JsonNamingPolicy.CamelCase, JsonNamingPolicy.CamelCase, JsonNamingPolicy.CamelCase))
        {
        }
    }
}