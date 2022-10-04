namespace SpanJson.Resolvers
{
    public sealed class ExcludeNullsSnakeCaseResolver<TSymbol> : ResolverBase<TSymbol, ExcludeNullsSnakeCaseResolver<TSymbol>> where TSymbol : struct
    {
        public ExcludeNullsSnakeCaseResolver()
            : base(new SpanJsonOptions(NullOptions.ExcludeNulls, EnumOptions.String, JsonNamingPolicy.SnakeCase, JsonNamingPolicy.SnakeCase, JsonNamingPolicy.SnakeCase))
        {
        }
    }
}