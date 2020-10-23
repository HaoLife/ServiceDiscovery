using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace Rainbow.Services.Proxy.Http.Formatters
{
    public class JsonContentFormatter : IContentFormatter
    {
        public bool CanRead(IOutputContext context)
        {
            if (context.ContentType == null) return false;

            return context.ContentType.StartsWith("application/json", StringComparison.InvariantCultureIgnoreCase);
        }

        public bool CanWrite(IInputContext context)
        {
            if (context.ContentType == null || context.Paramters.Length != 1) return false;

            return context.ContentType.StartsWith("application/json", StringComparison.InvariantCultureIgnoreCase);
        }

        public void Read(IOutputContext context)
        {
            using (var responseStream = context.Stream)
            {
                StreamReader reader = new StreamReader(responseStream);
                var responseFromServer = reader.ReadToEnd();
                context.Result = JsonSerializer.Deserialize(responseFromServer, context.OutType);
            }
        }

        public void Write(IInputContext context)
        {
            context.Result = JsonSerializer.Serialize(context.Args.First());
        }

    }
}
