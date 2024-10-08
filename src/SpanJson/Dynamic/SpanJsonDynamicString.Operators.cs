﻿using System;
// Autogenerated
// ReSharper disable BuiltInTypeReferenceStyle
namespace SpanJson.Dynamic
{
    public partial class SpanJsonDynamicString<TSymbol> : SpanJsonDynamic<TSymbol> where TSymbol : struct
    {
        public static explicit operator Char(SpanJsonDynamicString<TSymbol> input)
        {
            if (DynamicConverter.TryConvertTo(typeof(Char), input.Symbols, out var value))
            {
                return (Char) value!;
            }
            throw ThrowHelper.GetInvalidCastException();
        }

        public static explicit operator Char?(SpanJsonDynamicString<TSymbol> input)
        {
            if (DynamicConverter.TryConvertTo(typeof(Char?), input.Symbols, out var value))
            {
                return (Char?) value;
            }
            throw ThrowHelper.GetInvalidCastException();
        }

        public static explicit operator DateTime(SpanJsonDynamicString<TSymbol> input)
        {
            if (DynamicConverter.TryConvertTo(typeof(DateTime), input.Symbols, out var value))
            {
                return (DateTime) value!;
            }
            throw ThrowHelper.GetInvalidCastException();
        }

        public static explicit operator DateTime?(SpanJsonDynamicString<TSymbol> input)
        {
            if (DynamicConverter.TryConvertTo(typeof(DateTime?), input.Symbols, out var value))
            {
                return (DateTime?) value;
            }
            throw ThrowHelper.GetInvalidCastException();
        }

        public static explicit operator DateTimeOffset(SpanJsonDynamicString<TSymbol> input)
        {
            if (DynamicConverter.TryConvertTo(typeof(DateTimeOffset), input.Symbols, out var value))
            {
                return (DateTimeOffset) value!;
            }
            throw ThrowHelper.GetInvalidCastException();
        }

        public static explicit operator DateTimeOffset?(SpanJsonDynamicString<TSymbol> input)
        {
            if (DynamicConverter.TryConvertTo(typeof(DateTimeOffset?), input.Symbols, out var value))
            {
                return (DateTimeOffset?) value;
            }
            throw ThrowHelper.GetInvalidCastException();
        }

        public static explicit operator TimeSpan(SpanJsonDynamicString<TSymbol> input)
        {
            if (DynamicConverter.TryConvertTo(typeof(TimeSpan), input.Symbols, out var value))
            {
                return (TimeSpan) value!;
            }
            throw ThrowHelper.GetInvalidCastException();
        }

        public static explicit operator TimeSpan?(SpanJsonDynamicString<TSymbol> input)
        {
            if (DynamicConverter.TryConvertTo(typeof(TimeSpan?), input.Symbols, out var value))
            {
                return (TimeSpan?) value;
            }
            throw ThrowHelper.GetInvalidCastException();
        }

        public static explicit operator Guid(SpanJsonDynamicString<TSymbol> input)
        {
            if (DynamicConverter.TryConvertTo(typeof(Guid), input.Symbols, out var value))
            {
                return (Guid) value!;
            }
            throw ThrowHelper.GetInvalidCastException();
        }

        public static explicit operator Guid?(SpanJsonDynamicString<TSymbol> input)
        {
            if (DynamicConverter.TryConvertTo(typeof(Guid?), input.Symbols, out var value))
            {
                return (Guid?) value;
            }
            throw ThrowHelper.GetInvalidCastException();
        }

    }
}