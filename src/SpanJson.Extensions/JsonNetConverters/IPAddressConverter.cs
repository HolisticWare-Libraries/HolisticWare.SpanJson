using System.Net;
using Newtonsoft.Json.Linq;

namespace Newtonsoft.Json.Converters;

public sealed class IPAddressConverter : JsonConverter
{
    public static readonly IPAddressConverter Instance = new IPAddressConverter();

    public override bool CanConvert(Type objectType)
    {
        return (objectType == typeof(IPAddress));
    }

    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        if (value is IPAddress ip)
        {
            writer.WriteValue(ip.ToString());
        }
        else
        {
            writer.WriteNull();
        }
    }

    public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        JToken token = JToken.Load(reader);
        var v = token.Value<string>();
        return v is not null ? IPAddress.Parse(v) : null;
    }
}
