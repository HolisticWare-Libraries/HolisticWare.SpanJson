﻿#region License
// Copyright (c) 2007 James Newton-King
//
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following
// conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.
#endregion

using System.Runtime.CompilerServices;

namespace SpanJson.Linq
{
    /// <summary>Contains the LINQ to JSON extension methods.</summary>
    public static class Extensions
    {
        /// <summary>Returns a collection of tokens that contains the ancestors of every token in the source collection.</summary>
        /// <typeparam name="T">The type of the objects in source, constrained to <see cref="JToken"/>.</typeparam>
        /// <param name="source">An <see cref="IEnumerable{T}"/> of <see cref="JToken"/> that contains the source collection.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> of <see cref="JToken"/> that contains the ancestors of every token in the source collection.</returns>
        public static IJEnumerable<JToken> Ancestors<T>(this IEnumerable<T> source) where T : JToken
        {
            if (source is null) { ThrowHelper.ThrowArgumentNullException(ExceptionArgument.source); }

            return source.SelectMany(j => j.Ancestors()).AsJEnumerable();
        }

        /// <summary>Returns a collection of tokens that contains every token in the source collection,
        /// and the ancestors of every token in the source collection.</summary>
        /// <typeparam name="T">The type of the objects in source, constrained to <see cref="JToken"/>.</typeparam>
        /// <param name="source">An <see cref="IEnumerable{T}"/> of <see cref="JToken"/> that contains the source collection.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> of <see cref="JToken"/> that contains every token in the source collection, the ancestors of every token in the source collection.</returns>
        public static IJEnumerable<JToken> AncestorsAndSelf<T>(this IEnumerable<T> source) where T : JToken
        {
            if (source is null) { ThrowHelper.ThrowArgumentNullException(ExceptionArgument.source); }

            return source.SelectMany(j => j.AncestorsAndSelf()).AsJEnumerable();
        }

        /// <summary>Returns a collection of tokens that contains the descendants of every token in the source collection.</summary>
        /// <typeparam name="T">The type of the objects in source, constrained to <see cref="JContainer"/>.</typeparam>
        /// <param name="source">An <see cref="IEnumerable{T}"/> of <see cref="JToken"/> that contains the source collection.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> of <see cref="JToken"/> that contains the descendants of every token in the source collection.</returns>
        public static IJEnumerable<JToken> Descendants<T>(this IEnumerable<T> source) where T : JContainer
        {
            if (source is null) { ThrowHelper.ThrowArgumentNullException(ExceptionArgument.source); }

            return source.SelectMany(j => j.Descendants()).AsJEnumerable();
        }

        /// <summary>Returns a collection of tokens that contains every token in the source collection,
        /// and the descendants of every token in the source collection.</summary>
        /// <typeparam name="T">The type of the objects in source, constrained to <see cref="JContainer"/>.</typeparam>
        /// <param name="source">An <see cref="IEnumerable{T}"/> of <see cref="JToken"/> that contains the source collection.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> of <see cref="JToken"/> that contains every token in the source collection, and the descendants of every token in the source collection.</returns>
        public static IJEnumerable<JToken> DescendantsAndSelf<T>(this IEnumerable<T> source) where T : JContainer
        {
            if (source is null) { ThrowHelper.ThrowArgumentNullException(ExceptionArgument.source); }

            return source.SelectMany(j => j.DescendantsAndSelf()).AsJEnumerable();
        }

        /// <summary>Returns a collection of child properties of every object in the source collection.</summary>
        /// <param name="source">An <see cref="IEnumerable{T}"/> of <see cref="JObject"/> that contains the source collection.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> of <see cref="JProperty"/> that contains the properties of every object in the source collection.</returns>
        public static IJEnumerable<JProperty> Properties(this IEnumerable<JObject> source)
        {
            if (source is null) { ThrowHelper.ThrowArgumentNullException(ExceptionArgument.source); }

            return source.SelectMany(d => d.Properties()).AsJEnumerable();
        }

        /// <summary>Returns a collection of child values of every object in the source collection with the given key.</summary>
        /// <param name="source">An <see cref="IEnumerable{T}"/> of <see cref="JToken"/> that contains the source collection.</param>
        /// <param name="key">The token key.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> of <see cref="JToken"/> that contains the values of every token in the source collection with the given key.</returns>
        public static IJEnumerable<JToken> Values(this IEnumerable<JToken> source, object? key)
        {
            return Values<JToken, JToken>(source, key)!.AsJEnumerable();
        }

        /// <summary>Returns a collection of child values of every object in the source collection.</summary>
        /// <param name="source">An <see cref="IEnumerable{T}"/> of <see cref="JToken"/> that contains the source collection.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> of <see cref="JToken"/> that contains the values of every token in the source collection.</returns>
        public static IJEnumerable<JToken> Values(this IEnumerable<JToken> source)
        {
            return source.Values(null);
        }

        /// <summary>Returns a collection of converted child values of every object in the source collection with the given key.</summary>
        /// <typeparam name="U">The type to convert the values to.</typeparam>
        /// <param name="source">An <see cref="IEnumerable{T}"/> of <see cref="JToken"/> that contains the source collection.</param>
        /// <param name="key">The token key.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> that contains the converted values of every token in the source collection with the given key.</returns>
        public static IEnumerable<U?> Values<U>(this IEnumerable<JToken> source, object? key)
        {
            return Values<JToken, U>(source, key);
        }

        /// <summary>Returns a collection of converted child values of every object in the source collection.</summary>
        /// <typeparam name="U">The type to convert the values to.</typeparam>
        /// <param name="source">An <see cref="IEnumerable{T}"/> of <see cref="JToken"/> that contains the source collection.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> that contains the converted values of every token in the source collection.</returns>
        public static IEnumerable<U?> Values<U>(this IEnumerable<JToken> source)
        {
            return Values<JToken, U>(source, null);
        }

        /// <summary>Converts the value.</summary>
        /// <typeparam name="U">The type to convert the value to.</typeparam>
        /// <param name="value">A <see cref="JToken"/> cast as a <see cref="IEnumerable{T}"/> of <see cref="JToken"/>.</param>
        /// <returns>A converted value.</returns>
        public static U? Value<U>(this IEnumerable<JToken> value)
        {
            return value.Value<JToken, U>();
        }

        /// <summary>Converts the value.</summary>
        /// <typeparam name="T">The source collection type.</typeparam>
        /// <typeparam name="U">The type to convert the value to.</typeparam>
        /// <param name="value">A <see cref="JToken"/> cast as a <see cref="IEnumerable{T}"/> of <see cref="JToken"/>.</param>
        /// <returns>A converted value.</returns>
        public static U? Value<T, U>(this IEnumerable<T> value) where T : JToken
        {
            switch (value)
            {
                case null:
                    throw ThrowHelper.GetArgumentNullException(ExceptionArgument.value);

                case JToken token:
                    return token.Convert<JToken, U>();

                default:
                    throw ThrowHelper2.GetArgumentException_Source_value_must_be_a_JToken();
            }
        }

        internal static IEnumerable<U?> Values<T, U>(this IEnumerable<T> source, object? key) where T : JToken
        {
            if (source is null) { ThrowHelper.ThrowArgumentNullException(ExceptionArgument.source); }

            if (key is null)
            {
                foreach (T token in source)
                {
                    if (token is JValue value)
                    {
                        yield return Convert<JValue, U>(value);
                    }
                    else
                    {
                        foreach (JToken t in token.Children())
                        {
                            yield return t.Convert<JToken, U>();
                        }
                    }
                }
            }
            else
            {
                foreach (T token in source)
                {
                    JToken? value = token[key];
                    if (value is not null)
                    {
                        yield return value.Convert<JToken, U>();
                    }
                }
            }
        }

        //public static IEnumerable<T> InDocumentOrder<T>(this IEnumerable<T> source) where T : JObject;

        /// <summary>Returns a collection of child tokens of every array in the source collection.</summary>
        /// <typeparam name="T">The source collection type.</typeparam>
        /// <param name="source">An <see cref="IEnumerable{T}"/> of <see cref="JToken"/> that contains the source collection.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> of <see cref="JToken"/> that contains the values of every token in the source collection.</returns>
        public static IJEnumerable<JToken> Children<T>(this IEnumerable<T> source) where T : JToken
        {
            return Children<T, JToken>(source)!.AsJEnumerable();
        }

        /// <summary>Returns a collection of converted child tokens of every array in the source collection.</summary>
        /// <param name="source">An <see cref="IEnumerable{T}"/> of <see cref="JToken"/> that contains the source collection.</param>
        /// <typeparam name="T">The source collection type.</typeparam>
        /// <typeparam name="U">The type to convert the values to.</typeparam>
        /// <returns>An <see cref="IEnumerable{T}"/> that contains the converted values of every token in the source collection.</returns>
        public static IEnumerable<U?> Children<T, U>(this IEnumerable<T> source) where T : JToken
        {
            if (source is null) { ThrowHelper.ThrowArgumentNullException(ExceptionArgument.source); }

            return source.SelectMany(c => c.Children()).Convert<JToken, U>();
        }

        internal static IEnumerable<U?> Convert<T, U>(this IEnumerable<T> source) where T : JToken
        {
            if (source is null) { ThrowHelper.ThrowArgumentNullException(ExceptionArgument.source); }

            foreach (T token in source)
            {
                yield return Convert<JToken, U>(token);
            }
        }

        internal static U? Convert<T, U>(this T token) where T : JToken
        {
            switch (token)
            {
                case null:
                    return default;

                case U castValue when typeof(U) != typeof(IComparable) && typeof(U) != typeof(IFormattable):
                    return castValue;

                case JValue value:
                    if (value.Value is U u) { return u; }

                    // 这儿直接采用 JValue 进行转换，考虑扩展基元类型 JTokenType.Number => U;JTokenType.String => U;JTokenType.Dynamic => U

                    //Type targetType = typeof(U);

                    //if (ReflectionUtils.IsNullableType(targetType))
                    //{
                    //    if (value.Value is null)
                    //    {
                    //        return default;
                    //    }

                    //    targetType = Nullable.GetUnderlyingType(targetType);
                    //}

                    ////return (U)System.Convert.ChangeType(value/*.Value*/, targetType, CultureInfo.InvariantCulture);
                    return value.ToObject<U>();

                default:
                    throw ThrowHelper2.GetInvalidCastException<T>(token);
            }
        }

        //public static void Remove<T>(this IEnumerable<T> source) where T : JContainer;

        /// <summary>Returns the input typed as <see cref="IJEnumerable{T}"/>.</summary>
        /// <param name="source">An <see cref="IEnumerable{T}"/> of <see cref="JToken"/> that contains the source collection.</param>
        /// <returns>The input typed as <see cref="IJEnumerable{T}"/>.</returns>
        public static IJEnumerable<JToken> AsJEnumerable(this IEnumerable<JToken> source)
        {
            return source.AsJEnumerable<JToken>();
        }

        /// <summary>Returns the input typed as <see cref="IJEnumerable{T}"/>.</summary>
        /// <typeparam name="T">The source collection type.</typeparam>
        /// <param name="source">An <see cref="IEnumerable{T}"/> of <see cref="JToken"/> that contains the source collection.</param>
        /// <returns>The input typed as <see cref="IJEnumerable{T}"/>.</returns>
        public static IJEnumerable<T> AsJEnumerable<T>(this IEnumerable<T> source) where T : JToken
        {
            switch (source)
            {
                case null:
                    return null!;

                case IJEnumerable<T> customEnumerable:
                    return customEnumerable;

                default:
                    return new JEnumerable<T>(source);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static bool IsString(this JTokenType tokenType)
        {
            switch (tokenType)
            {
                case JTokenType.Dynamic:
                case JTokenType.String:
                    return true;
                default:
                    return false;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static bool IsEqual(this int result)
        {
            return 0u >= (uint)result ? true : false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static bool IsNotEqual(this int result)
        {
            return 0u >= (uint)result ? false : true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static bool IsLessThan(this int result)
        {
            return (uint)result > JsonSharedConstant.TooBigOrNegative ? true : false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static bool IsLessThanOrEqual(this int result)
        {
            uint uresult = (uint)result;
            return uresult > JsonSharedConstant.TooBigOrNegative || 0u >= uresult ? true : false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static bool IsGreaterThan(this int result)
        {
            uint uresult = (uint)result;
            return uresult > JsonSharedConstant.TooBigOrNegative || 0u >= uresult ? false : true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static bool IsGreaterThanOrEqual(this int result)
        {
            uint uresult = (uint)result;
            return uresult > JsonSharedConstant.TooBigOrNegative ? false : true;
        }
    }
}