namespace SpanJson
{
    using SpanJson.Internal;

    /// <summary>A ada case naming strategy.</summary>
    sealed class JsonAdaCaseNamingPolicy : JsonNamingPolicy
    {
        public static readonly JsonAdaCaseNamingPolicy Instance = new();

        private JsonAdaCaseNamingPolicy() { }

        /// <inheritdoc />
        public override string ConvertName(string name) => StringMutator.ToAdaCase(name);
    }
}
