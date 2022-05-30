namespace NodaTime.Serialization.JsonSpan.Tests
{
    using SpanJson;
    using Xunit;

    public class NodaAnnualDateConverterTest
    {
        [Fact]
        public void Serialize_NonNullableType()
        {
            var annualDate = new AnnualDate(07, 01);
            var json = JsonSerializer.Generic.Utf16.Serialize<AnnualDate, NodaExcludeNullsCamelCaseResolver<char>>(annualDate);
            string expectedJson = "\"07-01\"";
            Assert.Equal(expectedJson, json);
        }

        [Fact]
        public void Serialize_NullableType_NonNullValue()
        {
            AnnualDate? annualDate = new AnnualDate(07, 01);
            var json = JsonSerializer.Generic.Utf16.Serialize<AnnualDate?, NodaExcludeNullsCamelCaseResolver<char>>(annualDate);
            string expectedJson = "\"07-01\"";
            Assert.Equal(expectedJson, json);
        }

        [Fact]
        public void Serialize_NullableType_NullValue()
        {
            AnnualDate? instant = null;
            var json = JsonSerializer.Generic.Utf16.Serialize<AnnualDate?, NodaExcludeNullsCamelCaseResolver<char>>(instant);
            string expectedJson = "null";
            Assert.Equal(expectedJson, json);
        }

        [Fact]
        public void Deserialize_ToNonNullableType()
        {
            string json = "\"07-01\"";
            var annualDate = JsonSerializer.Generic.Utf16.Deserialize<AnnualDate, NodaExcludeNullsCamelCaseResolver<char>>(json);
            var expectedAnnualDate = new AnnualDate(07, 01);
            Assert.Equal(expectedAnnualDate, annualDate);
        }

        [Fact]
        public void Deserialize_ToNullableType_NonNullValue()
        {
            string json = "\"07-01\"";
            var annualDate = JsonSerializer.Generic.Utf16.Deserialize<AnnualDate?, NodaExcludeNullsCamelCaseResolver<char>>(json);
            AnnualDate? expectedAnnualDate = new AnnualDate(07, 01);
            Assert.Equal(expectedAnnualDate, annualDate);
        }

        [Fact]
        public void Deserialize_ToNullableType_NullValue()
        {
            string json = "null";
            var annualDate = JsonSerializer.Generic.Utf16.Deserialize<AnnualDate?, NodaExcludeNullsCamelCaseResolver<char>>(json);
            Assert.Null(annualDate);
        }
    }
}
