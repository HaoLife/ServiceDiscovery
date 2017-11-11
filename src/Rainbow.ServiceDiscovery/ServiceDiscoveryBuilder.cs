using Microsoft.Extensions.DependencyInjection;
using Rainbow.ServiceDiscovery.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rainbow.ServiceDiscovery
{
    public class ServiceDiscoveryBuilder : IServiceDiscoveryBuilder
    {
        public IServiceCollection ServiceCollection { get; set; }

        public ServiceDiscoveryBuilder(IServiceCollection serviceCollection)
        {
            this.ServiceCollection = serviceCollection;
        }

    }
}
