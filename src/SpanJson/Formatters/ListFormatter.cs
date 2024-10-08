﻿using System;
using System.Collections.Generic;
using SpanJson.Helpers;
using SpanJson.Resolvers;

namespace SpanJson.Formatters
{
    /// <summary>
    /// Used for types which are not built-in
    /// </summary>
    public sealed class ListFormatter<TList, T, TSymbol, TResolver> : BaseFormatter, IJsonFormatter<TList, TSymbol>
        where TResolver : IJsonFormatterResolver<TSymbol, TResolver>, new() where TSymbol : struct where TList : class, IList<T>
    {
        private static readonly Func<TList> CreateFunctor = StandardResolvers.GetCreateFunctor<TSymbol, TResolver, TList>();
        public static readonly ListFormatter<TList, T, TSymbol, TResolver> Default = new();

        private static readonly IJsonFormatter<T, TSymbol> ElementFormatter = StandardResolvers.GetFormatter<TSymbol, TResolver, T>();

        private static readonly bool IsRecursionCandidate = RecursionCandidate<T>.IsRecursionCandidate;

        public TList? Deserialize(ref JsonReader<TSymbol> reader, IJsonFormatterResolver<TSymbol> resolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }

            reader.ReadBeginArrayOrThrow();
            var list = CreateFunctor();
            var count = 0;
            while (!reader.TryReadIsEndArrayOrValueSeparator(ref count))
            {
                list.Add(ElementFormatter.Deserialize(ref reader, resolver)!);
            }

            return list;
        }

        public void Serialize(ref JsonWriter<TSymbol> writer, TList? value, IJsonFormatterResolver<TSymbol> resolver)
        {
            if (value is null)
            {
                writer.WriteNull();
                return;
            }

            if (IsRecursionCandidate)
            {
                writer.IncrementDepth();
            }
            var valueLength = value.Count;
            writer.WriteBeginArray();
            if (valueLength > 0)
            {
                SerializeRuntimeDecisionInternal<T, TSymbol, TResolver>(ref writer, value[0], ElementFormatter, resolver);
                for (var i = 1; i < valueLength; i++)
                {
                    writer.WriteValueSeparator();
                    SerializeRuntimeDecisionInternal<T, TSymbol, TResolver>(ref writer, value[i], ElementFormatter, resolver);
                }
            }
            if (IsRecursionCandidate)
            {
                writer.DecrementDepth();
            }
            writer.WriteEndArray();
        }
    }
}