using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rainbow.ServiceDiscovery
{
    /// <summary>
    /// 服务发现提供者
    /// </summary>
    public interface IServiceDiscovery
    {
        ServiceEndpoint GetService(string serviceName);

        T GetProxy<T>();
    }
}
