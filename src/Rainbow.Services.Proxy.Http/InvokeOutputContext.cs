using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Reflection;
using System.Text;

namespace Rainbow.Services.Proxy.Http
{
    public class InvokeOutputContext : IHttpOutputContext
    {

        private readonly HttpResponseMessage _response;
        private readonly MethodInfo _method;

        public InvokeOutputContext(HttpResponseMessage response, MethodInfo method)
        {
            this._response = response;
            this._method = method;
        }

        public Type OutType => this._method.ReturnType;


        public object Result { get; set; }

        public HttpResponseMessage Response => _response;
    }
}
