using System.Runtime.CompilerServices;

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

                /// <summary>Serialize to string with specific resolver.</summary>
                /// <typeparam name="TResolver">Resolver</typeparam>
                /// <param name="input">Input</param>
                /// <returns>String</returns>
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public static string Serialize<TResolver>(object? input) where TResolver : IJsonFormatterResolver<char, TResolver>, new()
                {
                    return Inner<char, TResolver>.InnerSerializeToString(input);
                }

                /// <summary>Serialize to string with specific resolver.</summary>
                /// <typeparam name="TResolver">Resolver</typeparam>
                /// <param name="input">Input</param>
                /// <returns>String</returns>
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public static char[] SerializeToCharArray<TResolver>(object? input) where TResolver : IJsonFormatterResolver<char, TResolver>, new()
                {
                    return Inner<char, TResolver>.InnerSerializeToCharArray(input);
                }

                /// <summary>Serialize to char array from Array Pool with specific resolver.
                /// The returned ArraySegment's Array needs to be returned to the ArrayPool.</summary>
                /// <typeparam name="TResolver">Resolver</typeparam>
                /// <param name="input">Input</param>
                /// <returns>Char array from Array Pool</returns>
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public static ArraySegment<char> SerializeToArrayPool<TResolver>(object? input) where TResolver : IJsonFormatterResolver<char, TResolver>, new()
                {
                    return Inner<char, TResolver>.InnerSerializeToCharArrayPool(input);
                }

                /// <summary>Serialize to TextWriter with specific resolver.</summary>
                /// <typeparam name="TResolver">Resolver</typeparam>
                /// <param name="input">Input</param>
                /// <param name="writer">TextWriter</param>
                /// <param name="cancellationToken">CancellationToken</param>
                /// <returns>Task</returns>
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public static ValueTask SerializeAsync<TResolver>(object? input, TextWriter writer, CancellationToken cancellationToken = default)
                    where TResolver : IJsonFormatterResolver<char, TResolver>, new()
                {
                    return Inner<char, TResolver>.InnerSerializeAsync(input, writer, cancellationToken);
                }

                #endregion

                #region -- Deserialize --

                /// <summary>Deserialize from string with specific resolver.</summary>
                /// <typeparam name="TResolver">Resolver</typeparam>
                /// <param name="input">Input</param>
                /// <param name="type">Object Type</param>
                /// <returns>Deserialized object</returns>
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public static object? Deserialize<TResolver>(string input, Type type)
                    where TResolver : IJsonFormatterResolver<char, TResolver>, new()
                {
#if NETSTANDARD2_0
                    return Inner<char, TResolver>.InnerDeserialize(input.AsSpan(), type);
#else
                    return Inner<char, TResolver>.InnerDeserialize(input, type);
#endif
                }

                /// <summary>Deserialize from string with specific resolver.</summary>
                /// <typeparam name="TResolver">Resolver</typeparam>
                /// <param name="input">Input</param>
                /// <param name="type">Object Type</param>
                /// <returns>Deserialized object</returns>
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public static object? Deserialize<TResolver>(char[] input, Type type)
                    where TResolver : IJsonFormatterResolver<char, TResolver>, new()
                {
                    return Inner<char, TResolver>.InnerDeserialize(input, type);
                }

                /// <summary>Deserialize from string with specific resolver.</summary>
                /// <typeparam name="TResolver">Resolver</typeparam>
                /// <param name="input">Input</param>
                /// <param name="type">Object Type</param>
                /// <returns>Deserialized object</returns>
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
#if !NETSTANDARD2_0
                public static object? Deserialize<TResolver>(in ArraySegment<char> input, Type type)
#else
                public static object? Deserialize<TResolver>(ArraySegment<char> input, Type type)
#endif
                    where TResolver : IJsonFormatterResolver<char, TResolver>, new()
                {
                    return Inner<char, TResolver>.InnerDeserialize(input, type);
                }

                /// <summary>Deserialize from string with specific resolver.</summary>
                /// <typeparam name="TResolver">Resolver</typeparam>
                /// <param name="input">Input</param>
                /// <param name="type">Object Type</param>
                /// <returns>Deserialized object</returns>
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public static object? Deserialize<TResolver>(in ReadOnlyMemory<char> input, Type type)
                    where TResolver : IJsonFormatterResolver<char, TResolver>, new()
                {
                    return Inner<char, TResolver>.InnerDeserialize(input, type);
                }

                /// <summary>Deserialize from string with specific resolver.</summary>
                /// <typeparam name="TResolver">Resolver</typeparam>
                /// <param name="input">Input</param>
                /// <param name="type">Object Type</param>
                /// <returns>Deserialized object</returns>
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public static object? Deserialize<TResolver>(in ReadOnlySpan<char> input, Type type)
                    where TResolver : IJsonFormatterResolver<char, TResolver>, new()
                {
                    return Inner<char, TResolver>.InnerDeserialize(input, type);
                }

                /// <summary>Deserialize from TextReader with specific resolver.</summary>
                /// <typeparam name="TResolver">Resolver</typeparam>
                /// <param name="reader">TextReader</param>
                /// <param name="type">Object Type</param>
                /// <param name="cancellationToken">CancellationToken</param>
                /// <returns>Task</returns>
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public static ValueTask<object?> DeserializeAsync<TResolver>(TextReader reader, Type type,
                    CancellationToken cancellationToken = default)
                    where TResolver : IJsonFormatterResolver<char, TResolver>, new()
                {
                    return Inner<char, TResolver>.InnerDeserializeAsync(reader, type, cancellationToken);
                }

                #endregion

                /// <summary>This is necessary to convert ValueTask of T to ValueTask of object</summary>
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                internal static ValueTask<object?> GenericTextReaderObjectWrapper<T, TResolver>(TextReader reader,
                    CancellationToken cancellationToken = default) where TResolver : IJsonFormatterResolver<char, TResolver>, new()
                {
                    var task = Generic.Utf16.DeserializeAsync<T, TResolver>(reader, cancellationToken);
                    if (task.IsCompletedSuccessfully)
                    {
                        return new ValueTask<object?>(task.Result);
                    }

                    return AwaitGenericTextReaderObjectWrapper(task);
                }

                private static async ValueTask<object?> AwaitGenericTextReaderObjectWrapper<T>(ValueTask<T> valueTask)
                {
                    return await valueTask.ConfigureAwait(false);
                }
            }
        }
    }
}