﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Runtime.InteropServices;
using SpanJson.Internal;
using Xunit;

namespace SpanJson.Tests.Internal
{
    public static partial class SpanTests
    {
        [Fact]
        public static void ZeroLengthSequenceCompareTo_Char()
        {
            var a = new char[3];

            var first = new Span<char>(a, 1, 0);
            var second = new ReadOnlySpan<char>(a, 2, 0);
            int result = SpanHelpers.SequenceCompareTo(ref MemoryMarshal.GetReference(first), first.Length, ref MemoryMarshal.GetReference(second), second.Length);
            Assert.Equal(0, result);
        }

        [Fact]
        public static void SameSpanSequenceCompareTo_Char()
        {
            char[] a = { '4', '5', '6' };
            var span = new Span<char>(a);
            ref char cStart = ref MemoryMarshal.GetReference(span);
            int result = SpanHelpers.SequenceCompareTo(ref cStart, span.Length, ref cStart, span.Length);
            Assert.Equal(0, result);
        }

        [Fact]
        public static void SequenceCompareToArrayImplicit_Char()
        {
            char[] a = { '4', '5', '6' };
            var first = new Span<char>(a, 0, 3);
            int result = SpanHelpers.SequenceCompareTo(ref MemoryMarshal.GetReference(first), first.Length, ref a[0], a.Length);
            Assert.Equal(0, result);
        }

        [Fact]
        public static void SequenceCompareToArraySegmentImplicit_Char()
        {
            char[] src = { '1', '2', '3' };
            char[] dst = { '5', '1', '2', '3', '9' };
            var segment = new ArraySegment<char>(dst, 1, 3);

            var first = new Span<char>(src, 0, 3);
            Span<char> second = segment;
            int result = SpanHelpers.SequenceCompareTo(ref MemoryMarshal.GetReference(first), first.Length, ref MemoryMarshal.GetReference(second), second.Length);
            Assert.Equal(0, result);
        }

        [Fact]
        public static void LengthMismatchSequenceCompareTo_Char()
        {
            char[] a = { '4', '5', '6' };
            var first = new Span<char>(a, 0, 2);
            var second = new Span<char>(a, 0, 3);
            int result = SpanHelpers.SequenceCompareTo(ref MemoryMarshal.GetReference(first), first.Length, ref MemoryMarshal.GetReference(second), second.Length);
            Assert.True(result < 0);

            result = SpanHelpers.SequenceCompareTo(ref MemoryMarshal.GetReference(second), second.Length, ref MemoryMarshal.GetReference(first), first.Length);
            Assert.True(result > 0);

            // one sequence is empty
            first = new Span<char>(a, 1, 0);

            result = SpanHelpers.SequenceCompareTo(ref MemoryMarshal.GetReference(first), first.Length, ref MemoryMarshal.GetReference(second), second.Length);
            Assert.True(result < 0);

            result = SpanHelpers.SequenceCompareTo(ref MemoryMarshal.GetReference(second), second.Length, ref MemoryMarshal.GetReference(first), first.Length);
            Assert.True(result > 0);
        }

        [Fact]
        public static void SequenceCompareToWithSingleMismatch_Char()
        {
            for (int length = 1; length < 32; length++)
            {
                for (int mismatchIndex = 0; mismatchIndex < length; mismatchIndex++)
                {
                    var first = new char[length];
                    var second = new char[length];
                    for (int i = 0; i < length; i++)
                    {
                        first[i] = second[i] = (char)(i + 1);
                    }

                    second[mismatchIndex] = (char)(second[mismatchIndex] + 1);

                    var firstSpan = new Span<char>(first);
                    var secondSpan = new ReadOnlySpan<char>(second);
                    int result = SpanHelpers.SequenceCompareTo(ref MemoryMarshal.GetReference(firstSpan), firstSpan.Length, ref MemoryMarshal.GetReference(secondSpan), secondSpan.Length);
                    Assert.True(result < 0);

                    result = SpanHelpers.SequenceCompareTo(ref MemoryMarshal.GetReference(secondSpan), secondSpan.Length, ref MemoryMarshal.GetReference(firstSpan), firstSpan.Length);
                    Assert.True(result > 0);
                }
            }
        }

        [Fact]
        public static void SequenceCompareToNoMatch_Char()
        {
            for (int length = 1; length < 32; length++)
            {
                var first = new char[length];
                var second = new char[length];

                for (int i = 0; i < length; i++)
                {
                    first[i] = (char)(i + 1);
                    second[i] = (char)(char.MaxValue - i);
                }

                var firstSpan = new Span<char>(first);
                var secondSpan = new ReadOnlySpan<char>(second);
                int result = SpanHelpers.SequenceCompareTo(ref MemoryMarshal.GetReference(firstSpan), firstSpan.Length, ref MemoryMarshal.GetReference(secondSpan), secondSpan.Length);
                Assert.True(result < 0);

                result = SpanHelpers.SequenceCompareTo(ref MemoryMarshal.GetReference(secondSpan), secondSpan.Length, ref MemoryMarshal.GetReference(firstSpan), firstSpan.Length);
                Assert.True(result > 0);
            }
        }

        [Fact]
        public static void MakeSureNoSequenceCompareToChecksGoOutOfRange_Char()
        {
            for (int length = 0; length < 100; length++)
            {
                var first = new char[length + 2];
                first[0] = '8';
                first[length + 1] = '8';

                var second = new char[length + 2];
                second[0] = '9';
                second[length + 1] = '9';

                var span1 = new Span<char>(first, 1, length);
                var span2 = new ReadOnlySpan<char>(second, 1, length);
                int result = SpanHelpers.SequenceCompareTo(ref MemoryMarshal.GetReference(span1), span1.Length, ref MemoryMarshal.GetReference(span2), span2.Length);
                Assert.Equal(0, result);
            }
        }
    }
}
