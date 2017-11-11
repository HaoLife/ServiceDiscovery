using Rainbow.ServiceDiscovery.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rainbow.ServiceDiscovery
{
    public static class ServiceEndpointExtensions
    {
        public static Uri ToUri(this IServiceEndpoint source)
        {
            return new Uri($"{source.Protocol}://{source.HostName}:{source.Port}{source.Path}");
        }
    }
}
