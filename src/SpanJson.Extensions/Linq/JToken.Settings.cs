using System.Runtime.CompilerServices;
using CuteAnt.Pool;
using SpanJson.Serialization;
using NJsonSerializer = Newtonsoft.Json.JsonSerializer;
using NJsonSerializerSettings = Newtonsoft.Json.JsonSerializerSettings;

namespace SpanJson.Linq
{
    partial class JToken
    {
        private static readonly NJsonSerializerSettings _defaultSerializerSettings;
        private static ObjectPool<NJsonSerializer>? _defaultSerializerPool;

        private static readonly NJsonSerializerSettings _polymorphicSerializerSettings;
        private static ObjectPool<NJsonSerializer>? _polymorphicSerializerPool;

        private static readonly NJsonSerializerSettings _polymorphicDeserializerSettings;
        private static ObjectPool<NJsonSerializer>? _polymorphicDeserializerPool;

        public static NJsonSerializerSettings DefaultSerializerSettings => _defaultSerializerSettings;
        public static ObjectPool<NJsonSerializer> DefaultSerializerPool
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _defaultSerializerPool ?? EnsureSerializerPoolCreated();
        }

        public static NJsonSerializerSettings PolymorphicSerializerSettings => _polymorphicSerializerSettings;
        public static ObjectPool<NJsonSerializer> PolymorphicSerializerPool
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _polymorphicSerializerPool ?? EnsurePolymorphicSerializerPoolCreated();
        }

        [Obsolete("=> PolymorphicDeserializerSettings")]
        public static NJsonSerializerSettings DefaultDeserializerSettings => _polymorphicDeserializerSettings;
        [Obsolete("=> PolymorphicDeserializerPool")]
        public static ObjectPool<NJsonSerializer> DefaultDeserializerPool => PolymorphicDeserializerPool;

        public static NJsonSerializerSettings PolymorphicDeserializerSettings => _polymorphicDeserializerSettings;
        public static ObjectPool<NJsonSerializer> PolymorphicDeserializerPool
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _polymorphicDeserializerPool ?? EnsurePolymorphicDeserializerPoolCreated();
        }

        static JToken()
        {
            _defaultSerializerSettings = new NJsonSerializerSettings();
            _polymorphicSerializerSettings = new NJsonSerializerSettings
            {
                SerializationBinder = JsonSerializationBinder.Instance,
                TypeNameHandling = Newtonsoft.Json.TypeNameHandling.Auto
            };
            var jonsConverters = new List<Newtonsoft.Json.JsonConverter>
            {
                SpanJson.Converters.DynamicObjectConverter.Instance,
                SpanJson.Converters.DynamicUtf16ArrayConverter.Instance,
                SpanJson.Converters.DynamicUtf16NumberConverter.Instance,
                SpanJson.Converters.DynamicUtf16StringConverter.Instance,
                SpanJson.Converters.DynamicUtf8ArrayConverter.Instance,
                SpanJson.Converters.DynamicUtf8NumberConverter.Instance,
                SpanJson.Converters.DynamicUtf8StringConverter.Instance,

                SpanJson.Converters.JsonDocumentConverter.Instance,
                SpanJson.Converters.JsonElementConverter.Instance,

                SpanJson.Converters.JTokenConverter.Instance,

                SpanJson.Converters.CombGuidJTokenConverter.Instance,
                Newtonsoft.Json.Converters.IPAddressConverter.Instance,
                Newtonsoft.Json.Converters.IPEndPointConverter.Instance,
            };
            _defaultSerializerSettings.Converters = jonsConverters;
            var converters = _polymorphicSerializerSettings.Converters;
            foreach (var item in jonsConverters)
            {
                converters.Add(item);
            }

            _polymorphicDeserializerSettings = JsonSerializerPool.CreateDeserializerSettings(JsonKnownNamingPolicy.Unspecified, false, true);
        }

        public static NJsonSerializerSettings CreateSerializerSettings(Action<NJsonSerializerSettings> configSettings)
        {
            if (configSettings is null) { ThrowHelper.ThrowArgumentNullException(ExceptionArgument.configSettings); }

            var serializerSettings = new NJsonSerializerSettings();
            var converters = serializerSettings.Converters;
            foreach (var item in _defaultSerializerSettings.Converters)
            {
                converters.Add(item);
            }
            configSettings.Invoke(serializerSettings);
            return serializerSettings;
        }

        public static NJsonSerializerSettings CreateDeserializerSettings(Action<NJsonSerializerSettings> configSettings)
        {
            if (configSettings is null) { ThrowHelper.ThrowArgumentNullException(ExceptionArgument.configSettings); }

            var serializerSettings = new NJsonSerializerSettings();
            var converters = serializerSettings.Converters;
            foreach (var item in _polymorphicDeserializerSettings.Converters)
            {
                converters.Add(item);
            }
            configSettings.Invoke(serializerSettings);
            return serializerSettings;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static ObjectPool<NJsonSerializer> EnsureSerializerPoolCreated()
        {
            Interlocked.CompareExchange(ref _defaultSerializerPool, JsonSerializerPool.GetJsonSerializerPool(_defaultSerializerSettings), null);
            return _defaultSerializerPool;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static ObjectPool<NJsonSerializer> EnsurePolymorphicSerializerPoolCreated()
        {
            Interlocked.CompareExchange(ref _polymorphicSerializerPool, JsonSerializerPool.GetJsonSerializerPool(_polymorphicSerializerSettings), null);
            return _polymorphicSerializerPool;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static ObjectPool<NJsonSerializer> EnsurePolymorphicDeserializerPoolCreated()
        {
            Interlocked.CompareExchange(ref _polymorphicDeserializerPool, JsonSerializerPool.GetJsonSerializerPool(_polymorphicDeserializerSettings), null);
            return _polymorphicDeserializerPool;
        }
    }
}