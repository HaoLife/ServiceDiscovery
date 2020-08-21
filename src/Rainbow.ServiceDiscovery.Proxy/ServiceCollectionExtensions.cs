using Microsoft.Extensions.DependencyInjection;
using Rainbow.ServiceDiscovery.Proxy;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.Configuration
{
    public static class ServiceCollectionExtensions
    {

        public static IServiceProxyBuilder AddServiceProxy(this IServiceCollection services)
        {
            services.AddSingleton<IServiceProxy, ServiceProxy>();
            return new ServiceProxyBuilder(services);
        }

    }
}
