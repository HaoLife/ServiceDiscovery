using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace Rainbow.Services.Proxy.Http.Formatters
{
    public class KVContentFormatter : IContentFormatter
    {
        public bool CanRead(IHttpOutputContext context)
        {
            return false;
        }

        public bool CanWrite(IHttpInputContext context)
        {
            if (context.ContentType == null) return false;

            return context.ContentType.StartsWith("application/x-www-form-urlencoded", StringComparison.InvariantCultureIgnoreCase);

        }

        public void Read(IHttpOutputContext context)
        {
            throw new Exception("不支持这种格式的输出内容");
        }

        public void Write(IHttpInputContext context)
        {
            var values = context.Query.Select(a => $"{a.Key}={a.Value?.ToString() ?? ""}").ToList();


            var queryPath = string.Join("&", values);

            if (string.Compare(context.HttpMethod, "GET", true) == 0)
            {
                var builder = new UriBuilder(context.Request.RequestUri);

                builder.Query = queryPath;
                context.Request.RequestUri = builder.Uri;
            }
            else
            {

                context.Request.Content = new StringContent(queryPath);
            }

        }
    }
}
