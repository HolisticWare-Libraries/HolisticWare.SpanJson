using CuteAnt;

namespace SpanJson.Converters;

public sealed class CombGuidJTokenConverter : CustomPrimitiveValueConverter<CombGuid>
{
    public static readonly CombGuidJTokenConverter Instance = new();

    private CombGuidJTokenConverter() { }
}
