using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Reflection;
using System.Text;

namespace Rainbow.Services.Proxy.Http
{
    public class InvokeInputContext : IHttpInputContext
    {
        public InvokeInputContext(string url, string httpMethod, string contentType, RouteValueDictionary query, object body)
        {
            this.ContentType = contentType;
            this.HttpMethod = httpMethod;
            this.Request = new HttpRequestMessage(new HttpMethod(httpMethod), url);
            this.Query = query;
            this.Body = body;
        }
        public string ContentType { get; private set; }

        public string HttpMethod { get; private set; }

        public HttpRequestMessage Request { get; private set; }

        public RouteValueDictionary Query { get; private set; }

        public object Body { get; private set; }
    }
}
