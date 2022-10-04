namespace SpanJson.Resolvers
{
    public sealed class IncludeNullsSnakeCaseResolver<TSymbol> : ResolverBase<TSymbol, IncludeNullsSnakeCaseResolver<TSymbol>> where TSymbol : struct
    {
        public IncludeNullsSnakeCaseResolver()
            : base(new SpanJsonOptions(NullOptions.IncludeNulls, EnumOptions.String, JsonNamingPolicy.SnakeCase, JsonNamingPolicy.SnakeCase, JsonNamingPolicy.SnakeCase))
        {
        }
    }
}