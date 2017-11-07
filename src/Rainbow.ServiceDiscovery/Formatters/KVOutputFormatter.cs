using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rainbow.ServiceDiscovery.Formatters
{
    public class KVOutputFormatter : IOutputFormatter
    {
        public bool CanRead(IOutputFormatterContext context)
        {
            return false;
        }

        public void Read(IOutputFormatterContext context)
        {
            throw new Exception("不支持这种格式的输出内容");
        }

        public bool CanWrite(IInputFormatterContext context)
        {
            if (context.ContentType == null) return false;

            return context.ContentType.StartsWith("application/x-www-form-urlencoded", StringComparison.InvariantCultureIgnoreCase);
        }

        public void Write(IInputFormatterContext context)
        {

            Dictionary<string, string> dict = new Dictionary<string, string>();
            var parms = context.Paramters;
            for (int i = 0; i < parms.Length; i++)
            {
                dict.Add(parms[i].Name, context.Args[i].ToString());
            }

            context.Result = dict.ToParams();
        }
    }
}
