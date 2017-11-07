using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rainbow.ServiceDiscovery
{
    /// <summary>
    /// 订阅映射器
    /// </summary>
    public interface IProxyMapper
    {
        void Add(MappingDescribe describe);

        IEnumerable<MappingDescribe> GetMappings();
    }
}
