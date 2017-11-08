using System;
using System.Collections.Generic;
using System.Text;

namespace Rainbow.ServiceDiscovery.Abstractions
{
    /// <summary>
    /// 服务订阅者
    /// </summary>
    public interface IServiceSubscriber
    {
        string Name { get; }

        void Subscribe();

        IEnumerable<ServiceEndpoint> GetEndpoints();
    }
}
