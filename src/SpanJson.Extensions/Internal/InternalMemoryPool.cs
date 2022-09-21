﻿namespace SpanJson.Internal
{
    using System;

    internal static class InternalMemoryPool
    {
        internal const int InitialCapacity = 64 * 1024;

        [ThreadStatic]
        static byte[]? s_buffer = null;

        public static byte[] GetBuffer()
        {
            s_buffer ??= new byte[InitialCapacity];
            return s_buffer;
        }
    }
}
