using SpanJson.Resolvers;

namespace SpanJson.AspNetCore.Formatter
{
    public class AspNetCoreDefaultResolver<TSymbol> : ResolverBase<TSymbol, AspNetCoreDefaultResolver<TSymbol>> where TSymbol : struct
    {
        public AspNetCoreDefaultResolver()
            : base(new SpanJsonOptions(NullOptions.IncludeNulls, EnumOptions.Integer, JsonNamingPolicy.CamelCase, JsonNamingPolicy.CamelCase, JsonNamingPolicy.CamelCase))
        {
        }
    }
}