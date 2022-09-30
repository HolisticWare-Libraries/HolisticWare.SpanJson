﻿using System;
using System.Text;
using Xunit;
using Utf16Serializer = SpanJson.JsonSerializer.Generic.Utf16;
using Utf8Serializer = SpanJson.JsonSerializer.Generic.Utf8;
using Utf16CamelCaseSerializer = SpanJson.JsonCamelCaseSerializer.Generic.Utf16;
using Utf8CamelCaseSerializer = SpanJson.JsonCamelCaseSerializer.Generic.Utf8;
using Utf16SnakeCaseSerializer = SpanJson.JsonSnakeCaseSerializer.Generic.Utf16;
using Utf8SnakeCaseSerializer = SpanJson.JsonSnakeCaseSerializer.Generic.Utf8;

namespace SpanJson.Tests
{
    public class CamelCaseTests
    {
        [Fact]
        public void SerializeDeserializeUtf16()
        {
            var input = new TestObject {Text = "Hello World"};
            var serialized = Utf16CamelCaseSerializer.Serialize<TestObject>(input);
            Assert.Contains("\"text\":", serialized);
            var deserialized = Utf16CamelCaseSerializer.Deserialize<TestObject>(serialized);
            Assert.Equal(input, deserialized);
        }

        [Fact]
        public void SerializeDeserializeUtf16_mix()
        {
            var input = new TestObject { Text = "Hello World" };
            var serialized = Utf16CamelCaseSerializer.Serialize<TestObject>(input);
            Assert.Contains("\"text\":", serialized);
            var deserialized = Utf16Serializer.Deserialize<TestObject>(serialized);
            Assert.Equal(input, deserialized);
            deserialized = Utf16SnakeCaseSerializer.Deserialize<TestObject>(serialized);
            Assert.Equal(input, deserialized);
        }

        [Fact]
        public void SerializeDeserializeUtf8()
        {
            var input = new TestObject {Text = "Hello World"};
            var serialized = Utf8CamelCaseSerializer.Serialize<TestObject>(input);
            Assert.Contains("\"text\":", Encoding.UTF8.GetString(serialized));
            var deserialized = Utf8CamelCaseSerializer.Deserialize<TestObject>(serialized);
            Assert.Equal(input, deserialized);
        }

        [Fact]
        public void SerializeDeserializeUtf8_mix()
        {
            var input = new TestObject { Text = "Hello World" };
            var serialized = Utf8CamelCaseSerializer.Serialize<TestObject>(input);
            Assert.Contains("\"text\":", Encoding.UTF8.GetString(serialized));
            var deserialized = Utf8Serializer.Deserialize<TestObject>(serialized);
            Assert.Equal(input, deserialized);
            deserialized = Utf8SnakeCaseSerializer.Deserialize<TestObject>(serialized);
            Assert.Equal(input, deserialized);
        }

        public class TestObject : IEquatable<TestObject>
        {
            public string Text { get; set; }

            public bool Equals(TestObject other)
            {
                if (ReferenceEquals(null, other)) return false;
                if (ReferenceEquals(this, other)) return true;
                return string.Equals(Text, other.Text);
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != GetType()) return false;
                return Equals((TestObject) obj);
            }

            public override int GetHashCode()
            {
                // ReSharper disable once NonReadonlyMemberInGetHashCode
                return Text?.GetHashCode() ?? 0;
            }
        }
    }
}