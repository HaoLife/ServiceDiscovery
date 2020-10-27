using Rainbow.Services.Proxy;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {

        public static ServiceProxyBuilder AddServiceProxy(this IServiceCollection services)
        {
            var builder = new ServiceProxyBuilder();
            services.TryAddSingleton<IServiceProxy>(provider => builder.Build(provider));
            return builder;
        }

    }
}
