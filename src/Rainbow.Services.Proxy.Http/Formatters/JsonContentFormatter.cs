using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace Rainbow.Services.Proxy.Http.Formatters
{
    public class JsonContentFormatter : IContentFormatter
    {
        private static JsonSerializerOptions _options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
        public bool CanRead(IHttpOutputContext context)
        {

            if (context.Response.Content != null && context.Response.Content.Headers.ContentType == null) return false;

            return context.Response.Content.Headers.ContentType.MediaType.StartsWith("application/json", StringComparison.InvariantCultureIgnoreCase);
        }

        public bool CanWrite(IHttpInputContext context)
        {
            if (context.ContentType == null) return false;

            return context.ContentType.StartsWith("application/json", StringComparison.InvariantCultureIgnoreCase);
        }

        public void Read(IHttpOutputContext context)
        {
            var content = context.Response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            context.Result = JsonSerializer.Deserialize(content, context.OutType, _options);

        }

        public void Write(IHttpInputContext context)
        {
            context.Request.Content = new StringContent(JsonSerializer.Serialize(context.Body));
        }

    }
}
