using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Xml.Linq;
using SpanJson.Helpers;
using SpanJson.Internal;
using SpanJson.Resolvers;

namespace SpanJson.Formatters
{
    public abstract class BaseEnumStringFormatter<T, TSymbol> : BaseFormatter where T : struct, Enum
        where TSymbol : struct
    {
        protected static SerializeDelegate BuildSerializeDelegate<TResolver>(Func<string, string> escapeFunctor)
            where TResolver : IJsonFormatterResolver<TSymbol, TResolver>, new()
        {
            var writerParameter = Expression.Parameter(typeof(JsonWriter<TSymbol>).MakeByRefType(), "writer");
            var valueParameter = Expression.Parameter(typeof(T), "value");
            MethodInfo? writerMethodInfo = null;
            if (SymbolHelper<TSymbol>.IsUtf8)
            {
                writerMethodInfo = FindPublicInstanceMethod(writerParameter.Type, nameof(JsonWriter<TSymbol>.WriteUtf8Verbatim), typeof(byte[]));
            }
            else if (SymbolHelper<TSymbol>.IsUtf16)
            {
                writerMethodInfo = FindPublicInstanceMethod(writerParameter.Type, nameof(JsonWriter<TSymbol>.WriteUtf16Verbatim), typeof(string));
            }
            else
            {
                ThrowHelper.ThrowNotSupportedException();
            }

            var resolver = StandardResolvers.GetResolver<TSymbol, TResolver>();

            var cases = new List<SwitchCase>();
            foreach (var name in Enum.GetNames(typeof(T)))
            {
                Expression? valueConstant = null;
                var formattedValue = escapeFunctor(resolver.GetEncodedPropertyName(GetFormattedValue(name)).ToString());
                if (SymbolHelper<TSymbol>.IsUtf8)
                {
                    valueConstant = Expression.Constant(TextEncodings.UTF8NoBOM.GetBytes(formattedValue));
                }
                else if (SymbolHelper<TSymbol>.IsUtf16)
                {
                    valueConstant = Expression.Constant(formattedValue);
                }
                else
                {
                    ThrowHelper.ThrowNotSupportedException();
                }

                var value = Enum.Parse(typeof(T), name);
                var switchCase = Expression.SwitchCase(Expression.Call(writerParameter, writerMethodInfo, valueConstant), Expression.Constant(value));
                cases.Add(switchCase);
            }

            var switchExpression = Expression.Switch(valueParameter,
                Expression.Throw(Expression.Constant(new InvalidOperationException())), cases.ToArray());

            var lambdaExpression =
                Expression.Lambda<SerializeDelegate>(switchExpression, writerParameter, valueParameter);
            return lambdaExpression.Compile();
        }

        private static string? GetAlias(string enumValue)
        {
            return typeof(T).GetMember(enumValue)?.FirstOrDefault()?.FirstAttribute<EnumMemberAttribute>()?.Value;
        }

        private static string GetFormattedValue(string enumValue)
        {
            return GetAlias(enumValue) ?? enumValue;
        }

        protected static TDelegate BuildDeserializeDelegateExpressions<TDelegate, TResolver, TReturn>(ParameterExpression inputExpression, Expression nameSpanExpression)
            where TResolver : IJsonFormatterResolver<TSymbol, TResolver>, new()
        {
            var nameSpan = Expression.Variable(typeof(ReadOnlySpan<TSymbol>), "nameSpan");
            var returnValue = Expression.Variable(typeof(TReturn), "returnValue");
            var lengthParameter = Expression.Variable(typeof(int), "length");
            var endOfBlockLabel = Expression.Label();
            var assignNameSpan = Expression.Assign(nameSpan, nameSpanExpression);
            var lengthExpression = Expression.Assign(lengthParameter, Expression.PropertyOrField(nameSpan, "Length"));
            var byteNameSpan = Expression.Variable(typeof(ReadOnlySpan<byte>), "byteNameSpan");
            var parameters = new List<ParameterExpression> { nameSpan, lengthParameter, returnValue };
            if (SymbolHelper<TSymbol>.IsUtf16)
            {
                var asBytesMethodInfo = FindGenericMethod(typeof(MemoryMarshal), nameof(MemoryMarshal.AsBytes), BindingFlags.Public | BindingFlags.Static,
                    typeof(char), typeof(ReadOnlySpan<>));
                nameSpanExpression = Expression.Call(null, asBytesMethodInfo, assignNameSpan);
                assignNameSpan = Expression.Assign(byteNameSpan, nameSpanExpression);
                parameters.Add(byteNameSpan);
            }
            else
            {
                byteNameSpan = nameSpan;
            }

            var resolver = StandardResolvers.GetResolver<TSymbol, TResolver>();
            var memberInfos = new List<JsonMemberInfo>();
            var dict = new Dictionary<string, TReturn>(StringComparer.Ordinal);
            foreach (var originalName in Enum.GetNames(typeof(T)))
            {
                var alias = GetAlias(originalName);
                var resolvedName = alias ?? resolver.ResolvePropertyName(originalName);
                var escapedName = JsonHelpers.GetEncodedText(resolvedName, resolver.EscapeHandling, resolver.Encoder);
                memberInfos.Add(new JsonMemberInfo(
                    originalName, alias, typeof(T), null,
                    resolvedName, escapedName,
                    false, true, false, null, null));
                var value = Enum.Parse(typeof(T), originalName);
                dict.Add(originalName, (TReturn)Convert.ChangeType(value, typeof(TReturn)));
            }

            Func<JsonMemberInfo, Expression> matchExpressionFunctor = memberInfo =>
            {
                var enumValue = dict[memberInfo.MemberName];
                return Expression.Assign(returnValue, Expression.Constant(enumValue));
            };

            var returnTarget = Expression.Label(returnValue.Type);
            var returnLabel = Expression.Label(returnTarget, returnValue);
            var expressions = new List<Expression>
            {
                assignNameSpan,
                lengthExpression,
                MemberComparisonBuilder.Build<TSymbol>(
                    MemberComparisonBuilder.ConvertMemberInfos(memberInfos, resolver.JsonOptions.PropertyNameCaseInsensitive),
                    0, lengthParameter, byteNameSpan, endOfBlockLabel, matchExpressionFunctor),
                Expression.Throw(Expression.Constant(new InvalidOperationException())),
                Expression.Label(endOfBlockLabel),
                returnLabel
            };
            var blockExpression = Expression.Block(parameters, expressions);
            var lambdaExpression =
                Expression.Lambda<TDelegate>(blockExpression, inputExpression);
            return lambdaExpression.Compile();
        }

        protected delegate void SerializeDelegate(ref JsonWriter<TSymbol> writer, T value);
    }
}