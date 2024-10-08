﻿namespace SpanJson.Dynamic
{
    public static class DynamicExtensions
    {
        internal static string? ToJsonValue(this object? input)
        {
            if (input is ISpanJsonDynamic dyn)
            {
                return dyn.ToJsonValue();
            }

            return input?.ToString();
        }
    }
}
