using Microsoft.Extensions.DependencyInjection;
using Rainbow.Services.Proxy;
using System;
using System.Collections.Generic;
using System.Text;


namespace Microsoft.Extensions.Configuration
{
    public static class ServiceCollectionExtensions
    {

        public static ServiceProxyBuilder AddServiceProxy(this IServiceCollection services)
        {
            //services.AddRollPolling();
            //services.AddDiscovery();
            var builder = new ServiceProxyBuilder();
            services.AddSingleton<IServiceProxy>(provider => builder.Build(provider));
            return builder;
        }

    }
}
