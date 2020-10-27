using Rainbow.Services.Discovery;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static ServiceDiscoveryBuilder AddDiscovery(this IServiceCollection services)
        {
            var builder = new ServiceDiscoveryBuilder();
            services.TryAddSingleton<IServiceDiscovery>((provider) => builder.Build(provider));
            return builder;
        }
    }
}
