﻿using System.Buffers;
using System.Reflection;
using System.Runtime.CompilerServices;
using CuteAnt.Reflection;
using NFormatting = Newtonsoft.Json.Formatting;
using NIJsonLineInfo = Newtonsoft.Json.IJsonLineInfo;
using NJsonConverter = Newtonsoft.Json.JsonConverter;
using NJsonReader = Newtonsoft.Json.JsonReader;
using NJsonSerializationException = Newtonsoft.Json.JsonSerializationException;
using NJsonSerializer = Newtonsoft.Json.JsonSerializer;
using NJsonSerializerSettings = Newtonsoft.Json.JsonSerializerSettings;

namespace SpanJson.Serialization
{
    static partial class JsonConvertX
    {
        #region @@ Constructors @@

        private static readonly FieldInfo s_checkAdditionalContentField;
        private static readonly MemberGetter<NJsonSerializer> s_checkAdditionalContentGetter;
        private static readonly MemberSetter<NJsonSerializer> s_checkAdditionalContentSetter;

        private static readonly FieldInfo s_formattingField;
        private static readonly MemberGetter<NJsonSerializer> s_formattingGetter;
        private static readonly MemberSetter<NJsonSerializer> s_formattingSetter;

        public static readonly Newtonsoft.Json.IArrayPool<char> GlobalCharacterArrayPool;

        public static readonly Newtonsoft.Json.Serialization.ISerializationBinder DefaultSerializationBinder;

        static JsonConvertX()
        {
            s_checkAdditionalContentField = typeof(NJsonSerializer).LookupTypeField("_checkAdditionalContent");
            s_checkAdditionalContentGetter = s_checkAdditionalContentField.GetValueGetter<NJsonSerializer>();
            s_checkAdditionalContentSetter = s_checkAdditionalContentField.GetValueSetter<NJsonSerializer>();

            s_formattingField = typeof(NJsonSerializer).LookupTypeField("_formatting");
            s_formattingGetter = s_formattingField.GetValueGetter<NJsonSerializer>();
            s_formattingSetter = s_formattingField.GetValueSetter<NJsonSerializer>();

            GlobalCharacterArrayPool = new JsonArrayPool<char>(ArrayPool<char>.Shared);

            DefaultSerializationBinder = JsonSerializationBinder.Instance;
        }

        #endregion

        #region -- NJsonSerializer.IsCheckAdditionalContentSetX --

        [MethodImpl(InlineMethod.AggressiveInlining)]
        public static bool IsCheckAdditionalContentSetX(this NJsonSerializer jsonSerializer)
        {
            if (jsonSerializer is null) { ThrowHelper.ThrowArgumentNullException(ExceptionArgument.jsonSerializer); }
            return s_checkAdditionalContentGetter(jsonSerializer) is not null;
        }

        [MethodImpl(InlineMethod.AggressiveInlining)]
        public static bool? GetCheckAdditionalContent(this NJsonSerializer jsonSerializer)
        {
            if (jsonSerializer is null) { ThrowHelper.ThrowArgumentNullException(ExceptionArgument.jsonSerializer); }
            return (bool?)s_checkAdditionalContentGetter(jsonSerializer);
        }

        [MethodImpl(InlineMethod.AggressiveInlining)]
        public static void SetCheckAdditionalContent(this NJsonSerializer jsonSerializer, bool? checkAdditionalContent = null)
        {
            if (jsonSerializer is null) { ThrowHelper.ThrowArgumentNullException(ExceptionArgument.jsonSerializer); }
            s_checkAdditionalContentSetter(jsonSerializer, checkAdditionalContent);
        }

        #endregion

        #region -- NJsonSerializer.Formatting --

        [MethodImpl(InlineMethod.AggressiveInlining)]
        public static NFormatting? GetFormatting(this NJsonSerializer jsonSerializer)
        {
            if (jsonSerializer is null) { ThrowHelper.ThrowArgumentNullException(ExceptionArgument.jsonSerializer); }
            return (NFormatting?)s_formattingGetter(jsonSerializer);
        }

        [MethodImpl(InlineMethod.AggressiveInlining)]
        public static void SetFormatting(this NJsonSerializer jsonSerializer, NFormatting? formatting = null)
        {
            if (jsonSerializer is null) { ThrowHelper.ThrowArgumentNullException(ExceptionArgument.jsonSerializer); }
            s_formattingSetter(jsonSerializer, formatting);
        }

        #endregion

        #region == CreateJsonSerializationException ==

        internal static NJsonSerializationException CreateJsonSerializationException(NJsonReader reader, string message)
        {
            return CreateJsonSerializationException(reader, message, null);
        }

        internal static NJsonSerializationException CreateJsonSerializationException(NJsonReader reader, string message, Exception? ex)
        {
            return CreateJsonSerializationException(reader as NIJsonLineInfo, reader.Path, message, ex);
        }

        internal static NJsonSerializationException CreateJsonSerializationException(NIJsonLineInfo? lineInfo, string path, string message, Exception? ex)
        {
            message = JsonPosition.FormatMessage(lineInfo, path, message);

            int lineNumber;
            int linePosition;
            if (lineInfo is not null && lineInfo.HasLineInfo())
            {
                lineNumber = lineInfo.LineNumber;
                linePosition = lineInfo.LinePosition;
            }
            else
            {
                lineNumber = 0;
                linePosition = 0;
            }

            return new NJsonSerializationException(message, path, lineNumber, linePosition, ex);
        }

        #endregion

        #region -- Serialize to Byte-Array --

        private const int c_initialBufferSize = 1024 * 64;

        /// <summary>Serializes the specified object to a JSON byte array.</summary>
        /// <param name="value">The object to serialize.</param>
        /// <param name="initialBufferSize">The initial buffer size.</param>
        /// <returns>A JSON string representation of the object.</returns>
        public static byte[] SerializeToByteArray(object value, int initialBufferSize = c_initialBufferSize)
        {
            return SerializeToByteArray(value, type: null, settings: null, initialBufferSize: initialBufferSize);
        }

        /// <summary>Serializes the specified object to a JSON byte array using formatting.</summary>
        /// <param name="value">The object to serialize.</param>
        /// <param name="formatting">Indicates how the output should be formatted.</param>
        /// <param name="initialBufferSize">The initial buffer size.</param>
        /// <returns>A JSON string representation of the object.</returns>
        public static byte[] SerializeToByteArray(object value, NFormatting formatting, int initialBufferSize = c_initialBufferSize)
        {
            return SerializeToByteArray(value, formatting, settings: null, initialBufferSize: initialBufferSize);
        }

        /// <summary>Serializes the specified object to a JSON byte array using a collection of <see cref="NJsonConverter"/>.</summary>
        /// <param name="value">The object to serialize.</param>
        /// <param name="converters">A collection of converters used while serializing.</param>
        /// <returns>A JSON string representation of the object.</returns>
        public static byte[] SerializeToByteArray(object? value, params NJsonConverter[] converters)
        {
            var settings = (converters is not null && (uint)converters.Length > 0u)
                ? new NJsonSerializerSettings { Converters = converters }
                : null;
            return SerializeToByteArray(value, null, settings);
        }

        /// <summary>Serializes the specified object to a JSON byte array using formatting and a collection of <see cref="NJsonConverter"/>.</summary>
        /// <param name="value">The object to serialize.</param>
        /// <param name="formatting">Indicates how the output should be formatted.</param>
        /// <param name="converters">A collection of converters used while serializing.</param>
        /// <returns>A JSON string representation of the object.</returns>
        public static byte[] SerializeToByteArray(object? value, NFormatting formatting, params NJsonConverter[] converters)
        {
            var settings = (converters is not null && (uint)converters.Length > 0u)
                ? new NJsonSerializerSettings { Converters = converters }
                : null;
            return SerializeToByteArray(value, null, formatting, settings);
        }

        /// <summary>Serializes the specified object to a JSON byte array using <see cref="NJsonSerializerSettings"/>.</summary>
        /// <param name="value">The object to serialize.</param>
        /// <param name="settings">The <see cref="NJsonSerializerSettings"/> used to serialize the object.
        /// If this is <c>null</c>, default serialization settings will be used.</param>
        /// <param name="initialBufferSize">The initial buffer size.</param>
        /// <returns>A JSON string representation of the object.</returns>
        public static byte[] SerializeToByteArray(object? value, NJsonSerializerSettings? settings, int initialBufferSize = c_initialBufferSize)
        {
            return SerializeToByteArray(value, type: null, settings: settings, initialBufferSize: initialBufferSize);
        }

        /// <summary>Serializes the specified object to a JSON byte array using a type, formatting and <see cref="NJsonSerializerSettings"/>.</summary>
        /// <param name="value">The object to serialize.</param>
        /// <param name="type">The type of the value being serialized.
        /// This parameter is used when <see cref="NJsonSerializer.TypeNameHandling"/> is <see cref="Newtonsoft.Json.TypeNameHandling.Auto"/> to write out the type name if the type of the value does not match.
        /// Specifying the type is optional.</param>
        /// <param name="settings">The <see cref="NJsonSerializerSettings"/> used to serialize the object.
        /// If this is <c>null</c>, default serialization settings will be used.</param>
        /// <param name="initialBufferSize">The initial buffer size.</param>
        /// <returns>A JSON string representation of the object.</returns>
        public static byte[] SerializeToByteArray(object? value, Type? type, NJsonSerializerSettings? settings, int initialBufferSize = c_initialBufferSize)
        {
            var jsonSerializer = NJsonSerializer.CreateDefault(settings);

            return jsonSerializer.SerializeToByteArray(value, type, initialBufferSize);
        }

        /// <summary>Serializes the specified object to a JSON byte array using formatting and <see cref="NJsonSerializerSettings"/>.</summary>
        /// <param name="value">The object to serialize.</param>
        /// <param name="formatting">Indicates how the output should be formatted.</param>
        /// <param name="settings">The <see cref="NJsonSerializerSettings"/> used to serialize the object.
        /// If this is <c>null</c>, default serialization settings will be used.</param>
        /// <param name="initialBufferSize">The initial buffer size.</param>
        /// <returns>A JSON string representation of the object.</returns>
        public static byte[] SerializeToByteArray(object? value, NFormatting formatting, NJsonSerializerSettings? settings, int initialBufferSize = c_initialBufferSize)
        {
            return SerializeToByteArray(value, null, formatting, settings, initialBufferSize);
        }

        /// <summary>Serializes the specified object to a JSON byte array using a type, formatting and <see cref="NJsonSerializerSettings"/>.</summary>
        /// <param name="value">The object to serialize.</param>
        /// <param name="type">The type of the value being serialized.
        /// This parameter is used when <see cref="NJsonSerializer.TypeNameHandling"/> is <see cref="Newtonsoft.Json.TypeNameHandling.Auto"/> to write out the type name if the type of the value does not match.
        /// Specifying the type is optional.</param>
        /// <param name="formatting">Indicates how the output should be formatted.</param>
        /// <param name="settings">The <see cref="NJsonSerializerSettings"/> used to serialize the object.
        /// If this is <c>null</c>, default serialization settings will be used.</param>
        /// <param name="initialBufferSize">The initial buffer size.</param>
        /// <returns>A JSON string representation of the object.</returns>
        public static byte[] SerializeToByteArray(object? value, Type? type, NFormatting formatting, NJsonSerializerSettings? settings, int initialBufferSize = c_initialBufferSize)
        {
            var jsonSerializer = NJsonSerializer.CreateDefault(settings);
            jsonSerializer.Formatting = formatting;

            return jsonSerializer.SerializeToByteArray(value, type, initialBufferSize);
        }

        #endregion

        #region -- Serialize to Memory-Pool --

        /// <summary>Serializes the specified object to a JSON byte array.</summary>
        /// <param name="value">The object to serialize.</param>
        /// <param name="initialBufferSize">The initial buffer size.</param>
        /// <returns>A JSON string representation of the object.</returns>
        public static ArraySegment<Byte> SerializeToMemoryPool(object? value, int initialBufferSize = c_initialBufferSize)
        {
            return SerializeToMemoryPool(value, type: null, settings: null, initialBufferSize);
        }

        /// <summary>Serializes the specified object to a JSON byte array using formatting.</summary>
        /// <param name="value">The object to serialize.</param>
        /// <param name="formatting">Indicates how the output should be formatted.</param>
        /// <param name="initialBufferSize">The initial buffer size.</param>
        /// <returns>A JSON string representation of the object.</returns>
        public static ArraySegment<Byte> SerializeToMemoryPool(object? value, NFormatting formatting, int initialBufferSize = c_initialBufferSize)
        {
            return SerializeToMemoryPool(value, formatting, settings: null, initialBufferSize);
        }

        /// <summary>Serializes the specified object to a JSON byte array using a collection of <see cref="NJsonConverter"/>.</summary>
        /// <param name="value">The object to serialize.</param>
        /// <param name="converters">A collection of converters used while serializing.</param>
        /// <returns>A JSON string representation of the object.</returns>
        public static ArraySegment<Byte> SerializeToMemoryPool(object? value, params NJsonConverter[] converters)
        {
            var settings = (converters is not null && (uint)converters.Length > 0u)
                ? new NJsonSerializerSettings { Converters = converters }
                : null;

            return SerializeToMemoryPool(value, null, settings);
        }

        /// <summary>Serializes the specified object to a JSON byte array using formatting and a collection of <see cref="NJsonConverter"/>.</summary>
        /// <param name="value">The object to serialize.</param>
        /// <param name="formatting">Indicates how the output should be formatted.</param>
        /// <param name="converters">A collection of converters used while serializing.</param>
        /// <returns>A JSON string representation of the object.</returns>
        public static ArraySegment<Byte> SerializeToMemoryPool(object? value, NFormatting formatting, params NJsonConverter[] converters)
        {
            var settings = (converters is not null && (uint)converters.Length > 0u)
                ? new NJsonSerializerSettings { Converters = converters }
                : null;
            return SerializeToMemoryPool(value, null, formatting, settings);
        }

        /// <summary>Serializes the specified object to a JSON byte array using <see cref="NJsonSerializerSettings"/>.</summary>
        /// <param name="value">The object to serialize.</param>
        /// <param name="settings">The <see cref="NJsonSerializerSettings"/> used to serialize the object.
        /// If this is <c>null</c>, default serialization settings will be used.</param>
        /// <param name="initialBufferSize">The initial buffer size.</param>
        /// <returns>A JSON string representation of the object.</returns>
        public static ArraySegment<Byte> SerializeToMemoryPool(object? value, NJsonSerializerSettings? settings, int initialBufferSize = c_initialBufferSize)
        {
            return SerializeToMemoryPool(value, type: null, settings: settings, initialBufferSize: initialBufferSize);
        }

        /// <summary>Serializes the specified object to a JSON byte array using a type, formatting and <see cref="NJsonSerializerSettings"/>.</summary>
        /// <param name="value">The object to serialize.</param>
        /// <param name="type">The type of the value being serialized.
        /// This parameter is used when <see cref="NJsonSerializer.TypeNameHandling"/> is <see cref="Newtonsoft.Json.TypeNameHandling.Auto"/> to write out the type name if the type of the value does not match.
        /// Specifying the type is optional.</param>
        /// <param name="settings">The <see cref="NJsonSerializerSettings"/> used to serialize the object.
        /// If this is <c>null</c>, default serialization settings will be used.</param>
        /// <param name="initialBufferSize">The initial buffer size.</param>
        /// <returns>A JSON string representation of the object.</returns>
        public static ArraySegment<Byte> SerializeToMemoryPool(object? value, Type? type, NJsonSerializerSettings? settings, int initialBufferSize = c_initialBufferSize)
        {
            var jsonSerializer = NJsonSerializer.CreateDefault(settings);

            return jsonSerializer.SerializeToMemoryPool(value, type, initialBufferSize);
        }

        /// <summary>Serializes the specified object to a JSON byte array using formatting and <see cref="NJsonSerializerSettings"/>.</summary>
        /// <param name="value">The object to serialize.</param>
        /// <param name="formatting">Indicates how the output should be formatted.</param>
        /// <param name="settings">The <see cref="NJsonSerializerSettings"/> used to serialize the object.
        /// If this is <c>null</c>, default serialization settings will be used.</param>
        /// <param name="initialBufferSize">The initial buffer size.</param>
        /// <returns>A JSON string representation of the object.</returns>
        public static ArraySegment<Byte> SerializeToMemoryPool(object? value, NFormatting formatting, NJsonSerializerSettings? settings, int initialBufferSize = c_initialBufferSize)
        {
            return SerializeToMemoryPool(value, null, formatting, settings, initialBufferSize);
        }

        /// <summary>Serializes the specified object to a JSON byte array using a type, formatting and <see cref="NJsonSerializerSettings"/>.</summary>
        /// <param name="value">The object to serialize.</param>
        /// <param name="type">The type of the value being serialized.
        /// This parameter is used when <see cref="NJsonSerializer.TypeNameHandling"/> is <see cref="Newtonsoft.Json.TypeNameHandling.Auto"/> to write out the type name if the type of the value does not match.
        /// Specifying the type is optional.</param>
        /// <param name="formatting">Indicates how the output should be formatted.</param>
        /// <param name="settings">The <see cref="NJsonSerializerSettings"/> used to serialize the object.
        /// If this is <c>null</c>, default serialization settings will be used.</param>
        /// <param name="initialBufferSize">The initial buffer size.</param>
        /// <returns>A JSON string representation of the object.</returns>
        public static ArraySegment<Byte> SerializeToMemoryPool(object? value, Type? type, NFormatting formatting, NJsonSerializerSettings? settings, int initialBufferSize = c_initialBufferSize)
        {
            var jsonSerializer = NJsonSerializer.CreateDefault(settings);
            jsonSerializer.Formatting = formatting;

            return jsonSerializer.SerializeToMemoryPool(value, type, initialBufferSize);
        }

        #endregion

        #region -- Deserialize from Byte-Array --

        /// <summary>Deserializes the JSON to a .NET object.</summary>
        /// <param name="bytes">The byte array containing the JSON data to read.</param>
        /// <returns>The deserialized object from the JSON string.</returns>
        public static object? DeserializeFromByteArray(byte[] bytes)
        {
            return DeserializeFromByteArray(bytes, type: null, settings: null);
        }

        /// <summary>Deserializes the JSON to a .NET object using <see cref="NJsonSerializerSettings"/>.</summary>
        /// <param name="bytes">The byte array containing the JSON data to read.</param>
        /// <param name="settings">The <see cref="NJsonSerializerSettings"/> used to deserialize the object.
        /// If this is <c>null</c>, default serialization settings will be used.</param>
        /// <returns>The deserialized object from the JSON string.</returns>
        public static object? DeserializeFromByteArray(byte[] bytes, NJsonSerializerSettings? settings)
        {
            return DeserializeFromByteArray(bytes, type: null, settings);
        }

        /// <summary>Deserializes the JSON to the specified .NET type.</summary>
        /// <param name="bytes">The byte array containing the JSON data to read.</param>
        /// <param name="type">The <see cref="Type"/> of object being deserialized.</param>
        /// <returns>The deserialized object from the JSON string.</returns>
        public static object? DeserializeFromByteArray(byte[] bytes, Type? type)
        {
            return DeserializeFromByteArray(bytes, type, settings: null);
        }

        /// <summary>Deserializes the JSON to the specified .NET type.</summary>
        /// <typeparam name="T">The type of the object to deserialize to.</typeparam>
        /// <param name="bytes">The byte array containing the JSON data to read.</param>
        /// <returns>The deserialized object from the JSON string.</returns>
        public static T? DeserializeFromByteArray<T>(byte[] bytes)
        {
            return DeserializeFromByteArray<T>(bytes, settings: null);
        }

        /// <summary>Deserializes the JSON to the specified .NET type using a collection of <see cref="NJsonConverter"/>.</summary>
        /// <typeparam name="T">The type of the object to deserialize to.</typeparam>
        /// <param name="bytes">The byte array containing the JSON data to read.</param>
        /// <param name="converters">Converters to use while deserializing.</param>
        /// <returns>The deserialized object from the JSON string.</returns>
        public static T? DeserializeFromByteArray<T>(byte[] bytes, params NJsonConverter[] converters)
        {
            return (T?)DeserializeFromByteArray(bytes, typeof(T), converters);
        }

        /// <summary>Deserializes the JSON to the specified .NET type using <see cref="NJsonSerializerSettings"/>.</summary>
        /// <typeparam name="T">The type of the object to deserialize to.</typeparam>
        /// <param name="bytes">The byte array containing the JSON data to read.</param>
        /// <param name="settings">The <see cref="NJsonSerializerSettings"/> used to deserialize the object.
        /// If this is <c>null</c>, default serialization settings will be used.</param>
        /// <returns>The deserialized object from the JSON string.</returns>
        public static T? DeserializeFromByteArray<T>(byte[] bytes, NJsonSerializerSettings? settings)
        {
            return (T?)DeserializeFromByteArray(bytes, typeof(T), settings);
        }

        /// <summary>Deserializes the JSON to the specified .NET type using a collection of <see cref="NJsonConverter"/>.</summary>
        /// <param name="bytes">The byte array containing the JSON data to read.</param>
        /// <param name="type">The type of the object to deserialize.</param>
        /// <param name="converters">Converters to use while deserializing.</param>
        /// <returns>The deserialized object from the JSON string.</returns>
        public static object? DeserializeFromByteArray(byte[] bytes, Type? type, params NJsonConverter[] converters)
        {
            var settings = (converters is not null && (uint)converters.Length > 0u)
                ? new NJsonSerializerSettings { Converters = converters }
                : null;
            return DeserializeFromByteArray(bytes, type, settings);
        }

        /// <summary>Deserializes the JSON to the specified .NET type using <see cref="NJsonSerializerSettings"/>.</summary>
        /// <param name="bytes">The byte array containing the JSON data to read.</param>
        /// <param name="type">The type of the object to deserialize to.</param>
        /// <param name="settings">
        /// The <see cref="NJsonSerializerSettings"/> used to deserialize the object.
        /// If this is <c>null</c>, default serialization settings will be used.
        /// </param>
        /// <returns>The deserialized object from the JSON string.</returns>
        public static object? DeserializeFromByteArray(byte[] bytes, Type? type, NJsonSerializerSettings? settings)
        {
            var jsonSerializer = NJsonSerializer.CreateDefault(settings);

            return jsonSerializer.DeserializeFromByteArray(bytes, type);
        }


        /// <summary>Deserializes the JSON to a .NET object.</summary>
        /// <param name="bytes">The byte array containing the JSON data to read.</param>
        /// <param name="index">The index of the first byte to deserialize.</param>
        /// <param name="count">The number of bytes to deserialize.</param>
        /// <returns>The deserialized object from the JSON string.</returns>
        public static object? DeserializeFromByteArray(byte[] bytes, int index, int count)
        {
            return DeserializeFromByteArray(bytes, index, count, null, settings: null);
        }

        /// <summary>Deserializes the JSON to a .NET object using <see cref="NJsonSerializerSettings"/>.</summary>
        /// <param name="bytes">The byte array containing the JSON data to read.</param>
        /// <param name="index">The index of the first byte to deserialize.</param>
        /// <param name="count">The number of bytes to deserialize.</param>
        /// <param name="settings">The <see cref="NJsonSerializerSettings"/> used to deserialize the object.
        /// If this is <c>null</c>, default serialization settings will be used.</param>
        /// <returns>The deserialized object from the JSON string.</returns>
        public static object? DeserializeFromByteArray(byte[] bytes, int index, int count, NJsonSerializerSettings? settings)
        {
            return DeserializeFromByteArray(bytes, index, count, null, settings);
        }

        /// <summary>Deserializes the JSON to the specified .NET type.</summary>
        /// <param name="bytes">The byte array containing the JSON data to read.</param>
        /// <param name="index">The index of the first byte to deserialize.</param>
        /// <param name="count">The number of bytes to deserialize.</param>
        /// <param name="type">The <see cref="Type"/> of object being deserialized.</param>
        /// <returns>The deserialized object from the JSON string.</returns>
        public static object? DeserializeFromByteArray(byte[] bytes, int index, int count, Type? type)
        {
            return DeserializeFromByteArray(bytes, index, count, type, settings: null);
        }

        /// <summary>Deserializes the JSON to the specified .NET type.</summary>
        /// <typeparam name="T">The type of the object to deserialize to.</typeparam>
        /// <param name="bytes">The byte array containing the JSON data to read.</param>
        /// <param name="index">The index of the first byte to deserialize.</param>
        /// <param name="count">The number of bytes to deserialize.</param>
        /// <returns>The deserialized object from the JSON string.</returns>
        public static T? DeserializeFromByteArray<T>(byte[] bytes, int index, int count)
        {
            return DeserializeFromByteArray<T>(bytes, index, count, settings: null);
        }

        /// <summary>Deserializes the JSON to the specified .NET type using a collection of <see cref="NJsonConverter"/>.</summary>
        /// <typeparam name="T">The type of the object to deserialize to.</typeparam>
        /// <param name="bytes">The byte array containing the JSON data to read.</param>
        /// <param name="index">The index of the first byte to deserialize.</param>
        /// <param name="count">The number of bytes to deserialize.</param>
        /// <param name="converters">Converters to use while deserializing.</param>
        /// <returns>The deserialized object from the JSON string.</returns>
        public static T? DeserializeFromByteArray<T>(byte[] bytes, int index, int count, params NJsonConverter[] converters)
        {
            return (T?)DeserializeFromByteArray(bytes, index, count, typeof(T), converters);
        }

        /// <summary>Deserializes the JSON to the specified .NET type using <see cref="NJsonSerializerSettings"/>.</summary>
        /// <typeparam name="T">The type of the object to deserialize to.</typeparam>
        /// <param name="bytes">The byte array containing the JSON data to read.</param>
        /// <param name="index">The index of the first byte to deserialize.</param>
        /// <param name="count">The number of bytes to deserialize.</param>
        /// <param name="settings">The <see cref="NJsonSerializerSettings"/> used to deserialize the object.
        /// If this is <c>null</c>, default serialization settings will be used.</param>
        /// <returns>The deserialized object from the JSON string.</returns>
        public static T? DeserializeFromByteArray<T>(byte[] bytes, int index, int count, NJsonSerializerSettings? settings)
        {
            return (T?)DeserializeFromByteArray(bytes, index, count, typeof(T), settings);
        }

        /// <summary>Deserializes the JSON to the specified .NET type using a collection of <see cref="NJsonConverter"/>.</summary>
        /// <param name="bytes">The byte array containing the JSON data to read.</param>
        /// <param name="index">The index of the first byte to deserialize.</param>
        /// <param name="count">The number of bytes to deserialize.</param>
        /// <param name="type">The type of the object to deserialize.</param>
        /// <param name="converters">Converters to use while deserializing.</param>
        /// <returns>The deserialized object from the JSON string.</returns>
        public static object? DeserializeFromByteArray(byte[] bytes, int index, int count, Type? type, params NJsonConverter[] converters)
        {
            var settings = (converters is not null && (uint)converters.Length > 0u)
                ? new NJsonSerializerSettings { Converters = converters }
                : null;
            return DeserializeFromByteArray(bytes, index, count, type, settings);
        }

        /// <summary>Deserializes the JSON to the specified .NET type using <see cref="NJsonSerializerSettings"/>.</summary>
        /// <param name="bytes">The byte array containing the JSON data to read.</param>
        /// <param name="index">The index of the first byte to deserialize.</param>
        /// <param name="count">The number of bytes to deserialize.</param>
        /// <param name="type">The type of the object to deserialize to.</param>
        /// <param name="settings">
        /// The <see cref="NJsonSerializerSettings"/> used to deserialize the object.
        /// If this is <c>null</c>, default serialization settings will be used.
        /// </param>
        /// <returns>The deserialized object from the JSON string.</returns>
        public static object? DeserializeFromByteArray(byte[] bytes, int index, int count, Type? type, NJsonSerializerSettings? settings)
        {
            var jsonSerializer = NJsonSerializer.CreateDefault(settings);

            return jsonSerializer.DeserializeFromByteArray(bytes, index, count, type);
        }

        #endregion

        #region -- Serialize to Stream --

        /// <summary>Serializes the specified object to a <see cref="Stream"/>.</summary>
        /// <param name="stream">The <see cref="Stream"/> to write to.</param>
        /// <param name="value">The object to serialize.</param>
        public static void SerializeToStream(Stream stream, object value)
        {
            SerializeToStream(stream, value, type: null, settings: null);
        }

        /// <summary>Serializes the specified object to a <see cref="Stream"/> using formatting.</summary>
        /// <param name="stream">The <see cref="Stream"/> to write to.</param>
        /// <param name="value">The object to serialize.</param>
        /// <param name="formatting">Indicates how the output should be formatted.</param>
        public static void SerializeToStream(Stream stream, object? value, NFormatting formatting)
        {
            SerializeToStream(stream, value, formatting, settings: null);
        }

        /// <summary>Serializes the specified object to a <see cref="Stream"/> using a collection of <see cref="NJsonConverter"/>.</summary>
        /// <param name="stream">The <see cref="Stream"/> to write to.</param>
        /// <param name="value">The object to serialize.</param>
        /// <param name="converters">A collection of converters used while serializing.</param>
        public static void SerializeToStream(Stream stream, object? value, params NJsonConverter[] converters)
        {
            var settings = (converters is not null && (uint)converters.Length > 0u)
                ? new NJsonSerializerSettings { Converters = converters }
                : null;
            SerializeToStream(stream, value, type: null, settings);
        }

        /// <summary>Serializes the specified object to a <see cref="Stream"/> using formatting and a collection of <see cref="NJsonConverter"/>.</summary>
        /// <param name="stream">The <see cref="Stream"/> to write to.</param>
        /// <param name="value">The object to serialize.</param>
        /// <param name="formatting">Indicates how the output should be formatted.</param>
        /// <param name="converters">A collection of converters used while serializing.</param>
        public static void SerializeToStream(Stream stream, object? value, NFormatting formatting, params NJsonConverter[] converters)
        {
            var settings = (converters is not null && (uint)converters.Length > 0u)
                ? new NJsonSerializerSettings { Converters = converters }
                : null;
            SerializeToStream(stream, value, type: null, formatting, settings);
        }

        /// <summary>Serializes the specified object to a <see cref="Stream"/> using <see cref="NJsonSerializerSettings"/>.</summary>
        /// <param name="stream">The <see cref="Stream"/> to write to.</param>
        /// <param name="value">The object to serialize.</param>
        /// <param name="settings">The <see cref="NJsonSerializerSettings"/> used to serialize the object.
        /// If this is <c>null</c>, default serialization settings will be used.</param>
        public static void SerializeToStream(Stream stream, object? value, NJsonSerializerSettings? settings)
        {
            SerializeToStream(stream, value, type: null, settings);
        }

        /// <summary>Serializes the specified object to a <see cref="Stream"/> using a type, formatting and <see cref="NJsonSerializerSettings"/>.</summary>
        /// <param name="stream">The <see cref="Stream"/> to write to.</param>
        /// <param name="value">The object to serialize.</param>
        /// <param name="type">The type of the value being serialized.
        /// This parameter is used when <see cref="NJsonSerializer.TypeNameHandling"/> is <see cref="Newtonsoft.Json.TypeNameHandling.Auto"/> to write out the type name if the type of the value does not match.
        /// Specifying the type is optional.</param>
        /// <param name="settings">The <see cref="NJsonSerializerSettings"/> used to serialize the object.
        /// If this is <c>null</c>, default serialization settings will be used.</param>
        public static void SerializeToStream(Stream stream, object? value, Type? type, NJsonSerializerSettings? settings)
        {
            var jsonSerializer = NJsonSerializer.CreateDefault(settings);

            jsonSerializer.SerializeToStream(stream, value, type);
        }

        /// <summary>Serializes the specified object to a <see cref="Stream"/> using formatting and <see cref="NJsonSerializerSettings"/>.</summary>
        /// <param name="stream">The <see cref="Stream"/> to write to.</param>
        /// <param name="value">The object to serialize.</param>
        /// <param name="formatting">Indicates how the output should be formatted.</param>
        /// <param name="settings">The <see cref="NJsonSerializerSettings"/> used to serialize the object.
        /// If this is <c>null</c>, default serialization settings will be used.</param>
        public static void SerializeToStream(Stream stream, object? value, NFormatting formatting, NJsonSerializerSettings? settings)
        {
            SerializeToStream(stream, value, type: null, formatting, settings);
        }

        /// <summary>Serializes the specified object to a <see cref="Stream"/> using a type, formatting and <see cref="NJsonSerializerSettings"/>.</summary>
        /// <param name="stream">The <see cref="Stream"/> to write to.</param>
        /// <param name="value">The object to serialize.</param>
        /// <param name="type">The type of the value being serialized.
        /// This parameter is used when <see cref="NJsonSerializer.TypeNameHandling"/> is <see cref="Newtonsoft.Json.TypeNameHandling.Auto"/> to write out the type name if the type of the value does not match.
        /// Specifying the type is optional.</param>
        /// <param name="formatting">Indicates how the output should be formatted.</param>
        /// <param name="settings">The <see cref="NJsonSerializerSettings"/> used to serialize the object.
        /// If this is <c>null</c>, default serialization settings will be used.</param>
        public static void SerializeToStream(Stream stream, object? value, Type? type, NFormatting formatting, NJsonSerializerSettings? settings)
        {
            var jsonSerializer = NJsonSerializer.CreateDefault(settings);
            jsonSerializer.Formatting = formatting;

            jsonSerializer.SerializeToStream(stream, value, type);
        }

        #endregion

        #region -- Deserialize from Stream --

        /// <summary>Deserializes the JSON to a .NET object.</summary>
        /// <param name="stream">The <see cref="Stream"/> containing the JSON data to read.</param>
        /// <returns>The deserialized object from the JSON string.</returns>
        public static object? DeserializeFromStream(Stream stream)
        {
            return DeserializeFromStream(stream, type: null, settings: null);
        }

        /// <summary>Deserializes the JSON to a .NET object using <see cref="NJsonSerializerSettings"/>.</summary>
        /// <param name="stream">The <see cref="Stream"/> containing the JSON data to read.</param>
        /// <param name="settings">The <see cref="NJsonSerializerSettings"/> used to deserialize the object.
        /// If this is <c>null</c>, default serialization settings will be used.</param>
        /// <returns>The deserialized object from the JSON string.</returns>
        public static object? DeserializeFromStream(Stream stream, NJsonSerializerSettings? settings)
        {
            return DeserializeFromStream(stream, type: null, settings);
        }

        /// <summary>Deserializes the JSON to the specified .NET type.</summary>
        /// <param name="stream">The <see cref="Stream"/> containing the JSON data to read.</param>
        /// <param name="type">The <see cref="Type"/> of object being deserialized.</param>
        /// <returns>The deserialized object from the JSON string.</returns>
        public static object? DeserializeFromStream(Stream stream, Type? type)
        {
            return DeserializeFromStream(stream, type, settings: null);
        }

        /// <summary>Deserializes the JSON to the specified .NET type.</summary>
        /// <typeparam name="T">The type of the object to deserialize to.</typeparam>
        /// <param name="stream">The <see cref="Stream"/> containing the JSON data to read.</param>
        /// <returns>The deserialized object from the JSON string.</returns>
        public static T? DeserializeFromStream<T>(Stream stream)
        {
            return DeserializeFromStream<T>(stream, settings: null);
        }

        /// <summary>Deserializes the JSON to the specified .NET type using a collection of <see cref="NJsonConverter"/>.</summary>
        /// <typeparam name="T">The type of the object to deserialize to.</typeparam>
        /// <param name="stream">The <see cref="Stream"/> containing the JSON data to read.</param>
        /// <param name="converters">Converters to use while deserializing.</param>
        /// <returns>The deserialized object from the JSON string.</returns>
        public static T? DeserializeFromStream<T>(Stream stream, params NJsonConverter[] converters)
        {
            return (T?)DeserializeFromStream(stream, typeof(T), converters);
        }

        /// <summary>Deserializes the JSON to the specified .NET type using <see cref="NJsonSerializerSettings"/>.</summary>
        /// <typeparam name="T">The type of the object to deserialize to.</typeparam>
        /// <param name="stream">The <see cref="Stream"/> containing the JSON data to read.</param>
        /// <param name="settings">The <see cref="NJsonSerializerSettings"/> used to deserialize the object.
        /// If this is <c>null</c>, default serialization settings will be used.</param>
        /// <returns>The deserialized object from the JSON string.</returns>
        public static T? DeserializeFromStream<T>(Stream stream, NJsonSerializerSettings? settings)
        {
            return (T?)DeserializeFromStream(stream, typeof(T), settings);
        }

        /// <summary>Deserializes the JSON to the specified .NET type using a collection of <see cref="NJsonConverter"/>.</summary>
        /// <param name="stream">The <see cref="Stream"/> containing the JSON data to read.</param>
        /// <param name="type">The type of the object to deserialize.</param>
        /// <param name="converters">Converters to use while deserializing.</param>
        /// <returns>The deserialized object from the JSON string.</returns>
        public static object? DeserializeFromStream(Stream stream, Type? type, params NJsonConverter[] converters)
        {
            var settings = (converters is not null && (uint)converters.Length > 0u)
                ? new NJsonSerializerSettings { Converters = converters }
                : null;
            return DeserializeFromStream(stream, type, settings);
        }

        /// <summary>Deserializes the JSON to the specified .NET type using <see cref="NJsonSerializerSettings"/>.</summary>
        /// <param name="stream">The <see cref="Stream"/> containing the JSON data to read.</param>
        /// <param name="type">The type of the object to deserialize to.</param>
        /// <param name="settings">
        /// The <see cref="NJsonSerializerSettings"/> used to deserialize the object.
        /// If this is <c>null</c>, default serialization settings will be used.
        /// </param>
        /// <returns>The deserialized object from the JSON string.</returns>
        public static object? DeserializeFromStream(Stream stream, Type? type, NJsonSerializerSettings? settings)
        {
            var jsonSerializer = NJsonSerializer.CreateDefault(settings);
            return jsonSerializer.DeserializeFromStream(stream, type);
        }

        #endregion

        #region -- Serialize to TextWriter --

        /// <summary>Serializes the specified object to a <see cref="TextWriter"/>.</summary>
        /// <param name="textWriter">The <see cref="TextWriter"/> to write to.</param>
        /// <param name="value">The object to serialize.</param>
        public static void SerializeToWriter(TextWriter textWriter, object? value)
        {
            SerializeToWriter(textWriter, value, type: null, settings: null);
        }

        /// <summary>Serializes the specified object to a <see cref="TextWriter"/> using formatting.</summary>
        /// <param name="textWriter">The <see cref="TextWriter"/> to write to.</param>
        /// <param name="value">The object to serialize.</param>
        /// <param name="formatting">Indicates how the output should be formatted.</param>
        public static void SerializeToWriter(TextWriter textWriter, object? value, NFormatting formatting)
        {
            SerializeToWriter(textWriter, value, formatting, settings: null);
        }

        /// <summary>Serializes the specified object to a <see cref="TextWriter"/> using a collection of <see cref="NJsonConverter"/>.</summary>
        /// <param name="textWriter">The <see cref="TextWriter"/> to write to.</param>
        /// <param name="value">The object to serialize.</param>
        /// <param name="converters">A collection of converters used while serializing.</param>
        public static void SerializeToWriter(TextWriter textWriter, object? value, params NJsonConverter[] converters)
        {
            var settings = (converters is not null && (uint)converters.Length > 0u)
                ? new NJsonSerializerSettings { Converters = converters }
                : null;
            SerializeToWriter(textWriter, value, type: null, settings);
        }

        /// <summary>Serializes the specified object to a <see cref="TextWriter"/> using formatting and a collection of <see cref="NJsonConverter"/>.</summary>
        /// <param name="textWriter">The <see cref="TextWriter"/> to write to.</param>
        /// <param name="value">The object to serialize.</param>
        /// <param name="formatting">Indicates how the output should be formatted.</param>
        /// <param name="converters">A collection of converters used while serializing.</param>
        public static void SerializeToWriter(TextWriter textWriter, object? value, NFormatting formatting, params NJsonConverter[] converters)
        {
            var settings = (converters is not null && (uint)converters.Length > 0u)
                ? new NJsonSerializerSettings { Converters = converters }
                : null;
            SerializeToWriter(textWriter, value, type: null, formatting, settings);
        }

        /// <summary>Serializes the specified object to a <see cref="TextWriter"/> using <see cref="NJsonSerializerSettings"/>.</summary>
        /// <param name="textWriter">The <see cref="TextWriter"/> to write to.</param>
        /// <param name="value">The object to serialize.</param>
        /// <param name="settings">The <see cref="NJsonSerializerSettings"/> used to serialize the object.
        /// If this is <c>null</c>, default serialization settings will be used.</param>
        public static void SerializeToWriter(TextWriter textWriter, object? value, NJsonSerializerSettings? settings)
        {
            SerializeToWriter(textWriter, value, type: null, settings);
        }

        /// <summary>Serializes the specified object to a JSON byte array using a type, formatting and <see cref="NJsonSerializerSettings"/>.</summary>
        /// <param name="textWriter">The <see cref="TextWriter"/> to write to.</param>
        /// <param name="value">The object to serialize.</param>
        /// <param name="type">The type of the value being serialized.
        /// This parameter is used when <see cref="NJsonSerializer.TypeNameHandling"/> is <see cref="Newtonsoft.Json.TypeNameHandling.Auto"/> to write out the type name if the type of the value does not match.
        /// Specifying the type is optional.</param>
        /// <param name="settings">The <see cref="NJsonSerializerSettings"/> used to serialize the object.
        /// If this is <c>null</c>, default serialization settings will be used.</param>
        public static void SerializeToWriter(TextWriter textWriter, object? value, Type? type, NJsonSerializerSettings? settings)
        {
            var jsonSerializer = NJsonSerializer.CreateDefault(settings);

            jsonSerializer.SerializeToWriter(textWriter, value, type);
        }

        /// <summary>Serializes the specified object to a <see cref="TextWriter"/> using formatting and <see cref="NJsonSerializerSettings"/>.</summary>
        /// <param name="textWriter">The <see cref="TextWriter"/> to write to.</param>
        /// <param name="value">The object to serialize.</param>
        /// <param name="formatting">Indicates how the output should be formatted.</param>
        /// <param name="settings">The <see cref="NJsonSerializerSettings"/> used to serialize the object.
        /// If this is <c>null</c>, default serialization settings will be used.</param>
        public static void SerializeToWriter(TextWriter textWriter, object? value, NFormatting formatting, NJsonSerializerSettings? settings)
        {
            SerializeToWriter(textWriter, value, type: null, formatting, settings);
        }

        /// <summary>Serializes the specified object to a <see cref="TextWriter"/> using a type, formatting and <see cref="NJsonSerializerSettings"/>.</summary>
        /// <param name="textWriter">The <see cref="TextWriter"/> to write to.</param>
        /// <param name="value">The object to serialize.</param>
        /// <param name="type">The type of the value being serialized.
        /// This parameter is used when <see cref="NJsonSerializer.TypeNameHandling"/> is <see cref="Newtonsoft.Json.TypeNameHandling.Auto"/> to write out the type name if the type of the value does not match.
        /// Specifying the type is optional.</param>
        /// <param name="formatting">Indicates how the output should be formatted.</param>
        /// <param name="settings">The <see cref="NJsonSerializerSettings"/> used to serialize the object.
        /// If this is <c>null</c>, default serialization settings will be used.</param>
        public static void SerializeToWriter(TextWriter textWriter, object? value, Type? type, NFormatting formatting, NJsonSerializerSettings? settings)
        {
            var jsonSerializer = NJsonSerializer.CreateDefault(settings);
            jsonSerializer.Formatting = formatting;

            jsonSerializer.SerializeToWriter(textWriter, value, type);
        }

        #endregion

        #region -- Deserialize from TextReader --

        /// <summary>Deserializes the JSON to a .NET object.</summary>
        /// <param name="reader">The <see cref="TextReader"/> containing the JSON data to read.</param>
        /// <returns>The deserialized object from the JSON string.</returns>
        public static object? DeserializeFromReader(TextReader reader)
        {
            return DeserializeFromReader(reader, type: null, settings: null);
        }

        /// <summary>Deserializes the JSON to a .NET object using <see cref="NJsonSerializerSettings"/>.</summary>
        /// <param name="reader">The <see cref="TextReader"/> containing the JSON data to read.</param>
        /// <param name="settings">The <see cref="NJsonSerializerSettings"/> used to deserialize the object.
        /// If this is <c>null</c>, default serialization settings will be used.</param>
        /// <returns>The deserialized object from the JSON string.</returns>
        public static object? DeserializeFromReader(TextReader reader, NJsonSerializerSettings? settings)
        {
            return DeserializeFromReader(reader, type: null, settings);
        }

        /// <summary>Deserializes the JSON to the specified .NET type.</summary>
        /// <param name="reader">The <see cref="TextReader"/> containing the JSON data to read.</param>
        /// <param name="type">The <see cref="Type"/> of object being deserialized.</param>
        /// <returns>The deserialized object from the JSON string.</returns>
        public static object? DeserializeFromReader(TextReader reader, Type? type)
        {
            return DeserializeFromReader(reader, type, settings: null);
        }

        /// <summary>Deserializes the JSON to the specified .NET type.</summary>
        /// <typeparam name="T">The type of the object to deserialize to.</typeparam>
        /// <param name="reader">The <see cref="TextReader"/> containing the JSON data to read.</param>
        /// <returns>The deserialized object from the JSON string.</returns>
        public static T? DeserializeFromReader<T>(TextReader reader)
        {
            return DeserializeFromReader<T>(reader, settings: null);
        }

        /// <summary>Deserializes the JSON to the specified .NET type using a collection of <see cref="NJsonConverter"/>.</summary>
        /// <typeparam name="T">The type of the object to deserialize to.</typeparam>
        /// <param name="reader">The <see cref="TextReader"/> containing the JSON data to read.</param>
        /// <param name="converters">Converters to use while deserializing.</param>
        /// <returns>The deserialized object from the JSON string.</returns>
        public static T? DeserializeFromReader<T>(TextReader reader, params NJsonConverter[] converters)
        {
            return (T?)DeserializeFromReader(reader, typeof(T), converters);
        }

        /// <summary>Deserializes the JSON to the specified .NET type using <see cref="NJsonSerializerSettings"/>.</summary>
        /// <typeparam name="T">The type of the object to deserialize to.</typeparam>
        /// <param name="reader">The <see cref="TextReader"/> containing the JSON data to read.</param>
        /// <param name="settings">The <see cref="NJsonSerializerSettings"/> used to deserialize the object.
        /// If this is <c>null</c>, default serialization settings will be used.</param>
        /// <returns>The deserialized object from the JSON string.</returns>
        public static T? DeserializeFromReader<T>(TextReader reader, NJsonSerializerSettings? settings)
        {
            return (T?)DeserializeFromReader(reader, typeof(T), settings);
        }

        /// <summary>Deserializes the JSON to the specified .NET type using a collection of <see cref="NJsonConverter"/>.</summary>
        /// <param name="reader">The <see cref="TextReader"/> containing the JSON data to read.</param>
        /// <param name="type">The type of the object to deserialize.</param>
        /// <param name="converters">Converters to use while deserializing.</param>
        /// <returns>The deserialized object from the JSON string.</returns>
        public static object? DeserializeFromReader(TextReader reader, Type? type, params NJsonConverter[] converters)
        {
            var settings = (converters is not null && (uint)converters.Length > 0u)
                ? new NJsonSerializerSettings { Converters = converters }
                : null;
            return DeserializeFromReader(reader, type, settings);
        }

        /// <summary>Deserializes the JSON to the specified .NET type using <see cref="NJsonSerializerSettings"/>.</summary>
        /// <param name="reader">The <see cref="TextReader"/> containing the JSON data to read.</param>
        /// <param name="type">The type of the object to deserialize to.</param>
        /// <param name="settings">
        /// The <see cref="NJsonSerializerSettings"/> used to deserialize the object.
        /// If this is <c>null</c>, default serialization settings will be used.
        /// </param>
        /// <returns>The deserialized object from the JSON string.</returns>
        public static object? DeserializeFromReader(TextReader reader, Type? type, NJsonSerializerSettings? settings)
        {
            var jsonSerializer = NJsonSerializer.CreateDefault(settings);
            return jsonSerializer.DeserializeFromReader(reader, type);
        }

        #endregion
    }
}
