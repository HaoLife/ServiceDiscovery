using Microsoft.Extensions.Configuration;
using Rainbow.Services.Discovery;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ConfigServiceDiscoveryBuilderExtensions
    {
        public static ServiceDiscoveryBuilder AddMemory(this ServiceDiscoveryBuilder builder, IConfiguration configuration)
        {
            var source = new ConfigServiceDiscoverySource(configuration);
            return builder.Add(source);
        }

    }
}
