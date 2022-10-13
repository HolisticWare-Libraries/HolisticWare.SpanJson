using SpanJson.Dynamic;

namespace SpanJson.Converters;

public sealed class DynamicUtf8NumberConverter : Newtonsoft.Json.JsonConverter
{
    public static readonly DynamicUtf8NumberConverter Instance = new();

    private DynamicUtf8NumberConverter() { }

    public override void WriteJson(Newtonsoft.Json.JsonWriter writer, object? value, Newtonsoft.Json.JsonSerializer serializer)
    {
        switch (value)
        {
            case null:
                writer.WriteNull();
                break;

            case SpanJsonDynamicUtf8Number utf8Number:
                writer.WriteValue(utf8Number);
                break;

            default:
                throw ThrowHelper2.GetJsonSerializationException<SpanJsonDynamicUtf8Number>();
        }
    }

    public override object? ReadJson(Newtonsoft.Json.JsonReader reader, Type objectType, object? existingValue, Newtonsoft.Json.JsonSerializer serializer)
    {
        throw ThrowHelper.GetNotSupportedException();
    }

    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(SpanJsonDynamicUtf8Number);
    }
}
