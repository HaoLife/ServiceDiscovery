using System;
using System.Collections.Generic;
using System.Text;

namespace Rainbow.ServiceDiscovery.Abstractions
{
    /// <summary>
    /// 订阅目录
    /// </summary>
    public interface ISubscriberDirectory
    {
        void Add(string serviceName);

        IEnumerable<IServiceSubscriber> GetSubscribers();
    }
}
