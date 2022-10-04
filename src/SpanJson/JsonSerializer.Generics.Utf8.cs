using System.Runtime.CompilerServices;
using SpanJson.Resolvers;

namespace SpanJson
{
    static partial class JsonSerializer
    {
        static partial class Generic
        {
            /// <summary>Serialize/Deserialize to/from byte array et al.</summary>
            public static partial class Utf8
            {
                #region -- Serialize --

                /// <summary>Serialize to byte array.</summary>
                /// <typeparam name="T">Type</typeparam>
                /// <param name="input">Input</param>
                /// <returns>Byte array</returns>
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public static byte[] Serialize<T>(T input)
                {
                    return Inner<T, byte, ExcludeNullsOriginalCaseResolver<byte>>.InnerSerializeToByteArray(input);
                }

                /// <summary>Serialize to byte array from ArrayPool.
                /// The returned ArraySegment's Array needs to be returned to the ArrayPool.</summary>
                /// <typeparam name="T">Type</typeparam>
                /// <param name="input">Input</param>
                /// <returns>Byte array from ArrayPool</returns>
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public static ArraySegment<byte> SerializeToArrayPool<T>(T input)
                {
                    return Inner<T, byte, ExcludeNullsOriginalCaseResolver<byte>>.InnerSerializeToByteArrayPool(input);
                }

                /// <summary>Serialize to stream.</summary>
                /// <typeparam name="T">Type</typeparam>
                /// <param name="input">Input</param>
                /// <param name="stream">Stream</param>
                /// <param name="cancellationToken">CancellationToken</param>
                /// <returns>Task</returns>
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public static ValueTask SerializeAsync<T>(T input, Stream stream, CancellationToken cancellationToken = default)
                {
                    return Inner<T, byte, ExcludeNullsOriginalCaseResolver<byte>>.InnerSerializeAsync(input, stream, cancellationToken);
                }

                #endregion

                #region -- Deserialize --

                /// <summary>Deserialize from byte array.</summary>
                /// <typeparam name="T">Type</typeparam>
                /// <param name="input">Input</param>
                /// <returns>Deserialized object</returns>
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public static T? Deserialize<T>(byte[] input)
                {
                    return Inner<T, byte, ExcludeNullsOriginalCaseResolver<byte>>.InnerDeserialize(input);
                }

                /// <summary>Deserialize from byte array.</summary>
                /// <typeparam name="T">Type</typeparam>
                /// <param name="input">Input</param>
                /// <returns>Deserialized object</returns>
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
#if !NETSTANDARD2_0
                public static T? Deserialize<T>(in ArraySegment<byte> input)
#else
                public static T? Deserialize<T>(ArraySegment<byte> input)
#endif
                {
                    return Inner<T, byte, ExcludeNullsOriginalCaseResolver<byte>>.InnerDeserialize(input);
                }

                /// <summary>Deserialize from byte array.</summary>
                /// <typeparam name="T">Type</typeparam>
                /// <param name="input">Input</param>
                /// <returns>Deserialized object</returns>
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public static T? Deserialize<T>(in ReadOnlyMemory<byte> input)
                {
                    return Inner<T, byte, ExcludeNullsOriginalCaseResolver<byte>>.InnerDeserialize(input);
                }

                /// <summary>Deserialize from byte array.</summary>
                /// <typeparam name="T">Type</typeparam>
                /// <param name="input">Input</param>
                /// <returns>Deserialized object</returns>
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public static T? Deserialize<T>(in ReadOnlySpan<byte> input)
                {
                    return Inner<T, byte, ExcludeNullsOriginalCaseResolver<byte>>.InnerDeserialize(input);
                }

                /// <summary>Deserialize from stream.</summary>
                /// <typeparam name="T">Type</typeparam>
                /// <param name="stream">Stream</param>
                /// <param name="cancellationToken">CancellationToken</param>
                /// <returns>Task</returns>
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public static ValueTask<T?> DeserializeAsync<T>(Stream stream, CancellationToken cancellationToken = default)
                {
                    return Inner<T, byte, ExcludeNullsOriginalCaseResolver<byte>>.InnerDeserializeAsync(stream, cancellationToken);
                }

                #endregion
            }
        }
    }
}