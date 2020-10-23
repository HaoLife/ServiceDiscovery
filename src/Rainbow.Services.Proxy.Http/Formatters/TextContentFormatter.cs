using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Rainbow.Services.Proxy.Http.Formatters
{
    public class TextContentFormatter : IContentFormatter
    {
        public bool CanRead(IOutputContext context)
        {
            if (context.ContentType == null) return false;

            return context.ContentType.StartsWith("text/plain", StringComparison.InvariantCultureIgnoreCase);
        }

        public void Read(IOutputContext context)
        {
            using (var responseStream = context.Stream)
            {
                StreamReader reader = new StreamReader(responseStream);
                var responseFromServer = reader.ReadToEnd();

                context.Result = responseFromServer;
            }
        }


        public bool CanWrite(IInputContext context)
        {
            return false;
        }


        public void Write(IInputContext context)
        {
            throw new Exception("不支持这种格式的写入");

        }
    }
}
