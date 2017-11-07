using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rainbow.ServiceDiscovery
{
    public interface IServiceSubscriberFactory
    {
        IServiceSubscriber CreateSubscriber(SubscribeDescribe subscribeDescribe);
    }
}
