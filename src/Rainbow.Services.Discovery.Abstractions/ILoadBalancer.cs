using System;
using System.Collections.Generic;
using System.Text;

namespace Rainbow.Services.Discovery
{
    public interface ILoadBalancer
    {
        IServiceEndpoint Get(IServiceDiscovery discovery, string serviceName);
        IServiceEndpoint TryGet(IServiceDiscovery discovery, string serviceName);
    }
}
