using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Reflection;
using System.Text;

namespace Rainbow.Services.Proxy.Http
{
    public class InvokeOutputContext : IOutputContext
    {

        private readonly HttpResponseMessage _response;
        private readonly MethodInfo _method;

        public InvokeOutputContext(HttpResponseMessage response, MethodInfo method)
        {
            this._response = response;
            this._method = method;
        }

        public long ContentLength => this._response.Content.Headers.ContentLength ?? 0;
        public string ContentType => this._response.Content.Headers.ContentType.MediaType;
        public Type OutType => this._method.ReturnType;
        public System.IO.Stream Stream => this._response.Content.ReadAsStreamAsync().GetAwaiter().GetResult();


        public object Result { get; set; }

    }
}
