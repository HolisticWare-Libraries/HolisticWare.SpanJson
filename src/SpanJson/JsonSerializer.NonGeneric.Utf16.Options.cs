using System.Runtime.CompilerServices;
using SpanJson.Resolvers;

namespace SpanJson
{
    static partial class JsonSerializer
    {
        static partial class NonGeneric
        {
            /// <summary>Serialize/Deserialize to/from string et al.</summary>
            static partial class Utf16
            {
                #region -- Serialize --

                /// <summary>Serialize to string.</summary>
                /// <param name="input">Input</param>
                /// <param name="namingPolicy"></param>
                /// <returns>String</returns>
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public static string Serialize(object? input, JsonKnownNamingPolicy namingPolicy)
                {
                    return namingPolicy switch
                    {
                        JsonKnownNamingPolicy.CamelCase => Inner<char, ExcludeNullsCamelCaseResolver<char>>.InnerSerializeToString(input),
                        JsonKnownNamingPolicy.SnakeCase => Inner<char, ExcludeNullsSnakeCaseResolver<char>>.InnerSerializeToString(input),
                        JsonKnownNamingPolicy.AdaCase => Inner<char, ExcludeNullsAdaCaseResolver<char>>.InnerSerializeToString(input),
                        JsonKnownNamingPolicy.MacroCase => Inner<char, ExcludeNullsMacroCaseResolver<char>>.InnerSerializeToString(input),
                        JsonKnownNamingPolicy.KebabCase => Inner<char, ExcludeNullsKebabCaseResolver<char>>.InnerSerializeToString(input),
                        JsonKnownNamingPolicy.TrainCase => Inner<char, ExcludeNullsTrainCaseResolver<char>>.InnerSerializeToString(input),
                        JsonKnownNamingPolicy.CobolCase => Inner<char, ExcludeNullsCobolCaseResolver<char>>.InnerSerializeToString(input),
                        _ => Inner<char, ExcludeNullsOriginalCaseResolver<char>>.InnerSerializeToString(input),
                    };
                }

                /// <summary>Serialize to string.</summary>
                /// <param name="input">Input</param>
                /// <param name="namingPolicy"></param>
                /// <returns>String</returns>
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public static char[] SerializeToCharArray(object? input, JsonKnownNamingPolicy namingPolicy)
                {
                    return namingPolicy switch
                    {
                        JsonKnownNamingPolicy.CamelCase => Inner<char, ExcludeNullsCamelCaseResolver<char>>.InnerSerializeToCharArray(input),
                        JsonKnownNamingPolicy.SnakeCase => Inner<char, ExcludeNullsSnakeCaseResolver<char>>.InnerSerializeToCharArray(input),
                        JsonKnownNamingPolicy.AdaCase => Inner<char, ExcludeNullsAdaCaseResolver<char>>.InnerSerializeToCharArray(input),
                        JsonKnownNamingPolicy.MacroCase => Inner<char, ExcludeNullsMacroCaseResolver<char>>.InnerSerializeToCharArray(input),
                        JsonKnownNamingPolicy.KebabCase => Inner<char, ExcludeNullsKebabCaseResolver<char>>.InnerSerializeToCharArray(input),
                        JsonKnownNamingPolicy.TrainCase => Inner<char, ExcludeNullsTrainCaseResolver<char>>.InnerSerializeToCharArray(input),
                        JsonKnownNamingPolicy.CobolCase => Inner<char, ExcludeNullsCobolCaseResolver<char>>.InnerSerializeToCharArray(input),
                        _ => Inner<char, ExcludeNullsOriginalCaseResolver<char>>.InnerSerializeToCharArray(input),
                    };
                }

                /// <summary>Serialize to char buffer from ArrayPool.
                /// The returned ArraySegment's Array needs to be returned to the ArrayPool.</summary>
                /// <param name="input">Input</param>
                /// <param name="namingPolicy"></param>
                /// <returns>Char array from ArrayPool</returns>
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public static ArraySegment<char> SerializeToArrayPool(object? input, JsonKnownNamingPolicy namingPolicy)
                {
                    return namingPolicy switch
                    {
                        JsonKnownNamingPolicy.CamelCase => Inner<char, ExcludeNullsCamelCaseResolver<char>>.InnerSerializeToCharArrayPool(input),
                        JsonKnownNamingPolicy.SnakeCase => Inner<char, ExcludeNullsSnakeCaseResolver<char>>.InnerSerializeToCharArrayPool(input),
                        JsonKnownNamingPolicy.AdaCase => Inner<char, ExcludeNullsAdaCaseResolver<char>>.InnerSerializeToCharArrayPool(input),
                        JsonKnownNamingPolicy.MacroCase => Inner<char, ExcludeNullsMacroCaseResolver<char>>.InnerSerializeToCharArrayPool(input),
                        JsonKnownNamingPolicy.KebabCase => Inner<char, ExcludeNullsKebabCaseResolver<char>>.InnerSerializeToCharArrayPool(input),
                        JsonKnownNamingPolicy.TrainCase => Inner<char, ExcludeNullsTrainCaseResolver<char>>.InnerSerializeToCharArrayPool(input),
                        JsonKnownNamingPolicy.CobolCase => Inner<char, ExcludeNullsCobolCaseResolver<char>>.InnerSerializeToCharArrayPool(input),
                        _ => Inner<char, ExcludeNullsOriginalCaseResolver<char>>.InnerSerializeToCharArrayPool(input),
                    };
                }

                /// <summary>Serialize to TextWriter.</summary>
                /// <param name="input">Input</param>
                /// <param name="namingPolicy"></param>
                /// <param name="writer">TextWriter</param>
                /// <param name="cancellationToken">CancellationToken</param>
                /// <returns>Task</returns>
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public static ValueTask SerializeAsync(object? input, JsonKnownNamingPolicy namingPolicy, TextWriter writer, CancellationToken cancellationToken = default)
                {
                    return namingPolicy switch
                    {
                        JsonKnownNamingPolicy.CamelCase => Inner<char, ExcludeNullsCamelCaseResolver<char>>.InnerSerializeAsync(input, writer, cancellationToken),
                        JsonKnownNamingPolicy.SnakeCase => Inner<char, ExcludeNullsSnakeCaseResolver<char>>.InnerSerializeAsync(input, writer, cancellationToken),
                        JsonKnownNamingPolicy.AdaCase => Inner<char, ExcludeNullsAdaCaseResolver<char>>.InnerSerializeAsync(input, writer, cancellationToken),
                        JsonKnownNamingPolicy.MacroCase => Inner<char, ExcludeNullsMacroCaseResolver<char>>.InnerSerializeAsync(input, writer, cancellationToken),
                        JsonKnownNamingPolicy.KebabCase => Inner<char, ExcludeNullsKebabCaseResolver<char>>.InnerSerializeAsync(input, writer, cancellationToken),
                        JsonKnownNamingPolicy.TrainCase => Inner<char, ExcludeNullsTrainCaseResolver<char>>.InnerSerializeAsync(input, writer, cancellationToken),
                        JsonKnownNamingPolicy.CobolCase => Inner<char, ExcludeNullsCobolCaseResolver<char>>.InnerSerializeAsync(input, writer, cancellationToken),
                        _ => Inner<char, ExcludeNullsOriginalCaseResolver<char>>.InnerSerializeAsync(input, writer, cancellationToken),
                    };
                }

                #endregion

                #region -- Deserialize --

                /// <summary>Deserialize from string.</summary>
                /// <param name="input">Input</param>
                /// <param name="type">Object Type</param>
                /// <param name="namingPolicy"></param>
                /// <returns>Deserialized object</returns>
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public static object? Deserialize(string input, Type type, JsonKnownNamingPolicy namingPolicy)
                {
                    switch (namingPolicy)
                    {
                        case JsonKnownNamingPolicy.CamelCase:
                            return Inner<char, ExcludeNullsCamelCaseResolver<char>>.InnerDeserialize(input
#if NETSTANDARD2_0
                                .AsSpan()
#endif
                                , type);
                        case JsonKnownNamingPolicy.SnakeCase:
                            return Inner<char, ExcludeNullsSnakeCaseResolver<char>>.InnerDeserialize(input
#if NETSTANDARD2_0
                                .AsSpan()
#endif
                                , type);
                        case JsonKnownNamingPolicy.AdaCase:
                            return Inner<char, ExcludeNullsAdaCaseResolver<char>>.InnerDeserialize(input
#if NETSTANDARD2_0
                                .AsSpan()
#endif
                                , type);
                        case JsonKnownNamingPolicy.MacroCase:
                            return Inner<char, ExcludeNullsMacroCaseResolver<char>>.InnerDeserialize(input
#if NETSTANDARD2_0
                                .AsSpan()
#endif
                                , type);
                        case JsonKnownNamingPolicy.KebabCase:
                            return Inner<char, ExcludeNullsKebabCaseResolver<char>>.InnerDeserialize(input
#if NETSTANDARD2_0
                                .AsSpan()
#endif
                                , type);
                        case JsonKnownNamingPolicy.TrainCase:
                            return Inner<char, ExcludeNullsTrainCaseResolver<char>>.InnerDeserialize(input
#if NETSTANDARD2_0
                                .AsSpan()
#endif
                                , type);
                        case JsonKnownNamingPolicy.CobolCase:
                            return Inner<char, ExcludeNullsCobolCaseResolver<char>>.InnerDeserialize(input
#if NETSTANDARD2_0
                                .AsSpan()
#endif
                                , type);
                        case JsonKnownNamingPolicy.Unspecified:
                        default:
                            return Inner<char, ExcludeNullsOriginalCaseResolver<char>>.InnerDeserialize(input
#if NETSTANDARD2_0
                                .AsSpan()
#endif
                                , type);
                    }
                }

                /// <summary>Deserialize from string.</summary>
                /// <param name="input">Input</param>
                /// <param name="type">Object Type</param>
                /// <param name="namingPolicy"></param>
                /// <returns>Deserialized object</returns>
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public static object? Deserialize(char[] input, Type type, JsonKnownNamingPolicy namingPolicy)
                {
                    return namingPolicy switch
                    {
                        JsonKnownNamingPolicy.CamelCase => Inner<char, ExcludeNullsCamelCaseResolver<char>>.InnerDeserialize(input, type),
                        JsonKnownNamingPolicy.SnakeCase => Inner<char, ExcludeNullsSnakeCaseResolver<char>>.InnerDeserialize(input, type),
                        JsonKnownNamingPolicy.AdaCase => Inner<char, ExcludeNullsAdaCaseResolver<char>>.InnerDeserialize(input, type),
                        JsonKnownNamingPolicy.MacroCase => Inner<char, ExcludeNullsMacroCaseResolver<char>>.InnerDeserialize(input, type),
                        JsonKnownNamingPolicy.KebabCase => Inner<char, ExcludeNullsKebabCaseResolver<char>>.InnerDeserialize(input, type),
                        JsonKnownNamingPolicy.TrainCase => Inner<char, ExcludeNullsTrainCaseResolver<char>>.InnerDeserialize(input, type),
                        JsonKnownNamingPolicy.CobolCase => Inner<char, ExcludeNullsCobolCaseResolver<char>>.InnerDeserialize(input, type),
                        _ => Inner<char, ExcludeNullsOriginalCaseResolver<char>>.InnerDeserialize(input, type),
                    };
                }

                /// <summary>Deserialize from string.</summary>
                /// <param name="input">Input</param>
                /// <param name="type">Object Type</param>
                /// <param name="namingPolicy"></param>
                /// <returns>Deserialized object</returns>
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
#if !NETSTANDARD2_0
                public static object? Deserialize(in ArraySegment<char> input, Type type, JsonKnownNamingPolicy namingPolicy)
#else
                public static object? Deserialize(ArraySegment<char> input, Type type, JsonKnownNamingPolicy namingPolicy)
#endif
                {
                    return namingPolicy switch
                    {
                        JsonKnownNamingPolicy.CamelCase => Inner<char, ExcludeNullsCamelCaseResolver<char>>.InnerDeserialize(input, type),
                        JsonKnownNamingPolicy.SnakeCase => Inner<char, ExcludeNullsSnakeCaseResolver<char>>.InnerDeserialize(input, type),
                        JsonKnownNamingPolicy.AdaCase => Inner<char, ExcludeNullsAdaCaseResolver<char>>.InnerDeserialize(input, type),
                        JsonKnownNamingPolicy.MacroCase => Inner<char, ExcludeNullsMacroCaseResolver<char>>.InnerDeserialize(input, type),
                        JsonKnownNamingPolicy.KebabCase => Inner<char, ExcludeNullsKebabCaseResolver<char>>.InnerDeserialize(input, type),
                        JsonKnownNamingPolicy.TrainCase => Inner<char, ExcludeNullsTrainCaseResolver<char>>.InnerDeserialize(input, type),
                        JsonKnownNamingPolicy.CobolCase => Inner<char, ExcludeNullsCobolCaseResolver<char>>.InnerDeserialize(input, type),
                        _ => Inner<char, ExcludeNullsOriginalCaseResolver<char>>.InnerDeserialize(input, type),
                    };
                }

                /// <summary>Deserialize from string.</summary>
                /// <param name="input">Input</param>
                /// <param name="type">Object Type</param>
                /// <param name="namingPolicy"></param>
                /// <returns>Deserialized object</returns>
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public static object? Deserialize(in ReadOnlyMemory<char> input, Type type, JsonKnownNamingPolicy namingPolicy)
                {
                    return namingPolicy switch
                    {
                        JsonKnownNamingPolicy.CamelCase => Inner<char, ExcludeNullsCamelCaseResolver<char>>.InnerDeserialize(input, type),
                        JsonKnownNamingPolicy.SnakeCase => Inner<char, ExcludeNullsSnakeCaseResolver<char>>.InnerDeserialize(input, type),
                        JsonKnownNamingPolicy.AdaCase => Inner<char, ExcludeNullsAdaCaseResolver<char>>.InnerDeserialize(input, type),
                        JsonKnownNamingPolicy.MacroCase => Inner<char, ExcludeNullsMacroCaseResolver<char>>.InnerDeserialize(input, type),
                        JsonKnownNamingPolicy.KebabCase => Inner<char, ExcludeNullsKebabCaseResolver<char>>.InnerDeserialize(input, type),
                        JsonKnownNamingPolicy.TrainCase => Inner<char, ExcludeNullsTrainCaseResolver<char>>.InnerDeserialize(input, type),
                        JsonKnownNamingPolicy.CobolCase => Inner<char, ExcludeNullsCobolCaseResolver<char>>.InnerDeserialize(input, type),
                        _ => Inner<char, ExcludeNullsOriginalCaseResolver<char>>.InnerDeserialize(input, type),
                    };
                }

                /// <summary>Deserialize from string.</summary>
                /// <param name="input">Input</param>
                /// <param name="type">Object Type</param>
                /// <param name="namingPolicy"></param>
                /// <returns>Deserialized object</returns>
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public static object? Deserialize(in ReadOnlySpan<char> input, Type type, JsonKnownNamingPolicy namingPolicy)
                {
                    return namingPolicy switch
                    {
                        JsonKnownNamingPolicy.CamelCase => Inner<char, ExcludeNullsCamelCaseResolver<char>>.InnerDeserialize(input, type),
                        JsonKnownNamingPolicy.SnakeCase => Inner<char, ExcludeNullsSnakeCaseResolver<char>>.InnerDeserialize(input, type),
                        JsonKnownNamingPolicy.AdaCase => Inner<char, ExcludeNullsAdaCaseResolver<char>>.InnerDeserialize(input, type),
                        JsonKnownNamingPolicy.MacroCase => Inner<char, ExcludeNullsMacroCaseResolver<char>>.InnerDeserialize(input, type),
                        JsonKnownNamingPolicy.KebabCase => Inner<char, ExcludeNullsKebabCaseResolver<char>>.InnerDeserialize(input, type),
                        JsonKnownNamingPolicy.TrainCase => Inner<char, ExcludeNullsTrainCaseResolver<char>>.InnerDeserialize(input, type),
                        JsonKnownNamingPolicy.CobolCase => Inner<char, ExcludeNullsCobolCaseResolver<char>>.InnerDeserialize(input, type),
                        _ => Inner<char, ExcludeNullsOriginalCaseResolver<char>>.InnerDeserialize(input, type),
                    };
                }

                /// <summary>Deserialize from TextReader.</summary>
                /// <param name="reader">TextReader</param>
                /// <param name="type">Object Type</param>
                /// <param name="namingPolicy"></param>
                /// <param name="cancellationToken">CancellationToken</param>
                /// <returns>Task</returns>
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public static ValueTask<object?> DeserializeAsync(TextReader reader, Type type, JsonKnownNamingPolicy namingPolicy,
                    CancellationToken cancellationToken = default)
                {
                    return namingPolicy switch
                    {
                        JsonKnownNamingPolicy.CamelCase => Inner<char, ExcludeNullsCamelCaseResolver<char>>.InnerDeserializeAsync(reader, type, cancellationToken),
                        JsonKnownNamingPolicy.SnakeCase => Inner<char, ExcludeNullsSnakeCaseResolver<char>>.InnerDeserializeAsync(reader, type, cancellationToken),
                        JsonKnownNamingPolicy.AdaCase => Inner<char, ExcludeNullsAdaCaseResolver<char>>.InnerDeserializeAsync(reader, type, cancellationToken),
                        JsonKnownNamingPolicy.MacroCase => Inner<char, ExcludeNullsMacroCaseResolver<char>>.InnerDeserializeAsync(reader, type, cancellationToken),
                        JsonKnownNamingPolicy.KebabCase => Inner<char, ExcludeNullsKebabCaseResolver<char>>.InnerDeserializeAsync(reader, type, cancellationToken),
                        JsonKnownNamingPolicy.TrainCase => Inner<char, ExcludeNullsTrainCaseResolver<char>>.InnerDeserializeAsync(reader, type, cancellationToken),
                        JsonKnownNamingPolicy.CobolCase => Inner<char, ExcludeNullsCobolCaseResolver<char>>.InnerDeserializeAsync(reader, type, cancellationToken),
                        _ => Inner<char, ExcludeNullsOriginalCaseResolver<char>>.InnerDeserializeAsync(reader, type, cancellationToken),
                    };
                }

                #endregion
            }
        }
    }
}