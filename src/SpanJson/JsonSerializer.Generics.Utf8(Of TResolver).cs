﻿using System.Runtime.CompilerServices;

namespace SpanJson
{
    static partial class JsonSerializer
    {
        static partial class Generic
        {
            /// <summary>Serialize/Deserialize to/from byte array et al.</summary>
            static partial class Utf8
            {
                #region -- Serialize --

                /// <summary>Serialize to byte array with specific resolver.</summary>
                /// <typeparam name="T">Type</typeparam>
                /// <typeparam name="TResolver">Resolver</typeparam>
                /// <param name="input">Input</param>
                /// <returns>Byte array</returns>
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public static byte[] Serialize<T, TResolver>(T input)
                    where TResolver : IJsonFormatterResolver<byte, TResolver>, new()
                {
                    return Inner<T, byte, TResolver>.InnerSerializeToByteArray(input);
                }

                /// <summary>Serialize to byte array from array pool with specific resolver.
                /// The returned ArraySegment's Array needs to be returned to the ArrayPool.</summary>
                /// <typeparam name="T">Type</typeparam>
                /// <typeparam name="TResolver">Resolver</typeparam>
                /// <param name="input">Input</param>
                /// <returns>Byte array from ArrayPool</returns>
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public static ArraySegment<byte> SerializeToArrayPool<T, TResolver>(T input)
                    where TResolver : IJsonFormatterResolver<byte, TResolver>, new()
                {
                    return Inner<T, byte, TResolver>.InnerSerializeToByteArrayPool(input);
                }

                /// <summary>Serialize to stream with specific resolver.</summary>
                /// <typeparam name="T">Type</typeparam>
                /// <typeparam name="TResolver">Resolver</typeparam>
                /// <param name="input">Input</param>
                /// <param name="stream">Stream</param>
                /// <param name="cancellationToken">CancellationToken</param>
                /// <returns>Task</returns>
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public static ValueTask SerializeAsync<T, TResolver>(T input, Stream stream, CancellationToken cancellationToken = default)
                    where TResolver : IJsonFormatterResolver<byte, TResolver>, new()
                {
                    return Inner<T, byte, TResolver>.InnerSerializeAsync(input, stream, cancellationToken);
                }

                #endregion

                #region -- Deserialize --

                /// <summary>Deserialize from byte array with specific resolver.</summary>
                /// <typeparam name="T">Type</typeparam>
                /// <typeparam name="TResolver">Resolver</typeparam>
                /// <param name="input">Input</param>
                /// <returns>Deserialized object</returns>
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public static T? Deserialize<T, TResolver>(byte[] input)
                    where TResolver : IJsonFormatterResolver<byte, TResolver>, new()
                {
                    return Inner<T, byte, TResolver>.InnerDeserialize(input);
                }

                /// <summary>Deserialize from byte array with specific resolver.</summary>
                /// <typeparam name="T">Type</typeparam>
                /// <typeparam name="TResolver">Resolver</typeparam>
                /// <param name="input">Input</param>
                /// <returns>Deserialized object</returns>
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
#if !NETSTANDARD2_0
                public static T? Deserialize<T, TResolver>(in ArraySegment<byte> input)
#else
                public static T? Deserialize<T, TResolver>(ArraySegment<byte> input)
#endif
                    where TResolver : IJsonFormatterResolver<byte, TResolver>, new()
                {
                    return Inner<T, byte, TResolver>.InnerDeserialize(input);
                }

                /// <summary>Deserialize from byte array with specific resolver.</summary>
                /// <typeparam name="T">Type</typeparam>
                /// <typeparam name="TResolver">Resolver</typeparam>
                /// <param name="input">Input</param>
                /// <returns>Deserialized object</returns>
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public static T? Deserialize<T, TResolver>(in ReadOnlyMemory<byte> input)
                    where TResolver : IJsonFormatterResolver<byte, TResolver>, new()
                {
                    return Inner<T, byte, TResolver>.InnerDeserialize(input);
                }

                /// <summary>Deserialize from byte array with specific resolver.</summary>
                /// <typeparam name="T">Type</typeparam>
                /// <typeparam name="TResolver">Resolver</typeparam>
                /// <param name="input">Input</param>
                /// <returns>Deserialized object</returns>
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public static T? Deserialize<T, TResolver>(in ReadOnlySpan<byte> input)
                    where TResolver : IJsonFormatterResolver<byte, TResolver>, new()
                {
                    return Inner<T, byte, TResolver>.InnerDeserialize(input);
                }

                /// <summary>Deserialize from stream with specific resolver.</summary>
                /// <typeparam name="T">Type</typeparam>
                /// <typeparam name="TResolver">Resolver</typeparam>
                /// <param name="stream">Stream</param>
                /// <param name="cancellationToken">CancellationToken</param>
                /// <returns>Task</returns>
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public static ValueTask<T?> DeserializeAsync<T, TResolver>(Stream stream, CancellationToken cancellationToken = default)
                    where TResolver : IJsonFormatterResolver<byte, TResolver>, new()
                {
                    return Inner<T, byte, TResolver>.InnerDeserializeAsync(stream, cancellationToken);
                }

                #endregion
            }
        }
    }
}