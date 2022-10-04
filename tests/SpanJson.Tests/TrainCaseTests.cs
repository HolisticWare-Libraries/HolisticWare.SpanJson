using System.Text;
using Xunit;
using Utf16Serializer = SpanJson.JsonSerializer.Generic.Utf16;
using Utf8Serializer = SpanJson.JsonSerializer.Generic.Utf8;

namespace SpanJson.Tests
{
    public class TrainCaseTests
    {
        [Fact]
        public void SerializeDeserializeUtf16()
        {
            var input = new TestObject { TrainCaseText = "Hello World"};
            var serialized = Utf16Serializer.Serialize<TestObject>(input, JsonKnownNamingPolicy.TrainCase);
            Assert.Contains("\"Train-Case-Text\":", serialized);
            var deserialized = Utf16Serializer.Deserialize<TestObject>(serialized, JsonKnownNamingPolicy.TrainCase);
            Assert.Equal(input, deserialized);
        }

        [Fact]
        public void SerializeDeserializeUtf8()
        {
            var input = new TestObject { TrainCaseText = "Hello World"};
            var serialized = Utf8Serializer.Serialize<TestObject>(input, JsonKnownNamingPolicy.TrainCase);
            Assert.Contains("\"Train-Case-Text\":", Encoding.UTF8.GetString(serialized));
            var deserialized = Utf8Serializer.Deserialize<TestObject>(serialized, JsonKnownNamingPolicy.TrainCase);
            Assert.Equal(input, deserialized);
        }

        public class TestObject : IEquatable<TestObject>
        {
            public string TrainCaseText { get; set; }

            public bool Equals(TestObject other)
            {
                if (ReferenceEquals(null, other)) return false;
                if (ReferenceEquals(this, other)) return true;
                return string.Equals(TrainCaseText, other.TrainCaseText);
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
                return TrainCaseText?.GetHashCode() ?? 0;
            }
        }
    }
}