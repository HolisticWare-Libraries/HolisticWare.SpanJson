using System.Runtime.CompilerServices;
using SpanJson.Resolvers;

namespace SpanJson
{
    static partial class JsonSerializer
    {
        static partial class NonGeneric
        {
            /// <summary>Serialize/Deserialize to/from byte array et al.</summary>
            public static partial class Utf8
            {
                #region -- Serialize --

                /// <summary>Serialize to byte array.</summary>
                /// <param name="input">Input</param>
                /// <returns>Byte array</returns>
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public static byte[] Serialize(object? input)
                {
                    return Inner<byte, ExcludeNullsOriginalCaseResolver<byte>>.InnerSerializeToByteArray(input);
                }

                /// <summary>Serialize to byte array from ArrayPool.
                /// The returned ArraySegment's Array needs to be returned to the ArrayPool.</summary>
                /// <param name="input">Input</param>
                /// <returns>Byte array from ArrayPool</returns>
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public static ArraySegment<byte> SerializeToArrayPool(object? input)
                {
                    return Inner<byte, ExcludeNullsOriginalCaseResolver<byte>>.InnerSerializeToByteArrayPool(input);
                }

                /// <summary>Serialize to stream.</summary>
                /// <param name="input">Input</param>
                /// <param name="stream">Stream</param>
                /// <param name="cancellationToken">CancellationToken</param>
                /// <returns>Task</returns>
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public static ValueTask SerializeAsync(object? input, Stream stream, CancellationToken cancellationToken = default)
                {
                    return Inner<byte, ExcludeNullsOriginalCaseResolver<byte>>.InnerSerializeAsync(input, stream, cancellationToken);
                }

                #endregion

                #region -- Deserialize --

                /// <summary>Deserialize from Byte array.</summary>
                /// <param name="input">Input</param>
                /// <param name="type">Object Type</param>
                /// <returns>Deserialized object</returns>
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public static object? Deserialize(byte[] input, Type type)
                {
                    return Inner<byte, ExcludeNullsOriginalCaseResolver<byte>>.InnerDeserialize(input, type);
                }

                /// <summary>Deserialize from Byte array.</summary>
                /// <param name="input">Input</param>
                /// <param name="type">Object Type</param>
                /// <returns>Deserialized object</returns>
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
#if !NETSTANDARD2_0
                public static object? Deserialize(in ArraySegment<byte> input, Type type)
#else
                public static object? Deserialize(ArraySegment<byte> input, Type type)
#endif
                {
                    return Inner<byte, ExcludeNullsOriginalCaseResolver<byte>>.InnerDeserialize(input, type);
                }

                /// <summary>Deserialize from Byte array.</summary>
                /// <param name="input">Input</param>
                /// <param name="type">Object Type</param>
                /// <returns>Deserialized object</returns>
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public static object? Deserialize(in ReadOnlyMemory<byte> input, Type type)
                {
                    return Inner<byte, ExcludeNullsOriginalCaseResolver<byte>>.InnerDeserialize(input, type);
                }

                /// <summary>Deserialize from Byte array.</summary>
                /// <param name="input">Input</param>
                /// <param name="type">Object Type</param>
                /// <returns>Deserialized object</returns>
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public static object? Deserialize(in ReadOnlySpan<byte> input, Type type)
                {
                    return Inner<byte, ExcludeNullsOriginalCaseResolver<byte>>.InnerDeserialize(input, type);
                }

                /// <summary>Deserialize from stream.</summary>
                /// <param name="stream">Stream</param>
                /// <param name="type">Object Type</param>
                /// <param name="cancellationToken">CancellationToken</param>
                /// <returns>Task</returns>
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public static ValueTask<object?> DeserializeAsync(Stream stream, Type type,
                    CancellationToken cancellationToken = default)
                {
                    return Inner<byte, ExcludeNullsOriginalCaseResolver<byte>>.InnerDeserializeAsync(stream, type, cancellationToken);
                }

                #endregion
            }
        }
    }
}