using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rainbow.ServiceDiscovery.Zookeeper
{
    public class ZookeeperSubscribeNotice
    {
        public ZookeeperSubscribeNotice(SubscribeDescribe describe, Action<IEnumerable<ServiceEndpoint>> handler)
        {
            this.SubscribeDescribe = describe;
            this.Handler = handler;
        }

        public SubscribeDescribe SubscribeDescribe { get; private set; }
        public Action<IEnumerable<ServiceEndpoint>> Handler { get; private set; }
    }
}
