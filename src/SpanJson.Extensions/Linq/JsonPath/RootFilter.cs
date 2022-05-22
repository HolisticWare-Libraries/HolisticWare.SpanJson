﻿using System.Collections.Generic;

namespace SpanJson.Linq.JsonPath
{
    internal class RootFilter : PathFilter
    {
        public static readonly RootFilter Instance = new RootFilter();

        private RootFilter()
        {
        }

        public override IEnumerable<JToken> ExecuteFilter(JToken root, IEnumerable<JToken> current, JsonSelectSettings settings)
        {
            return new[] { root };
        }
    }
}