using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rainbow.ServiceDiscovery.Proxy
{
    public static class ProxyServiceExtensions
    {
        public static IServiceCollection AddProxyService<T>(this IServiceCollection services,string factoryType)
        {
            return services.AddSingleton<ProxyDescriptor>(new ProxyDescriptor());
        }


    }
}
