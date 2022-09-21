namespace SpanJson.Linq.JsonPath
{
    internal class ArrayMultipleIndexFilter : PathFilter
    {
        internal List<int> Indexes;

        public ArrayMultipleIndexFilter(List<int> indexes)
        {
            Indexes = indexes;
        }

        public override IEnumerable<JToken> ExecuteFilter(JToken root, IEnumerable<JToken> current, JsonSelectSettings? settings)
        {
            foreach (JToken t in current)
            {
                foreach (int i in Indexes)
                {
                    var v = GetTokenIndex(t, settings, i);

                    if (v is not null)
                    {
                        yield return v;
                    }
                }
            }
        }
    }
}