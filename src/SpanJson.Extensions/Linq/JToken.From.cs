﻿using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using SpanJson.Document;
using SpanJson.Dynamic;
using SpanJson.Resolvers;
using SpanJson.Serialization;
using NJsonSerializer = Newtonsoft.Json.JsonSerializer;

namespace SpanJson.Linq
{
    partial class JToken
    {
        #region -- From --

        /// <summary>Creates a <see cref="JToken"/> from an object.</summary>
        /// <param name="input">The object that will be used to create <see cref="JToken"/>.</param>
        /// <returns>A <see cref="JToken"/> with the value of the specified object.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static JToken From<T>(T input)
        {
            return From<T, IncludeNullsOriginalCaseResolver<char>>(input);
        }

        /// <summary>Creates a <see cref="JToken"/> from an object.</summary>
        /// <param name="input">The object that will be used to create <see cref="JToken"/>.</param>
        /// <returns>A <see cref="JToken"/> with the value of the specified object.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static JToken From<T, TResolver>(T input)
            where TResolver : IJsonFormatterResolver<char, TResolver>, new()
        {
            if (TryReadJsonDynamic(input, out var token)) { return token; }

            if (input is null) { ThrowHelper.ThrowArgumentNullException(ExceptionArgument.input); }

            var utf16Json = JsonSerializer.Generic.Utf16.SerializeToCharArray<T, TResolver>(input);
            return Parse(utf16Json);
        }

        #endregion

        #region -- FromDynamic --

        /// <summary>Creates a <see cref="JToken"/> from an object.</summary>
        /// <param name="o">The object that will be used to create <see cref="JToken"/>.</param>
        /// <returns>A <see cref="JToken"/> with the value of the specified object.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static JToken FromDynamic(object o)
        {
            return FromDynamic<IncludeNullsOriginalCaseResolver<char>>(o);
        }

        /// <summary>Creates a <see cref="JToken"/> from an object.</summary>
        /// <param name="o">The object that will be used to create <see cref="JToken"/>.</param>
        /// <returns>A <see cref="JToken"/> with the value of the specified object.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static JToken FromDynamic<TResolver>(object o)
            where TResolver : IJsonFormatterResolver<char, TResolver>, new()
        {
            if (TryReadJsonDynamic(o, out var token)) { return token; }

            if (o is null) { ThrowHelper.ThrowArgumentNullException(ExceptionArgument.o); }

            var utf16Json = JsonSerializer.NonGeneric.Utf16.SerializeToCharArray<TResolver>(o);
            return Parse(utf16Json);
        }

        #endregion

        #region -- FromObject --

        /// <summary>Creates a <see cref="JToken"/> from an object.</summary>
        /// <param name="o">The object that will be used to create <see cref="JToken"/>.</param>
        /// <returns>A <see cref="JToken"/> with the value of the specified object.</returns>
        public static JToken FromObject(object o)
        {
            if (TryReadJsonDynamic(o, out var token)) { return token; }

            if (o is null) { ThrowHelper.ThrowArgumentNullException(ExceptionArgument.o); }

            var jsonSerializer = DefaultSerializerPool.Take();
            try
            {
                return FromObjectInternal0(o, jsonSerializer);
            }
            finally
            {
                DefaultSerializerPool.Return(jsonSerializer);
            }
        }

        /// <summary>Creates a <see cref="JToken"/> from an object.</summary>
        /// <param name="o">The object that will be used to create <see cref="JToken"/>.</param>
        /// <returns>A <see cref="JToken"/> with the value of the specified object.</returns>
        public static JToken FromPolymorphicObject(object o)
        {
            if (TryReadJsonDynamic(o, out var token)) { return token; }

            if (o is null) { ThrowHelper.ThrowArgumentNullException(ExceptionArgument.o); }

            var jsonSerializer = PolymorphicSerializerPool.Take();
            try
            {
                return FromObjectInternal0(o, jsonSerializer);
            }
            finally
            {
                PolymorphicSerializerPool.Return(jsonSerializer);
            }
        }

        /// <summary>Creates a <see cref="JToken"/> from an object using the specified <see cref="NJsonSerializer"/>.</summary>
        /// <param name="o">The object that will be used to create <see cref="JToken"/>.</param>
        /// <param name="jsonSerializer">The <see cref="NJsonSerializer"/> that will be used when reading the object.</param>
        /// <returns>A <see cref="JToken"/> with the value of the specified object.</returns>
        public static JToken FromObject(object o, NJsonSerializer jsonSerializer)
        {
            if (TryReadJsonDynamic(o, out var token)) { return token; }

            if (o is null) { ThrowHelper.ThrowArgumentNullException(ExceptionArgument.o); }
            if (jsonSerializer is null) { ThrowHelper.ThrowArgumentNullException(ExceptionArgument.jsonSerializer); }

            return FromObjectInternal0(o, jsonSerializer);
        }

        private static JToken FromObjectInternal0(object o, NJsonSerializer jsonSerializer)
        {
            using JTokenWriter jsonWriter = new();
            jsonSerializer.Serialize(jsonWriter, o);
            return jsonWriter.Token!;
        }

        internal static bool HasJsonDynamic(IEnumerable content)
        {
            foreach (var item in content)
            {
                switch (item)
                {
                    case JsonDocument doc when doc.RootElement.ValueKind == JsonValueKind.Object || doc.RootElement.ValueKind == JsonValueKind.Array:
                        return true;

                    case JsonElement element when element.ValueKind == JsonValueKind.Object || element.ValueKind == JsonValueKind.Array:
                        return true;

                    case JsonProperty:
                        return true;

                    case SpanJsonDynamicObject:
                        return true;

                    case ISpanJsonDynamicArray:
                        return true;
                }
            }
            return false;
        }

        internal static bool TryReadJsonDynamic(object? jsonData, [MaybeNullWhen(false)] out JToken token)
        {
            switch (jsonData)
            {
                case JsonDocument doc:
                    token = FromDocument(doc);
                    return true;

                case JsonElement element:
                    token = FromElement(element);
                    return true;

                case JsonProperty property:
                    token = ReadProperty(property);
                    return true;

                case SpanJsonDynamicObject dynamicObject:
                    token = ReadDynamicObject(dynamicObject);
                    return true;

                case ISpanJsonDynamicArray dynamicArray:
                    token = ReadDynamicArray(dynamicArray);
                    return true;

                case SpanJsonDynamicUtf16Number utf16Number:
                    token = new JValue(utf16Number);
                    return true;

                case SpanJsonDynamicUtf16String utf16String:
                    token = new JValue(utf16String);
                    return true;

                case SpanJsonDynamicUtf8Number utf8Number:
                    token = new JValue(utf8Number);
                    return true;

                case SpanJsonDynamicUtf8String utf8String:
                    token = new JValue(utf8String);
                    return true;

                case JToken jToken:
                    token = jToken;
                    return true;

                default:
                    token = null;
                    return false;
            }
        }

        private static void ReadTokenFrom(object element, JContainer parent)
        {
            switch (element)
            {
                case SpanJsonDynamicObject dynamicObject:
                    parent.Add(ReadDynamicObject(dynamicObject));
                    break;

                case ISpanJsonDynamicArray dynamicArray:
                    parent.Add(ReadDynamicArray(dynamicArray));
                    break;

                case SpanJsonDynamicUtf16Number utf16Number:
                    parent.Add(new JValue(utf16Number));
                    break;

                case SpanJsonDynamicUtf16String utf16String:
                    parent.Add(new JValue(utf16String));
                    break;

                case SpanJsonDynamicUtf8Number utf8Number:
                    parent.Add(new JValue(utf8Number));
                    break;

                case SpanJsonDynamicUtf8String utf8String:
                    parent.Add(new JValue(utf8String));
                    break;

                default:
                    throw ThrowHelper.GetNotSupportedException();
            }
        }

        private static JObject ReadDynamicObject(dynamic dynamicObject)
        {
            JObject jObject = new JObject();
            var dict = (IDictionary<string, object>)dynamicObject;
            foreach (var item in dict)
            {
                jObject.Add(ReadDynamicProperty(item));
            }
            jObject._dynamicJson = dynamicObject;
            return jObject;
        }

        private static JArray ReadDynamicArray(ISpanJsonDynamicArray dynamicArray)
        {
            JArray jArray = new JArray();
            foreach (var item in dynamicArray)
            {
                ReadTokenFrom(item, jArray);
            }
            jArray._dynamicJson = dynamicArray;
            return jArray;
        }

        private static JProperty ReadDynamicProperty(KeyValuePair<string, object> property)
        {
            JProperty jProperty = new JProperty(property.Key);

            ReadTokenFrom(property.Value, jProperty);

            return jProperty;
        }

        #endregion

        #region -- FromDocument --

        public static JToken FromDocument(JsonDocument doc)
        {
            if (doc is null) { ThrowHelper.ThrowArgumentNullException(ExceptionArgument.doc); }

            var token = FromElement(doc.RootElement);
            switch (token.Type)
            {
                case JTokenType.Object:
                    ((JObject)token)._dynamicJson = doc;
                    break;
                case JTokenType.Array:
                    ((JArray)token)._dynamicJson = doc;
                    break;
            }
            return token;
        }

        public static JToken FromElement(in JsonElement element)
        {
            switch (element.ValueKind)
            {
                case JsonValueKind.Object:
                    return ReadObject(element);

                case JsonValueKind.Array:
                    return ReadArray(element);

                case JsonValueKind.String:
                    return new JValue(element);

                case JsonValueKind.Number:
                    return new JValue(element);

                case JsonValueKind.True:
                    return new JValue(true, JTokenType.Boolean);

                case JsonValueKind.False:
                    return new JValue(false, JTokenType.Boolean);

                case JsonValueKind.Null:
                    return JValue.CreateNull();

                case JsonValueKind.Undefined:
                default:
                    throw ThrowHelper.GetArgumentNullException(ExceptionArgument.element);
            }
        }

        private static void ReadTokenFrom(in JsonElement element, JContainer parent)
        {
            switch (element.ValueKind)
            {
                case JsonValueKind.Object:
                    parent.Add(ReadObject(element));
                    break;

                case JsonValueKind.Array:
                    parent.Add(ReadArray(element));
                    break;

                case JsonValueKind.String:
                    parent.Add(new JValue(element));
                    break;

                case JsonValueKind.Number:
                    parent.Add(new JValue(element));
                    break;

                case JsonValueKind.True:
                    parent.Add(new JValue(true, JTokenType.Boolean));
                    break;

                case JsonValueKind.False:
                    parent.Add(new JValue(false, JTokenType.Boolean));
                    break;

                case JsonValueKind.Null:
                    parent.Add(JValue.CreateNull());
                    break;

                case JsonValueKind.Undefined:
                default:
                    throw ThrowHelper.GetNotSupportedException();
            }
        }

        private static JObject ReadObject(in JsonElement element)
        {
            JObject jObject = new();
            foreach (var item in element.EnumerateObject())
            {
                jObject.Add(ReadProperty(item));
            }
            jObject._dynamicJson = element;
            return jObject;
        }

        private static JArray ReadArray(in JsonElement element)
        {
            JArray jArray = new();
            foreach (var item in element.EnumerateArray())
            {
                ReadTokenFrom(item, jArray);
            }
            jArray._dynamicJson = element;
            return jArray;
        }

        private static JProperty ReadProperty(in JsonProperty property)
        {
            JProperty jProperty = new(property.Name);

            ReadTokenFrom(property.Value, jProperty);

            return jProperty;
        }

        #endregion
    }
}