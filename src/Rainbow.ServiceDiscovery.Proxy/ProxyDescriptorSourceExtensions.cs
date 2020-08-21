using Rainbow.ServiceDiscovery.Proxy;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ProxyDescriptorSourceExtensions
    {
        public static IProxyDescriptorSource Add<T>(this IProxyDescriptorSource source, string service)
        {
            source.AddService(typeof(T), service);
            return source;
        }
    }
}
