using SpanJson;
using SpanJson.Resolvers;

namespace NodaTime.Serialization.JsonSpan.Tests
{
    public sealed class NodaIncludeNullsOriginalCaseResolver<TSymbol> : ResolverBase<TSymbol, NodaIncludeNullsOriginalCaseResolver<TSymbol>>
        where TSymbol : struct
    {
        public NodaIncludeNullsOriginalCaseResolver()
            : base(new SpanJsonOptions(NullOptions.IncludeNulls, EnumOptions.String))
        {
            RegisterGlobalCustomrResolver(NodatimeResolver.Instance);
        }
    }

    public sealed class NodaExcludeNullsOriginalCaseResolver<TSymbol> : ResolverBase<TSymbol, NodaExcludeNullsOriginalCaseResolver<TSymbol>>
        where TSymbol : struct
    {
        public NodaExcludeNullsOriginalCaseResolver()
            : base(new SpanJsonOptions(NullOptions.ExcludeNulls, EnumOptions.String))
        {
            RegisterGlobalCustomrResolver(NodatimeResolver.Instance);
        }
    }

    public sealed class NodaIsoExcludeNullsOriginalCaseResolver<TSymbol> : ResolverBase<TSymbol, NodaIsoExcludeNullsOriginalCaseResolver<TSymbol>>
        where TSymbol : struct
    {
        public NodaIsoExcludeNullsOriginalCaseResolver()
            : base(new SpanJsonOptions(NullOptions.ExcludeNulls, EnumOptions.String))
        {
            RegisterGlobalCustomrResolver(IsoNodatimeResolver.Instance);
        }
    }

    public sealed class NodaExcludeNullsCamelCaseResolver<TSymbol> : ResolverBase<TSymbol, NodaExcludeNullsCamelCaseResolver<TSymbol>> where TSymbol : struct
    {
        public NodaExcludeNullsCamelCaseResolver()
            : base(new SpanJsonOptions(NullOptions.ExcludeNulls, EnumOptions.String, JsonNamingPolicy.CamelCase, JsonNamingPolicy.CamelCase, JsonNamingPolicy.CamelCase))
        {
            RegisterGlobalCustomrResolver(NodatimeResolver.Instance);
        }
    }

    public sealed class NodaIsoExcludeNullsCamelCaseResolver<TSymbol> : ResolverBase<TSymbol, NodaIsoExcludeNullsCamelCaseResolver<TSymbol>> where TSymbol : struct
    {
        public NodaIsoExcludeNullsCamelCaseResolver()
            : base(new SpanJsonOptions(NullOptions.ExcludeNulls, EnumOptions.String, JsonNamingPolicy.CamelCase, JsonNamingPolicy.CamelCase, JsonNamingPolicy.CamelCase))
        {
            RegisterGlobalCustomrResolver(IsoNodatimeResolver.Instance);
        }
    }
}
