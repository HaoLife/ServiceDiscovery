using Rainbow.Services.Discovery;
using Rainbow.Services.Proxy;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class LoadBalancerExtensions
    {
        public static ServiceProxyBuilder AddRollPolling(this ServiceProxyBuilder builder)
        {
            return builder.AddLoadBalancer(new RollPollingLoadBalancer());
        }

    }
}
