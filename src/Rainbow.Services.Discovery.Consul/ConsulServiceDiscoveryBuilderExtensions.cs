using Microsoft.Extensions.Configuration;
using Rainbow.Services.Discovery;
using Rainbow.Services.Discovery.Consul;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ConsulServiceDiscoveryBuilderExtensions
    {
        public static IServiceDiscoveryBuilder AddConsul(this IServiceDiscoveryBuilder builder, IConfiguration configuration)
        {
            var source = new ConsulServiceDiscoverySource(configuration);
            builder.ServiceCollection.AddSingleton<IServiceDiscoveryProvider>(source.Build);
            return builder;
        }
    }
}
