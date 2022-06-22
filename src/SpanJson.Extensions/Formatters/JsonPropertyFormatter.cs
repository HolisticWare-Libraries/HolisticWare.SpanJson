using SpanJson.Document;
using SpanJson.Internal;

namespace SpanJson.Formatters
{
    public sealed class JsonPropertyFormatter : JsonElementFormatterBase<JsonProperty>
    {
        public static readonly JsonPropertyFormatter Default = new JsonPropertyFormatter();

        public override void Serialize(ref JsonWriter<byte> writer, JsonProperty value, IJsonFormatterResolver<byte> resolver)
        {
            writer.WriteUtf8Name(JsonHelpers.GetEncodedText(value.Name, resolver.EscapeHandling, resolver.Encoder));
            JsonElementFormatter.Default.Serialize(ref writer, value.Value, resolver);
        }

        public override void Serialize(ref JsonWriter<char> writer, JsonProperty value, IJsonFormatterResolver<char> resolver)
        {
            writer.WriteUtf16Name(JsonHelpers.GetEncodedText(value.Name, resolver.EscapeHandling, resolver.Encoder));
            JsonElementFormatter.Default.Serialize(ref writer, value.Value, resolver);
        }
    }
}
