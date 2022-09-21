using System.Net;
using Newtonsoft.Json.Linq;

namespace Newtonsoft.Json.Converters;

public sealed class IPEndPointConverter : JsonConverter
{
    public static readonly IPEndPointConverter Instance = new IPEndPointConverter();

    public override bool CanConvert(Type objectType)
    {
        return (objectType == typeof(IPEndPoint));
    }

    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        var ep = value as IPEndPoint;
        if (ep is null) { writer.WriteNull(); return; }

        writer.WriteStartObject();
        writer.WritePropertyName("Address");
        serializer.Serialize(writer, ep.Address);
        writer.WritePropertyName("Port");
        writer.WriteValue(ep.Port);
        writer.WriteEndObject();
    }

    public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        JObject jo = JObject.Load(reader);
        var address = jo["Address"]?.ToObject<IPAddress>(serializer);
        if (address is null) { return null; }
        var port = jo["Port"]?.Value<int>();
        if (port is null) { return null; }
        return new IPEndPoint(address, port.Value);
    }
}
