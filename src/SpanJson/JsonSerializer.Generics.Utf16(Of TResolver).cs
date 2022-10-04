using System.Runtime.CompilerServices;

namespace SpanJson
{
    static partial class JsonSerializer
    {
        static partial class Generic
        {
            /// <summary>Serialize/Deserialize to/from string et al.</summary>
            static partial class Utf16
            {
                #region -- Serialize --

                /// <summary>Serialize to string with specific resolver.</summary>
                /// <typeparam name="T">Type</typeparam>
                /// <typeparam name="TResolver">Resolver</typeparam>
                /// <param name="input">Input</param>
                /// <returns>String</returns>
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public static string Serialize<T, TResolver>(T input)
                    where TResolver : IJsonFormatterResolver<char, TResolver>, new()
                {
                    return Inner<T, char, TResolver>.InnerSerializeToString(input);
                }

                /// <summary>Serialize to string with specific resolver.</summary>
                /// <typeparam name="T">Type</typeparam>
                /// <typeparam name="TResolver">Resolver</typeparam>
                /// <param name="input">Input</param>
                /// <returns>String</returns>
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public static char[] SerializeToCharArray<T, TResolver>(T input)
                    where TResolver : IJsonFormatterResolver<char, TResolver>, new()
                {
                    return Inner<T, char, TResolver>.InnerSerializeToCharArray(input);
                }

                /// <summary>Serialize to string with specific resolver.
                /// The returned ArraySegment's Array needs to be returned to the ArrayPool.</summary>
                /// <typeparam name="T">Type</typeparam>
                /// <typeparam name="TResolver">Resolver</typeparam>
                /// <param name="input">Input</param>
                /// <returns>String</returns>
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public static ArraySegment<char> SerializeToArrayPool<T, TResolver>(T input)
                    where TResolver : IJsonFormatterResolver<char, TResolver>, new()
                {
                    return Inner<T, char, TResolver>.InnerSerializeToCharArrayPool(input);
                }

                /// <summary>Serialize to TextWriter with specific resolver.</summary>
                /// <typeparam name="T">Type</typeparam>
                /// <typeparam name="TResolver">Resolver</typeparam>
                /// <param name="input">Input</param>
                /// <param name="writer">Writer</param>
                /// <param name="cancellationToken">CancellationToken</param>
                /// <returns>Task</returns>
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public static ValueTask SerializeAsync<T, TResolver>(T input, TextWriter writer, CancellationToken cancellationToken = default)
                    where TResolver : IJsonFormatterResolver<char, TResolver>, new()
                {
                    return Inner<T, char, TResolver>.InnerSerializeAsync(input, writer, cancellationToken);
                }

                #endregion

                #region -- Deserialize --

                /// <summary>Deserialize from string with specific resolver.</summary>
                /// <typeparam name="T">Type</typeparam>
                /// <typeparam name="TResolver">Resolver</typeparam>
                /// <param name="input">Input</param>
                /// <returns>Deserialized object</returns>
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public static T? Deserialize<T, TResolver>(string input)
                    where TResolver : IJsonFormatterResolver<char, TResolver>, new()
                {
#if NETSTANDARD2_0
                    return Inner<T, char, TResolver>.InnerDeserialize(input.AsSpan());
#else
                    return Inner<T, char, TResolver>.InnerDeserialize(input);
#endif
                }

                /// <summary>Deserialize from string with specific resolver.</summary>
                /// <typeparam name="T">Type</typeparam>
                /// <typeparam name="TResolver">Resolver</typeparam>
                /// <param name="input">Input</param>
                /// <returns>Deserialized object</returns>
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public static T? Deserialize<T, TResolver>(char[] input)
                    where TResolver : IJsonFormatterResolver<char, TResolver>, new()
                {
                    return Inner<T, char, TResolver>.InnerDeserialize(input);
                }

                /// <summary>Deserialize from string with specific resolver.</summary>
                /// <typeparam name="T">Type</typeparam>
                /// <typeparam name="TResolver">Resolver</typeparam>
                /// <param name="input">Input</param>
                /// <returns>Deserialized object</returns>
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
#if !NETSTANDARD2_0
                public static T? Deserialize<T, TResolver>(in ArraySegment<char> input)
#else
                public static T? Deserialize<T, TResolver>(ArraySegment<char> input)
#endif
                    where TResolver : IJsonFormatterResolver<char, TResolver>, new()
                {
                    return Inner<T, char, TResolver>.InnerDeserialize(input);
                }

                /// <summary>Deserialize from string with specific resolver.</summary>
                /// <typeparam name="T">Type</typeparam>
                /// <typeparam name="TResolver">Resolver</typeparam>
                /// <param name="input">Input</param>
                /// <returns>Deserialized object</returns>
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public static T? Deserialize<T, TResolver>(in ReadOnlyMemory<char> input)
                    where TResolver : IJsonFormatterResolver<char, TResolver>, new()
                {
                    return Inner<T, char, TResolver>.InnerDeserialize(input);
                }

                /// <summary>Deserialize from string with specific resolver.</summary>
                /// <typeparam name="T">Type</typeparam>
                /// <typeparam name="TResolver">Resolver</typeparam>
                /// <param name="input">Input</param>
                /// <returns>Deserialized object</returns>
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public static T? Deserialize<T, TResolver>(in ReadOnlySpan<char> input)
                    where TResolver : IJsonFormatterResolver<char, TResolver>, new()
                {
                    return Inner<T, char, TResolver>.InnerDeserialize(input);
                }

                /// <summary>Deserialize from TextReader with specific resolver.</summary>
                /// <typeparam name="T">Type</typeparam>
                /// <typeparam name="TResolver">Resolver</typeparam>
                /// <param name="reader">TextReader</param>
                /// <param name="cancellationToken">CancellationToken</param>
                /// <returns>Task</returns>
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public static ValueTask<T?> DeserializeAsync<T, TResolver>(TextReader reader, CancellationToken cancellationToken = default)
                    where TResolver : IJsonFormatterResolver<char, TResolver>, new()
                {
                    return Inner<T, char, TResolver>.InnerDeserializeAsync(reader, cancellationToken);
                }

                #endregion
            }
        }
    }
}