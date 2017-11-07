using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rainbow.ServiceDiscovery
{
    /// <summary>
    /// 订阅目录
    /// </summary>
    public interface ISubscriberDirectory
    {
        void Add(SubscribeDescribe describe);

        IEnumerable<IServiceSubscriber> GetSubscribers();
    }
}
