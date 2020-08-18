using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rainbow.Services.Discovery
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
