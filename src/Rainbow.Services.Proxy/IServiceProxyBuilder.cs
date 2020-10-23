using Rainbow.Services.Discovery;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Rainbow.Services.Proxy
{
    public interface IServiceProxyBuilder
    {
        IList<IServiceProxySource> Sources { get; }
        IList<ServiceProxyDescriptor> Descriptors { get; }
        IList<ILoadBalancer> LoadBalancers { get; }
        IServiceProxy Build(IServiceProvider services);
    }
}
