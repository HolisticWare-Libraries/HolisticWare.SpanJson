using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace SpanJson.Internal
{
    static partial class JsonHelpers
    {
        /// <summary>TBD</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static bool IsCompletedSuccessfully(this Task task)
        {
#if NETSTANDARD2_0
            return task.IsCompleted && !task.IsFaulted && !task.IsCanceled;
#else
            return task.IsCompletedSuccessfully;
#endif
        }

        /// <summary>TBD</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static bool IsCompletedSuccessfully<T>(this Task<T> task)
        {
#if NETSTANDARD2_0
            return task.IsCompleted && !task.IsFaulted && !task.IsCanceled;
#else
            return task.IsCompletedSuccessfully;
#endif
        }
    }
}
