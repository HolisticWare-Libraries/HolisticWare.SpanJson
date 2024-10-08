﻿using System.Buffers;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.InteropServices;
using SpanJson.Helpers;
using SpanJson.Internal;
using SpanJson.Resolvers;

namespace SpanJson.Formatters
{
    /// <summary>
    /// Main type for handling complex types
    /// </summary>
    public abstract class ComplexFormatter : BaseFormatter
    {
        /// <summary>
        /// Creates the serializer for both utf8 and utf16
        /// There should not be a large difference between utf8 and utf16 besides member names
        /// </summary>
        protected static SerializeDelegate<T, TSymbol> BuildSerializeDelegate<T, TSymbol, TResolver>()
            where TResolver : IJsonFormatterResolver<TSymbol, TResolver>, new() where TSymbol : struct
        {
            var resolver = StandardResolvers.GetResolver<TSymbol, TResolver>();
            var objectDescription = resolver.GetObjectDescription<T>();
            var memberInfos = objectDescription.Where(a => a.CanRead).ToList();
            var writerParameter = Expression.Parameter(typeof(JsonWriter<TSymbol>).MakeByRefType(), "writer");
            var valueParameter = Expression.Parameter(typeof(T), "value");
            var resolverParameter = Expression.Parameter(typeof(IJsonFormatterResolver<TSymbol>), "resolver");
            var expressions = new List<Expression>();
            if (RecursionCandidate<T>.IsRecursionCandidate)
            {
                expressions.Add(Expression.Call(writerParameter, nameof(JsonWriter<TSymbol>.AssertDepth), Type.EmptyTypes));
            }

            MethodInfo separatorWriteMethodInfo;
            MethodInfo writeBeginObjectMethodInfo;
            MethodInfo writeEndObjectMethodInfo;
            if (SymbolHelper<TSymbol>.IsUtf8)
            {
                separatorWriteMethodInfo = FindPublicInstanceMethod(writerParameter.Type, nameof(JsonWriter<TSymbol>.WriteUtf8ValueSeparator));
                writeBeginObjectMethodInfo = FindPublicInstanceMethod(writerParameter.Type, nameof(JsonWriter<TSymbol>.WriteUtf8BeginObject));
                writeEndObjectMethodInfo = FindPublicInstanceMethod(writerParameter.Type, nameof(JsonWriter<TSymbol>.WriteUtf8EndObject));
            }
            else if (SymbolHelper<TSymbol>.IsUtf16)
            {
                separatorWriteMethodInfo = FindPublicInstanceMethod(writerParameter.Type, nameof(JsonWriter<TSymbol>.WriteUtf16ValueSeparator));
                writeBeginObjectMethodInfo = FindPublicInstanceMethod(writerParameter.Type, nameof(JsonWriter<TSymbol>.WriteUtf16BeginObject));
                writeEndObjectMethodInfo = FindPublicInstanceMethod(writerParameter.Type, nameof(JsonWriter<TSymbol>.WriteUtf16EndObject));
            }
            else
            {
                throw ThrowHelper.GetNotSupportedException();
            }

            expressions.Add(Expression.Call(writerParameter, writeBeginObjectMethodInfo));
            var writeSeparator = Expression.Variable(typeof(bool), "writeSeparator");
            for (var i = 0; i < memberInfos.Count; i++)
            {
                var memberInfo = memberInfos[i];
                var formatterType = resolver.GetFormatter(memberInfo).GetType();
                Expression? serializerInstance = null;
                MethodInfo serializeMethodInfo;
                Expression memberExpression = Expression.PropertyOrField(valueParameter, memberInfo.MemberName);
                var parameterExpressions = new List<Expression> { writerParameter, memberExpression };
                var fieldInfo = formatterType.GetField("Default", BindingFlags.Static | BindingFlags.Public)!;
                if (IsNoRuntimeDecisionRequired(memberInfo.MemberType))
                {
                    var underlyingType = Nullable.GetUnderlyingType(memberInfo.MemberType);
                    // if it's nullable and we don't need the null and we don't have a custom formatter, we call the underlying provider directly
                    if (memberInfo.ExcludeNull && underlyingType is not null && !typeof(ICustomJsonFormatter).IsAssignableFrom(formatterType))
                    {
                        formatterType = resolver.GetFormatter(memberInfo, underlyingType).GetType();
                        fieldInfo = formatterType.GetField("Default", BindingFlags.Static | BindingFlags.Public)!;
                        var methodInfo = memberInfo.MemberType.GetMethod("GetValueOrDefault", Type.EmptyTypes)!;
                        memberExpression = Expression.Call(memberExpression, methodInfo);
                        parameterExpressions = new List<Expression> { writerParameter, memberExpression };
                    }
                    parameterExpressions.Add(resolverParameter);

                    serializeMethodInfo = FindPublicInstanceMethod(formatterType, "Serialize", writerParameter.Type.MakeByRefType(),
                        underlyingType ?? memberInfo.MemberType, resolverParameter.Type);
                    serializerInstance = Expression.Field(null, fieldInfo);
                }
                else
                {
                    serializeMethodInfo = typeof(BaseFormatter)
                        .GetMethod(nameof(SerializeRuntimeDecisionInternal), BindingFlags.NonPublic | BindingFlags.Static)!
                        .MakeGenericMethod(memberInfo.MemberType, typeof(TSymbol), typeof(TResolver));
                    parameterExpressions.Add(Expression.Field(null, fieldInfo));
                    parameterExpressions.Add(resolverParameter);
                }

                bool isCandidate = RecursionCandidate.LookupRecursionCandidate(memberInfo.MemberType);

                if (isCandidate) // only for possible candidates
                {
                    expressions.Add(Expression.Call(writerParameter, nameof(JsonWriter<TSymbol>.IncrementDepth), Type.EmptyTypes));
                }

                ConstantExpression[]? writeNameExpressions = null;
                var formattedMemberInfoName = $"\"{memberInfo.EscapedName}\":";

                MethodInfo? propertyNameWriterMethodInfo = null;
                // utf8 has special logic for writing the attribute names as Expression.Constant(byte-Array) is slower than Expression.Constant(string)
                if (SymbolHelper<TSymbol>.IsUtf8)
                {
                    // Everything above a length of 32 is not optimized
                    // 这儿不应直接采用字符串长度来判断
                    //if ((uint)formattedMemberInfoName.Length > 32u)
                    var utf8Bytes = TextEncodings.Utf8.GetBytesWithCache(formattedMemberInfoName);
                    if ((uint)utf8Bytes.Length > 32u)
                    {
                        writeNameExpressions = new[] { Expression.Constant(utf8Bytes) };
                        propertyNameWriterMethodInfo =
                            FindPublicInstanceMethod(writerParameter.Type, nameof(JsonWriter<TSymbol>.WriteUtf8Verbatim), typeof(byte[]));
                    }
                    else
                    {
                        writeNameExpressions = GetIntegersForMemberName(utf8Bytes);
                        var typesToMatch = writeNameExpressions.Select(a => a.Value!.GetType());
                        propertyNameWriterMethodInfo =
                            FindPublicInstanceMethod(writerParameter.Type, nameof(JsonWriter<TSymbol>.WriteUtf8Verbatim), typesToMatch.ToArray());
                    }
                }
                else if (SymbolHelper<TSymbol>.IsUtf16)
                {
                    writeNameExpressions = new[] { Expression.Constant(formattedMemberInfoName) };
                    propertyNameWriterMethodInfo =
                        FindPublicInstanceMethod(writerParameter.Type, nameof(JsonWriter<TSymbol>.WriteUtf16Verbatim), typeof(string));
                }
                else
                {
                    ThrowHelper.ThrowNotSupportedException();
                }

                var valueExpressions = new List<Expression>();
                // we need to add the separator, but only if a value was written before
                // we write the separator and set the marker after writing each field
                if (i > 0)
                {
                    valueExpressions.Add(
                        Expression.IfThen(
                            writeSeparator,
                            Expression.Block(
                                Expression.Call(writerParameter, separatorWriteMethodInfo))
                        ));
                }

                valueExpressions.Add(Expression.Call(writerParameter, propertyNameWriterMethodInfo, writeNameExpressions));
                valueExpressions.Add(Expression.Call(serializerInstance, serializeMethodInfo, parameterExpressions));
                valueExpressions.Add(Expression.Assign(writeSeparator, Expression.Constant(true)));
                Expression? testNullExpression = null;
                if (memberInfo.ExcludeNull)
                {
                    if (memberInfo.MemberType.IsClass)
                    {
                        testNullExpression = Expression.ReferenceNotEqual(
                            Expression.PropertyOrField(valueParameter, memberInfo.MemberName),
                            Expression.Constant(null));
                    }
                    else if (memberInfo.MemberType.IsValueType && Nullable.GetUnderlyingType(memberInfo.MemberType) is not null) // nullable value type
                    {
                        testNullExpression = Expression.IsTrue(
                            Expression.Property(Expression.PropertyOrField(valueParameter, memberInfo.MemberName), "HasValue"));
                    }
                }

                var shouldSerializeExpression = memberInfo.ShouldSerialize is not null
                    ? Expression.IsTrue(Expression.Call(valueParameter, memberInfo.ShouldSerialize))
                    : null;
                Expression? testExpression = null;
                if (testNullExpression is not null && shouldSerializeExpression is not null)
                {
                    testExpression = Expression.AndAlso(testNullExpression, shouldSerializeExpression);
                }
                else if (testNullExpression is not null)
                {
                    testExpression = testNullExpression;
                }
                else if (shouldSerializeExpression is not null)
                {
                    testExpression = shouldSerializeExpression;
                }

                if (testExpression is not null)
                {
                    expressions.Add(Expression.IfThen(testExpression, Expression.Block(valueExpressions)));
                }
                else
                {
                    expressions.AddRange(valueExpressions);
                }

                if (isCandidate) // only for possible candidates
                {
                    expressions.Add(Expression.Call(writerParameter, nameof(JsonWriter<TSymbol>.DecrementDepth), Type.EmptyTypes));
                }
            }

            if (objectDescription.ExtensionMemberInfo is not null)
            {
                var knownNames = objectDescription.Members.Where(t => t.CanWrite).Select(a => a.Name).ToHashSet(StringComparer.Ordinal);
                var memberInfo = typeof(ComplexFormatter).GetMethod(nameof(SerializeExtension), BindingFlags.Static | BindingFlags.NonPublic)!;
                var closedMemberInfo = memberInfo.MakeGenericMethod(typeof(TSymbol), typeof(TResolver));
                var valueExpression = Expression.TypeAs(Expression.PropertyOrField(valueParameter, objectDescription.ExtensionMemberInfo.MemberName),
                    typeof(IDictionary<string, object>));
                expressions.Add(Expression.IfThen(Expression.ReferenceNotEqual(valueExpression, Expression.Constant(null)), Expression.Call(null,
                    closedMemberInfo, writerParameter, resolverParameter, valueExpression, writeSeparator, Expression.Constant(objectDescription.ExtensionMemberInfo.ExcludeNulls),
                    Expression.Constant(knownNames))));
            }

            expressions.Add(Expression.Call(writerParameter, writeEndObjectMethodInfo));
            var blockExpression = Expression.Block(new[] { writeSeparator }, expressions);
            var lambda =
                Expression.Lambda<SerializeDelegate<T, TSymbol>>(blockExpression, writerParameter, valueParameter, resolverParameter);
            return lambda.Compile();
        }

        /// <summary>
        /// Creates the deserializer for both utf8 and utf16
        /// There should not be a large difference between utf8 and utf16 besides member names
        /// </summary>
        protected static DeserializeDelegate<T, TSymbol> BuildDeserializeDelegate<T, TSymbol, TResolver>()
            where TResolver : IJsonFormatterResolver<TSymbol, TResolver>, new() where TSymbol : struct
        {
            var resolver = StandardResolvers.GetResolver<TSymbol, TResolver>();
            var objectDescription = resolver.GetObjectDescription<T>();
            var memberInfos = objectDescription.Where(a => a.CanWrite).ToList();
            var readerParameter = Expression.Parameter(typeof(JsonReader<TSymbol>).MakeByRefType(), "reader");
            var resolverParameter = Expression.Parameter(typeof(IJsonFormatterResolver<TSymbol>), "resolver");
            // can't deserialize abstract and only support interfaces based on IEnumerable<T> (this includes, IList, IReadOnlyList, IDictionary et al.)
            foreach (var memberInfo in memberInfos)
            {
                var memberType = memberInfo.MemberType;
                if (memberType.IsAbstract)
                {
                    if (resolver.IsSupportedType(memberType)) { continue; }

                    if (memberType.TryGetTypeOfGenericInterface(typeof(IEnumerable<>), out _)) { continue; }

                    return Expression
                        .Lambda<DeserializeDelegate<T, TSymbol>>(Expression.Block(
                                Expression.Throw(Expression.Constant(new NotSupportedException($"{typeof(T).Name} contains abstract members."))),
                                Expression.Default(typeof(T))),
                            readerParameter, resolverParameter).Compile();
                }
            }

            if (typeof(T).IsAbstract)
            {
                return Expression.Lambda<DeserializeDelegate<T, TSymbol>>(Expression.Default(typeof(T)), readerParameter, resolverParameter).Compile();
            }

            if (0u >= (uint)memberInfos.Count && objectDescription.ExtensionMemberInfo is null)
            {
                Expression? createExpression = null;
                if (typeof(T).IsClass)
                {
                    var ci = typeof(T).GetConstructor(Type.EmptyTypes);
                    if (ci is not null)
                    {
                        createExpression = Expression.New(ci);
                    }
                }

                if (createExpression is null)
                {
                    createExpression = Expression.Default(typeof(T));
                }

                return Expression.Lambda<DeserializeDelegate<T, TSymbol>>(createExpression, readerParameter, resolverParameter).Compile();
            }

            var returnValue = Expression.Variable(typeof(T), "result");
            MethodInfo? nameSpanMethodInfo = null;
            MethodInfo? tryReadEndObjectMethodInfo = null;
            MethodInfo? beginObjectOrThrowMethodInfo = null;
            if (SymbolHelper<TSymbol>.IsUtf8)
            {
                nameSpanMethodInfo = FindPublicInstanceMethod(readerParameter.Type, nameof(JsonReader<TSymbol>.ReadUtf8EscapedNameSpan));
                tryReadEndObjectMethodInfo = FindPublicInstanceMethod(readerParameter.Type, nameof(JsonReader<TSymbol>.TryReadUtf8IsEndObjectOrValueSeparator));
                beginObjectOrThrowMethodInfo = FindPublicInstanceMethod(readerParameter.Type, nameof(JsonReader<TSymbol>.ReadUtf8BeginObjectOrThrow));
            }
            else if (SymbolHelper<TSymbol>.IsUtf16)
            {
                nameSpanMethodInfo = FindPublicInstanceMethod(readerParameter.Type, nameof(JsonReader<TSymbol>.ReadUtf16EscapedNameSpan));
                tryReadEndObjectMethodInfo =
                    FindPublicInstanceMethod(readerParameter.Type, nameof(JsonReader<TSymbol>.TryReadUtf16IsEndObjectOrValueSeparator));
                beginObjectOrThrowMethodInfo = FindPublicInstanceMethod(readerParameter.Type, nameof(JsonReader<TSymbol>.ReadUtf16BeginObjectOrThrow));
            }
            else
            {
                ThrowHelper.ThrowNotSupportedException();
            }

            // We need to decide during generation if we handle constructors or normal member assignment, the difference is done in the functor below
            Func<JsonMemberInfo, Expression> matchExpressionFunctor;
            Expression[]? constructorParameterExpressions = null;
            var additionalAfterCtorParameterExpressions = new Dictionary<string, (ParameterExpression HasVariable, Expression Variable)>(StringComparer.OrdinalIgnoreCase);
            if (objectDescription.Constructor is not null)
            {
                // If we want to use the constructor we serialize into an array of variables internally and then create the object from that
                var dict = objectDescription.ConstructorMapping!;
                constructorParameterExpressions = new Expression[dict.Count];
                foreach (var valueTuple in dict)
                {
                    constructorParameterExpressions[valueTuple.Value.Index] = Expression.Variable(valueTuple.Value.Type, StringMutator.ToCamelCase(valueTuple.Key));
                }

                matchExpressionFunctor = memberInfo =>
                {
                    Expression assignValueExpression;
                    // Ctor-Functor stores values in local variables to assign them before calling the ctor.
                    // Therefore we need a 'hasVariable' to check if it has been assigned, like:
                    // var result = new Value(var1, var2);
                    // if(hasVar3) { result.Var3 = var3; }
                    ParameterExpression? hasVariable = null;
                    if (dict.TryGetValue(memberInfo.MemberName, out var element))
                    {
                        assignValueExpression = constructorParameterExpressions[element.Index];
                    }
                    else
                    {
                        if (!additionalAfterCtorParameterExpressions.TryGetValue(memberInfo.MemberName, out var variableInfo))
                        {
                            var variable = Expression.Variable(memberInfo.MemberType, StringMutator.ToCamelCase(memberInfo.MemberName));
                            hasVariable = Expression.Variable(typeof(bool), $"has{ToPascalCase(variable.Name!)}");
                            variableInfo = (hasVariable, variable);
                            additionalAfterCtorParameterExpressions.Add(memberInfo.MemberName, variableInfo);
                        }
                        else
                        {
                            hasVariable = variableInfo.HasVariable;
                        }
                        assignValueExpression = variableInfo.Variable;
                    }
                    var formatter = resolver.GetFormatter(memberInfo);
                    var formatterType = formatter.GetType();
                    var fieldInfo = formatterType.GetField("Default", BindingFlags.Static | BindingFlags.Public)!;
                    var assignVariableExpression = Expression.Assign(assignValueExpression,
                        Expression.Call(Expression.Field(null, fieldInfo),
                            FindPublicInstanceMethod(formatterType, "Deserialize", readerParameter.Type.MakeByRefType(), resolverParameter.Type),
                            readerParameter, resolverParameter));

                    if (hasVariable is null) { return assignVariableExpression; }

                    var assignHasVariableExpression = Expression.Assign(hasVariable, Expression.Constant(true));
                    return Expression.Block(assignHasVariableExpression, assignVariableExpression);
                };

                static string ToPascalCase(string name) => char.ToUpperInvariant(name[0]) + name.Substring(1);
            }
            else
            {
                // The normal assign to member type
                matchExpressionFunctor = memberInfo =>
                {
                    var formatter = resolver.GetFormatter(memberInfo);
                    var formatterType = formatter.GetType();
                    var fieldInfo = formatterType.GetField("Default", BindingFlags.Static | BindingFlags.Public)!;
                    var pf = Expression.PropertyOrField(returnValue, memberInfo.MemberName);
                    return Expression.Assign(pf,
                        Expression.Call(Expression.Field(null, fieldInfo),
                            FindPublicInstanceMethod(formatterType, "Deserialize", readerParameter.Type.MakeByRefType(), resolverParameter.Type),
                            readerParameter, resolverParameter));
                };
            }

            var nameSpan = Expression.Variable(typeof(ReadOnlySpan<TSymbol>), "nameSpan");
            var lengthParameter = Expression.Variable(typeof(int), "length");
            var endOfBlockLabel = Expression.Label();
            var nameSpanExpression = Expression.Call(readerParameter, nameSpanMethodInfo);
            var assignNameSpan = Expression.Assign(nameSpan, nameSpanExpression);
            var lengthExpression = Expression.Assign(lengthParameter, Expression.PropertyOrField(nameSpan, "Length"));
            var byteNameSpan = Expression.Variable(typeof(ReadOnlySpan<byte>), "byteNameSpan");
            var parameters = new List<ParameterExpression> { nameSpan, lengthParameter };
            if (SymbolHelper<TSymbol>.IsUtf16)
            {
                // For utf16 we need to convert the attribute name to bytes to feed it to the matching logic
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

            MethodInfo? skipNextMethodInfo = null;
            if (SymbolHelper<TSymbol>.IsUtf8)
            {
                skipNextMethodInfo = FindPublicInstanceMethod(readerParameter.Type, nameof(JsonReader<TSymbol>.SkipNextUtf8Segment));
            }
            else if (SymbolHelper<TSymbol>.IsUtf16)
            {
                skipNextMethodInfo = FindPublicInstanceMethod(readerParameter.Type, nameof(JsonReader<TSymbol>.SkipNextUtf16Segment));
            }
            else
            {
                ThrowHelper.ThrowNotSupportedException();
            }

            Expression skipCall = Expression.Call(readerParameter, skipNextMethodInfo);

            // we don't support constructor and extensions at the same time, this only leads to chaos
            if (objectDescription.ExtensionMemberInfo is not null && objectDescription.Constructor is null)
            {
                var extensionExpressions = new List<Expression>();
                var dictExpression = Expression.PropertyOrField(returnValue, objectDescription.ExtensionMemberInfo.MemberName);
                var createExpression = objectDescription.ExtensionMemberInfo.MemberType.IsInterface
                    ? Expression.New(typeof(Dictionary<string, object>))
                    : Expression.New(objectDescription.ExtensionMemberInfo.MemberType);
                extensionExpressions.Add(Expression.IfThen(Expression.ReferenceEqual(dictExpression, Expression.Constant(null)),
                    Expression.Assign(dictExpression, createExpression)));
                var memberInfo = typeof(ComplexFormatter).GetMethod(nameof(DeserializeExtension), BindingFlags.Static | BindingFlags.NonPublic)!;
                var closedMemberInfo = memberInfo.MakeGenericMethod(typeof(TSymbol), typeof(TResolver));
                extensionExpressions.Add(Expression.Call(null, closedMemberInfo, readerParameter, resolverParameter, byteNameSpan, dictExpression,
                    Expression.Constant(objectDescription.ExtensionMemberInfo.ExcludeNulls)));
                var extensionBlock = Expression.Block(extensionExpressions);
                skipCall = extensionBlock;
            }

            var expressions = new List<Expression>();
            if ((uint)memberInfos.Count > 0u)
            {
                expressions.Add(assignNameSpan);
                expressions.Add(lengthExpression);
                expressions.Add(
                    MemberComparisonBuilder.Build<TSymbol>(MemberComparisonBuilder.ConvertMemberInfos(memberInfos, resolver.JsonOptions.PropertyNameCaseInsensitive), 0, lengthParameter, byteNameSpan, endOfBlockLabel, matchExpressionFunctor));
                expressions.Add(skipCall);
                expressions.Add(Expression.Label(endOfBlockLabel));
            }
            else
            {
                expressions.Add(assignNameSpan);
                expressions.Add(skipCall);
            }

            var deserializeMemberBlock = Expression.Block(parameters, expressions);
            var countExpression = Expression.Parameter(typeof(int), "count");
            var abortExpression = Expression.IsTrue(Expression.Call(readerParameter, tryReadEndObjectMethodInfo, countExpression));
            var readBeginObject = Expression.Call(readerParameter, beginObjectOrThrowMethodInfo);
            var loopAbort = Expression.Label(typeof(void));
            var returnTarget = Expression.Label(returnValue.Type);
            Expression block;
            if (objectDescription.Constructor is not null)
            {
                var blockParameters = new List<ParameterExpression> { returnValue, countExpression };
                blockParameters.AddRange(constructorParameterExpressions!.OfType<ParameterExpression>());
                blockParameters.AddRange(additionalAfterCtorParameterExpressions.SelectMany(a => new[] { a.Value.HasVariable, a.Value.Variable })
                    .OfType<ParameterExpression>());

                var blockExpressions = new List<Expression>();
                for (var i = 0; i < objectDescription.Constructor.GetParameters().Length; i++)
                {
                    var parameterInfo = objectDescription.Constructor.GetParameters()[i];
                    if (parameterInfo.HasDefaultValue)
                    {
                        var parameterType = parameterInfo.ParameterType;
                        var defaultValue = parameterInfo.DefaultValue;
                        if (defaultValue is not null && parameterType.TryGetNullableUnderlyingType(out var notNullType) && notNullType.IsEnum)
                        {
                            // Fix for nullable enums, which come with their integer value.
                            defaultValue = Enum.ToObject(notNullType, defaultValue);
                        }

                        blockExpressions.Add(Expression.Assign(constructorParameterExpressions![i], Expression.Constant(defaultValue, parameterType)));
                    }
                }

                blockExpressions.Add(readBeginObject);
                blockExpressions.Add(Expression.Loop(Expression.IfThenElse(abortExpression, Expression.Break(loopAbort), deserializeMemberBlock), loopAbort));
                blockExpressions.Add(Expression.Assign(returnValue, Expression.New(objectDescription.Constructor, constructorParameterExpressions)));

                foreach (var item in additionalAfterCtorParameterExpressions)
                {
                    var assignValueExpression = Expression.Assign(Expression.PropertyOrField(returnValue, item.Key), item.Value.Variable);
                    var assignIfHasValueExpression = Expression.IfThen(item.Value.HasVariable, assignValueExpression);
                    blockExpressions.Add(assignIfHasValueExpression);
                }

                blockExpressions.Add(Expression.Label(returnTarget, returnValue));
                block = Expression.Block(blockParameters, blockExpressions.ToArray());
            }
            else
            {
                block = Expression.Block(new[] { returnValue, countExpression }, readBeginObject,
                    Expression.Assign(returnValue, Expression.New(returnValue.Type)),
                    Expression.Loop(
                        Expression.IfThenElse(abortExpression, Expression.Break(loopAbort),
                            deserializeMemberBlock), loopAbort
                    ),
                    Expression.Label(returnTarget, returnValue)
                );
            }

            var lambda = Expression.Lambda<DeserializeDelegate<T, TSymbol>>(block, readerParameter, resolverParameter);
            return lambda.Compile();
        }

        private static void DeserializeExtension<TSymbol, TResolver>(ref JsonReader<TSymbol> reader,
            IJsonFormatterResolver<TSymbol> resolver, in ReadOnlySpan<byte> nameSpan,
            IDictionary<string, object?> dictionary, bool excludeNulls)
            where TResolver : IJsonFormatterResolver<TSymbol, TResolver>, new() where TSymbol : struct
        {
            var value = RuntimeFormatter<TSymbol, TResolver>.Default.Deserialize(ref reader, resolver);
            if (excludeNulls && value is null)
            {
                return;
            }

            string? key = null;
            if (SymbolHelper<TSymbol>.IsUtf8)
            {
                key = TextEncodings.Utf8.GetStringWithCache(nameSpan);
            }
            else if (SymbolHelper<TSymbol>.IsUtf16)
            {
                key = TextEncodings.Utf16.GetStringWithCache(nameSpan);
            }
            else
            {
                ThrowHelper.ThrowNotSupportedException();
            }

            dictionary[key] = value;
        }

        private static void SerializeExtension<TSymbol, TResolver>(ref JsonWriter<TSymbol> writer,
            IJsonFormatterResolver<TSymbol> resolver, IDictionary<string, object?> value, bool writeSeparator,
            bool excludeNulls, HashSet<string> knownNames)
            where TResolver : IJsonFormatterResolver<TSymbol, TResolver>, new() where TSymbol : struct
        {
            var valueLength = value.Count;
            if (valueLength > 0)
            {
                foreach (var kvp in value)
                {
                    if (excludeNulls && kvp.Value is null)
                    {
                        continue;
                    }

                    if (writeSeparator)
                    {
                        writer.WriteValueSeparator();
                    }

                    var name = kvp.Key;
                    writer.WriteName(resolver.GetEncodedExtensionDataName(name));

                    if (knownNames.Contains(name))
                    {
                        continue;
                    }

                    writer.IncrementDepth();
                    SerializeRuntimeDecisionInternal<object?, TSymbol, TResolver>(ref writer, kvp.Value, RuntimeFormatter<TSymbol, TResolver>.Default, resolver);
                    writer.DecrementDepth();
                    writeSeparator = true;
                }
            }
        }

        /// <summary>
        /// This is basically the same algorithm as in the t4 template to create the methods
        /// It's necessary to update both
        /// </summary>
        private static ConstantExpression[] GetIntegersForMemberName(byte[] utf8Bytes)
        {
            var result = new List<ConstantExpression>();
            var remaining = utf8Bytes.Length;
            var ulongCount = Math.DivRem(remaining, 8, out remaining);
            var offset = 0;
            for (var j = 0; j < ulongCount; j++)
            {
                result.Add(Expression.Constant(BitConverter.ToUInt64(utf8Bytes, offset)));
                offset += sizeof(ulong);
            }

            var uintCount = Math.DivRem(remaining, 4, out remaining);
            for (var j = 0; j < uintCount; j++)
            {
                result.Add(Expression.Constant(BitConverter.ToUInt32(utf8Bytes, offset)));
                offset += sizeof(uint);
            }

            var ushortCount = Math.DivRem(remaining, 2, out remaining);
            for (var j = 0; j < ushortCount; j++)
            {
                result.Add(Expression.Constant(BitConverter.ToUInt16(utf8Bytes, offset)));
                offset += sizeof(ushort);
            }

            var byteCount = Math.DivRem(remaining, 1, out remaining);
            for (var j = 0; j < byteCount; j++)
            {
                result.Add(Expression.Constant(utf8Bytes[offset]));
                offset++;
            }

            Debug.Assert(remaining == 0);
            Debug.Assert(offset == utf8Bytes.Length);
            return result.ToArray();
        }

        /// <summary>
        /// In some cases it is necessary to decide at runtime which serializer to use
        /// Structs and sealed type are safe (no derived types for them)
        /// </summary>
        private static bool IsNoRuntimeDecisionRequired(Type memberType)
        {
            return memberType.IsValueType || memberType.IsSealed;
        }

        protected delegate T DeserializeDelegate<out T, TSymbol>(ref JsonReader<TSymbol> reader, IJsonFormatterResolver<TSymbol> resolver) where TSymbol : struct;


        protected delegate void SerializeDelegate<in T, TSymbol>(ref JsonWriter<TSymbol> writer, T value, IJsonFormatterResolver<TSymbol> resolver) where TSymbol : struct;
    }
}