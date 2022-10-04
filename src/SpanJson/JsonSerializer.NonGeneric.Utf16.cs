using System.Runtime.CompilerServices;
using SpanJson.Resolvers;

namespace SpanJson
{
    static partial class JsonSerializer
    {
        static partial class NonGeneric
        {
            /// <summary>Serialize/Deserialize to/from string et al.</summary>
            public static partial class Utf16
            {
                #region -- Serialize --

                /// <summary>Serialize to string.</summary>
                /// <param name="input">Input</param>
                /// <returns>String</returns>
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public static string Serialize(object? input)
                {
                    return Inner<char, ExcludeNullsOriginalCaseResolver<char>>.InnerSerializeToString(input);
                }

                /// <summary>Serialize to string.</summary>
                /// <param name="input">Input</param>
                /// <returns>String</returns>
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public static char[] SerializeToCharArray(object? input)
                {
                    return Inner<char, ExcludeNullsOriginalCaseResolver<char>>.InnerSerializeToCharArray(input);
                }

                /// <summary>Serialize to char buffer from ArrayPool.
                /// The returned ArraySegment's Array needs to be returned to the ArrayPool.</summary>
                /// <param name="input">Input</param>
                /// <returns>Char array from ArrayPool</returns>
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public static ArraySegment<char> SerializeToArrayPool(object? input)
                {
                    return Inner<char, ExcludeNullsOriginalCaseResolver<char>>.InnerSerializeToCharArrayPool(input);
                }

                /// <summary>Serialize to TextWriter.</summary>
                /// <param name="input">Input</param>
                /// <param name="writer">TextWriter</param>
                /// <param name="cancellationToken">CancellationToken</param>
                /// <returns>Task</returns>
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public static ValueTask SerializeAsync(object? input, TextWriter writer, CancellationToken cancellationToken = default)
                {
                    return Inner<char, ExcludeNullsOriginalCaseResolver<char>>.InnerSerializeAsync(input, writer, cancellationToken);
                }

                #endregion

                #region -- Deserialize --

                /// <summary>Deserialize from string.</summary>
                /// <param name="input">Input</param>
                /// <param name="type">Object Type</param>
                /// <returns>Deserialized object</returns>
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public static object? Deserialize(string input, Type type)
                {
#if NETSTANDARD2_0
                    return Inner<char, ExcludeNullsOriginalCaseResolver<char>>.InnerDeserialize(input.AsSpan(), type);
#else
                    return Inner<char, ExcludeNullsOriginalCaseResolver<char>>.InnerDeserialize(input, type);
#endif
                }

                /// <summary>Deserialize from string.</summary>
                /// <param name="input">Input</param>
                /// <param name="type">Object Type</param>
                /// <returns>Deserialized object</returns>
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public static object? Deserialize(char[] input, Type type)
                {
                    return Inner<char, ExcludeNullsOriginalCaseResolver<char>>.InnerDeserialize(input, type);
                }

                /// <summary>Deserialize from string.</summary>
                /// <param name="input">Input</param>
                /// <param name="type">Object Type</param>
                /// <returns>Deserialized object</returns>
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
#if !NETSTANDARD2_0
                public static object? Deserialize(in ArraySegment<char> input, Type type)
#else
                public static object? Deserialize(ArraySegment<char> input, Type type)
#endif
                {
                    return Inner<char, ExcludeNullsOriginalCaseResolver<char>>.InnerDeserialize(input, type);
                }

                /// <summary>Deserialize from string.</summary>
                /// <param name="input">Input</param>
                /// <param name="type">Object Type</param>
                /// <returns>Deserialized object</returns>
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public static object? Deserialize(in ReadOnlyMemory<char> input, Type type)
                {
                    return Inner<char, ExcludeNullsOriginalCaseResolver<char>>.InnerDeserialize(input, type);
                }

                /// <summary>Deserialize from string.</summary>
                /// <param name="input">Input</param>
                /// <param name="type">Object Type</param>
                /// <returns>Deserialized object</returns>
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public static object? Deserialize(in ReadOnlySpan<char> input, Type type)
                {
                    return Inner<char, ExcludeNullsOriginalCaseResolver<char>>.InnerDeserialize(input, type);
                }

                /// <summary>Deserialize from TextReader.</summary>
                /// <param name="reader">TextReader</param>
                /// <param name="type">Object Type</param>
                /// <param name="cancellationToken">CancellationToken</param>
                /// <returns>Task</returns>
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public static ValueTask<object?> DeserializeAsync(TextReader reader, Type type,
                    CancellationToken cancellationToken = default)
                {
                    return Inner<char, ExcludeNullsOriginalCaseResolver<char>>.InnerDeserializeAsync(reader, type, cancellationToken);
                }

                #endregion
            }
        }
    }
}