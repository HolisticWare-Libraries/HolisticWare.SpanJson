using SpanJson.Resolvers;

namespace SpanJson.AspNetCore.Formatter.Tests
{
    public sealed class CustomResolver<TSymbol> : ResolverBase<TSymbol, CustomResolver<TSymbol>> where TSymbol : struct
    {
        public CustomResolver()
            : base(new SpanJsonOptions(NullOptions.ExcludeNulls, EnumOptions.Integer, JsonNamingPolicy.CamelCase, JsonNamingPolicy.CamelCase, JsonNamingPolicy.CamelCase))
        {
        }
    }
}