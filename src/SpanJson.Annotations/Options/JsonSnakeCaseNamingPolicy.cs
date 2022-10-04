namespace SpanJson
{
    using SpanJson.Internal;

    /// <summary>A snake case naming strategy.</summary>
    sealed class JsonSnakeCaseNamingPolicy : JsonNamingPolicy
    {
        public static readonly JsonSnakeCaseNamingPolicy Instance = new();

        private JsonSnakeCaseNamingPolicy() { }

        /// <inheritdoc />
        public override string ConvertName(string name) => StringMutator.ToSnakeCase(name);
    }
}
