﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Buffers;

namespace SpanJson.Serialization
{
    public class JsonArrayPool<T> : Newtonsoft.Json.IArrayPool<T>
    {
        private readonly ArrayPool<T> _inner;

        public JsonArrayPool(ArrayPool<T> inner)
        {
            if (inner is null) { ThrowHelper.ThrowArgumentNullException(ExceptionArgument.inner); }
            _inner = inner;
        }

        public T[] Rent(int minimumLength) => _inner.Rent(minimumLength);

        public void Return(T[]? array)
        {
            if (array is null) { return; }

            _inner.Return(array);
        }
    }
}
