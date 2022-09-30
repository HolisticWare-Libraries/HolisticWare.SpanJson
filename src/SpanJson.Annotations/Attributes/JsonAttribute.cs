namespace SpanJson
{
    /// <summary>
    /// The base class of serialization attributes.
    /// </summary>
    [AttributeUsage(AttributeTargets.All, Inherited = true, AllowMultiple = false)]
    public abstract class JsonAttribute : Attribute { }
}
