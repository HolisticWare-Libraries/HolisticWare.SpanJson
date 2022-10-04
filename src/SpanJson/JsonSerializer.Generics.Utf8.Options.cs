using System.Runtime.CompilerServices;
using SpanJson.Resolvers;

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

                /// <summary>Serialize to byte array.</summary>
                /// <typeparam name="T">Type</typeparam>
                /// <param name="input">Input</param>
                /// <param name="namingPolicy"></param>
                /// <returns>Byte array</returns>
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public static byte[] Serialize<T>(T input, JsonKnownNamingPolicy namingPolicy)
                {
                    return namingPolicy switch
                    {
                        JsonKnownNamingPolicy.CamelCase => Inner<T, byte, ExcludeNullsCamelCaseResolver<byte>>.InnerSerializeToByteArray(input),
                        JsonKnownNamingPolicy.SnakeCase => Inner<T, byte, ExcludeNullsSnakeCaseResolver<byte>>.InnerSerializeToByteArray(input),
                        JsonKnownNamingPolicy.AdaCase => Inner<T, byte, ExcludeNullsAdaCaseResolver<byte>>.InnerSerializeToByteArray(input),
                        JsonKnownNamingPolicy.MacroCase => Inner<T, byte, ExcludeNullsMacroCaseResolver<byte>>.InnerSerializeToByteArray(input),
                        JsonKnownNamingPolicy.KebabCase => Inner<T, byte, ExcludeNullsKebabCaseResolver<byte>>.InnerSerializeToByteArray(input),
                        JsonKnownNamingPolicy.TrainCase => Inner<T, byte, ExcludeNullsTrainCaseResolver<byte>>.InnerSerializeToByteArray(input),
                        JsonKnownNamingPolicy.CobolCase => Inner<T, byte, ExcludeNullsCobolCaseResolver<byte>>.InnerSerializeToByteArray(input),
                        _ => Inner<T, byte, ExcludeNullsOriginalCaseResolver<byte>>.InnerSerializeToByteArray(input),
                    };
                }

                /// <summary>Serialize to byte array from ArrayPool.
                /// The returned ArraySegment's Array needs to be returned to the ArrayPool.</summary>
                /// <typeparam name="T">Type</typeparam>
                /// <param name="input">Input</param>
                /// <param name="namingPolicy"></param>
                /// <returns>Byte array from ArrayPool</returns>
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public static ArraySegment<byte> SerializeToArrayPool<T>(T input, JsonKnownNamingPolicy namingPolicy)
                {
                    return namingPolicy switch
                    {
                        JsonKnownNamingPolicy.CamelCase => Inner<T, byte, ExcludeNullsCamelCaseResolver<byte>>.InnerSerializeToByteArrayPool(input),
                        JsonKnownNamingPolicy.SnakeCase => Inner<T, byte, ExcludeNullsSnakeCaseResolver<byte>>.InnerSerializeToByteArrayPool(input),
                        JsonKnownNamingPolicy.AdaCase => Inner<T, byte, ExcludeNullsAdaCaseResolver<byte>>.InnerSerializeToByteArrayPool(input),
                        JsonKnownNamingPolicy.MacroCase => Inner<T, byte, ExcludeNullsMacroCaseResolver<byte>>.InnerSerializeToByteArrayPool(input),
                        JsonKnownNamingPolicy.KebabCase => Inner<T, byte, ExcludeNullsKebabCaseResolver<byte>>.InnerSerializeToByteArrayPool(input),
                        JsonKnownNamingPolicy.TrainCase => Inner<T, byte, ExcludeNullsTrainCaseResolver<byte>>.InnerSerializeToByteArrayPool(input),
                        JsonKnownNamingPolicy.CobolCase => Inner<T, byte, ExcludeNullsCobolCaseResolver<byte>>.InnerSerializeToByteArrayPool(input),
                        _ => Inner<T, byte, ExcludeNullsOriginalCaseResolver<byte>>.InnerSerializeToByteArrayPool(input),
                    };
                }

                /// <summary>Serialize to stream.</summary>
                /// <typeparam name="T">Type</typeparam>
                /// <param name="input">Input</param>
                /// <param name="stream">Stream</param>
                /// <param name="namingPolicy"></param>
                /// <param name="cancellationToken">CancellationToken</param>
                /// <returns>Task</returns>
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public static ValueTask SerializeAsync<T>(T input, Stream stream, JsonKnownNamingPolicy namingPolicy, CancellationToken cancellationToken = default)
                {
                    return namingPolicy switch
                    {
                        JsonKnownNamingPolicy.CamelCase => Inner<T, byte, ExcludeNullsCamelCaseResolver<byte>>.InnerSerializeAsync(input, stream, cancellationToken),
                        JsonKnownNamingPolicy.SnakeCase => Inner<T, byte, ExcludeNullsSnakeCaseResolver<byte>>.InnerSerializeAsync(input, stream, cancellationToken),
                        JsonKnownNamingPolicy.AdaCase => Inner<T, byte, ExcludeNullsAdaCaseResolver<byte>>.InnerSerializeAsync(input, stream, cancellationToken),
                        JsonKnownNamingPolicy.MacroCase => Inner<T, byte, ExcludeNullsMacroCaseResolver<byte>>.InnerSerializeAsync(input, stream, cancellationToken),
                        JsonKnownNamingPolicy.KebabCase => Inner<T, byte, ExcludeNullsKebabCaseResolver<byte>>.InnerSerializeAsync(input, stream, cancellationToken),
                        JsonKnownNamingPolicy.TrainCase => Inner<T, byte, ExcludeNullsTrainCaseResolver<byte>>.InnerSerializeAsync(input, stream, cancellationToken),
                        JsonKnownNamingPolicy.CobolCase => Inner<T, byte, ExcludeNullsCobolCaseResolver<byte>>.InnerSerializeAsync(input, stream, cancellationToken),
                        _ => Inner<T, byte, ExcludeNullsOriginalCaseResolver<byte>>.InnerSerializeAsync(input, stream, cancellationToken),
                    };
                }

                #endregion

                #region -- Deserialize --

                /// <summary>Deserialize from byte array.</summary>
                /// <typeparam name="T">Type</typeparam>
                /// <param name="input">Input</param>
                /// <param name="namingPolicy"></param>
                /// <returns>Deserialized object</returns>
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public static T? Deserialize<T>(byte[] input, JsonKnownNamingPolicy namingPolicy)
                {
                    return namingPolicy switch
                    {
                        JsonKnownNamingPolicy.CamelCase => Inner<T, byte, ExcludeNullsCamelCaseResolver<byte>>.InnerDeserialize(input),
                        JsonKnownNamingPolicy.SnakeCase => Inner<T, byte, ExcludeNullsSnakeCaseResolver<byte>>.InnerDeserialize(input),
                        JsonKnownNamingPolicy.AdaCase => Inner<T, byte, ExcludeNullsAdaCaseResolver<byte>>.InnerDeserialize(input),
                        JsonKnownNamingPolicy.MacroCase => Inner<T, byte, ExcludeNullsMacroCaseResolver<byte>>.InnerDeserialize(input),
                        JsonKnownNamingPolicy.KebabCase => Inner<T, byte, ExcludeNullsKebabCaseResolver<byte>>.InnerDeserialize(input),
                        JsonKnownNamingPolicy.TrainCase => Inner<T, byte, ExcludeNullsTrainCaseResolver<byte>>.InnerDeserialize(input),
                        JsonKnownNamingPolicy.CobolCase => Inner<T, byte, ExcludeNullsCobolCaseResolver<byte>>.InnerDeserialize(input),
                        _ => Inner<T, byte, ExcludeNullsOriginalCaseResolver<byte>>.InnerDeserialize(input),
                    };
                }

                /// <summary>Deserialize from byte array.</summary>
                /// <typeparam name="T">Type</typeparam>
                /// <param name="input">Input</param>
                /// <param name="namingPolicy"></param>
                /// <returns>Deserialized object</returns>
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
#if !NETSTANDARD2_0
                public static T? Deserialize<T>(in ArraySegment<byte> input, JsonKnownNamingPolicy namingPolicy)
#else
                public static T? Deserialize<T>(ArraySegment<byte> input, JsonKnownNamingPolicy namingPolicy)
#endif
                {
                    return namingPolicy switch
                    {
                        JsonKnownNamingPolicy.CamelCase => Inner<T, byte, ExcludeNullsCamelCaseResolver<byte>>.InnerDeserialize(input),
                        JsonKnownNamingPolicy.SnakeCase => Inner<T, byte, ExcludeNullsSnakeCaseResolver<byte>>.InnerDeserialize(input),
                        JsonKnownNamingPolicy.AdaCase => Inner<T, byte, ExcludeNullsAdaCaseResolver<byte>>.InnerDeserialize(input),
                        JsonKnownNamingPolicy.MacroCase => Inner<T, byte, ExcludeNullsMacroCaseResolver<byte>>.InnerDeserialize(input),
                        JsonKnownNamingPolicy.KebabCase => Inner<T, byte, ExcludeNullsKebabCaseResolver<byte>>.InnerDeserialize(input),
                        JsonKnownNamingPolicy.TrainCase => Inner<T, byte, ExcludeNullsTrainCaseResolver<byte>>.InnerDeserialize(input),
                        JsonKnownNamingPolicy.CobolCase => Inner<T, byte, ExcludeNullsCobolCaseResolver<byte>>.InnerDeserialize(input),
                        _ => Inner<T, byte, ExcludeNullsOriginalCaseResolver<byte>>.InnerDeserialize(input),
                    };
                }

                /// <summary>Deserialize from byte array.</summary>
                /// <typeparam name="T">Type</typeparam>
                /// <param name="input">Input</param>
                /// <param name="namingPolicy"></param>
                /// <returns>Deserialized object</returns>
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public static T? Deserialize<T>(in ReadOnlyMemory<byte> input, JsonKnownNamingPolicy namingPolicy)
                {
                    return namingPolicy switch
                    {
                        JsonKnownNamingPolicy.CamelCase => Inner<T, byte, ExcludeNullsCamelCaseResolver<byte>>.InnerDeserialize(input),
                        JsonKnownNamingPolicy.SnakeCase => Inner<T, byte, ExcludeNullsSnakeCaseResolver<byte>>.InnerDeserialize(input),
                        JsonKnownNamingPolicy.AdaCase => Inner<T, byte, ExcludeNullsAdaCaseResolver<byte>>.InnerDeserialize(input),
                        JsonKnownNamingPolicy.MacroCase => Inner<T, byte, ExcludeNullsMacroCaseResolver<byte>>.InnerDeserialize(input),
                        JsonKnownNamingPolicy.KebabCase => Inner<T, byte, ExcludeNullsKebabCaseResolver<byte>>.InnerDeserialize(input),
                        JsonKnownNamingPolicy.TrainCase => Inner<T, byte, ExcludeNullsTrainCaseResolver<byte>>.InnerDeserialize(input),
                        JsonKnownNamingPolicy.CobolCase => Inner<T, byte, ExcludeNullsCobolCaseResolver<byte>>.InnerDeserialize(input),
                        _ => Inner<T, byte, ExcludeNullsOriginalCaseResolver<byte>>.InnerDeserialize(input),
                    };
                }

                /// <summary>Deserialize from byte array.</summary>
                /// <typeparam name="T">Type</typeparam>
                /// <param name="input">Input</param>
                /// <param name="namingPolicy"></param>
                /// <returns>Deserialized object</returns>
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public static T? Deserialize<T>(in ReadOnlySpan<byte> input, JsonKnownNamingPolicy namingPolicy)
                {
                    return namingPolicy switch
                    {
                        JsonKnownNamingPolicy.CamelCase => Inner<T, byte, ExcludeNullsCamelCaseResolver<byte>>.InnerDeserialize(input),
                        JsonKnownNamingPolicy.SnakeCase => Inner<T, byte, ExcludeNullsSnakeCaseResolver<byte>>.InnerDeserialize(input),
                        JsonKnownNamingPolicy.AdaCase => Inner<T, byte, ExcludeNullsAdaCaseResolver<byte>>.InnerDeserialize(input),
                        JsonKnownNamingPolicy.MacroCase => Inner<T, byte, ExcludeNullsMacroCaseResolver<byte>>.InnerDeserialize(input),
                        JsonKnownNamingPolicy.KebabCase => Inner<T, byte, ExcludeNullsKebabCaseResolver<byte>>.InnerDeserialize(input),
                        JsonKnownNamingPolicy.TrainCase => Inner<T, byte, ExcludeNullsTrainCaseResolver<byte>>.InnerDeserialize(input),
                        JsonKnownNamingPolicy.CobolCase => Inner<T, byte, ExcludeNullsCobolCaseResolver<byte>>.InnerDeserialize(input),
                        _ => Inner<T, byte, ExcludeNullsOriginalCaseResolver<byte>>.InnerDeserialize(input),
                    };
                }

                /// <summary>Deserialize from stream.</summary>
                /// <typeparam name="T">Type</typeparam>
                /// <param name="stream">Stream</param>
                /// <param name="namingPolicy"></param>
                /// <param name="cancellationToken">CancellationToken</param>
                /// <returns>Task</returns>
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public static ValueTask<T?> DeserializeAsync<T>(Stream stream, JsonKnownNamingPolicy namingPolicy, CancellationToken cancellationToken = default)
                {
                    return namingPolicy switch
                    {
                        JsonKnownNamingPolicy.CamelCase => Inner<T, byte, ExcludeNullsCamelCaseResolver<byte>>.InnerDeserializeAsync(stream, cancellationToken),
                        JsonKnownNamingPolicy.SnakeCase => Inner<T, byte, ExcludeNullsSnakeCaseResolver<byte>>.InnerDeserializeAsync(stream, cancellationToken),
                        JsonKnownNamingPolicy.AdaCase => Inner<T, byte, ExcludeNullsAdaCaseResolver<byte>>.InnerDeserializeAsync(stream, cancellationToken),
                        JsonKnownNamingPolicy.MacroCase => Inner<T, byte, ExcludeNullsMacroCaseResolver<byte>>.InnerDeserializeAsync(stream, cancellationToken),
                        JsonKnownNamingPolicy.KebabCase => Inner<T, byte, ExcludeNullsKebabCaseResolver<byte>>.InnerDeserializeAsync(stream, cancellationToken),
                        JsonKnownNamingPolicy.TrainCase => Inner<T, byte, ExcludeNullsTrainCaseResolver<byte>>.InnerDeserializeAsync(stream, cancellationToken),
                        JsonKnownNamingPolicy.CobolCase => Inner<T, byte, ExcludeNullsCobolCaseResolver<byte>>.InnerDeserializeAsync(stream, cancellationToken),
                        _ => Inner<T, byte, ExcludeNullsOriginalCaseResolver<byte>>.InnerDeserializeAsync(stream, cancellationToken),
                    };
                }

                #endregion
            }
        }
    }
}