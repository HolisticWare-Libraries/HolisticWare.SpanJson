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
                /// <returns>String</returns>
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public static string Serialize<T>(T input)
                {
                    return Inner<T, char, ExcludeNullsOriginalCaseResolver<char>>.InnerSerializeToString(input);
                }

                /// <summary>Serialize to string.</summary>
                /// <typeparam name="T">Type</typeparam>
                /// <param name="input">Input</param>
                /// <returns>String</returns>
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public static char[] SerializeToCharArray<T>(T input)
                {
                    return Inner<T, char, ExcludeNullsOriginalCaseResolver<char>>.InnerSerializeToCharArray(input);
                }

                /// <summary>Serialize to char buffer from ArrayPool
                /// The returned ArraySegment's Array needs to be returned to the ArrayPool.</summary>
                /// <typeparam name="T">Type</typeparam>
                /// <param name="input">Input</param>
                /// <returns>Char array from ArrayPool</returns>
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public static ArraySegment<char> SerializeToArrayPool<T>(T input)
                {
                    return Inner<T, char, ExcludeNullsOriginalCaseResolver<char>>.InnerSerializeToCharArrayPool(input);
                }

                /// <summary>Serialize to TextWriter.</summary>
                /// <typeparam name="T">Type</typeparam>
                /// <param name="input">Input</param>
                /// <param name="writer">Writer</param>
                /// <param name="cancellationToken">CancellationToken</param>
                /// <returns>Task</returns>
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public static ValueTask SerializeAsync<T>(T input, TextWriter writer, CancellationToken cancellationToken = default)
                {
                    return Inner<T, char, ExcludeNullsOriginalCaseResolver<char>>.InnerSerializeAsync(input, writer, cancellationToken);
                }

                #endregion

                #region -- Deserialize --

                /// <summary>Deserialize from string.</summary>
                /// <typeparam name="T">Type</typeparam>
                /// <param name="input">Input</param>
                /// <returns>Deserialized object</returns>
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public static T? Deserialize<T>(string input)
                {
#if NETSTANDARD2_0
                    return Inner<T, char, ExcludeNullsOriginalCaseResolver<char>>.InnerDeserialize(input.AsSpan());
#else
                    return Inner<T, char, ExcludeNullsOriginalCaseResolver<char>>.InnerDeserialize(input);
#endif
                }

                /// <summary>Deserialize from string.</summary>
                /// <typeparam name="T">Type</typeparam>
                /// <param name="input">Input</param>
                /// <returns>Deserialized object</returns>
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public static T? Deserialize<T>(char[] input)
                {
                    return Inner<T, char, ExcludeNullsOriginalCaseResolver<char>>.InnerDeserialize(input);
                }

                /// <summary>Deserialize from string.</summary>
                /// <typeparam name="T">Type</typeparam>
                /// <param name="input">Input</param>
                /// <returns>Deserialized object</returns>
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
#if !NETSTANDARD2_0
                public static T? Deserialize<T>(in ArraySegment<char> input)
#else
                public static T? Deserialize<T>(ArraySegment<char> input)
#endif
                {
                    return Inner<T, char, ExcludeNullsOriginalCaseResolver<char>>.InnerDeserialize(input);
                }

                /// <summary>Deserialize from string.</summary>
                /// <typeparam name="T">Type</typeparam>
                /// <param name="input">Input</param>
                /// <returns>Deserialized object</returns>
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public static T? Deserialize<T>(in ReadOnlyMemory<char> input)
                {
                    return Inner<T, char, ExcludeNullsOriginalCaseResolver<char>>.InnerDeserialize(input);
                }

                /// <summary>Deserialize from string.</summary>
                /// <typeparam name="T">Type</typeparam>
                /// <param name="input">Input</param>
                /// <returns>Deserialized object</returns>
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public static T? Deserialize<T>(in ReadOnlySpan<char> input)
                {
                    return Inner<T, char, ExcludeNullsOriginalCaseResolver<char>>.InnerDeserialize(input);
                }

                /// <summary>Deserialize from TextReader.</summary>
                /// <typeparam name="T">Type</typeparam>
                /// <param name="reader">TextReader</param>
                /// <param name="cancellationToken">CancellationToken</param>
                /// <returns>Deserialized object</returns>
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public static ValueTask<T?> DeserializeAsync<T>(TextReader reader, CancellationToken cancellationToken = default)
                {
                    return Inner<T, char, ExcludeNullsOriginalCaseResolver<char>>.InnerDeserializeAsync(reader, cancellationToken);
                }

                #endregion
            }
        }
    }
}