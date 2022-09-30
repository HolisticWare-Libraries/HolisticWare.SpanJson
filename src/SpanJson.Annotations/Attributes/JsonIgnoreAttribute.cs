namespace SpanJson
{
    /// <summary>
    /// Prevents a property or field from being serialized or deserialized.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public sealed class JsonIgnoreAttribute : JsonAttribute { }
}
