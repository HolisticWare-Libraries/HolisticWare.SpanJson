﻿namespace SpanJson.Internal
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Text;

    static partial class TextEncodings
    {
        public static partial class Utf16
        {
            static readonly AsymmetricKeyHashTable<string> s_stringCache = new(StringReadOnlySpanByteAscymmetricEqualityComparer.Instance);

            /// <summary>For short strings use only.</summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static string GetStringWithCache(in ReadOnlySpan<byte> utf16Source)
            {
                if (utf16Source.IsEmpty) { return string.Empty; }
                if (s_stringCache.TryGetValue(utf16Source, out var value))
                {
                    return value;
                }
                return GetStringWithCacheSlow(utf16Source);
            }

            [MethodImpl(MethodImplOptions.NoInlining)]
            private static string GetStringWithCacheSlow(in ReadOnlySpan<byte> utf16Source)
            {
                var buffer = utf16Source.ToArray();
                var value = Encoding.Unicode.GetString(buffer);
                s_stringCache.TryAdd(buffer, value);
                return value;
            }
        }
    }
}
