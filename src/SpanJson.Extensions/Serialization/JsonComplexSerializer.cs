using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using CuteAnt.Pool;
using SpanJson.Resolvers;
using NJsonSerializer = Newtonsoft.Json.JsonSerializer;
using NJsonSerializerSettings = Newtonsoft.Json.JsonSerializerSettings;

namespace SpanJson.Serialization
{
    public sealed class JsonComplexSerializer
    {
        public static readonly IJsonSerializer Default = new JsonComplexSerializer<IncludeNullsOriginalCaseResolver<char>, IncludeNullsOriginalCaseResolver<byte>>(JsonKnownNamingPolicy.CamelCase);
        public static readonly IJsonSerializer CamelCase = new JsonComplexSerializer<IncludeNullsCamelCaseResolver<char>, IncludeNullsCamelCaseResolver<byte>>(JsonKnownNamingPolicy.CamelCase);
        public static readonly IJsonSerializer SnakeCase = new JsonComplexSerializer<IncludeNullsSnakeCaseResolver<char>, IncludeNullsSnakeCaseResolver<byte>>(JsonKnownNamingPolicy.SnakeCase);
        public static readonly IJsonSerializer AdaCase = new JsonComplexSerializer<IncludeNullsAdaCaseResolver<char>, IncludeNullsAdaCaseResolver<byte>>(JsonKnownNamingPolicy.AdaCase);
        public static readonly IJsonSerializer MacroCase = new JsonComplexSerializer<IncludeNullsMacroCaseResolver<char>, IncludeNullsMacroCaseResolver<byte>>(JsonKnownNamingPolicy.MacroCase);
        public static readonly IJsonSerializer KebabCase = new JsonComplexSerializer<IncludeNullsKebabCaseResolver<char>, IncludeNullsKebabCaseResolver<byte>>(JsonKnownNamingPolicy.KebabCase);
        public static readonly IJsonSerializer TrainCase = new JsonComplexSerializer<IncludeNullsTrainCaseResolver<char>, IncludeNullsTrainCaseResolver<byte>>(JsonKnownNamingPolicy.TrainCase);
        public static readonly IJsonSerializer CobolCase = new JsonComplexSerializer<IncludeNullsCobolCaseResolver<char>, IncludeNullsCobolCaseResolver<byte>>(JsonKnownNamingPolicy.CobolCase);
    }

    public partial class JsonComplexSerializer<TUtf16Resolver, TUtf8Resolver> : IJsonSerializer
        where TUtf16Resolver : IJsonFormatterResolver<char, TUtf16Resolver>, new()
        where TUtf8Resolver : IJsonFormatterResolver<byte, TUtf8Resolver>, new()
    {
        static readonly ConcurrentDictionary<Type, JsonSerializer.NonGeneric.Inner<char, TUtf16Resolver>.Invoker> Utf16Invokers =
            JsonSerializer.NonGeneric.Inner<char, TUtf16Resolver>.Invokers;
        static readonly Func<Type, JsonSerializer.NonGeneric.Inner<char, TUtf16Resolver>.Invoker> Utf16InvokerFactory =
            JsonSerializer.NonGeneric.Inner<char, TUtf16Resolver>.InvokerFactory;

        static readonly ConcurrentDictionary<Type, JsonSerializer.NonGeneric.Inner<byte, TUtf8Resolver>.Invoker> Utf8Invokers =
            JsonSerializer.NonGeneric.Inner<byte, TUtf8Resolver>.Invokers;
        static readonly Func<Type, JsonSerializer.NonGeneric.Inner<byte, TUtf8Resolver>.Invoker> Utf8InvokerFactory =
            JsonSerializer.NonGeneric.Inner<byte, TUtf8Resolver>.InvokerFactory;

        private readonly NJsonSerializerSettings _serializerSettings;
        private readonly NJsonSerializerSettings _deserializerSettings;
        private ObjectPool<NJsonSerializer>? _serializerPool;
        private ObjectPool<NJsonSerializer>? _deserializerPool;

        internal JsonComplexSerializer(JsonKnownNamingPolicy namingPolicy)
        {
            _serializerSettings = JsonSerializerPool.CreateSerializerSettings(namingPolicy, excludeNulls: false, isPolymorphic: true);
            _deserializerSettings = JsonSerializerPool.CreateDeserializerSettings(namingPolicy, excludeNulls: false, isPolymorphic: true);
        }

        public JsonComplexSerializer(NJsonSerializerSettings serializerSettings, NJsonSerializerSettings deserializerSettings)
        {
            if (serializerSettings is null) { ThrowHelper.ThrowArgumentNullException(ExceptionArgument.serializerSettings); }
            if (deserializerSettings is null) { ThrowHelper.ThrowArgumentNullException(ExceptionArgument.deserializerSettings); }
            if (ReferenceEquals(serializerSettings, serializerSettings)) { ThrowHelper.ThrowArgumentException_SerializerSettings_same_instance(); }

            _serializerSettings = serializerSettings;
            _deserializerSettings = deserializerSettings;
        }

        /// <summary>Gets or sets the default <see cref="NJsonSerializerSettings"/> used to configure the <see cref="NJsonSerializer"/>.</summary>
        public NJsonSerializerSettings SerializerSettings => _serializerSettings;
        public ObjectPool<NJsonSerializer> SerializerPool
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _serializerPool ?? EnsureSerializerPoolCreated();
        }
        [MethodImpl(MethodImplOptions.NoInlining)]
        private ObjectPool<NJsonSerializer> EnsureSerializerPoolCreated()
        {
            Interlocked.CompareExchange(ref _serializerPool, JsonSerializerPool.GetJsonSerializerPool(_serializerSettings), null);
            return _serializerPool;
        }

        /// <summary>Gets or sets the default <see cref="NJsonSerializerSettings"/> used to configure the <see cref="NJsonSerializer"/>.</summary>
        public NJsonSerializerSettings DeserializerSettings => _deserializerSettings;
        public ObjectPool<NJsonSerializer> DeserializerPool
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _deserializerPool ?? EnsureDeserializerPoolCreated();
        }
        [MethodImpl(MethodImplOptions.NoInlining)]
        private ObjectPool<NJsonSerializer> EnsureDeserializerPoolCreated()
        {
            Interlocked.CompareExchange(ref _deserializerPool, JsonSerializerPool.GetJsonSerializerPool(_deserializerSettings), null);
            return _deserializerPool;
        }

        public NJsonSerializerSettings CreateSerializerSettings(Action<NJsonSerializerSettings> configSettings)
        {
            if (configSettings is null) { ThrowHelper.ThrowArgumentNullException(ExceptionArgument.configSettings); }

            var serializerSettings = new NJsonSerializerSettings();
            var converters = serializerSettings.Converters;
            foreach (var item in _serializerSettings.Converters)
            {
                converters.Add(item);
            }
            configSettings.Invoke(serializerSettings);
            return serializerSettings;
        }

        public NJsonSerializerSettings CreateDeserializerSettings(Action<NJsonSerializerSettings> configSettings)
        {
            if (configSettings is null) { ThrowHelper.ThrowArgumentNullException(ExceptionArgument.configSettings); }

            var serializerSettings = new NJsonSerializerSettings();
            var converters = serializerSettings.Converters;
            foreach (var item in _deserializerSettings.Converters)
            {
                converters.Add(item);
            }
            configSettings.Invoke(serializerSettings);
            return serializerSettings;
        }
    }
}
