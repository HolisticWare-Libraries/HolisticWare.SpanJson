namespace SpanJson.Resolvers
{
    public sealed class IncludeNullsTrainCaseResolver<TSymbol> : ResolverBase<TSymbol, IncludeNullsTrainCaseResolver<TSymbol>> where TSymbol : struct
    {
        public IncludeNullsTrainCaseResolver()
            : base(new SpanJsonOptions(NullOptions.IncludeNulls, EnumOptions.String, JsonNamingPolicy.TrainCase, JsonNamingPolicy.TrainCase, JsonNamingPolicy.TrainCase))
        {
        }
    }
}