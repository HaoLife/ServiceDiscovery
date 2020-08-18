using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rainbow.Services.Registery;
using Rainbow.Services.Registery.Consul;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ConsulServiceRegisteryExtensions
    {

        public static IServiceRegisteryBuilder AddConsul(this IServiceRegisteryBuilder builder, IConfiguration configuration)
        {
            builder.ServiceCollection.Configure<ConsulServiceRegisteryOptions>(configuration);

            var source = new ConsulServiceRegisterySource(configuration);
            builder.ServiceCollection.AddSingleton<IServiceRegisteryProvider>(source.Build);
            return builder;
        }
    }
}
