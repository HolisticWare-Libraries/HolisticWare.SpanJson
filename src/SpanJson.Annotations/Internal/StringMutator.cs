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

        private static readonly object s_camelCaseLock = new();
        private static readonly Dictionary<string, string> s_camelCaseCache = new(StringComparer.Ordinal);

        /// <summary>MyProperty -> myProperty</summary>
        [Obsolete("=> ToCamelCase")]
        [return: NotNullIfNotNull("s")]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string? ToCamelCaseWithCache(string? s) => ToCamelCase(s);

        /// <summary>MyProperty -> myProperty</summary>
        [return: NotNullIfNotNull("s")]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string? ToCamelCase(string? s)
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
            lock (s_camelCaseLock)
            {
                if (!s_camelCaseCache.TryGetValue(s, out var result))
                {
                    result = ToCamelCase0(s);
                    s_camelCaseCache.Add(s, result);
                }
                return result;
            }
        }

        [return: NotNullIfNotNull("s")]
        private static string ToCamelCase0(string s)
        {
            if (!char.IsUpper(s[0])) { return s; }

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

        private static readonly object s_snakeCaseLock = new();
        private static readonly Dictionary<string, string> s_snakeCaseCache = new(StringComparer.Ordinal);

        /// <summary>MyProperty -> my_property</summary>
        [return: NotNullIfNotNull("s")]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string? ToSnakeCaseWithCache(string? s) => ToSnakeCase(s);

        /// <summary>MyProperty -> my_property</summary>
        [return: NotNullIfNotNull("s")]
        public static string? ToSnakeCase(string? s)
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
            lock (s_snakeCaseLock)
            {
                if (!s_snakeCaseCache.TryGetValue(s, out var result))
                {
                    result = ToSeparatedCase(s, Underscore);
                    s_snakeCaseCache.Add(s, result);
                }
                return result;
            }
        }

        #endregion

        #region -- AdaCase --

        private static readonly object s_adaCaseLock = new();
        private static readonly Dictionary<string, string> s_adaCaseCache = new(StringComparer.Ordinal);

        /// <summary>MyProperty -> My_Property</summary>
        [return: NotNullIfNotNull("s")]
        public static string? ToAdaCase(string? s)
        {
            if (s is null || 0u >= (uint)s.Length) { return s; }

            if (!s_adaCaseCache.TryGetValue(s, out var result))
            {
                result = ToAdaCaseSlow(s);
            }
            return result;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static string ToAdaCaseSlow(string s)
        {
            lock (s_adaCaseLock)
            {
                if (!s_adaCaseCache.TryGetValue(s, out var result))
                {
                    result = ToPascalSeparatedCase(s, Underscore);
                    s_adaCaseCache.Add(s, result);
                }
                return result;
            }
        }

        #endregion

        #region -- MacroCase --

        private static readonly object s_macroCaseLock = new();
        private static readonly Dictionary<string, string> s_macroCaseCache = new(StringComparer.Ordinal);

        /// <summary>MyProperty -> MY_PROPERTY</summary>
        [return: NotNullIfNotNull("s")]
        public static string? ToMacroCase(string? s)
        {
            if (s is null || 0u >= (uint)s.Length) { return s; }

            if (!s_macroCaseCache.TryGetValue(s, out var result))
            {
                result = ToMacroCaseSlow(s);
            }
            return result;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static string ToMacroCaseSlow(string s)
        {
            lock (s_macroCaseLock)
            {
                if (!s_macroCaseCache.TryGetValue(s, out var result))
                {
                    result = ToConstantSeparatedCase(s, Underscore);
                    s_macroCaseCache.Add(s, result);
                }
                return result;
            }
        }

        #endregion

        #region -- KebabCase --

        private static readonly object s_kebabCaseLock = new();
        private static readonly Dictionary<string, string> s_kebabCaseCache = new(StringComparer.Ordinal);

        /// <summary>MyProperty -> my-property</summary>
        [return: NotNullIfNotNull("s")]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string? ToKebabCase(string? s)
        {
            if (s is null || 0u >= (uint)s.Length) { return s; }

            if (!s_kebabCaseCache.TryGetValue(s, out var result))
            {
                result = ToKebabCaseSlow(s);
            }
            return result;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static string ToKebabCaseSlow(string s)
        {
            lock (s_kebabCaseLock)
            {
                if (!s_kebabCaseCache.TryGetValue(s, out var result))
                {
                    result = ToSeparatedCase(s, Hyphen);
                    s_kebabCaseCache.Add(s, result);
                }
                return result;
            }
        }

        #endregion

        #region -- TrainCase --

        private static readonly object s_trainCaseLock = new();
        private static readonly Dictionary<string, string> s_trainCaseCache = new(StringComparer.Ordinal);

        /// <summary>MyProperty -> My-Property</summary>
        [return: NotNullIfNotNull("s")]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string? ToTrainCase(string? s)
        {
            if (s is null || 0u >= (uint)s.Length) { return s; }

            if (!s_trainCaseCache.TryGetValue(s, out var result))
            {
                result = ToTrainCaseSlow(s);
            }
            return result;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static string ToTrainCaseSlow(string s)
        {
            lock (s_trainCaseLock)
            {
                if (!s_trainCaseCache.TryGetValue(s, out var result))
                {
                    result = ToPascalSeparatedCase(s, Hyphen);
                    s_trainCaseCache.Add(s, result);
                }
                return result;
            }
        }

        #endregion

        #region -- CobolCase --

        private static readonly object s_cobolCaseLock = new();
        private static readonly Dictionary<string, string> s_cobolCaseCache = new(StringComparer.Ordinal);

        /// <summary>MyProperty -> MY-PROPERTY</summary>
        [return: NotNullIfNotNull("s")]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string? ToCobolCase(string? s)
        {
            if (s is null || 0u >= (uint)s.Length) { return s; }

            if (!s_cobolCaseCache.TryGetValue(s, out var result))
            {
                result = ToCobolCaseSlow(s);
            }
            return result;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static string ToCobolCaseSlow(string s)
        {
            lock (s_cobolCaseLock)
            {
                if (!s_cobolCaseCache.TryGetValue(s, out var result))
                {
                    result = ToConstantSeparatedCase(s, Hyphen);
                    s_cobolCaseCache.Add(s, result);
                }
                return result;
            }
        }

        #endregion

        #region ** ToSeparatedCase **

        const char Underscore = '_';
        const char Hyphen = '-';

        // borrowed from https://github.com/JamesNK/Newtonsoft.Json/blob/4ab34b0461fb595805d092a46a58f35f66c84d6a/Src/Newtonsoft.Json/Utilities/StringUtils.cs#L208

        private static string ToSeparatedCase(string s, char separator)
        {
            var sb = StringBuilderCache.Acquire();
            var state = SeparatedCaseState.Start;

            for (int i = 0; i < s!.Length; i++)
            {
                if (s[i] == ' ')
                {
                    if (state != SeparatedCaseState.Start)
                    {
                        state = SeparatedCaseState.NewWord;
                    }
                }
                else if (char.IsUpper(s[i]))
                {
                    switch (state)
                    {
                        case SeparatedCaseState.Upper:
                            bool hasNext = (i + 1 < s.Length);
                            if (i > 0 && hasNext)
                            {
                                char nextChar = s[i + 1];
                                if (!char.IsUpper(nextChar) && nextChar != separator)
                                {
                                    sb.Append(separator);
                                }
                            }
                            break;
                        case SeparatedCaseState.Lower:
                        case SeparatedCaseState.NewWord:
                            sb.Append(separator);
                            break;
                    }

                    var c = char.ToLower(s[i], CultureInfo.InvariantCulture);
                    sb.Append(c);

                    state = SeparatedCaseState.Upper;
                }
                else if (s[i] == separator)
                {
                    sb.Append(separator);
                    state = SeparatedCaseState.Start;
                }
                else
                {
                    if (state == SeparatedCaseState.NewWord)
                    {
                        sb.Append(separator);
                    }

                    sb.Append(s[i]);
                    state = SeparatedCaseState.Lower;
                }
            }

            return StringBuilderCache.GetStringAndRelease(sb);
        }

        private static string ToPascalSeparatedCase(this string s, char separator)
        {
            var sb = StringBuilderCache.Acquire();
            var state = SeparatedCaseState.Start;

            for (int i = 0; i < s!.Length; i++)
            {
                if (s[i] == ' ')
                {
                    if (state != SeparatedCaseState.Start)
                    {
                        state = SeparatedCaseState.NewWord;
                    }
                }
                else if (char.IsUpper(s[i]))
                {
                    var first = false;
                    switch (state)
                    {
                        case SeparatedCaseState.Upper:
                            bool hasNext = (i + 1 < s.Length);
                            if (i > 0 && hasNext)
                            {
                                char nextChar = s[i + 1];
                                if (!char.IsUpper(nextChar) && nextChar != separator)
                                {
                                    sb.Append(separator);
                                    first = true;
                                }
                            }
                            break;
                        case SeparatedCaseState.Lower:
                        case SeparatedCaseState.NewWord:
                            sb.Append(separator);
                            first = true;
                            break;
                        case SeparatedCaseState.Start:
                            first = true;
                            break;
                    }

                    sb.Append(first
                        ? char.ToUpper(s[i], CultureInfo.InvariantCulture)
                        : char.ToLower(s[i], CultureInfo.InvariantCulture));

                    state = SeparatedCaseState.Upper;
                }
                else if (s[i] == separator)
                {
                    sb.Append(separator);
                    state = SeparatedCaseState.Start;
                }
                else
                {
                    if (state == SeparatedCaseState.NewWord)
                    {
                        sb.Append(separator);
                        sb.Append(char.ToUpper(s[i], CultureInfo.InvariantCulture));
                    }
                    else
                    {
                        if (state == SeparatedCaseState.Start)
                        {
                            sb.Append(char.ToUpper(s[i], CultureInfo.InvariantCulture));
                        }
                        else
                        {
                            sb.Append(s[i]);
                        }
                    }
                    state = SeparatedCaseState.Lower;
                }
            }

            return StringBuilderCache.GetStringAndRelease(sb);
        }

        private static string ToConstantSeparatedCase(this string s, char separator)
        {
            var sb = StringBuilderCache.Acquire();
            var state = SeparatedCaseState.Start;

            for (int i = 0; i < s!.Length; i++)
            {
                if (s[i] == ' ')
                {
                    if (state != SeparatedCaseState.Start)
                    {
                        state = SeparatedCaseState.NewWord;
                    }
                }
                else if (char.IsUpper(s[i]))
                {
                    switch (state)
                    {
                        case SeparatedCaseState.Upper:
                            bool hasNext = (i + 1 < s.Length);
                            if (i > 0 && hasNext)
                            {
                                char nextChar = s[i + 1];
                                if (!char.IsUpper(nextChar) && nextChar != separator)
                                {
                                    sb.Append(separator);
                                }
                            }
                            break;
                        case SeparatedCaseState.Lower:
                        case SeparatedCaseState.NewWord:
                            sb.Append(separator);
                            break;
                    }

                    sb.Append(s[i]);

                    state = SeparatedCaseState.Upper;
                }
                else if (s[i] == separator)
                {
                    sb.Append(separator);
                    state = SeparatedCaseState.Start;
                }
                else
                {
                    if (state == SeparatedCaseState.NewWord)
                    {
                        sb.Append(separator);
                    }

                    sb.Append(char.ToUpper(s[i], CultureInfo.InvariantCulture));
                    state = SeparatedCaseState.Lower;
                }
            }

            return StringBuilderCache.GetStringAndRelease(sb);
        }

        private enum SeparatedCaseState
        {
            Start,
            Lower,
            Upper,
            NewWord
        }

        #endregion
    }
}
