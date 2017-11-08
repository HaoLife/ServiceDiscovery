using Rainbow.ServiceDiscovery.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rainbow.ServiceDiscovery
{
    public static class SubscriberDirectoryExtensions
    {
        public static ISubscriberDirectory Subscribe(this ISubscriberDirectory source, string serviceName)
        {
            source.Add(serviceName);
            return source;
        }
    }
}
