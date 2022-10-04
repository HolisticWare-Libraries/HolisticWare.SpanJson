namespace SpanJson
{
    using SpanJson.Internal;

    /// <summary>A train case naming strategy.</summary>
    sealed class JsonTrainCaseNamingPolicy : JsonNamingPolicy
    {
        public static readonly JsonTrainCaseNamingPolicy Instance = new();

        private JsonTrainCaseNamingPolicy() { }

        /// <inheritdoc />
        public override string ConvertName(string name) => StringMutator.ToTrainCase(name);
    }
}
