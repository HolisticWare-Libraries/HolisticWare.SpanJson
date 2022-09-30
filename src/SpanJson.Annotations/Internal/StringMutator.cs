using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace SpanJson.Internal
{
    public static class StringMutator
    {
        /// <summary>MyProperty -> MyProperty</summary>
        [return: NotNullIfNotNull("s")]
        public static string? Original(string? s) => s;

        #region -- CamelCase --

        // borrowed from https://github.com/JamesNK/Newtonsoft.Json/blob/4ab34b0461fb595805d092a46a58f35f66c84d6a/Src/Newtonsoft.Json/Utilities/StringUtils.cs#L149

        private static Dictionary<string, string> s_camelCaseCache = new(StringComparer.Ordinal);

        /// <summary>MyProperty -> myProperty</summary>
        [return: NotNullIfNotNull("s")]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string? ToCamelCaseWithCache(string? s)
        {
            if (s is null || 0u >= (uint)s.Length) { return s; }

            if (!s_camelCaseCache.TryGetValue(s, out var result))
            {
                result = ToCamelCaseSlow(s);
            }
            return result;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static string ToCamelCaseSlow(string s)
        {
            var snakeCaseCache = s_camelCaseCache;

            var value = ToCamelCase(s);

            // Swap the previous cache with a new copy if no other thread has updated the reference.
            // This ensures the dictionary can only grow and not replace another one of the same size.
            _ = Interlocked.CompareExchange(ref s_camelCaseCache, new Dictionary<string, string>(snakeCaseCache, StringComparer.Ordinal)
            {
                [s] = value
            }, snakeCaseCache);

            return value;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        [return: NotNullIfNotNull("s")]
        public static string? ToCamelCase(string? s)
        {
            if (s is null || 0u >= (uint)s.Length || !char.IsUpper(s[0])) { return s; }

            char[] chars = s.ToCharArray();

            for (int i = 0; i < chars.Length; i++)
            {
                if (i == 1 && !char.IsUpper(chars[i]))
                {
                    break;
                }

                bool hasNext = (i + 1 < chars.Length);
                if (i > 0 && hasNext && !char.IsUpper(chars[i + 1]))
                {
                    // if the next character is a space, which is not considered uppercase 
                    // (otherwise we wouldn't be here...)
                    // we want to ensure that the following:
                    // 'FOO bar' is rewritten as 'foo bar', and not as 'foO bar'
                    // The code was written in such a way that the first word in uppercase
                    // ends when if finds an uppercase letter followed by a lowercase letter.
                    // now a ' ' (space, (char)32) is considered not upper
                    // but in that case we still want our current character to become lowercase
                    if (char.IsSeparator(chars[i + 1]))
                    {
                        chars[i] = char.ToLower(chars[i], CultureInfo.InvariantCulture);
                    }

                    break;
                }

                chars[i] = char.ToLower(chars[i], CultureInfo.InvariantCulture);
            }

            return new string(chars);
        }

        #endregion

        #region -- SnakeCase --

        // borrowed from https://github.com/JamesNK/Newtonsoft.Json/blob/4ab34b0461fb595805d092a46a58f35f66c84d6a/Src/Newtonsoft.Json/Utilities/StringUtils.cs#L208

        private static Dictionary<string, string> s_snakeCaseCache = new(StringComparer.Ordinal);

        /// <summary>MyProperty -> my_property</summary>
        [return: NotNullIfNotNull("s")]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string? ToSnakeCaseWithCache(string? s)
        {
            if (s is null || 0u >= (uint)s.Length) { return s; }

            if (!s_snakeCaseCache.TryGetValue(s, out var result))
            {
                result = ToSnakeCaseSlow(s);
            }
            return result;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static string ToSnakeCaseSlow(string s)
        {
            var snakeCaseCache = s_snakeCaseCache;

            var value = ToSnakeCase(s);

            // Swap the previous cache with a new copy if no other thread has updated the reference.
            // This ensures the dictionary can only grow and not replace another one of the same size.
            _ = Interlocked.CompareExchange(ref s_snakeCaseCache, new Dictionary<string, string>(snakeCaseCache, StringComparer.Ordinal)
            {
                [s] = value
            }, snakeCaseCache);

            return value;
        }

        [return: NotNullIfNotNull("s")]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static string? ToSnakeCase(string? s)
        {
            if (s is null || 0u >= (uint)s.Length) { return s; }

            var sb = StringBuilderCache.Acquire();
            var state = SnakeCaseState.Start;

            for (int i = 0; i < s!.Length; i++)
            {
                if (s[i] == ' ')
                {
                    if (state != SnakeCaseState.Start)
                    {
                        state = SnakeCaseState.NewWord;
                    }
                }
                else if (char.IsUpper(s[i]))
                {
                    switch (state)
                    {
                        case SnakeCaseState.Upper:
                            bool hasNext = (i + 1 < s.Length);
                            if (i > 0 && hasNext)
                            {
                                char nextChar = s[i + 1];
                                if (!char.IsUpper(nextChar) && nextChar != '_')
                                {
                                    sb.Append('_');
                                }
                            }
                            break;
                        case SnakeCaseState.Lower:
                        case SnakeCaseState.NewWord:
                            sb.Append('_');
                            break;
                    }

                    var c = char.ToLower(s[i], CultureInfo.InvariantCulture);
                    sb.Append(c);

                    state = SnakeCaseState.Upper;
                }
                else if (s[i] == '_')
                {
                    sb.Append('_');
                    state = SnakeCaseState.Start;
                }
                else
                {
                    if (state == SnakeCaseState.NewWord)
                    {
                        sb.Append('_');
                    }

                    sb.Append(s[i]);
                    state = SnakeCaseState.Lower;
                }
            }

            return StringBuilderCache.GetStringAndRelease(sb);
        }

        private enum SnakeCaseState
        {
            Start,
            Lower,
            Upper,
            NewWord
        }

        #endregion

        #region -- ConstantCase --

        private static Dictionary<string, string> s_constantCaseCache = new(StringComparer.Ordinal);

        /// <summary>MyProperty -> my_property</summary>
        [return: NotNullIfNotNull("s")]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string? ToConstantCaseWithCache(string? s)
        {
            if (s is null || 0u >= (uint)s.Length) { return s; }

            if (!s_constantCaseCache.TryGetValue(s, out var result))
            {
                result = ToConstantCaseSlow(s);
            }
            return result;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static string ToConstantCaseSlow(string s)
        {
            var constantCaseCache = s_constantCaseCache;

            var value = ToConstantCase(s);

            // Swap the previous cache with a new copy if no other thread has updated the reference.
            // This ensures the dictionary can only grow and not replace another one of the same size.
            _ = Interlocked.CompareExchange(ref s_constantCaseCache, new Dictionary<string, string>(constantCaseCache, StringComparer.Ordinal)
            {
                [s] = value
            }, constantCaseCache);

            return value;
        }

        /// <summary>
        /// Returns a constant case version of this string. For example, converts 'StringError' into 'STRING_ERROR'.
        /// </summary>
        [return: NotNullIfNotNull("s")]
        public static string? ToConstantCase(this string? s)
        {
            if (s is null || 0u >= (uint)s.Length) { return s; }

            int i;
            int strLength = s!.Length;
            // iterate through each character in the string, stopping a character short of the end
            for (i = 0; i < strLength - 1; ++i)
            {
                var curChar = s[i];
                var nextChar = s[i + 1];
                // look for the pattern [a-z][A-Z]
                if (char.IsLower(curChar) && char.IsUpper(nextChar))
                {
                    InsertUnderscore();
                    // then skip the remaining match checks since we already found a match here
                    continue;
                }
                // look for the pattern [0-9][A-Za-z]
                if (char.IsDigit(curChar) && char.IsLetter(nextChar))
                {
                    InsertUnderscore();
                    continue;
                }
                // look for the pattern [A-Za-z][0-9]
                if (char.IsLetter(curChar) && char.IsDigit(nextChar))
                {
                    InsertUnderscore();
                    continue;
                }
                // if there's enough characters left, look for the pattern [A-Z][A-Z][a-z]
                if (i < strLength - 2 && char.IsUpper(curChar) && char.IsUpper(nextChar) && char.IsLower(s[i + 2]))
                {
                    InsertUnderscore();
                    continue;
                }
            }
            // convert the resulting string to uppercase
            return s.ToUpperInvariant();

            void InsertUnderscore()
            {
                // add an underscore between the two characters, increment i to skip the underscore, and increase strLength because the string is longer now
                s = s.Substring(0, ++i) + '_' + s.Substring(i);
                ++strLength;
            }
        }

        #endregion
    }
}
