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


namespace SpanJson.Linq
{
    /// <summary>Represents a raw JSON string.</summary>
    public partial class JRaw : JValue
    {
        /// <summary>Initializes a new instance of the <see cref="JRaw"/> class from another <see cref="JRaw"/> object.</summary>
        /// <param name="other">A <see cref="JRaw"/> object to copy from.</param>
        public JRaw(JRaw other) : base(other) { }

        /// <summary>Initializes a new instance of the <see cref="JRaw"/> class.</summary>
        /// <param name="rawJson">The raw json.</param>
        public JRaw(string? rawJson) : base(rawJson, JTokenType.Raw) { }

        /// <summary>Initializes a new instance of the <see cref="JRaw"/> class.</summary>
        /// <param name="utf8Json">The raw json.</param>
        public JRaw(byte[]? utf8Json) : base(utf8Json, JTokenType.Raw) { }

        internal override JToken CloneToken() => new JRaw(this);
    }
}