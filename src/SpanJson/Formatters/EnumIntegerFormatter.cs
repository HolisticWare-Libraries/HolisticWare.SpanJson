﻿using System;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace SpanJson.Formatters
{
    public sealed class EnumIntegerFormatter<T, TSymbol, TResolver> : BaseFormatter, IJsonFormatter<T, TSymbol> where T : struct, Enum
        where TResolver : IJsonFormatterResolver<TSymbol, TResolver>, new()
        where TSymbol : struct
    {
        private static readonly SerializeDelegate Serializer = BuildSerializeDelegate();
        private static readonly DeserializeDelegate Deserializer = BuildDeserializeDelegate();
        public static readonly EnumIntegerFormatter<T, TSymbol, TResolver> Default = new EnumIntegerFormatter<T, TSymbol, TResolver>();

        public T Deserialize(ref JsonReader<TSymbol> reader, IJsonFormatterResolver<TSymbol> resolver)
        {
            return Deserializer(ref reader);
        }

        public void Serialize(ref JsonWriter<TSymbol> writer, T value, IJsonFormatterResolver<TSymbol> resolver)
        {
            Serializer(ref writer, value);
        }


        private static SerializeDelegate BuildSerializeDelegate()
        {
            var underlyingType = Enum.GetUnderlyingType(typeof(T));
            var writerParameter = Expression.Parameter(typeof(JsonWriter<TSymbol>).MakeByRefType(), "writer");
            var valueParameter = Expression.Parameter(typeof(T), "value");
            string methodName = null;
            if (SymbolHelper<TSymbol>.IsUtf8)
            {
                methodName = $"WriteUtf8{underlyingType.Name}";
            }
            else if (SymbolHelper<TSymbol>.IsUtf16)
            {
                methodName = $"WriteUtf16{underlyingType.Name}";
            }
            else
            {
                ThrowHelper.ThrowNotSupportedException();
            }

            var writerMethodInfo = FindPublicInstanceMethod(writerParameter.Type, methodName, underlyingType);
            var lambda = Expression.Lambda<SerializeDelegate>(Expression.Call(writerParameter, writerMethodInfo,
                Expression.Convert(valueParameter, underlyingType)), writerParameter, valueParameter);
            return lambda.Compile();
        }


        private static DeserializeDelegate BuildDeserializeDelegate()
        {
            var underlyingType = Enum.GetUnderlyingType(typeof(T));
            var readerParameter = Expression.Parameter(typeof(JsonReader<TSymbol>).MakeByRefType(), "reader");
            string methodName = null;
            if (SymbolHelper<TSymbol>.IsUtf8)
            {
                methodName = $"ReadUtf8{underlyingType.Name}";
            }
            else if (SymbolHelper<TSymbol>.IsUtf16)
            {
                methodName = $"ReadUtf16{underlyingType.Name}";
            }
            else
            {
                ThrowHelper.ThrowNotSupportedException();
            }

            var readerMethodInfo = FindPublicInstanceMethod(readerParameter.Type, methodName);
            var lambda = Expression.Lambda<DeserializeDelegate>(Expression.Convert(Expression.Call(readerParameter, readerMethodInfo), typeof(T)),
                readerParameter);
            return lambda.Compile();
        }


        private delegate T DeserializeDelegate(ref JsonReader<TSymbol> reader);

        private delegate void SerializeDelegate(ref JsonWriter<TSymbol> writer, T value);
    }

    public sealed class EnumIntegerFormatter<T> : ICustomJsonFormatter<T> where T : struct, Enum
    {
        public static readonly EnumIntegerFormatter<T> Default = new EnumIntegerFormatter<T>();

        public object Arguments { get; set; }

        public T Deserialize(ref JsonReader<byte> reader, IJsonFormatterResolver<byte> resolver)
        {
            return resolver.GetEnumIntegerFormatter<T>().Deserialize(ref reader, resolver);
        }

        public T Deserialize(ref JsonReader<char> reader, IJsonFormatterResolver<char> resolver)
        {
            return resolver.GetEnumIntegerFormatter<T>().Deserialize(ref reader, resolver);
        }

        public void Serialize(ref JsonWriter<byte> writer, T value, IJsonFormatterResolver<byte> resolver)
        {
            resolver.GetEnumIntegerFormatter<T>().Serialize(ref writer, value, resolver);
        }

        public void Serialize(ref JsonWriter<char> writer, T value, IJsonFormatterResolver<char> resolver)
        {
            resolver.GetEnumIntegerFormatter<T>().Serialize(ref writer, value, resolver);
        }
    }
}