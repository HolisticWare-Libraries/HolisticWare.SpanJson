using SpanJson.Dynamic;

namespace SpanJson.Converters;

public sealed class DynamicUtf8StringConverter : Newtonsoft.Json.JsonConverter
{
    public static readonly DynamicUtf8StringConverter Instance = new();

    private DynamicUtf8StringConverter() { }

    public override void WriteJson(Newtonsoft.Json.JsonWriter writer, object? value, Newtonsoft.Json.JsonSerializer serializer)
    {
        switch (value)
        {
            case null:
                writer.WriteNull();
                break;

            case SpanJsonDynamicUtf8String utf8String:
                writer.WriteValue(utf8String);
                break;

            default:
                throw ThrowHelper2.GetJsonSerializationException<SpanJsonDynamicUtf8String>();
        }
    }

    public override object? ReadJson(Newtonsoft.Json.JsonReader reader, Type objectType, object? existingValue, Newtonsoft.Json.JsonSerializer serializer)
    {
        throw ThrowHelper.GetNotSupportedException();
    }

    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(SpanJsonDynamicUtf8String);
    }
}
