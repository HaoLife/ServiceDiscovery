using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Rainbow.ServiceDiscovery.Proxy.Http
{
    public static class UrlExtension
    {

        public static string ToParams(this Dictionary<string, string> source)
        {
            return string.Join("&", source.Select(a => string.Format("{0}={1}", a.Key, Uri.EscapeDataString(a.Value.ToString()))));
        }


        public static Dictionary<string, string> ToParamDict(this string source)
        {
            var dict = new Dictionary<string, string>();

            if (string.IsNullOrWhiteSpace(source)) return dict;

            if (source[0] == '?')
            {
                source = source.Substring(1);
            }
            return source.Split('&').Select(a => a.ToUrlKeyValue()).ToDictionary(a => a.Key, a => a.Value);
        }

        public static KeyValuePair<string, string> ToUrlKeyValue(this string source)
        {
            if (!source.Contains("=")) throw new ArgumentException("参数不是一个有效的url key=value 结构");
            var values = source.Split('=');
            if (values.Length != 2) throw new ArgumentException("参数不是一个有效的url key=value 结构");

            return new KeyValuePair<string, string>(values[0], Uri.UnescapeDataString(values[1]));
        }
    }
}
