using SpanJson.Resolvers;

namespace SpanJson.Tests
{
    public sealed class ExcludeNullCamelCaseIntegerEnumResolver<TSymbol> : ResolverBase<TSymbol, ExcludeNullCamelCaseIntegerEnumResolver<TSymbol>>
        where TSymbol : struct
    {
        public ExcludeNullCamelCaseIntegerEnumResolver()
            : base(new SpanJsonOptions(NullOptions.ExcludeNulls, EnumOptions.Integer, JsonNamingPolicy.CamelCase, JsonNamingPolicy.CamelCase, JsonNamingPolicy.CamelCase))
        {
        }
    }
}