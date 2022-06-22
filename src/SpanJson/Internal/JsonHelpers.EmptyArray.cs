using System;
using System.Runtime.CompilerServices;

namespace SpanJson.Internal
{
    static partial class JsonHelpers
    {
        internal sealed class EmptyArray<T>
        {
            public static readonly T[] Instance;

            static EmptyArray()
            {
                Instance = Array.Empty<T>();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T[] Empty<T>() => EmptyArray<T>.Instance;
    }
}
