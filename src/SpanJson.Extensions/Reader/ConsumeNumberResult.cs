﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace SpanJson
{
    /// <summary>
    /// This enum captures the tri-state return value when trying to read a
    /// JSON number.
    /// </summary>
    internal enum ConsumeNumberResult : byte
    {
        /// <summary>
        /// Reached a valid end of number and hence no action is required.
        /// </summary>
        Success,
        /// <summary>
        /// Successfully processed a portion of the number and need to
        /// read to the next region of the number.
        /// </summary>
        OperationIncomplete,
        /// <summary>
        /// Observed incomplete data.
        /// Return false if we have more data to follow. Otherwise throw.
        /// </summary>
        NeedMoreData,
    }
}
