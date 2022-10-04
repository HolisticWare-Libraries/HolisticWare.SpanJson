namespace SpanJson
{
    using SpanJson.Internal;

    /// <summary>A macro case naming strategy.</summary>
    sealed class JsonMacroCaseNamingPolicy : JsonNamingPolicy
    {
        public static readonly JsonMacroCaseNamingPolicy Instance = new();

        private JsonMacroCaseNamingPolicy() { }

        /// <inheritdoc />
        public override string ConvertName(string name) => StringMutator.ToMacroCase(name);
    }
}
