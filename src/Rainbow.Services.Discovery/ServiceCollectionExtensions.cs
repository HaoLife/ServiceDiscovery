using Rainbow.Services.Discovery;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static ServiceDiscoveryBuilder AddDiscovery(this IServiceCollection services)
        {
            var builder = new ServiceDiscoveryBuilder();
            services.AddSingleton<IServiceDiscovery>((provider) => builder.Build(provider));
            return builder;
        }
    }
}
