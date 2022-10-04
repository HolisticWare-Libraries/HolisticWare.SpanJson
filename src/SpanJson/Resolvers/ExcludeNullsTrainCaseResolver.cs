namespace SpanJson.Resolvers
{
    public sealed class ExcludeNullsTrainCaseResolver<TSymbol> : ResolverBase<TSymbol, ExcludeNullsTrainCaseResolver<TSymbol>> where TSymbol : struct
    {
        public ExcludeNullsTrainCaseResolver()
            : base(new SpanJsonOptions(NullOptions.ExcludeNulls, EnumOptions.String, JsonNamingPolicy.TrainCase, JsonNamingPolicy.TrainCase, JsonNamingPolicy.TrainCase))
        {
        }
    }
}