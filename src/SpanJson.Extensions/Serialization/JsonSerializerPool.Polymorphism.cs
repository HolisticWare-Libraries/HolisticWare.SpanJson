using System.Runtime.CompilerServices;
using CuteAnt.Pool;
using NJsonSerializer = Newtonsoft.Json.JsonSerializer;
using NJsonSerializerSettings = Newtonsoft.Json.JsonSerializerSettings;

namespace SpanJson.Serialization
{
    static partial class JsonSerializerPool
    {
        public static class Polymorphism
        {
            #region @@ Constructor @@

            static Polymorphism()
            {
                DefaultSerializerSettings = CreateSerializerSettings(JsonKnownNamingPolicy.Unspecified, excludeNulls: true, isPolymorphic: true);
                DefaultDeserializerSettings = CreateDeserializerSettings(JsonKnownNamingPolicy.Unspecified, excludeNulls: true, isPolymorphic: true);
                CamelCaseSerializerSettings = CreateSerializerSettings(JsonKnownNamingPolicy.CamelCase, excludeNulls: true, isPolymorphic: true);
                CamelCaseDeserializerSettings = CreateDeserializerSettings(JsonKnownNamingPolicy.CamelCase, excludeNulls: true, isPolymorphic: true);
                SnakeCaseSerializerSettings = CreateSerializerSettings(JsonKnownNamingPolicy.SnakeCase, excludeNulls: true, isPolymorphic: true);
                SnakeCaseDeserializerSettings = CreateDeserializerSettings(JsonKnownNamingPolicy.SnakeCase, excludeNulls: true, isPolymorphic: true);
                AdaCaseSerializerSettings = CreateSerializerSettings(JsonKnownNamingPolicy.AdaCase, excludeNulls: true, isPolymorphic: true);
                AdaCaseDeserializerSettings = CreateDeserializerSettings(JsonKnownNamingPolicy.AdaCase, excludeNulls: true, isPolymorphic: true);
                MacroCaseSerializerSettings = CreateSerializerSettings(JsonKnownNamingPolicy.MacroCase, excludeNulls: true, isPolymorphic: true);
                MacroCaseDeserializerSettings = CreateDeserializerSettings(JsonKnownNamingPolicy.MacroCase, excludeNulls: true, isPolymorphic: true);
                KebabCaseSerializerSettings = CreateSerializerSettings(JsonKnownNamingPolicy.KebabCase, excludeNulls: true, isPolymorphic: true);
                KebabCaseDeserializerSettings = CreateDeserializerSettings(JsonKnownNamingPolicy.KebabCase, excludeNulls: true, isPolymorphic: true);
                TrainCaseSerializerSettings = CreateSerializerSettings(JsonKnownNamingPolicy.TrainCase, excludeNulls: true, isPolymorphic: true);
                TrainCaseDeserializerSettings = CreateDeserializerSettings(JsonKnownNamingPolicy.TrainCase, excludeNulls: true, isPolymorphic: true);
                CobolCaseSerializerSettings = CreateSerializerSettings(JsonKnownNamingPolicy.CobolCase, excludeNulls: true, isPolymorphic: true);
                CobolCaseDeserializerSettings = CreateDeserializerSettings(JsonKnownNamingPolicy.CobolCase, excludeNulls: true, isPolymorphic: true);
            }

            #endregion

            #region -- Properties --

            #region - PascalCase -

            private static ObjectPool<NJsonSerializer>? _defaultSerializerPool;

            public static NJsonSerializerSettings DefaultSerializerSettings { get; }
            public static ObjectPool<NJsonSerializer> DefaultSerializerPool
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get => _defaultSerializerPool ?? EnsureSerializerPoolCreated();
            }
            [MethodImpl(MethodImplOptions.NoInlining)]
            private static ObjectPool<NJsonSerializer> EnsureSerializerPoolCreated()
            {
                Interlocked.CompareExchange(ref _defaultSerializerPool, GetJsonSerializerPool(DefaultSerializerSettings), null);
                return _defaultSerializerPool;
            }

            private static ObjectPool<NJsonSerializer>? _defaultDeserializerPool;

            public static NJsonSerializerSettings DefaultDeserializerSettings { get; }
            public static ObjectPool<NJsonSerializer> DefaultDeserializerPool
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get => _defaultDeserializerPool ?? EnsureDeserializerPoolCreated();
            }
            [MethodImpl(MethodImplOptions.NoInlining)]
            private static ObjectPool<NJsonSerializer> EnsureDeserializerPoolCreated()
            {
                Interlocked.CompareExchange(ref _defaultDeserializerPool, GetJsonSerializerPool(DefaultDeserializerSettings), null);
                return _defaultDeserializerPool;
            }

            #endregion

            #region - CamelCase -

            private static ObjectPool<NJsonSerializer>? _camelCaseSerializerPool;

            public static NJsonSerializerSettings CamelCaseSerializerSettings { get; }
            public static ObjectPool<NJsonSerializer> CamelCaseSerializerPool
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get => _camelCaseSerializerPool ?? EnsureCamelCaseSerializerPoolCreated();
            }
            [MethodImpl(MethodImplOptions.NoInlining)]
            private static ObjectPool<NJsonSerializer> EnsureCamelCaseSerializerPoolCreated()
            {
                Interlocked.CompareExchange(ref _camelCaseSerializerPool, GetJsonSerializerPool(CamelCaseSerializerSettings), null);
                return _camelCaseSerializerPool;
            }

            private static ObjectPool<NJsonSerializer>? _camelCaseDeserializerPool;

            public static NJsonSerializerSettings CamelCaseDeserializerSettings { get; }
            public static ObjectPool<NJsonSerializer> CamelCaseDeserializerPool
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get => _camelCaseDeserializerPool ?? EnsureCamelCaseDeserializerPoolCreated();
            }
            [MethodImpl(MethodImplOptions.NoInlining)]
            private static ObjectPool<NJsonSerializer> EnsureCamelCaseDeserializerPoolCreated()
            {
                Interlocked.CompareExchange(ref _camelCaseDeserializerPool, GetJsonSerializerPool(CamelCaseDeserializerSettings), null);
                return _camelCaseDeserializerPool;
            }

            #endregion

            #region - SnakeCase -

            private static ObjectPool<NJsonSerializer>? _snakeCaseSerializerPool;

            public static NJsonSerializerSettings SnakeCaseSerializerSettings { get; }
            public static ObjectPool<NJsonSerializer> SnakeCaseSerializerPool
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get => _snakeCaseSerializerPool ?? EnsureSnakeCaseSerializerPoolCreated();
            }
            [MethodImpl(MethodImplOptions.NoInlining)]
            private static ObjectPool<NJsonSerializer> EnsureSnakeCaseSerializerPoolCreated()
            {
                Interlocked.CompareExchange(ref _snakeCaseSerializerPool, GetJsonSerializerPool(SnakeCaseSerializerSettings), null);
                return _snakeCaseSerializerPool;
            }

            private static ObjectPool<NJsonSerializer>? _snakeCaseDeserializerPool;

            public static NJsonSerializerSettings SnakeCaseDeserializerSettings { get; }
            public static ObjectPool<NJsonSerializer> SnakeCaseDeserializerPool
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get => _snakeCaseDeserializerPool ?? EnsureSnakeCaseDeserializerPoolCreated();
            }
            [MethodImpl(MethodImplOptions.NoInlining)]
            private static ObjectPool<NJsonSerializer> EnsureSnakeCaseDeserializerPoolCreated()
            {
                Interlocked.CompareExchange(ref _snakeCaseDeserializerPool, GetJsonSerializerPool(SnakeCaseDeserializerSettings), null);
                return _snakeCaseDeserializerPool;
            }

            #endregion

            #region - AdaCase -

            private static ObjectPool<NJsonSerializer>? _adaCaseSerializerPool;

            public static NJsonSerializerSettings AdaCaseSerializerSettings { get; }
            public static ObjectPool<NJsonSerializer> AdaCaseSerializerPool
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get => _adaCaseSerializerPool ?? EnsureAdaCaseSerializerPoolCreated();
            }
            [MethodImpl(MethodImplOptions.NoInlining)]
            private static ObjectPool<NJsonSerializer> EnsureAdaCaseSerializerPoolCreated()
            {
                Interlocked.CompareExchange(ref _adaCaseSerializerPool, GetJsonSerializerPool(AdaCaseSerializerSettings), null);
                return _adaCaseSerializerPool;
            }

            private static ObjectPool<NJsonSerializer>? _adaCaseDeserializerPool;

            public static NJsonSerializerSettings AdaCaseDeserializerSettings { get; }
            public static ObjectPool<NJsonSerializer> AdaCaseDeserializerPool
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get => _adaCaseDeserializerPool ?? EnsureAdaCaseDeserializerPoolCreated();
            }
            [MethodImpl(MethodImplOptions.NoInlining)]
            private static ObjectPool<NJsonSerializer> EnsureAdaCaseDeserializerPoolCreated()
            {
                Interlocked.CompareExchange(ref _adaCaseDeserializerPool, GetJsonSerializerPool(AdaCaseDeserializerSettings), null);
                return _adaCaseDeserializerPool;
            }

            #endregion

            #region - MacroCase -

            private static ObjectPool<NJsonSerializer>? _macroCaseSerializerPool;

            public static NJsonSerializerSettings MacroCaseSerializerSettings { get; }
            public static ObjectPool<NJsonSerializer> MacroCaseSerializerPool
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get => _macroCaseSerializerPool ?? EnsureMacroCaseSerializerPoolCreated();
            }
            [MethodImpl(MethodImplOptions.NoInlining)]
            private static ObjectPool<NJsonSerializer> EnsureMacroCaseSerializerPoolCreated()
            {
                Interlocked.CompareExchange(ref _macroCaseSerializerPool, GetJsonSerializerPool(MacroCaseSerializerSettings), null);
                return _macroCaseSerializerPool;
            }

            private static ObjectPool<NJsonSerializer>? _macroCaseDeserializerPool;

            public static NJsonSerializerSettings MacroCaseDeserializerSettings { get; }
            public static ObjectPool<NJsonSerializer> MacroCaseDeserializerPool
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get => _macroCaseDeserializerPool ?? EnsureMacroCaseDeserializerPoolCreated();
            }
            [MethodImpl(MethodImplOptions.NoInlining)]
            private static ObjectPool<NJsonSerializer> EnsureMacroCaseDeserializerPoolCreated()
            {
                Interlocked.CompareExchange(ref _macroCaseDeserializerPool, GetJsonSerializerPool(MacroCaseDeserializerSettings), null);
                return _macroCaseDeserializerPool;
            }

            #endregion

            #region - KebabCase -

            private static ObjectPool<NJsonSerializer>? _kebabCaseSerializerPool;

            public static NJsonSerializerSettings KebabCaseSerializerSettings { get; }
            public static ObjectPool<NJsonSerializer> KebabCaseSerializerPool
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get => _kebabCaseSerializerPool ?? EnsureKebabCaseSerializerPoolCreated();
            }
            [MethodImpl(MethodImplOptions.NoInlining)]
            private static ObjectPool<NJsonSerializer> EnsureKebabCaseSerializerPoolCreated()
            {
                Interlocked.CompareExchange(ref _kebabCaseSerializerPool, GetJsonSerializerPool(KebabCaseSerializerSettings), null);
                return _kebabCaseSerializerPool;
            }

            private static ObjectPool<NJsonSerializer>? _kebabCaseDeserializerPool;

            public static NJsonSerializerSettings KebabCaseDeserializerSettings { get; }
            public static ObjectPool<NJsonSerializer> KebabCaseDeserializerPool
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get => _kebabCaseDeserializerPool ?? EnsureKebabCaseDeserializerPoolCreated();
            }
            [MethodImpl(MethodImplOptions.NoInlining)]
            private static ObjectPool<NJsonSerializer> EnsureKebabCaseDeserializerPoolCreated()
            {
                Interlocked.CompareExchange(ref _kebabCaseDeserializerPool, GetJsonSerializerPool(KebabCaseDeserializerSettings), null);
                return _kebabCaseDeserializerPool;
            }

            #endregion

            #region - TrainCase -

            private static ObjectPool<NJsonSerializer>? _trainCaseSerializerPool;

            public static NJsonSerializerSettings TrainCaseSerializerSettings { get; }
            public static ObjectPool<NJsonSerializer> TrainCaseSerializerPool
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get => _trainCaseSerializerPool ?? EnsureTrainCaseSerializerPoolCreated();
            }
            [MethodImpl(MethodImplOptions.NoInlining)]
            private static ObjectPool<NJsonSerializer> EnsureTrainCaseSerializerPoolCreated()
            {
                Interlocked.CompareExchange(ref _trainCaseSerializerPool, GetJsonSerializerPool(TrainCaseSerializerSettings), null);
                return _trainCaseSerializerPool;
            }

            private static ObjectPool<NJsonSerializer>? _trainCaseDeserializerPool;

            public static NJsonSerializerSettings TrainCaseDeserializerSettings { get; }
            public static ObjectPool<NJsonSerializer> TrainCaseDeserializerPool
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get => _trainCaseDeserializerPool ?? EnsureTrainCaseDeserializerPoolCreated();
            }
            [MethodImpl(MethodImplOptions.NoInlining)]
            private static ObjectPool<NJsonSerializer> EnsureTrainCaseDeserializerPoolCreated()
            {
                Interlocked.CompareExchange(ref _trainCaseDeserializerPool, GetJsonSerializerPool(TrainCaseDeserializerSettings), null);
                return _trainCaseDeserializerPool;
            }

            #endregion

            #region - CobolCase -

            private static ObjectPool<NJsonSerializer>? _cobolCaseSerializerPool;

            public static NJsonSerializerSettings CobolCaseSerializerSettings { get; }
            public static ObjectPool<NJsonSerializer> CobolCaseSerializerPool
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get => _cobolCaseSerializerPool ?? EnsureCobolCaseSerializerPoolCreated();
            }
            [MethodImpl(MethodImplOptions.NoInlining)]
            private static ObjectPool<NJsonSerializer> EnsureCobolCaseSerializerPoolCreated()
            {
                Interlocked.CompareExchange(ref _cobolCaseSerializerPool, GetJsonSerializerPool(CobolCaseSerializerSettings), null);
                return _cobolCaseSerializerPool;
            }

            private static ObjectPool<NJsonSerializer>? _cobolCaseDeserializerPool;

            public static NJsonSerializerSettings CobolCaseDeserializerSettings { get; }
            public static ObjectPool<NJsonSerializer> CobolCaseDeserializerPool
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get => _cobolCaseDeserializerPool ?? EnsureCobolCaseDeserializerPoolCreated();
            }
            [MethodImpl(MethodImplOptions.NoInlining)]
            private static ObjectPool<NJsonSerializer> EnsureCobolCaseDeserializerPoolCreated()
            {
                Interlocked.CompareExchange(ref _cobolCaseDeserializerPool, GetJsonSerializerPool(CobolCaseDeserializerSettings), null);
                return _cobolCaseDeserializerPool;
            }

            #endregion

            #endregion

            #region -- SerializeObject --

            /// <summary>Serializes the specified object to a JSON string using a type, formatting and <see cref="NJsonSerializer"/>.</summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static string SerializeObject(object? value, Type? type = null, JsonKnownNamingPolicy namingPolicy = JsonKnownNamingPolicy.Unspecified)
            {
                return GetSerializerPool(namingPolicy).SerializeObject(value, type);
            }

            /// <summary>Serializes the specified object to a JSON string using a type, formatting and <see cref="NJsonSerializer"/>.</summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static string SerializeObject(object? value, bool writeIndented, Type? type = null, JsonKnownNamingPolicy namingPolicy = JsonKnownNamingPolicy.Unspecified)
            {
                return GetSerializerPool(namingPolicy).SerializeObject(value, writeIndented, type);
            }

            #endregion

            #region -- DeserializeObject --

            /// <summary>Deserializes the JSON to the specified .NET type using <see cref="NJsonSerializer"/>.</summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static T? DeserializeObject<T>(string json, JsonKnownNamingPolicy namingPolicy = JsonKnownNamingPolicy.Unspecified)
            {
                return (T?)GetDeserializerPool(namingPolicy).DeserializeObject(json, typeof(T));
            }

            /// <summary>Deserializes the JSON to the specified .NET type using <see cref="NJsonSerializer"/>.</summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static object? DeserializeObject(string value, Type? type = null, JsonKnownNamingPolicy namingPolicy = JsonKnownNamingPolicy.Unspecified)
            {
                return GetDeserializerPool(namingPolicy).DeserializeObject(value, type);
            }

            #endregion

            #region -- Serialize to Byte-Array --

            /// <summary>Serializes the specified object to a JSON string using a type, formatting and <see cref="NJsonSerializer"/>.</summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static byte[] SerializeToByteArray(object? value, Type? type = null, JsonKnownNamingPolicy namingPolicy = JsonKnownNamingPolicy.Unspecified)
            {
                return GetSerializerPool(namingPolicy).SerializeToByteArray(value, type);
            }

            /// <summary>Serializes the specified object to a JSON string using a type, formatting and <see cref="NJsonSerializer"/>.</summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static byte[] SerializeToByteArray(object? value, bool writeIndented, Type? type = null, JsonKnownNamingPolicy namingPolicy = JsonKnownNamingPolicy.Unspecified)
            {
                return GetSerializerPool(namingPolicy).SerializeToByteArray(value, writeIndented, type);
            }

            #endregion

            #region -- Serialize ot Memory-Pool --

            /// <summary>Serializes the specified object to a JSON string using a type, formatting and <see cref="NJsonSerializer"/>.</summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static ArraySegment<byte> SerializeToMemoryPool(object? value, Type? type = null, JsonKnownNamingPolicy namingPolicy = JsonKnownNamingPolicy.Unspecified)
            {
                return GetSerializerPool(namingPolicy).SerializeToMemoryPool(value, type);
            }

            /// <summary>Serializes the specified object to a JSON string using a type, formatting and <see cref="NJsonSerializer"/>.</summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static ArraySegment<byte> SerializeToMemoryPool(object? value, bool writeIndented, Type? type = null, JsonKnownNamingPolicy namingPolicy = JsonKnownNamingPolicy.Unspecified)
            {
                return GetSerializerPool(namingPolicy).SerializeToMemoryPool(value, writeIndented, type);
            }

            #endregion

            #region -- Deserialize from Byte-Array --

            /// <summary>Deserializes the JSON to the specified .NET type using <see cref="NJsonSerializer"/>.</summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static T? DeserializeFromByteArray<T>(byte[] value, JsonKnownNamingPolicy namingPolicy = JsonKnownNamingPolicy.Unspecified)
            {
                return (T?)GetDeserializerPool(namingPolicy).DeserializeFromByteArray(value, typeof(T));
            }

            /// <summary>Deserializes the JSON to the specified .NET type using <see cref="NJsonSerializer"/>.</summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static T? DeserializeFromByteArray<T>(byte[] value, int offset, int count, JsonKnownNamingPolicy namingPolicy = JsonKnownNamingPolicy.Unspecified)
            {
                return (T?)GetDeserializerPool(namingPolicy).DeserializeFromByteArray(value, offset, count, typeof(T));
            }

            /// <summary>Deserializes the JSON to the specified .NET type using <see cref="NJsonSerializer"/>.</summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static object? DeserializeFromByteArray(byte[] value, Type? type = null, JsonKnownNamingPolicy namingPolicy = JsonKnownNamingPolicy.Unspecified)
            {
                return GetDeserializerPool(namingPolicy).DeserializeFromByteArray(value, type);
            }

            /// <summary>Deserializes the JSON to the specified .NET type using <see cref="NJsonSerializer"/>.</summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static object? DeserializeFromByteArray(byte[] value, int offset, int count, Type? type = null, JsonKnownNamingPolicy namingPolicy = JsonKnownNamingPolicy.Unspecified)
            {
                return GetDeserializerPool(namingPolicy).DeserializeFromByteArray(value, offset, count, type);
            }

            #endregion

            #region -- Serialize to Stream --

            /// <summary>Serializes the specified object to a JSON string using a type, formatting and <see cref="NJsonSerializer"/>.</summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void SerializeToStream(Stream outputStream, object? value, Type? type = null, JsonKnownNamingPolicy namingPolicy = JsonKnownNamingPolicy.Unspecified)
            {
                GetSerializerPool(namingPolicy).SerializeToStream(outputStream, value, type);
            }

            /// <summary>Serializes the specified object to a JSON string using a type, formatting and <see cref="NJsonSerializer"/>.</summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void SerializeToStream(Stream outputStream, object? value, bool writeIndented, Type? type = null, JsonKnownNamingPolicy namingPolicy = JsonKnownNamingPolicy.Unspecified)
            {
                GetSerializerPool(namingPolicy).SerializeToStream(outputStream, value, writeIndented, type);
            }

            #endregion

            #region -- Deserialize from Stream --

            /// <summary>Deserializes the JSON to the specified .NET type using <see cref="NJsonSerializer"/>.</summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static T? DeserializeFromStream<T>(Stream inputStream, JsonKnownNamingPolicy namingPolicy = JsonKnownNamingPolicy.Unspecified)
            {
                return (T?)GetDeserializerPool(namingPolicy).DeserializeFromStream(inputStream, typeof(T));
            }

            /// <summary>Deserializes the JSON to the specified .NET type using <see cref="NJsonSerializer"/>.</summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static object? DeserializeFromStream(Stream inputStream, Type? type = null, JsonKnownNamingPolicy namingPolicy = JsonKnownNamingPolicy.Unspecified)
            {
                return GetDeserializerPool(namingPolicy).DeserializeFromStream(inputStream, type);
            }

            #endregion

            #region -- Serialize to TextReader --

            /// <summary>Serializes the specified object to a JSON string using a type, formatting and <see cref="NJsonSerializer"/>.</summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void SerializeToWriter(TextWriter textWriter, object? value, Type? type = null, JsonKnownNamingPolicy namingPolicy = JsonKnownNamingPolicy.Unspecified)
            {
                GetSerializerPool(namingPolicy).SerializeToWriter(textWriter, value, type);
            }

            /// <summary>Serializes the specified object to a JSON string using a type, formatting and <see cref="NJsonSerializer"/>.</summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void SerializeToWriter(TextWriter textWriter, object? value, bool writeIndented, Type? type = null, JsonKnownNamingPolicy namingPolicy = JsonKnownNamingPolicy.Unspecified)
            {
                GetSerializerPool(namingPolicy).SerializeToWriter(textWriter, value, writeIndented, type);
            }

            #endregion

            #region -- Deserialize from TextReader --

            /// <summary>Deserializes the JSON to the specified .NET type using <see cref="NJsonSerializer"/>.</summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static T? DeserializeFromReader<T>(TextReader textReader, JsonKnownNamingPolicy namingPolicy = JsonKnownNamingPolicy.Unspecified)
            {
                return (T?)GetDeserializerPool(namingPolicy).DeserializeFromReader(textReader, typeof(T));
            }

            /// <summary>Deserializes the JSON to the specified .NET type using <see cref="NJsonSerializer"/>.</summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static object? DeserializeFromReader(TextReader textReader, Type? type = null, JsonKnownNamingPolicy namingPolicy = JsonKnownNamingPolicy.Unspecified)
            {
                return GetDeserializerPool(namingPolicy).DeserializeFromReader(textReader, type);
            }

            #endregion

            #region -- PopulateObject --

            /// <summary>Populates the object with values from the JSON string using <see cref="NJsonSerializer"/>.</summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void PopulateObject(object target, string value, JsonKnownNamingPolicy namingPolicy = JsonKnownNamingPolicy.Unspecified)
            {
                GetDeserializerPool(namingPolicy).PopulateObject(target, value);
            }

            /// <summary>Populates the object with values from the JSON string using <see cref="NJsonSerializer"/>.</summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void PopulateObject(object target, byte[] value, JsonKnownNamingPolicy namingPolicy = JsonKnownNamingPolicy.Unspecified)
            {
                GetDeserializerPool(namingPolicy).PopulateObject(target, value);
            }

            /// <summary>Populates the object with values from the JSON string using <see cref="NJsonSerializer"/>.</summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void PopulateObject(object target, byte[] value, int offset, int count, JsonKnownNamingPolicy namingPolicy = JsonKnownNamingPolicy.Unspecified)
            {
                GetDeserializerPool(namingPolicy).PopulateObject(target, value, offset, count);
            }

            #endregion

            #region ** GetSerializerPool **

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private static ObjectPool<NJsonSerializer> GetSerializerPool(JsonKnownNamingPolicy namingPolicy) => namingPolicy switch
            {
                JsonKnownNamingPolicy.CamelCase => CamelCaseSerializerPool,
                JsonKnownNamingPolicy.SnakeCase => SnakeCaseSerializerPool,
                JsonKnownNamingPolicy.AdaCase => AdaCaseSerializerPool,
                JsonKnownNamingPolicy.MacroCase => MacroCaseSerializerPool,
                JsonKnownNamingPolicy.KebabCase => KebabCaseSerializerPool,
                JsonKnownNamingPolicy.TrainCase => TrainCaseSerializerPool,
                JsonKnownNamingPolicy.CobolCase => CobolCaseSerializerPool,
                _ => DefaultSerializerPool,
            };

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private static ObjectPool<NJsonSerializer> GetDeserializerPool(JsonKnownNamingPolicy namingPolicy) => namingPolicy switch
            {
                JsonKnownNamingPolicy.CamelCase => CamelCaseDeserializerPool,
                JsonKnownNamingPolicy.SnakeCase => SnakeCaseDeserializerPool,
                JsonKnownNamingPolicy.AdaCase => AdaCaseDeserializerPool,
                JsonKnownNamingPolicy.MacroCase => MacroCaseDeserializerPool,
                JsonKnownNamingPolicy.KebabCase => KebabCaseDeserializerPool,
                JsonKnownNamingPolicy.TrainCase => TrainCaseDeserializerPool,
                JsonKnownNamingPolicy.CobolCase => CobolCaseDeserializerPool,
                _ => DefaultDeserializerPool,
            };

            #endregion
        }
    }
}
