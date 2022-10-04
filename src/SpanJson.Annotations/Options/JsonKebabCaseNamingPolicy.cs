namespace SpanJson
{
    using SpanJson.Internal;

    /// <summary>A kebab case naming strategy.</summary>
    sealed class JsonKebabCaseNamingPolicy : JsonNamingPolicy
    {
        public static readonly JsonKebabCaseNamingPolicy Instance = new();

        private JsonKebabCaseNamingPolicy() { }

        /// <inheritdoc />
        public override string ConvertName(string name) => StringMutator.ToKebabCase(name);
    }
}
