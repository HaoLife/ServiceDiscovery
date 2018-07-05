using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Rainbow.ServiceDiscovery.Proxy.Http.Formatters
{
    public class TextOutputFormatter : IOutputFormatter
    {
        public bool CanRead(IOutputFormatterContext context)
        {
            if (context.ContentType == null) return false;

            return context.ContentType.StartsWith("text/plain", StringComparison.InvariantCultureIgnoreCase);
        }

        public void Read(IOutputFormatterContext context)
        {
            using (var responseStream = context.GetStream())
            {
                StreamReader reader = new StreamReader(responseStream);
                var responseFromServer = reader.ReadToEnd();

                context.Result = responseFromServer;
            }
        }


        public bool CanWrite(IInputFormatterContext context)
        {
            return false;
        }


        public void Write(IInputFormatterContext context)
        {
            throw new Exception("不支持这种格式的写入");

        }
    }
}
