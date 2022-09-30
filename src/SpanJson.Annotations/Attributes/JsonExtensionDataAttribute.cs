namespace SpanJson
{
    /// <summary>Annotate properties or fields of type IDictionary&lt;string,object&gt;
    /// to serialize the values from that dictionary into the parent object as root level attributes
    /// During deserialization any unknown attribute will be put into the dictionary instead of being skipped.</summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class JsonExtensionDataAttribute : JsonAttribute { }
}