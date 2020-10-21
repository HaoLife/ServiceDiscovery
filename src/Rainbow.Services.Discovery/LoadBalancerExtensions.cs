using Microsoft.Extensions.DependencyInjection;
using Rainbow.Services.Discovery;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class LoadBalancerExtensions
    {
        public static IServiceCollection AddRollPolling(this IServiceCollection services)
        {
            return services.AddSingleton<ILoadBalancer, RollPollingLoadBalancer>();
        }
    }
}
