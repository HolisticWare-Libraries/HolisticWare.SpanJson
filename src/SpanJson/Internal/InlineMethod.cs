using System.Runtime.CompilerServices;

namespace SpanJson
{
    /// <summary>Helper class for constants for inlining methods</summary>
    internal static class InlineMethod
    {
        /// <summary>Value for lining method</summary>
        public const MethodImplOptions AggressiveInlining = MethodImplOptions.AggressiveInlining;

        /// <summary>Value for lining method</summary>
        public const MethodImplOptions AggressiveOptimization =
#if NET || NETCOREAPP3_0_OR_GREATER
            MethodImplOptions.AggressiveOptimization;
#else
            MethodImplOptions.AggressiveInlining;
#endif
    }
}