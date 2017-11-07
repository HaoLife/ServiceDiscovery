using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rainbow.ServiceDiscovery
{
    /// <summary>
    /// 服务注册者
    /// </summary>
    public interface IServiceRegister
    {
        string ServiceName { get; }
        void Register();
        void Deregister();
    }
}
