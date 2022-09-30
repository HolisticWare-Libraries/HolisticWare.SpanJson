﻿namespace SpanJson
{
    using System.Text.Encodings.Web;

    public class SpanJsonOptions
    {
        public NullOptions NullOption { get; set; }
        public EnumOptions EnumOption { get; set; }

        public JsonEscapeHandling EscapeHandling { get; set; }

        /// <summary>The encoder to use when escaping strings, or <see langword="null" /> to use the default encoder.</summary>
        public JavaScriptEncoder? Encoder { get; set; }

        /// <summary>Not yet supported</summary>
        public JsonNamingPolicy? DictionaryKeyPolicy { get; set; }

        public JsonNamingPolicy? ExtensionDataNamingPolicy { get; set; }
        public JsonNamingPolicy? PropertyNamingPolicy { get; set; }

        /// <summary>Determines whether a property's name uses a case-insensitive comparison during deserialization.
        /// The default value is true.</summary>
        internal bool PropertyNameCaseInsensitive { get; set; } = true;
    }
}