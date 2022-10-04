namespace SpanJson
{
    using SpanJson.Internal;

    /// <summary>A cobol case naming strategy.</summary>
    sealed class JsonCobolCaseNamingPolicy : JsonNamingPolicy
    {
        public static readonly JsonCobolCaseNamingPolicy Instance = new();

        private JsonCobolCaseNamingPolicy() { }

        /// <inheritdoc />
        public override string ConvertName(string name) => StringMutator.ToCobolCase(name);
    }
}
