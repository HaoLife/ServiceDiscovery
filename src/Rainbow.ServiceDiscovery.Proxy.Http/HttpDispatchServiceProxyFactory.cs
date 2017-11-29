using Microsoft.Extensions.Logging;
using Rainbow.ServiceDiscovery.Abstractions;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Rainbow.ServiceDiscovery.Proxy.Http
{
    public class HttpDispatchServiceProxyFactory : IServiceProxyFactory
    {
        public IServiceDiscovery ServiceDiscovery { get; }
        public ILoggerFactory LoggerFactory { get; }


        public HttpDispatchServiceProxyFactory(IServiceDiscovery serviceDiscovery, ILoggerFactory loggerFactory)
        {
            this.ServiceDiscovery = serviceDiscovery;
            this.LoggerFactory = loggerFactory;
        }

        public T CreateProxy<T>()
        {
            return HttpDispatchServiceProxy.CreateProxy<T>(this);
        }
    }
}
