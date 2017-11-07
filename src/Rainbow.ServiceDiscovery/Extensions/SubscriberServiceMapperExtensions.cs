using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rainbow.ServiceDiscovery
{
    public static class SubscriberServiceMapperExtensions
    {
        public static IProxyServiceMapper Map<T>(this IProxyServiceMapper source)
        {
            source.Mapper.Add(new MappingDescribe() { ServiceName = source.ServiceName, MapType = typeof(T) });
            return source;
        }
    }
}
