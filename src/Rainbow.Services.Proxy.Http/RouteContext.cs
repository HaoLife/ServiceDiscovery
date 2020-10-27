using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Rainbow.Services.Proxy.Http
{
    public class RouteContext
    {
        public MethodInfo TargetMethod { get; set; }
        public object[] Args { get; set; }
        public HttpServiceProxyOptions Options { get; set; }
        public ServiceProxyDescriptor Descriptor { get; set; }


    }
}
