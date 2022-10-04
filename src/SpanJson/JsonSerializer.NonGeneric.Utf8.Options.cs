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
                /// <param name="namingPolicy"></param>
                /// <returns>Byte array</returns>
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public static byte[] Serialize(object? input, JsonKnownNamingPolicy namingPolicy)
                {
                    return namingPolicy switch
                    {
                        JsonKnownNamingPolicy.CamelCase => Inner<byte, ExcludeNullsCamelCaseResolver<byte>>.InnerSerializeToByteArray(input),
                        JsonKnownNamingPolicy.SnakeCase => Inner<byte, ExcludeNullsSnakeCaseResolver<byte>>.InnerSerializeToByteArray(input),
                        JsonKnownNamingPolicy.AdaCase => Inner<byte, ExcludeNullsAdaCaseResolver<byte>>.InnerSerializeToByteArray(input),
                        JsonKnownNamingPolicy.MacroCase => Inner<byte, ExcludeNullsMacroCaseResolver<byte>>.InnerSerializeToByteArray(input),
                        JsonKnownNamingPolicy.KebabCase => Inner<byte, ExcludeNullsKebabCaseResolver<byte>>.InnerSerializeToByteArray(input),
                        JsonKnownNamingPolicy.TrainCase => Inner<byte, ExcludeNullsTrainCaseResolver<byte>>.InnerSerializeToByteArray(input),
                        JsonKnownNamingPolicy.CobolCase => Inner<byte, ExcludeNullsCobolCaseResolver<byte>>.InnerSerializeToByteArray(input),
                        _ => Inner<byte, ExcludeNullsOriginalCaseResolver<byte>>.InnerSerializeToByteArray(input),
                    };
                }

                /// <summary>Serialize to byte array from ArrayPool.
                /// The returned ArraySegment's Array needs to be returned to the ArrayPool.</summary>
                /// <param name="input">Input</param>
                /// <param name="namingPolicy"></param>
                /// <returns>Byte array from ArrayPool</returns>
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public static ArraySegment<byte> SerializeToArrayPool(object? input, JsonKnownNamingPolicy namingPolicy)
                {
                    return namingPolicy switch
                    {
                        JsonKnownNamingPolicy.CamelCase => Inner<byte, ExcludeNullsCamelCaseResolver<byte>>.InnerSerializeToByteArrayPool(input),
                        JsonKnownNamingPolicy.SnakeCase => Inner<byte, ExcludeNullsSnakeCaseResolver<byte>>.InnerSerializeToByteArrayPool(input),
                        JsonKnownNamingPolicy.AdaCase => Inner<byte, ExcludeNullsAdaCaseResolver<byte>>.InnerSerializeToByteArrayPool(input),
                        JsonKnownNamingPolicy.MacroCase => Inner<byte, ExcludeNullsMacroCaseResolver<byte>>.InnerSerializeToByteArrayPool(input),
                        JsonKnownNamingPolicy.KebabCase => Inner<byte, ExcludeNullsKebabCaseResolver<byte>>.InnerSerializeToByteArrayPool(input),
                        JsonKnownNamingPolicy.TrainCase => Inner<byte, ExcludeNullsTrainCaseResolver<byte>>.InnerSerializeToByteArrayPool(input),
                        JsonKnownNamingPolicy.CobolCase => Inner<byte, ExcludeNullsCobolCaseResolver<byte>>.InnerSerializeToByteArrayPool(input),
                        _ => Inner<byte, ExcludeNullsOriginalCaseResolver<byte>>.InnerSerializeToByteArrayPool(input),
                    };
                }

                /// <summary>Serialize to stream.</summary>
                /// <param name="input">Input</param>
                /// <param name="stream">Stream</param>
                /// <param name="namingPolicy"></param>
                /// <param name="cancellationToken">CancellationToken</param>
                /// <returns>Task</returns>
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public static ValueTask SerializeAsync(object? input, Stream stream, JsonKnownNamingPolicy namingPolicy, CancellationToken cancellationToken = default)
                {
                    return namingPolicy switch
                    {
                        JsonKnownNamingPolicy.CamelCase => Inner<byte, ExcludeNullsCamelCaseResolver<byte>>.InnerSerializeAsync(input, stream, cancellationToken),
                        JsonKnownNamingPolicy.SnakeCase => Inner<byte, ExcludeNullsSnakeCaseResolver<byte>>.InnerSerializeAsync(input, stream, cancellationToken),
                        JsonKnownNamingPolicy.AdaCase => Inner<byte, ExcludeNullsAdaCaseResolver<byte>>.InnerSerializeAsync(input, stream, cancellationToken),
                        JsonKnownNamingPolicy.MacroCase => Inner<byte, ExcludeNullsMacroCaseResolver<byte>>.InnerSerializeAsync(input, stream, cancellationToken),
                        JsonKnownNamingPolicy.KebabCase => Inner<byte, ExcludeNullsKebabCaseResolver<byte>>.InnerSerializeAsync(input, stream, cancellationToken),
                        JsonKnownNamingPolicy.TrainCase => Inner<byte, ExcludeNullsTrainCaseResolver<byte>>.InnerSerializeAsync(input, stream, cancellationToken),
                        JsonKnownNamingPolicy.CobolCase => Inner<byte, ExcludeNullsCobolCaseResolver<byte>>.InnerSerializeAsync(input, stream, cancellationToken),
                        _ => Inner<byte, ExcludeNullsOriginalCaseResolver<byte>>.InnerSerializeAsync(input, stream, cancellationToken),
                    };
                }

                #endregion

                #region -- Deserialize --

                /// <summary>Deserialize from Byte array.</summary>
                /// <param name="input">Input</param>
                /// <param name="type">Object Type</param>
                /// <param name="namingPolicy"></param>
                /// <returns>Deserialized object</returns>
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public static object? Deserialize(byte[] input, Type type, JsonKnownNamingPolicy namingPolicy)
                {
                    return namingPolicy switch
                    {
                        JsonKnownNamingPolicy.CamelCase => Inner<byte, ExcludeNullsCamelCaseResolver<byte>>.InnerDeserialize(input, type),
                        JsonKnownNamingPolicy.SnakeCase => Inner<byte, ExcludeNullsSnakeCaseResolver<byte>>.InnerDeserialize(input, type),
                        JsonKnownNamingPolicy.AdaCase => Inner<byte, ExcludeNullsAdaCaseResolver<byte>>.InnerDeserialize(input, type),
                        JsonKnownNamingPolicy.MacroCase => Inner<byte, ExcludeNullsMacroCaseResolver<byte>>.InnerDeserialize(input, type),
                        JsonKnownNamingPolicy.KebabCase => Inner<byte, ExcludeNullsKebabCaseResolver<byte>>.InnerDeserialize(input, type),
                        JsonKnownNamingPolicy.TrainCase => Inner<byte, ExcludeNullsTrainCaseResolver<byte>>.InnerDeserialize(input, type),
                        JsonKnownNamingPolicy.CobolCase => Inner<byte, ExcludeNullsCobolCaseResolver<byte>>.InnerDeserialize(input, type),
                        _ => Inner<byte, ExcludeNullsOriginalCaseResolver<byte>>.InnerDeserialize(input, type),
                    };
                }

                /// <summary>Deserialize from Byte array.</summary>
                /// <param name="input">Input</param>
                /// <param name="type">Object Type</param>
                /// <param name="namingPolicy"></param>
                /// <returns>Deserialized object</returns>
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
#if !NETSTANDARD2_0
                public static object? Deserialize(in ArraySegment<byte> input, Type type, JsonKnownNamingPolicy namingPolicy)
#else
                public static object? Deserialize(ArraySegment<byte> input, Type type, JsonKnownNamingPolicy namingPolicy)
#endif
                {
                    return namingPolicy switch
                    {
                        JsonKnownNamingPolicy.CamelCase => Inner<byte, ExcludeNullsCamelCaseResolver<byte>>.InnerDeserialize(input, type),
                        JsonKnownNamingPolicy.SnakeCase => Inner<byte, ExcludeNullsSnakeCaseResolver<byte>>.InnerDeserialize(input, type),
                        JsonKnownNamingPolicy.AdaCase => Inner<byte, ExcludeNullsAdaCaseResolver<byte>>.InnerDeserialize(input, type),
                        JsonKnownNamingPolicy.MacroCase => Inner<byte, ExcludeNullsMacroCaseResolver<byte>>.InnerDeserialize(input, type),
                        JsonKnownNamingPolicy.KebabCase => Inner<byte, ExcludeNullsKebabCaseResolver<byte>>.InnerDeserialize(input, type),
                        JsonKnownNamingPolicy.TrainCase => Inner<byte, ExcludeNullsTrainCaseResolver<byte>>.InnerDeserialize(input, type),
                        JsonKnownNamingPolicy.CobolCase => Inner<byte, ExcludeNullsCobolCaseResolver<byte>>.InnerDeserialize(input, type),
                        _ => Inner<byte, ExcludeNullsOriginalCaseResolver<byte>>.InnerDeserialize(input, type),
                    };
                }

                /// <summary>Deserialize from Byte array.</summary>
                /// <param name="input">Input</param>
                /// <param name="type">Object Type</param>
                /// <param name="namingPolicy"></param>
                /// <returns>Deserialized object</returns>
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public static object? Deserialize(in ReadOnlyMemory<byte> input, Type type, JsonKnownNamingPolicy namingPolicy)
                {
                    return namingPolicy switch
                    {
                        JsonKnownNamingPolicy.CamelCase => Inner<byte, ExcludeNullsCamelCaseResolver<byte>>.InnerDeserialize(input, type),
                        JsonKnownNamingPolicy.SnakeCase => Inner<byte, ExcludeNullsSnakeCaseResolver<byte>>.InnerDeserialize(input, type),
                        JsonKnownNamingPolicy.AdaCase => Inner<byte, ExcludeNullsAdaCaseResolver<byte>>.InnerDeserialize(input, type),
                        JsonKnownNamingPolicy.MacroCase => Inner<byte, ExcludeNullsMacroCaseResolver<byte>>.InnerDeserialize(input, type),
                        JsonKnownNamingPolicy.KebabCase => Inner<byte, ExcludeNullsKebabCaseResolver<byte>>.InnerDeserialize(input, type),
                        JsonKnownNamingPolicy.TrainCase => Inner<byte, ExcludeNullsTrainCaseResolver<byte>>.InnerDeserialize(input, type),
                        JsonKnownNamingPolicy.CobolCase => Inner<byte, ExcludeNullsCobolCaseResolver<byte>>.InnerDeserialize(input, type),
                        _ => Inner<byte, ExcludeNullsOriginalCaseResolver<byte>>.InnerDeserialize(input, type),
                    };
                }

                /// <summary>Deserialize from Byte array.</summary>
                /// <param name="input">Input</param>
                /// <param name="type">Object Type</param>
                /// <param name="namingPolicy"></param>
                /// <returns>Deserialized object</returns>
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public static object? Deserialize(in ReadOnlySpan<byte> input, Type type, JsonKnownNamingPolicy namingPolicy)
                {
                    return namingPolicy switch
                    {
                        JsonKnownNamingPolicy.CamelCase => Inner<byte, ExcludeNullsCamelCaseResolver<byte>>.InnerDeserialize(input, type),
                        JsonKnownNamingPolicy.SnakeCase => Inner<byte, ExcludeNullsSnakeCaseResolver<byte>>.InnerDeserialize(input, type),
                        JsonKnownNamingPolicy.AdaCase => Inner<byte, ExcludeNullsAdaCaseResolver<byte>>.InnerDeserialize(input, type),
                        JsonKnownNamingPolicy.MacroCase => Inner<byte, ExcludeNullsMacroCaseResolver<byte>>.InnerDeserialize(input, type),
                        JsonKnownNamingPolicy.KebabCase => Inner<byte, ExcludeNullsKebabCaseResolver<byte>>.InnerDeserialize(input, type),
                        JsonKnownNamingPolicy.TrainCase => Inner<byte, ExcludeNullsTrainCaseResolver<byte>>.InnerDeserialize(input, type),
                        JsonKnownNamingPolicy.CobolCase => Inner<byte, ExcludeNullsCobolCaseResolver<byte>>.InnerDeserialize(input, type),
                        _ => Inner<byte, ExcludeNullsOriginalCaseResolver<byte>>.InnerDeserialize(input, type),
                    };
                }

                /// <summary>Deserialize from stream.</summary>
                /// <param name="stream">Stream</param>
                /// <param name="type">Object Type</param>
                /// <param name="namingPolicy"></param>
                /// <param name="cancellationToken">CancellationToken</param>
                /// <returns>Task</returns>
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public static ValueTask<object?> DeserializeAsync(Stream stream, Type type, JsonKnownNamingPolicy namingPolicy,
                    CancellationToken cancellationToken = default)
                {
                    return namingPolicy switch
                    {
                        JsonKnownNamingPolicy.CamelCase => Inner<byte, ExcludeNullsCamelCaseResolver<byte>>.InnerDeserializeAsync(stream, type, cancellationToken),
                        JsonKnownNamingPolicy.SnakeCase => Inner<byte, ExcludeNullsSnakeCaseResolver<byte>>.InnerDeserializeAsync(stream, type, cancellationToken),
                        JsonKnownNamingPolicy.AdaCase => Inner<byte, ExcludeNullsAdaCaseResolver<byte>>.InnerDeserializeAsync(stream, type, cancellationToken),
                        JsonKnownNamingPolicy.MacroCase => Inner<byte, ExcludeNullsMacroCaseResolver<byte>>.InnerDeserializeAsync(stream, type, cancellationToken),
                        JsonKnownNamingPolicy.KebabCase => Inner<byte, ExcludeNullsKebabCaseResolver<byte>>.InnerDeserializeAsync(stream, type, cancellationToken),
                        JsonKnownNamingPolicy.TrainCase => Inner<byte, ExcludeNullsTrainCaseResolver<byte>>.InnerDeserializeAsync(stream, type, cancellationToken),
                        JsonKnownNamingPolicy.CobolCase => Inner<byte, ExcludeNullsCobolCaseResolver<byte>>.InnerDeserializeAsync(stream, type, cancellationToken),
                        _ => Inner<byte, ExcludeNullsOriginalCaseResolver<byte>>.InnerDeserializeAsync(stream, type, cancellationToken),
                    };
                }

                #endregion
            }
        }
    }
}