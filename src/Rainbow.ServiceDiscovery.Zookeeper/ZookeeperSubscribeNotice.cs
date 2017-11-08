using Rainbow.ServiceDiscovery.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rainbow.ServiceDiscovery.Zookeeper
{
    public class ZookeeperSubscribeNotice
    {
        public ZookeeperSubscribeNotice(string serviceName, Action<IEnumerable<ServiceEndpoint>> handler)
        {
            this.ServiceName = serviceName;
            this.Handler = handler;
        }

        public string ServiceName { get; private set; }
        public Action<IEnumerable<ServiceEndpoint>> Handler { get; private set; }
    }
}
