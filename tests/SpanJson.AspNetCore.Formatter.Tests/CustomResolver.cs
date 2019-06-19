﻿using SpanJson.Resolvers;

namespace SpanJson.AspNetCore.Formatter.Tests
{
    public sealed class CustomResolver<TSymbol> : ResolverBase<TSymbol, CustomResolver<TSymbol>> where TSymbol : struct
    {
        public CustomResolver() : base(new SpanJsonOptions
        {
            NullOption = NullOptions.ExcludeNulls,
            EnumOption = EnumOptions.Integer,
            ExtensionDataNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        })
        {
        }
    }
}