using System.Text;
using Xunit;
using Utf16Serializer = SpanJson.JsonSerializer.Generic.Utf16;
using Utf8Serializer = SpanJson.JsonSerializer.Generic.Utf8;

namespace SpanJson.Tests
{
    public class AdaCaseTests
    {
        [Fact]
        public void SerializeDeserializeUtf16()
        {
            var input = new TestObject { AdaCaseText = "Hello World"};
            var serialized = Utf16Serializer.Serialize<TestObject>(input, JsonKnownNamingPolicy.AdaCase);
            Assert.Contains("\"Ada_Case_Text\":", serialized);
            var deserialized = Utf16Serializer.Deserialize<TestObject>(serialized, JsonKnownNamingPolicy.AdaCase);
            Assert.Equal(input, deserialized);
        }

        [Fact]
        public void SerializeDeserializeUtf8()
        {
            var input = new TestObject { AdaCaseText = "Hello World"};
            var serialized = Utf8Serializer.Serialize<TestObject>(input, JsonKnownNamingPolicy.AdaCase);
            Assert.Contains("\"Ada_Case_Text\":", Encoding.UTF8.GetString(serialized));
            var deserialized = Utf8Serializer.Deserialize<TestObject>(serialized, JsonKnownNamingPolicy.AdaCase);
            Assert.Equal(input, deserialized);
        }

        public class TestObject : IEquatable<TestObject>
        {
            public string AdaCaseText { get; set; }

            public bool Equals(TestObject other)
            {
                if (ReferenceEquals(null, other)) return false;
                if (ReferenceEquals(this, other)) return true;
                return string.Equals(AdaCaseText, other.AdaCaseText);
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
                return AdaCaseText?.GetHashCode() ?? 0;
            }
        }
    }
}