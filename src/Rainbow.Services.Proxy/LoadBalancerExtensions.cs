using Rainbow.Services.Discovery;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rainbow.Services.Proxy
{
    public static class LoadBalancerExtensions
    {
        public static ServiceProxyBuilder AddRollPolling(this ServiceProxyBuilder builder)
        {
            return builder.AddLoadBalancer(new RollPollingLoadBalancer());
        }

    }
}
