using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Rainbow.Services.Discovery;
using Rainbow.Services.Discovery.Consul;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ConsulServiceDiscoveryBuilderExtensions
    {

        public static ServiceDiscoveryBuilder AddConsul(this ServiceDiscoveryBuilder builder, IConfiguration configuration, bool reloadOnChange = false, bool isAsync = false)
        {
            return builder.AddConsul((c) =>
            {
                c.Configuration = configuration;
                c.ReloadOnChange = reloadOnChange;
                c.IsAsync = isAsync;
            });
        }

        public static ServiceDiscoveryBuilder AddConsul(this ServiceDiscoveryBuilder builder, Action<ConsulServiceDiscoverySource> action = null)
        {
            var source = new ConsulServiceDiscoverySource();
            action?.Invoke(source);
            return builder.Add(source);
        }
    }
}
