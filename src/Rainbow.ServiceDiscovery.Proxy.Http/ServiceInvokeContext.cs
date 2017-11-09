using Rainbow.ServiceDiscovery.Abstractions;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Rainbow.ServiceDiscovery.Proxy.Http
{
    public class ServiceInvokeContext
    {
        public ServiceInvokeContext(ServiceEndpoint serviceEndpoint, Type service, MethodInfo method, object[] args)
        {
            this.ServiceEndpoint = serviceEndpoint;
            this.Service = service;
            this.Method = method;
            this.Args = args;
        }

        public ServiceEndpoint ServiceEndpoint { get; private set; }
        public MethodInfo Method { get; private set; }
        public Type Service { get; private set; }
        public object[] Args { get; private set; }
    }
}
