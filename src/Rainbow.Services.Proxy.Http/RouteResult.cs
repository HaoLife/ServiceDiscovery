using System;
using System.Collections.Generic;
using System.Text;

namespace Rainbow.Services.Proxy.Http
{
    public class RouteResult
    {
        public string ProxyRoute { get; set; }
        public bool ProxyResult { get; set; }

        public string MethodRoute { get; set; }
        public bool MethodResult { get; set; }

        public string HttpMethod { get; set; }

        public string ContentType { get; set; }

    }
}
