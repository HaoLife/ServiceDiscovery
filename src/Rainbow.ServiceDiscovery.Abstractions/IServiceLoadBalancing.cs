using System;
using System.Collections.Generic;
using System.Text;

namespace Rainbow.ServiceDiscovery.Abstractions
{
    public interface IServiceLoadBalancing
    {
        bool TryGet(IServiceSubscriber subscriber, out ServiceEndpoint endpoint);

    }
}
