﻿using System;
using System.Buffers;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.CSharp.RuntimeBinder;
using SpanJson.Dynamic;
using SpanJson.Helpers;
using SpanJson.Resolvers;
using SpanJson.Internal;
using Binder = Microsoft.CSharp.RuntimeBinder.Binder;

namespace SpanJson.Formatters
{
    public class DynamicMetaObjectProviderFormatter<T, TSymbol, TResolver> : BaseFormatter, IJsonFormatter<T, TSymbol> // TODO Utf8 / Utf16
        where T : IDynamicMetaObjectProvider
        where TResolver : IJsonFormatterResolver<TSymbol, TResolver>, new()
        where TSymbol : struct
    {
        private static readonly Func<T> CreateFunctor;

        public static readonly DynamicMetaObjectProviderFormatter<T, TSymbol, TResolver> Default;

        private static readonly IJsonFormatterResolver<TSymbol, TResolver> Resolver;
        private static readonly Dictionary<string, DeserializeDelegate> KnownMembersDictionary;
        private static readonly ConcurrentDictionary<string, Func<T, object?>> GetMemberCache;
        private static readonly ConcurrentDictionary<string, Action<T, object?>> SetMemberCache;

        static DynamicMetaObjectProviderFormatter()
        {
            Resolver = StandardResolvers.GetResolver<TSymbol, TResolver>();
            GetMemberCache = new ConcurrentDictionary<string, Func<T, object?>>(StringComparer.Ordinal);
            SetMemberCache = new ConcurrentDictionary<string, Action<T, object?>>(StringComparer.Ordinal);
            KnownMembersDictionary = BuildKnownMembers(Resolver);

            CreateFunctor = StandardResolvers.GetCreateFunctor<TSymbol, TResolver, T>();
            Default = new DynamicMetaObjectProviderFormatter<T, TSymbol, TResolver>();
        }


        public T? Deserialize(ref JsonReader<TSymbol> reader, IJsonFormatterResolver<TSymbol> resolver)
        {
            if (reader.ReadIsNull())
            {
                return default;
            }

            reader.ReadBeginObjectOrThrow();
            var result = CreateFunctor();
            var count = 0;
            while (!reader.TryReadIsEndObjectOrValueSeparator(ref count))
            {
                var name = reader.ReadEscapedName(); // TODO read cache
                if (KnownMembersDictionary.TryGetValue(name, out var action))
                {
                    action(result, ref reader, resolver); // if we have known members we try to assign them directly without dynamic
                }
                else
                {
                    var setter = GetOrAddSetMember(name);
                    setter(result, resolver.DynamicDeserializer(ref reader));
                }
            }

            return result;
        }

        public void Serialize(ref JsonWriter<TSymbol> writer, T value, IJsonFormatterResolver<TSymbol> resolver)
        {
            // if we serialize our dynamic value again we simply write the symbols directly if it is the same type
            if (value is ISpanJsonDynamicValue<TSymbol> dynamicValue)
            {
                writer.WriteVerbatim(dynamicValue.Symbols);
                return;
            }

            switch (value)
            {
                case null:
                    writer.WriteNull();
                    return;

                case ISpanJsonDynamicValue<byte> bValue:
                    var cMaxLength = Encoding.UTF8.GetMaxCharCount(bValue.Symbols.Count);
                    char[]? cBuffer = null;
                    try
                    {
                        Span<char> utf16Span = (uint)cMaxLength <= JsonSharedConstant.StackallocCharThresholdU ?
                            stackalloc char[JsonSharedConstant.StackallocCharThreshold] :
                            (cBuffer = ArrayPool<char>.Shared.Rent(cMaxLength));

                        var written = TextEncodings.Utf8.GetChars(bValue.Symbols, utf16Span);

#if NETSTANDARD2_0
                        unsafe
                        {
                            writer.WriteUtf16Verbatim(new ReadOnlySpan<char>(Unsafe.AsPointer(ref MemoryMarshal.GetReference(utf16Span)), written));
                        }
#else
                        writer.WriteUtf16Verbatim(MemoryMarshal.CreateReadOnlySpan(ref MemoryMarshal.GetReference(utf16Span), written));
#endif
                    }
                    finally
                    {
                        if (cBuffer is not null) { ArrayPool<char>.Shared.Return(cBuffer); }
                    }
                    break;

                case ISpanJsonDynamicValue<char> cValue:
                    var bMaxLength = TextEncodings.Utf8.GetMaxByteCount(cValue.Symbols.Count);
                    byte[]? bBuffer = null;
                    try
                    {
                        Span<byte> utf8Span = (uint)bMaxLength <= JsonSharedConstant.StackallocByteThresholdU ?
                            stackalloc byte[JsonSharedConstant.StackallocByteThreshold] :
                            (bBuffer = ArrayPool<byte>.Shared.Rent(bMaxLength));

                        var written = TextEncodings.Utf8.GetBytes(cValue.Symbols, utf8Span);

#if NETSTANDARD2_0
                        unsafe
                        {
                            writer.WriteUtf8Verbatim(new ReadOnlySpan<byte>(Unsafe.AsPointer(ref MemoryMarshal.GetReference(utf8Span)), written));
                        }
#else
                        writer.WriteUtf8Verbatim(MemoryMarshal.CreateReadOnlySpan(ref MemoryMarshal.GetReference(utf8Span), written));
#endif
                    }
                    finally
                    {
                        if (bBuffer is not null) { ArrayPool<byte>.Shared.Return(bBuffer); }
                    }
                    break;

                case ISpanJsonDynamicArray dynamicArray:
                    writer.IncrementDepth();
                    EnumerableFormatter<IEnumerable<object>, object, TSymbol, TResolver>.Default.Serialize(ref writer, dynamicArray, resolver);
                    writer.DecrementDepth();
                    break;

                default:
                    var memberInfos = Resolver.GetDynamicObjectDescription(value);
                    var counter = 0;
                    writer.WriteBeginObject();
                    foreach (var memberInfo in memberInfos)
                    {
                        var getter = GetOrAddGetMember(memberInfo.MemberName);
                        var child = getter(value);
                        if (memberInfo.ExcludeNull && child is null) { continue; }

                        if (counter++ > 0) { writer.WriteValueSeparator(); }

                        writer.IncrementDepth();
                        writer.WriteName(memberInfo.EscapedName);
                        RuntimeFormatter<TSymbol, TResolver>.Default.Serialize(ref writer, child, resolver);
                        writer.DecrementDepth();
                    }

                    // Some dlr objects also have properties which are defined on the custom type, we also need to serialize/deserialize them
                    var definedMembers = Resolver.GetObjectDescription<T>();
                    foreach (var memberInfo in definedMembers)
                    {
                        var getter = GetOrAddGetDefinedMember(memberInfo.MemberName);
                        var child = getter(value);
                        if (memberInfo.ExcludeNull && child is null) { continue; }

                        if (counter++ > 0) { writer.WriteValueSeparator(); }

                        writer.IncrementDepth();
                        writer.WriteName(memberInfo.EscapedName);
                        RuntimeFormatter<TSymbol, TResolver>.Default.Serialize(ref writer, child, resolver);
                        writer.DecrementDepth();
                    }

                    writer.WriteEndObject();
                    break;
            }
        }

        private static Dictionary<string, DeserializeDelegate> BuildKnownMembers(IJsonFormatterResolver<TSymbol, TResolver> resolver)
        {
            var memberInfos = resolver.GetObjectDescription<T>().ToList();
            var inputParameter = Expression.Parameter(typeof(T), "input");
            var readerParameter = Expression.Parameter(typeof(JsonReader<TSymbol>).MakeByRefType(), "reader");
            var resolverParameter = Expression.Parameter(typeof(IJsonFormatterResolver<TSymbol>), "resolver");
            var result = new Dictionary<string, DeserializeDelegate>(resolver.JsonOptions.PropertyNameCaseInsensitive ? StringComparer.OrdinalIgnoreCase : StringComparer.Ordinal);
            // can't deserialize abstract or interface
            foreach (var memberInfo in memberInfos)
            {
                if (!memberInfo.CanWrite)
                {
                    var skipNextMethodInfo = FindPublicInstanceMethod(readerParameter.Type, nameof(JsonReader<TSymbol>.SkipNextSegment));
                    var skipExpression = Expression
                        .Lambda<DeserializeDelegate>(Expression.Call(readerParameter, skipNextMethodInfo), inputParameter, readerParameter, resolverParameter).Compile();
                    result.Add(memberInfo.Name, skipExpression);
                    continue;
                }

                // can't deserialize abstract and only support interfaces based on IEnumerable<T> (this includes, IList, IReadOnlyList, IDictionary et al.)
                if (memberInfo.MemberType.IsAbstract && !resolver.IsSupportedType(memberInfo.MemberType) && !memberInfo.MemberType.TryGetTypeOfGenericInterface(typeof(IEnumerable<>), out _))
                {
                    var throwExpression = Expression.Lambda<DeserializeDelegate>(Expression.Block(
                            Expression.Throw(Expression.Constant(new NotSupportedException($"{typeof(T).Name} contains abstract members."))),
                            Expression.Default(typeof(T))),
                        inputParameter, readerParameter, resolverParameter).Compile();
                    result.Add(memberInfo.Name, throwExpression);
                    continue;
                }

                var formatterType = resolver.GetFormatter(memberInfo).GetType();
                var fieldInfo = formatterType.GetField("Default", BindingFlags.Static | BindingFlags.Public)!;
                var assignExpression = Expression.Assign(Expression.PropertyOrField(inputParameter, memberInfo.MemberName),
                    Expression.Call(Expression.Field(null, fieldInfo),
                        FindPublicInstanceMethod(formatterType, "Deserialize", readerParameter.Type.MakeByRefType(), resolverParameter.Type), readerParameter, resolverParameter));
                var lambda = Expression.Lambda<DeserializeDelegate>(assignExpression, inputParameter, readerParameter, resolverParameter).Compile();
                result.Add(memberInfo.Name, lambda);
            }

            return result;
        }

        private static Func<T, object?> GetOrAddGetDefinedMember(string memberName)
        {
            return GetMemberCache.GetOrAdd(memberName, s =>
            {
                var paramExpression = Expression.Parameter(typeof(T), "input");
                return Expression.Lambda<Func<T, object>>(Expression.Convert(Expression.PropertyOrField(paramExpression, s), typeof(object)), paramExpression)
                    .Compile();
            });
        }

        private static Func<T, object?> GetOrAddGetMember(string memberName)
        {
            return GetMemberCache.GetOrAdd(memberName, s =>
            {
                var binder = (GetMemberBinder)Binder.GetMember(CSharpBinderFlags.None, s, typeof(T),
                    new[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) });
                var callSite = CallSite<Func<CallSite, object, object>>.Create(binder);
                return target => callSite.Target(callSite, target);
            });
        }

        private static Action<T, object?> GetOrAddSetMember(string memberName)
        {
            return SetMemberCache.GetOrAdd(memberName, s =>
            {
                var binder = Binder.SetMember(CSharpBinderFlags.None, memberName, null,
                    new[]
                    {
                        CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
                        CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
                    });
                var callsite = CallSite<Func<CallSite, object, object?, object>>.Create(binder);
                return (target, value) => callsite.Target(callsite, target, value);
            });
        }

        protected delegate void DeserializeDelegate(T input, ref JsonReader<TSymbol> reader, IJsonFormatterResolver<TSymbol> resolver);
    }
}