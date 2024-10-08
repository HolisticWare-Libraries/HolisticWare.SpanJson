﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SpanJson.Tests
{
    public static class EscapeHelper
    {
        public static string NonAsciiEscape(string serialized)
        {
            StringBuilder sb = new StringBuilder();
            int from = 0;
            int index = 0;
            while (index < serialized.Length)
            {
                var c = serialized[index++];
                if (c > 0x7F)
                {
                    sb.Append(@"\u");
                    sb.Append(((uint)c).ToString("X4"));
                }
                else
                {
#if (NET || NETCOREAPP2_1_OR_GREATER)
                    sb.Append(serialized.AsSpan(from, index - from));
#else
                    sb.Append(serialized.Substring(from, index - from));
#endif
                }
                from = index;
            }

            return sb.ToString();
        }


        public static string EscapeMore(string serialized)
        {
            StringBuilder sb = new StringBuilder();
            int from = 0;
            int index = 0;
            while (index < serialized.Length)
            {
                var c = serialized[index++];
                if (c == ':' || c == '-')
                {
                    sb.Append(@"\u");
                    sb.Append(((uint)c).ToString("X4"));
                }
                else
                {
#if (NET || NETCOREAPP2_1_OR_GREATER)
                    sb.Append(serialized.AsSpan(from, index - from));
#else
                    sb.Append(serialized.Substring(from, index - from));
#endif
                }
                from = index;
            }

            return sb.ToString();
        }
    }
}
