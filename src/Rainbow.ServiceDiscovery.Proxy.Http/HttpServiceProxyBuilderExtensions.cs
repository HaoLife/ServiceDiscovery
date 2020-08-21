using Microsoft.Extensions.Configuration;
using Rainbow.ServiceDiscovery.Proxy;
using Rainbow.ServiceDiscovery.Proxy.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class HttpServiceProxyBuilderExtensions
    {
        public static IServiceProxyBuilder AddHttp(this IServiceProxyBuilder builder)
        {
            var source = new HttpServiceProxySource();
            builder.ServiceCollection.AddSingleton<IServiceProxyProvider>(source.Build);
            return builder;
        }
        public static IServiceProxyBuilder AddHttp(this IServiceProxyBuilder builder, IConfiguration configuration)
        {
            var source = new HttpServiceProxySource(configuration);
            builder.ServiceCollection.AddSingleton<IServiceProxyProvider>(source.Build);
            return builder;
        }

    }
}
