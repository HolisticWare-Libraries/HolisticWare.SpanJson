namespace SpanJson
{
    /// <summary>
    /// The <see cref="JsonNamingPolicy"/> to be used at run time.
    /// </summary>
    public enum JsonKnownNamingPolicy
    {
        /// <summary>
        /// Specifies that JSON property names should not be converted.
        /// </summary>
        Unspecified = 0,

        /// <summary>
        /// Specifies that the built-in <see cref="JsonNamingPolicy.CamelCase"/> be used to convert JSON property names.
        /// </summary>
        CamelCase = 1,

        /// <summary>
        /// Specifies that the built-in <see cref="JsonNamingPolicy.SnakeCase"/> be used to convert JSON property names.
        /// </summary>
        SnakeCase = 2,

        /// <summary>
        /// Specifies that the built-in <see cref="JsonNamingPolicy.AdaCase"/> be used to convert JSON property names.
        /// </summary>
        AdaCase = 3,

        /// <summary>
        /// Specifies that the built-in <see cref="JsonNamingPolicy.MacroCase"/> be used to convert JSON property names.
        /// </summary>
        MacroCase = 4,

        /// <summary>
        /// Specifies that the built-in <see cref="JsonNamingPolicy.KebabCase"/> be used to convert JSON property names.
        /// </summary>
        KebabCase = 5,

        /// <summary>
        /// Specifies that the built-in <see cref="JsonNamingPolicy.TrainCase"/> be used to convert JSON property names.
        /// </summary>
        TrainCase = 6,

        /// <summary>
        /// Specifies that the built-in <see cref="JsonNamingPolicy.CobolCase"/> be used to convert JSON property names.
        /// </summary>
        CobolCase = 7,
    }
}
