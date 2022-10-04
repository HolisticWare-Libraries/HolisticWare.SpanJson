using System.Runtime.CompilerServices;
using SpanJson.Resolvers;

namespace SpanJson
{
    static partial class JsonSerializer
    {
        static partial class Generic
        {
            /// <summary>Serialize/Deserialize to/from string et al.</summary>
            public static partial class Utf16
            {
                #region -- Serialize --

                /// <summary>Serialize to string.</summary>
                /// <typeparam name="T">Type</typeparam>
                /// <param name="input">Input</param>
                /// <param name="namingPolicy"></param>
                /// <returns>String</returns>
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public static string Serialize<T>(T input, JsonKnownNamingPolicy namingPolicy)
                {
                    return namingPolicy switch
                    {
                        JsonKnownNamingPolicy.CamelCase => Inner<T, char, ExcludeNullsCamelCaseResolver<char>>.InnerSerializeToString(input),
                        JsonKnownNamingPolicy.SnakeCase => Inner<T, char, ExcludeNullsSnakeCaseResolver<char>>.InnerSerializeToString(input),
                        JsonKnownNamingPolicy.AdaCase => Inner<T, char, ExcludeNullsAdaCaseResolver<char>>.InnerSerializeToString(input),
                        JsonKnownNamingPolicy.MacroCase => Inner<T, char, ExcludeNullsMacroCaseResolver<char>>.InnerSerializeToString(input),
                        JsonKnownNamingPolicy.KebabCase => Inner<T, char, ExcludeNullsKebabCaseResolver<char>>.InnerSerializeToString(input),
                        JsonKnownNamingPolicy.TrainCase => Inner<T, char, ExcludeNullsTrainCaseResolver<char>>.InnerSerializeToString(input),
                        JsonKnownNamingPolicy.CobolCase => Inner<T, char, ExcludeNullsCobolCaseResolver<char>>.InnerSerializeToString(input),
                        _ => Inner<T, char, ExcludeNullsOriginalCaseResolver<char>>.InnerSerializeToString(input),
                    };
                }

                /// <summary>Serialize to string.</summary>
                /// <typeparam name="T">Type</typeparam>
                /// <param name="input">Input</param>
                /// <param name="namingPolicy"></param>
                /// <returns>String</returns>
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public static char[] SerializeToCharArray<T>(T input, JsonKnownNamingPolicy namingPolicy)
                {
                    return namingPolicy switch
                    {
                        JsonKnownNamingPolicy.CamelCase => Inner<T, char, ExcludeNullsCamelCaseResolver<char>>.InnerSerializeToCharArray(input),
                        JsonKnownNamingPolicy.SnakeCase => Inner<T, char, ExcludeNullsSnakeCaseResolver<char>>.InnerSerializeToCharArray(input),
                        JsonKnownNamingPolicy.AdaCase => Inner<T, char, ExcludeNullsAdaCaseResolver<char>>.InnerSerializeToCharArray(input),
                        JsonKnownNamingPolicy.MacroCase => Inner<T, char, ExcludeNullsMacroCaseResolver<char>>.InnerSerializeToCharArray(input),
                        JsonKnownNamingPolicy.KebabCase => Inner<T, char, ExcludeNullsKebabCaseResolver<char>>.InnerSerializeToCharArray(input),
                        JsonKnownNamingPolicy.TrainCase => Inner<T, char, ExcludeNullsTrainCaseResolver<char>>.InnerSerializeToCharArray(input),
                        JsonKnownNamingPolicy.CobolCase => Inner<T, char, ExcludeNullsCobolCaseResolver<char>>.InnerSerializeToCharArray(input),
                        _ => Inner<T, char, ExcludeNullsOriginalCaseResolver<char>>.InnerSerializeToCharArray(input),
                    };
                }

                /// <summary>Serialize to char buffer from ArrayPool
                /// The returned ArraySegment's Array needs to be returned to the ArrayPool.</summary>
                /// <typeparam name="T">Type</typeparam>
                /// <param name="input">Input</param>
                /// <param name="namingPolicy"></param>
                /// <returns>Char array from ArrayPool</returns>
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public static ArraySegment<char> SerializeToArrayPool<T>(T input, JsonKnownNamingPolicy namingPolicy)
                {
                    return namingPolicy switch
                    {
                        JsonKnownNamingPolicy.CamelCase => Inner<T, char, ExcludeNullsCamelCaseResolver<char>>.InnerSerializeToCharArrayPool(input),
                        JsonKnownNamingPolicy.SnakeCase => Inner<T, char, ExcludeNullsSnakeCaseResolver<char>>.InnerSerializeToCharArrayPool(input),
                        JsonKnownNamingPolicy.AdaCase => Inner<T, char, ExcludeNullsAdaCaseResolver<char>>.InnerSerializeToCharArrayPool(input),
                        JsonKnownNamingPolicy.MacroCase => Inner<T, char, ExcludeNullsMacroCaseResolver<char>>.InnerSerializeToCharArrayPool(input),
                        JsonKnownNamingPolicy.KebabCase => Inner<T, char, ExcludeNullsKebabCaseResolver<char>>.InnerSerializeToCharArrayPool(input),
                        JsonKnownNamingPolicy.TrainCase => Inner<T, char, ExcludeNullsTrainCaseResolver<char>>.InnerSerializeToCharArrayPool(input),
                        JsonKnownNamingPolicy.CobolCase => Inner<T, char, ExcludeNullsCobolCaseResolver<char>>.InnerSerializeToCharArrayPool(input),
                        _ => Inner<T, char, ExcludeNullsOriginalCaseResolver<char>>.InnerSerializeToCharArrayPool(input),
                    };
                }

                /// <summary>Serialize to TextWriter.</summary>
                /// <typeparam name="T">Type</typeparam>
                /// <param name="input">Input</param>
                /// <param name="writer">Writer</param>
                /// <param name="namingPolicy"></param>
                /// <param name="cancellationToken">CancellationToken</param>
                /// <returns>Task</returns>
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public static ValueTask SerializeAsync<T>(T input, TextWriter writer, JsonKnownNamingPolicy namingPolicy, CancellationToken cancellationToken = default)
                {
                    return namingPolicy switch
                    {
                        JsonKnownNamingPolicy.CamelCase => Inner<T, char, ExcludeNullsCamelCaseResolver<char>>.InnerSerializeAsync(input, writer, cancellationToken),
                        JsonKnownNamingPolicy.SnakeCase => Inner<T, char, ExcludeNullsSnakeCaseResolver<char>>.InnerSerializeAsync(input, writer, cancellationToken),
                        JsonKnownNamingPolicy.AdaCase => Inner<T, char, ExcludeNullsAdaCaseResolver<char>>.InnerSerializeAsync(input, writer, cancellationToken),
                        JsonKnownNamingPolicy.MacroCase => Inner<T, char, ExcludeNullsMacroCaseResolver<char>>.InnerSerializeAsync(input, writer, cancellationToken),
                        JsonKnownNamingPolicy.KebabCase => Inner<T, char, ExcludeNullsKebabCaseResolver<char>>.InnerSerializeAsync(input, writer, cancellationToken),
                        JsonKnownNamingPolicy.TrainCase => Inner<T, char, ExcludeNullsTrainCaseResolver<char>>.InnerSerializeAsync(input, writer, cancellationToken),
                        JsonKnownNamingPolicy.CobolCase => Inner<T, char, ExcludeNullsCobolCaseResolver<char>>.InnerSerializeAsync(input, writer, cancellationToken),
                        _ => Inner<T, char, ExcludeNullsOriginalCaseResolver<char>>.InnerSerializeAsync(input, writer, cancellationToken),
                    };
                }

                #endregion

                #region -- Deserialize --

                /// <summary>Deserialize from string.</summary>
                /// <typeparam name="T">Type</typeparam>
                /// <param name="input">Input</param>
                /// <param name="namingPolicy"></param>
                /// <returns>Deserialized object</returns>
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public static T? Deserialize<T>(string input, JsonKnownNamingPolicy namingPolicy)
                {
                    return namingPolicy switch
                    {
                        JsonKnownNamingPolicy.CamelCase => Inner<T, char, ExcludeNullsCamelCaseResolver<char>>.InnerDeserialize(input
#if NETSTANDARD2_0
                            .AsSpan()
#endif
                        ),
                        JsonKnownNamingPolicy.SnakeCase => Inner<T, char, ExcludeNullsSnakeCaseResolver<char>>.InnerDeserialize(input
#if NETSTANDARD2_0
                            .AsSpan()
#endif
                        ),
                        JsonKnownNamingPolicy.AdaCase => Inner<T, char, ExcludeNullsAdaCaseResolver<char>>.InnerDeserialize(input
#if NETSTANDARD2_0
                            .AsSpan()
#endif
                        ),
                        JsonKnownNamingPolicy.MacroCase => Inner<T, char, ExcludeNullsMacroCaseResolver<char>>.InnerDeserialize(input
#if NETSTANDARD2_0
                            .AsSpan()
#endif
                        ),
                        JsonKnownNamingPolicy.KebabCase => Inner<T, char, ExcludeNullsKebabCaseResolver<char>>.InnerDeserialize(input
#if NETSTANDARD2_0
                            .AsSpan()
#endif
                        ),
                        JsonKnownNamingPolicy.TrainCase => Inner<T, char, ExcludeNullsTrainCaseResolver<char>>.InnerDeserialize(input
#if NETSTANDARD2_0
                            .AsSpan()
#endif
                        ),
                        JsonKnownNamingPolicy.CobolCase => Inner<T, char, ExcludeNullsCobolCaseResolver<char>>.InnerDeserialize(input
#if NETSTANDARD2_0
                            .AsSpan()
#endif
                        ),
                        _ => Inner<T, char, ExcludeNullsOriginalCaseResolver<char>>.InnerDeserialize(input
#if NETSTANDARD2_0
                            .AsSpan()
#endif
                        ),
                    };
                }

                /// <summary>Deserialize from string.</summary>
                /// <typeparam name="T">Type</typeparam>
                /// <param name="input">Input</param>
                /// <param name="namingPolicy"></param>
                /// <returns>Deserialized object</returns>
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public static T? Deserialize<T>(char[] input, JsonKnownNamingPolicy namingPolicy)
                {
                    return namingPolicy switch
                    {
                        JsonKnownNamingPolicy.CamelCase => Inner<T, char, ExcludeNullsCamelCaseResolver<char>>.InnerDeserialize(input),
                        JsonKnownNamingPolicy.SnakeCase => Inner<T, char, ExcludeNullsSnakeCaseResolver<char>>.InnerDeserialize(input),
                        JsonKnownNamingPolicy.AdaCase => Inner<T, char, ExcludeNullsAdaCaseResolver<char>>.InnerDeserialize(input),
                        JsonKnownNamingPolicy.MacroCase => Inner<T, char, ExcludeNullsMacroCaseResolver<char>>.InnerDeserialize(input),
                        JsonKnownNamingPolicy.KebabCase => Inner<T, char, ExcludeNullsKebabCaseResolver<char>>.InnerDeserialize(input),
                        JsonKnownNamingPolicy.TrainCase => Inner<T, char, ExcludeNullsTrainCaseResolver<char>>.InnerDeserialize(input),
                        JsonKnownNamingPolicy.CobolCase => Inner<T, char, ExcludeNullsCobolCaseResolver<char>>.InnerDeserialize(input),
                        _ => Inner<T, char, ExcludeNullsOriginalCaseResolver<char>>.InnerDeserialize(input),
                    };
                }

                /// <summary>Deserialize from string.</summary>
                /// <typeparam name="T">Type</typeparam>
                /// <param name="input">Input</param>
                /// <param name="namingPolicy"></param>
                /// <returns>Deserialized object</returns>
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
#if !NETSTANDARD2_0
                public static T? Deserialize<T>(in ArraySegment<char> input, JsonKnownNamingPolicy namingPolicy)
#else
                public static T? Deserialize<T>(ArraySegment<char> input, JsonKnownNamingPolicy namingPolicy)
#endif
                {
                    return namingPolicy switch
                    {
                        JsonKnownNamingPolicy.CamelCase => Inner<T, char, ExcludeNullsCamelCaseResolver<char>>.InnerDeserialize(input),
                        JsonKnownNamingPolicy.SnakeCase => Inner<T, char, ExcludeNullsSnakeCaseResolver<char>>.InnerDeserialize(input),
                        JsonKnownNamingPolicy.AdaCase => Inner<T, char, ExcludeNullsAdaCaseResolver<char>>.InnerDeserialize(input),
                        JsonKnownNamingPolicy.MacroCase => Inner<T, char, ExcludeNullsMacroCaseResolver<char>>.InnerDeserialize(input),
                        JsonKnownNamingPolicy.KebabCase => Inner<T, char, ExcludeNullsKebabCaseResolver<char>>.InnerDeserialize(input),
                        JsonKnownNamingPolicy.TrainCase => Inner<T, char, ExcludeNullsTrainCaseResolver<char>>.InnerDeserialize(input),
                        JsonKnownNamingPolicy.CobolCase => Inner<T, char, ExcludeNullsCobolCaseResolver<char>>.InnerDeserialize(input),
                        _ => Inner<T, char, ExcludeNullsOriginalCaseResolver<char>>.InnerDeserialize(input),
                    };
                }

                /// <summary>Deserialize from string.</summary>
                /// <typeparam name="T">Type</typeparam>
                /// <param name="input">Input</param>
                /// <param name="namingPolicy"></param>
                /// <returns>Deserialized object</returns>
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public static T? Deserialize<T>(in ReadOnlyMemory<char> input, JsonKnownNamingPolicy namingPolicy)
                {
                    return namingPolicy switch
                    {
                        JsonKnownNamingPolicy.CamelCase => Inner<T, char, ExcludeNullsCamelCaseResolver<char>>.InnerDeserialize(input),
                        JsonKnownNamingPolicy.SnakeCase => Inner<T, char, ExcludeNullsSnakeCaseResolver<char>>.InnerDeserialize(input),
                        JsonKnownNamingPolicy.AdaCase => Inner<T, char, ExcludeNullsAdaCaseResolver<char>>.InnerDeserialize(input),
                        JsonKnownNamingPolicy.MacroCase => Inner<T, char, ExcludeNullsMacroCaseResolver<char>>.InnerDeserialize(input),
                        JsonKnownNamingPolicy.KebabCase => Inner<T, char, ExcludeNullsKebabCaseResolver<char>>.InnerDeserialize(input),
                        JsonKnownNamingPolicy.TrainCase => Inner<T, char, ExcludeNullsTrainCaseResolver<char>>.InnerDeserialize(input),
                        JsonKnownNamingPolicy.CobolCase => Inner<T, char, ExcludeNullsCobolCaseResolver<char>>.InnerDeserialize(input),
                        _ => Inner<T, char, ExcludeNullsOriginalCaseResolver<char>>.InnerDeserialize(input),
                    };
                }

                /// <summary>Deserialize from string.</summary>
                /// <typeparam name="T">Type</typeparam>
                /// <param name="input">Input</param>
                /// <param name="namingPolicy"></param>
                /// <returns>Deserialized object</returns>
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public static T? Deserialize<T>(in ReadOnlySpan<char> input, JsonKnownNamingPolicy namingPolicy)
                {
                    return namingPolicy switch
                    {
                        JsonKnownNamingPolicy.CamelCase => Inner<T, char, ExcludeNullsCamelCaseResolver<char>>.InnerDeserialize(input),
                        JsonKnownNamingPolicy.SnakeCase => Inner<T, char, ExcludeNullsSnakeCaseResolver<char>>.InnerDeserialize(input),
                        JsonKnownNamingPolicy.AdaCase => Inner<T, char, ExcludeNullsAdaCaseResolver<char>>.InnerDeserialize(input),
                        JsonKnownNamingPolicy.MacroCase => Inner<T, char, ExcludeNullsMacroCaseResolver<char>>.InnerDeserialize(input),
                        JsonKnownNamingPolicy.KebabCase => Inner<T, char, ExcludeNullsKebabCaseResolver<char>>.InnerDeserialize(input),
                        JsonKnownNamingPolicy.TrainCase => Inner<T, char, ExcludeNullsTrainCaseResolver<char>>.InnerDeserialize(input),
                        JsonKnownNamingPolicy.CobolCase => Inner<T, char, ExcludeNullsCobolCaseResolver<char>>.InnerDeserialize(input),
                        _ => Inner<T, char, ExcludeNullsOriginalCaseResolver<char>>.InnerDeserialize(input),
                    };
                }

                /// <summary>Deserialize from TextReader.</summary>
                /// <typeparam name="T">Type</typeparam>
                /// <param name="reader">TextReader</param>
                /// <param name="namingPolicy"></param>
                /// <param name="cancellationToken">CancellationToken</param>
                /// <returns>Deserialized object</returns>
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public static ValueTask<T?> DeserializeAsync<T>(TextReader reader, JsonKnownNamingPolicy namingPolicy, CancellationToken cancellationToken = default)
                {
                    return namingPolicy switch
                    {
                        JsonKnownNamingPolicy.CamelCase => Inner<T, char, ExcludeNullsCamelCaseResolver<char>>.InnerDeserializeAsync(reader, cancellationToken),
                        JsonKnownNamingPolicy.SnakeCase => Inner<T, char, ExcludeNullsSnakeCaseResolver<char>>.InnerDeserializeAsync(reader, cancellationToken),
                        JsonKnownNamingPolicy.AdaCase => Inner<T, char, ExcludeNullsAdaCaseResolver<char>>.InnerDeserializeAsync(reader, cancellationToken),
                        JsonKnownNamingPolicy.MacroCase => Inner<T, char, ExcludeNullsMacroCaseResolver<char>>.InnerDeserializeAsync(reader, cancellationToken),
                        JsonKnownNamingPolicy.KebabCase => Inner<T, char, ExcludeNullsKebabCaseResolver<char>>.InnerDeserializeAsync(reader, cancellationToken),
                        JsonKnownNamingPolicy.TrainCase => Inner<T, char, ExcludeNullsTrainCaseResolver<char>>.InnerDeserializeAsync(reader, cancellationToken),
                        JsonKnownNamingPolicy.CobolCase => Inner<T, char, ExcludeNullsCobolCaseResolver<char>>.InnerDeserializeAsync(reader, cancellationToken),
                        _ => Inner<T, char, ExcludeNullsOriginalCaseResolver<char>>.InnerDeserializeAsync(reader, cancellationToken),
                    };
                }

                #endregion
            }
        }
    }
}