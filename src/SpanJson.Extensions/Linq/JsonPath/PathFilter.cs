namespace SpanJson.Linq.JsonPath;

internal abstract class PathFilter
{
    public abstract IEnumerable<JToken> ExecuteFilter(JToken root, IEnumerable<JToken> current, JsonSelectSettings? settings);

    protected static JToken? GetTokenIndex(JToken t, JsonSelectSettings? settings, int index)
    {
        if (t is JArray a)
        {
            if ((uint)a.Count <= (uint)index)
            {
                if (settings?.ErrorWhenNoMatch ?? false)
                {
                    ThrowHelper2.ThrowJsonException_Index_outside_the_bounds_of_JArray(index);
                }

                return null;
            }

            return a[index];
        }
        else
        {
            if (settings?.ErrorWhenNoMatch ?? false)
            {
                ThrowHelper2.ThrowJsonException_Index_not_valid_on(index, t);
            }

            return null;
        }
    }

    protected static JToken? GetNextScanValue(JToken originalParent, JToken? container, JToken? value)
    {
        // step into container's values
        if (container is not null && container.HasValues)
        {
            value = container.First;
        }
        else
        {
            // finished container, move to parent
            while (value is not null && value != originalParent && value == value.Parent!.Last)
            {
                value = value.Parent;
            }

            // finished
            if (value is null || value == originalParent)
            {
                return null;
            }

            // move to next value in container
            value = value.Next;
        }

        return value;
    }
}