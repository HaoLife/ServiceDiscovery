using Rainbow.Services.Discovery;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceDiscoveryBuilder AddDiscovery(this IServiceCollection services)
        {
            services.AddRollPolling();

            services.AddSingleton<IServiceDiscovery, ServiceDiscovery>();
            services.AddSingleton<IServiceDiscoveryRoot>((provider) => provider.GetRequiredService<IServiceDiscovery>() as IServiceDiscoveryRoot);

            var builder = new ServiceDiscoveryBuilder(services);
            return builder;
        }

    }
}
