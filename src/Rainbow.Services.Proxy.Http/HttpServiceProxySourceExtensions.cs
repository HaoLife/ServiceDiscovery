using Rainbow.Services.Proxy;
using Rainbow.Services.Proxy.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.Configuration
{
    public static class HttpServiceProxySourceExtensions
    {

        public static ServiceProxyBuilder AddHttpProxy<T>(this ServiceProxyBuilder builder, string serviceName, string prefix = null, string loadBalancer = null)
        {
            return builder.AddProxy(new ServiceProxyDescriptor()
            {
                ServiceName = serviceName,
                ProxyType = typeof(T),
                ProviderName = ProxyDefaults.ProviderName,
                LoadBalancer = loadBalancer,
                Prefix = prefix,
            });
        }
    }
}
