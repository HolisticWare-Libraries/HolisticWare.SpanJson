using System.Runtime.CompilerServices;
using SpanJson.Document;
using SpanJson.Resolvers;
using NJsonSerializer = Newtonsoft.Json.JsonSerializer;

namespace SpanJson.Linq
{
    partial class JObject
    {
        /// <summary>Creates a <see cref="JObject"/> from an object.</summary>
        /// <param name="input">The object that will be used to create <see cref="JObject"/>.</param>
        /// <returns>A <see cref="JObject"/> with the value of the specified object.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public new static JObject From<T>(T input)
        {
            JToken token = JToken.From<T, IncludeNullsOriginalCaseResolver<char>>(input);

            return AsObject(token);
        }

        /// <summary>Creates a <see cref="JObject"/> from an object.</summary>
        /// <param name="input">The object that will be used to create <see cref="JObject"/>.</param>
        /// <returns>A <see cref="JObject"/> with the value of the specified object.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public new static JObject From<T, TResolver>(T input)
            where TResolver : IJsonFormatterResolver<char, TResolver>, new()
        {
            JToken token = JToken.From<T, TResolver>(input);

            return AsObject(token);
        }

        /// <summary>Creates a <see cref="JObject"/> from an object.</summary>
        /// <param name="o">The object that will be used to create <see cref="JObject"/>.</param>
        /// <returns>A <see cref="JObject"/> with the values of the specified object.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public new static JObject FromDynamic(object o)
        {
            JToken token = JToken.FromDynamic<IncludeNullsOriginalCaseResolver<char>>(o);

            return AsObject(token);
        }

        /// <summary>Creates a <see cref="JObject"/> from an object.</summary>
        /// <param name="o">The object that will be used to create <see cref="JObject"/>.</param>
        /// <returns>A <see cref="JObject"/> with the values of the specified object.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public new static JObject FromDynamic<TResolver>(object o)
            where TResolver : IJsonFormatterResolver<char, TResolver>, new()
        {
            JToken token = JToken.FromDynamic<TResolver>(o);

            return AsObject(token);
        }

        /// <summary>Creates a <see cref="JObject"/> from an object.</summary>
        /// <param name="o">The object that will be used to create <see cref="JObject"/>.</param>
        /// <returns>A <see cref="JObject"/> with the value of the specified object.</returns>
        public new static JObject FromObject(object o)
        {
            JToken token = JToken.FromObject(o);

            return AsObject(token);
        }

        /// <summary>Creates a <see cref="JObject"/> from an object.</summary>
        /// <param name="o">The object that will be used to create <see cref="JObject"/>.</param>
        /// <returns>A <see cref="JObject"/> with the value of the specified object.</returns>
        public new static JObject FromPolymorphicObject(object o)
        {
            JToken token = JToken.FromPolymorphicObject(o);

            return AsObject(token);
        }

        /// <summary>Creates a <see cref="JObject"/> from an object using the specified <see cref="NJsonSerializer"/>.</summary>
        /// <param name="o">The object that will be used to create <see cref="JObject"/>.</param>
        /// <param name="jsonSerializer">The <see cref="NJsonSerializer"/> that will be used when reading the object.</param>
        /// <returns>A <see cref="JObject"/> with the value of the specified object.</returns>
        public new static JObject FromObject(object o, NJsonSerializer jsonSerializer)
        {
            JToken token = JToken.FromObject(o, jsonSerializer);

            return AsObject(token);
        }

        public new static JObject FromDocument(JsonDocument doc)
        {
            JToken token = JToken.FromDocument(doc);

            return AsObject(token);
        }

        public new static JObject FromElement(in JsonElement element)
        {
            JToken token = JToken.FromElement(element);

            return AsObject(token);
        }

        #region ** AsObject **

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static JObject AsObject(JToken token)
        {
            if (token.Type != JTokenType.Object)
            {
                ThrowHelper2.ThrowArgumentException_Object_serialized_to_JObject_instance_expected(token.Type);
            }

            return (JObject)token;
        }

        #endregion
    }
}
