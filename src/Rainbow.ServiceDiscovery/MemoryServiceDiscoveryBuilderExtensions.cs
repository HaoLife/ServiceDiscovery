using Microsoft.Extensions.Configuration;
using Rainbow.ServiceDiscovery.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace Rainbow.ServiceDiscovery
{
    public static class MemoryServiceDiscoveryBuilderExtensions
    {
        public static IServiceDiscoveryBuilder AddMemory(this IServiceDiscoveryBuilder builder, IConfiguration configuration)
        {

            var source = new MemoryServiceDiscoverySource(configuration);
            builder.ServiceCollection.AddSingleton<IServiceDiscoveryProvider>(source.Build);
            return builder;
        }

    }
}
