using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;

namespace Rainbow.Services.Proxy.Http.Patterns
{
    internal static class RoutePatternParser
    {
        private static Regex regex = new Regex(@"(\{([A-Za-z0-9]*)\})");

        public static string Parse(string pattern, RouteValueDictionary dict)
        {
            var matchs = regex.Matches(pattern);

            var path = pattern;
            foreach (Match item in matchs)
            {
                var key = item.Groups[2].Value;
                if (!dict.ContainsKey(key)) throw new RoutePatternException(pattern, $"无法匹配参数 {item.Value}");

                var value = dict[key];
                path = path.Replace(item.Value, value.ToString());

            }
            return path;
        }


    }
}
