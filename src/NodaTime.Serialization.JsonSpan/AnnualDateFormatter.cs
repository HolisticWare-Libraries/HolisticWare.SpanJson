namespace NodaTime.Serialization.JsonSpan
{
    using NodaTime.Text;

    /// <summary>Formatter for local dates, using the ISO-8601 date pattern.</summary>
    public sealed class AnnualDateFormatter : NodaPatternFormatterBase<AnnualDate>
    {
        public static readonly AnnualDateFormatter Default = new AnnualDateFormatter();

        private AnnualDateFormatter() : base(AnnualDatePattern.Iso) { }
    }
}
