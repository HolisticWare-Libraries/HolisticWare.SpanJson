﻿namespace SpanJson.Linq.JsonPath
{
    internal class FieldFilter : PathFilter
    {
        internal string? Name;

        public FieldFilter(string? name)
        {
            Name = name;
        }

        public override IEnumerable<JToken> ExecuteFilter(JToken root, IEnumerable<JToken> current, JsonSelectSettings? settings)
        {
            var errorWhenNoMatch = settings?.ErrorWhenNoMatch ?? false;

            foreach (JToken t in current)
            {
                if (t is JObject o)
                {
                    if (Name is not null)
                    {
                        var v = o[Name];

                        if (v is not null)
                        {
                            yield return v;
                        }
                        else if (errorWhenNoMatch)
                        {
                            ThrowHelper2.ThrowJsonException_Property_does_not_exist_on_JObject(Name);
                        }
                    }
                    else
                    {
                        foreach (KeyValuePair<string, JToken?> p in o)
                        {
                            yield return p.Value!;
                        }
                    }
                }
                else
                {
                    if (errorWhenNoMatch)
                    {
                        ThrowHelper2.ThrowJsonException_Property_not_valid_on(Name, t);
                    }
                }
            }
        }
    }
}