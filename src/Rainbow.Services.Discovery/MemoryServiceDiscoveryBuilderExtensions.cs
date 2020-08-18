using Microsoft.Extensions.Configuration;
using Rainbow.Services.Discovery;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
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
