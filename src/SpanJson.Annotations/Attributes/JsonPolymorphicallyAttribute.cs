﻿namespace SpanJson
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class JsonPolymorphicallyAttribute : JsonAttribute { }
}
