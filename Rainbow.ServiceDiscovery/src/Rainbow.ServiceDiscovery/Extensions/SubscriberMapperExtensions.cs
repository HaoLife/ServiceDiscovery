using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rainbow.ServiceDiscovery
{
    public static class SubscriberMapperExtensions
    {
        public static IProxyMapper Map<T>(this IProxyMapper source, string serviceName)
        {
            source.Add(new MappingDescribe() { ServiceName = serviceName, MapType = typeof(T) });
            return source;
        }
        public static IProxyServiceMapper Service(this IProxyMapper source, string serviceName)
        {
            return new ProxyServiceMapper(serviceName, source);
        }
    }
}
