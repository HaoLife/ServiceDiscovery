using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace Rainbow.Services.Proxy.Http.Formatters
{
    public class TextContentFormatter : IContentFormatter
    {
        public bool CanRead(IHttpOutputContext context)
        {

            if (context.Response.Content != null && context.Response.Content.Headers.ContentType == null) return false;

            return context.Response.Content.Headers.ContentType.MediaType.StartsWith("text/plain", StringComparison.InvariantCultureIgnoreCase);
        }

        public void Read(IHttpOutputContext context)
        {
            context.Result = context.Response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
        }


        public bool CanWrite(IHttpInputContext context)
        {
            if (context.ContentType == null) return false;

            return context.ContentType.StartsWith("application/plain", StringComparison.InvariantCultureIgnoreCase);
        }


        public void Write(IHttpInputContext context)
        {
            context.Request.Content = new StringContent(JsonSerializer.Serialize(context.Body));

        }
    }
}
