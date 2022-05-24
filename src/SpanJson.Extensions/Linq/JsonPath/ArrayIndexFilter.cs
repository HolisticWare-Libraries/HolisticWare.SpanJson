using System.Collections.Generic;

namespace SpanJson.Linq.JsonPath
{
    internal class ArrayIndexFilter : PathFilter
    {
        public int? Index { get; set; }

        public override IEnumerable<JToken> ExecuteFilter(JToken root, IEnumerable<JToken> current, JsonSelectSettings settings)
        {
            var errorWhenNoMatch = settings?.ErrorWhenNoMatch ?? false;

            foreach (JToken t in current)
            {
                if (Index is not null)
                {
                    JToken v = GetTokenIndex(t, settings, Index.GetValueOrDefault());

                    if (v is not null)
                    {
                        yield return v;
                    }
                }
                else
                {
                    if (t is JArray /*|| t is JConstructor*/)
                    {
                        foreach (JToken v in t)
                        {
                            yield return v;
                        }
                    }
                    else
                    {
                        if (errorWhenNoMatch)
                        {
                            ThrowHelper2.ThrowJsonException_Index_not_valid_on(t);
                        }
                    }
                }
            }
        }
    }
}