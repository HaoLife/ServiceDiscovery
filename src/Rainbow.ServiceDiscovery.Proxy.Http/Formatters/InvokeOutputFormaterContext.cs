using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;

namespace Rainbow.ServiceDiscovery.Proxy.Http.Formatters
{
    public class InvokeOutputFormaterContext : IOutputFormatterContext
    {

        private readonly WebResponse _response;
        private readonly MethodInfo _method;

        public InvokeOutputFormaterContext(WebResponse response, MethodInfo method)
        {
            this._response = response;
            this._method = method;
        }

        public long ContentLength => this._response.ContentLength;
        public string ContentType => this._response.ContentType;
        public Type OutType => this._method.ReturnType;
        public System.IO.Stream Stream => this._response.GetResponseStream();


        public object Result { get; set; }

    }
}
