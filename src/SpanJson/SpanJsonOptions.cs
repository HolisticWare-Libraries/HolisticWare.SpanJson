namespace SpanJson
{
    using System.Text.Encodings.Web;

    public class SpanJsonOptions
    {
        public SpanJsonOptions()
            : this(NullOptions.ExcludeNulls, EnumOptions.String)
        {
        }

        public SpanJsonOptions(NullOptions nullOptions, EnumOptions enumOptions,
            JsonNamingPolicy? propertyNamingPolicy = null, JsonNamingPolicy? extensionDataNamingPolicy = null, JsonNamingPolicy? dictionaryKeyPolicy = null)
        {
            NullOption = nullOptions;
            EnumOption = enumOptions;
            PropertyNamingPolicy = propertyNamingPolicy;
            ExtensionDataNamingPolicy = extensionDataNamingPolicy;
            DictionaryKeyPolicy = dictionaryKeyPolicy;
        }

        public NullOptions NullOption { get; }
        public EnumOptions EnumOption { get; set; }

        /// <summary>Determines whether a property's name uses a case-insensitive comparison during deserialization.
        /// The default value is true.</summary>
        public bool PropertyNameCaseInsensitive { get; set; } = true;

        public JsonNamingPolicy? PropertyNamingPolicy { get; }
        public JsonNamingPolicy? ExtensionDataNamingPolicy { get; }

        /// <summary>Not yet supported</summary>
        public JsonNamingPolicy? DictionaryKeyPolicy { get; }

        public JsonEscapeHandling EscapeHandling { get; set; }

        /// <summary>The encoder to use when escaping strings, or <see langword="null" /> to use the default encoder.</summary>
        public JavaScriptEncoder? Encoder { get; set; }
    }
}