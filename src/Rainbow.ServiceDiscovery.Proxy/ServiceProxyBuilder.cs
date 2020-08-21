using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rainbow.ServiceDiscovery.Proxy
{
    public class ServiceProxyBuilder : IServiceProxyBuilder
    {
        public ServiceProxyBuilder(IServiceCollection serviceCollection)
        {
            this.ServiceCollection = serviceCollection;
        }
        public IServiceCollection ServiceCollection { get; set; }

    }
}
