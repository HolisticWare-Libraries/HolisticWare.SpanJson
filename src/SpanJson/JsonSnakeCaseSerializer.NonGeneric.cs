﻿using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using SpanJson.Resolvers;

namespace SpanJson
{
    partial class JsonSnakeCaseSerializer
    {
        /// <summary>Non-Generic part</summary>
        public static class NonGeneric
        {
            public static class Utf16
            {
                #region -- Serialize --

                /// <summary>Serialize to string.</summary>
                /// <param name="input">Input</param>
                /// <returns>String</returns>
                public static string Serialize(object input)
                {
                    return JsonSerializer.NonGeneric.Inner<char, IncludeNullsSnakeCaseResolver<char>>.InnerSerializeToString(input);
                }

                /// <summary>Serialize to char buffer from ArrayPool.
                /// The returned ArraySegment's Array needs to be returned to the ArrayPool.</summary>
                /// <param name="input">Input</param>
                /// <returns>Char array from ArrayPool</returns>
                public static ArraySegment<char> SerializeToArrayPool(object input)
                {
                    return JsonSerializer.NonGeneric.Inner<char, IncludeNullsSnakeCaseResolver<char>>.InnerSerializeToCharArrayPool(input);
                }

                /// <summary>Serialize to TextWriter.</summary>
                /// <param name="input">Input</param>
                /// <param name="writer">TextWriter</param>
                /// <param name="cancellationToken">CancellationToken</param>
                /// <returns>Task</returns>
                public static ValueTask SerializeAsync(object input, TextWriter writer, CancellationToken cancellationToken = default)
                {
                    return JsonSerializer.NonGeneric.Inner<char, IncludeNullsSnakeCaseResolver<char>>.InnerSerializeAsync(input, writer, cancellationToken);
                }

                #endregion

                #region -- Deserialize --

                /// <summary>Deserialize from string.</summary>
                /// <param name="input">Input</param>
                /// <param name="type">Object Type</param>
                /// <returns>Deserialized object</returns>
                public static object Deserialize(string input, Type type)
                {
#if NETSTANDARD2_0 || NET471 || NET451
                    return JsonSerializer.NonGeneric.Inner<char, IncludeNullsSnakeCaseResolver<char>>.InnerDeserialize(input.AsSpan(), type);
#else
                    return JsonSerializer.NonGeneric.Inner<char, IncludeNullsSnakeCaseResolver<char>>.InnerDeserialize(input, type);
#endif
                }

                /// <summary>Deserialize from string.</summary>
                /// <param name="input">Input</param>
                /// <param name="type">Object Type</param>
                /// <returns>Deserialized object</returns>
                public static object Deserialize(char[] input, Type type)
                {
                    return JsonSerializer.NonGeneric.Inner<char, IncludeNullsSnakeCaseResolver<char>>.InnerDeserialize(input, type);
                }

                /// <summary>Deserialize from string.</summary>
                /// <param name="input">Input</param>
                /// <param name="type">Object Type</param>
                /// <returns>Deserialized object</returns>
                public static object Deserialize(in ArraySegment<char> input, Type type)
                {
                    return JsonSerializer.NonGeneric.Inner<char, IncludeNullsSnakeCaseResolver<char>>.InnerDeserialize(input, type);
                }

                /// <summary>Deserialize from string.</summary>
                /// <param name="input">Input</param>
                /// <param name="type">Object Type</param>
                /// <returns>Deserialized object</returns>
                public static object Deserialize(in ReadOnlyMemory<char> input, Type type)
                {
                    return JsonSerializer.NonGeneric.Inner<char, IncludeNullsSnakeCaseResolver<char>>.InnerDeserialize(input, type);
                }

                /// <summary>Deserialize from string.</summary>
                /// <param name="input">Input</param>
                /// <param name="type">Object Type</param>
                /// <returns>Deserialized object</returns>
                public static object Deserialize(in ReadOnlySpan<char> input, Type type)
                {
                    return JsonSerializer.NonGeneric.Inner<char, IncludeNullsSnakeCaseResolver<char>>.InnerDeserialize(input, type);
                }

                /// <summary>Deserialize from TextReader.</summary>
                /// <param name="reader">TextReader</param>
                /// <param name="type">Object Type</param>
                /// <param name="cancellationToken">CancellationToken</param>
                /// <returns>Task</returns>
                public static ValueTask<object> DeserializeAsync(TextReader reader, Type type,
                    CancellationToken cancellationToken = default)
                {
                    return JsonSerializer.NonGeneric.Inner<char, IncludeNullsSnakeCaseResolver<char>>.InnerDeserializeAsync(reader, type, cancellationToken);
                }

                #endregion
            }

            public static class Utf8
            {
                #region -- Serialize --

                /// <summary>Serialize to byte array.</summary>
                /// <param name="input">Input</param>
                /// <returns>Byte array</returns>
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public static byte[] Serialize(object input)
                {
                    return JsonSerializer.NonGeneric.Inner<byte, IncludeNullsSnakeCaseResolver<byte>>.InnerSerializeToByteArray(input);
                }

                /// <summary>Serialize to byte array from ArrayPool.
                /// The returned ArraySegment's Array needs to be returned to the ArrayPool.</summary>
                /// <param name="input">Input</param>
                /// <returns>Byte array from ArrayPool</returns>
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public static ArraySegment<byte> SerializeToArrayPool(object input)
                {
                    return JsonSerializer.NonGeneric.Inner<byte, IncludeNullsSnakeCaseResolver<byte>>.InnerSerializeToByteArrayPool(input);
                }

                /// <summary>Serialize to stream.</summary>
                /// <param name="input">Input</param>
                /// <param name="stream">Stream</param>
                /// <param name="cancellationToken">CancellationToken</param>
                /// <returns>Task</returns>
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public static ValueTask SerializeAsync(object input, Stream stream, CancellationToken cancellationToken = default)
                {
                    return JsonSerializer.NonGeneric.Inner<byte, IncludeNullsSnakeCaseResolver<byte>>.InnerSerializeAsync(input, stream, cancellationToken);
                }
                #endregion

                #region -- Deserialize --

                /// <summary>Deserialize from Byte array.</summary>
                /// <param name="input">Input</param>
                /// <param name="type">Object Type</param>
                /// <returns>Deserialized object</returns>
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public static object Deserialize(byte[] input, Type type)
                {
                    return JsonSerializer.NonGeneric.Inner<byte, IncludeNullsSnakeCaseResolver<byte>>.InnerDeserialize(input, type);
                }

                /// <summary>Deserialize from Byte array.</summary>
                /// <param name="input">Input</param>
                /// <param name="type">Object Type</param>
                /// <returns>Deserialized object</returns>
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public static object Deserialize(in ArraySegment<byte> input, Type type)
                {
                    return JsonSerializer.NonGeneric.Inner<byte, IncludeNullsSnakeCaseResolver<byte>>.InnerDeserialize(input, type);
                }

                /// <summary>Deserialize from Byte array.</summary>
                /// <param name="input">Input</param>
                /// <param name="type">Object Type</param>
                /// <returns>Deserialized object</returns>
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public static object Deserialize(in ReadOnlyMemory<byte> input, Type type)
                {
                    return JsonSerializer.NonGeneric.Inner<byte, IncludeNullsSnakeCaseResolver<byte>>.InnerDeserialize(input, type);
                }

                /// <summary>Deserialize from Byte array.</summary>
                /// <param name="input">Input</param>
                /// <param name="type">Object Type</param>
                /// <returns>Deserialized object</returns>
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public static object Deserialize(in ReadOnlySpan<byte> input, Type type)
                {
                    return JsonSerializer.NonGeneric.Inner<byte, IncludeNullsSnakeCaseResolver<byte>>.InnerDeserialize(input, type);
                }

                /// <summary>Deserialize from stream.</summary>
                /// <param name="stream">Stream</param>
                /// <param name="type">Object Type</param>
                /// <param name="cancellationToken">CancellationToken</param>
                /// <returns>Task</returns>
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public static ValueTask<object> DeserializeAsync(Stream stream, Type type,
                    CancellationToken cancellationToken = default)
                {
                    return JsonSerializer.NonGeneric.Inner<byte, IncludeNullsSnakeCaseResolver<byte>>.InnerDeserializeAsync(stream, type, cancellationToken);
                }

                #endregion
            }
        }
    }
}
