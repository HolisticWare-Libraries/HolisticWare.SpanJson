namespace SpanJson
{
    using SpanJson.Internal;

    /// <summary>A camel case naming strategy.</summary>
    sealed class JsonCamelCaseNamingPolicy : JsonNamingPolicy
    {
        public static readonly JsonCamelCaseNamingPolicy Instance = new();

        private JsonCamelCaseNamingPolicy() { }

        /// <inheritdoc />
        public override string ConvertName(string name) => StringMutator.ToCamelCase(name);
    }
}
