namespace SpanJson.Linq.JsonPath
{
    internal class FieldMultipleFilter : PathFilter
    {
        internal List<string> Names;

        public FieldMultipleFilter(List<string> names)
        {
            Names = names;
        }

        public override IEnumerable<JToken> ExecuteFilter(JToken root, IEnumerable<JToken> current, JsonSelectSettings? settings)
        {
            var errorWhenNoMatch = settings?.ErrorWhenNoMatch ?? false;

            foreach (JToken t in current)
            {
                if (t is JObject o)
                {
                    foreach (string name in Names)
                    {
                        var v = o[name];

                        if (v is not null)
                        {
                            yield return v;
                        }

                        if (errorWhenNoMatch)
                        {
                            ThrowHelper2.ThrowJsonException_Property_does_not_exist_on_JObject(name);
                        }
                    }
                }
                else
                {
                    if (errorWhenNoMatch)
                    {
                        ThrowHelper2.ThrowJsonException_Property_not_valid_on(Names, t);
                    }
                }
            }
        }
    }
}