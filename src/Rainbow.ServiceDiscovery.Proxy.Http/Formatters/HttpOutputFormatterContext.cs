using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;

namespace Rainbow.ServiceDiscovery.Proxy.Http.Formatters
{
    public class HttpOutputFormatterContext : IOutputFormatterContext
    {
        private readonly WebResponse _response;
        private readonly MethodInfo _method;

        public HttpOutputFormatterContext(WebResponse response, MethodInfo method)
        {
            this._response = response;
            this._method = method;
        }

        public long ContentLength
        {
            get { return this._response.ContentLength; }
        }

        public string ContentType
        {
            get { return this._response.ContentType; }
        }

        public Type OutType
        {
            get { return this._method.ReturnType; }
        }
        public System.IO.Stream GetStream()
        {
            return this._response.GetResponseStream();
        }




        public object Result { get; set; }
    }
}
