namespace SpanJson
{
    /// <summary>Determines the naming policy used to convert a string-based name to another format, such as a camel-casing format.</summary>
    public abstract class JsonNamingPolicy
    {
        /// <summary>Initializes a new instance of <see cref="JsonNamingPolicy"/>.</summary>
        protected JsonNamingPolicy() { }

        /// <summary>Returns the naming policy for camel-casing.
        /// Output example: <see langword="myNameIsBond"/></summary>
        public static JsonNamingPolicy CamelCase { get; } = JsonCamelCaseNamingPolicy.Instance;

        /// <summary>Returns the naming policy for snake-casing.
        /// Output example: <see langword="my_name_is_bond"/></summary>
        public static JsonNamingPolicy SnakeCase { get; } = JsonSnakeCaseNamingPolicy.Instance;

        /// <summary>Returns the naming policy for ada-casing.
        /// Output example: <see langword="My_Name_Is_Bond"/></summary>
        public static JsonNamingPolicy AdaCase { get; } = JsonAdaCaseNamingPolicy.Instance;

        /// <summary>Returns the naming policy for macro-casing.
        /// Output example: <see langword="MY_NAME_IS_BOND"/></summary>
        public static JsonNamingPolicy MacroCase { get; } = JsonMacroCaseNamingPolicy.Instance;

        /// <summary>Returns the naming policy for kebab-casing.
        /// Output example: <see langword="my-name-is-bond"/></summary>
        public static JsonNamingPolicy KebabCase { get; } = JsonKebabCaseNamingPolicy.Instance;

        /// <summary>Returns the naming policy for train-casing.
        /// Output example: <see langword="My-Name-Is-Bond"/></summary>
        public static JsonNamingPolicy TrainCase { get; } = JsonTrainCaseNamingPolicy.Instance;

        /// <summary>Returns the naming policy for cobol-casing.
        /// Output example: <see langword="MY-NAME-IS-BOND"/></summary>
        public static JsonNamingPolicy CobolCase { get; } = JsonCobolCaseNamingPolicy.Instance;

        /// <summary>When overridden in a derived class, converts the specified name according to the policy.</summary>
        /// <param name="name">The name to convert.</param>
        /// <returns>The converted name.</returns>
        public abstract string ConvertName(string name);
    }
}
