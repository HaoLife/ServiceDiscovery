using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rainbow.Services.Proxy.Http.Formatters
{
    public class KVContentFormatter : IContentFormatter
    {
        public bool CanRead(IOutputContext context)
        {
            return false;
        }

        public bool CanWrite(IInputContext context)
        {
            if (context.ContentType == null) return false;

            return context.ContentType.StartsWith("application/x-www-form-urlencoded", StringComparison.InvariantCultureIgnoreCase);

        }

        public void Read(IOutputContext context)
        {
            throw new Exception("不支持这种格式的输出内容");
        }

        public void Write(IInputContext context)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            var parms = context.Paramters;
            for (int i = 0; i < parms.Length; i++)
            {
                dict.Add(parms[i].Name, context.Args[i].ToString());
            }

            var value = string.Join("&", dict.Select(a => string.Format("{0}={1}", a.Key, Uri.EscapeDataString(a.Value.ToString()))));

            context.Result = value;
        }
    }
}
