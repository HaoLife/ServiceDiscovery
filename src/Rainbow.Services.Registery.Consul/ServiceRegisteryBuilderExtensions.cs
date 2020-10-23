using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rainbow.Services.Registery;
using Rainbow.Services.Registery.Consul;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceRegisteryBuilderExtensions
    {
        public static ServiceRegisteryBuilder AddConsul(this ServiceRegisteryBuilder builder, IConfiguration configuration)
        {

            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            var options = configuration.Get<ConsulServiceRegisteryOptions>();

            var source = new ConsulServiceRegisterySource(options);

            return builder.Add(source);
        }
    }
}
