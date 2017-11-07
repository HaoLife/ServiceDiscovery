using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rainbow.ServiceDiscovery
{
    /// <summary>
    /// 订阅服务映射器
    /// </summary>
    public class ProxyServiceMapper : IProxyServiceMapper
    {
        public ProxyServiceMapper(string servicenName, IProxyMapper mapper)
        {
            this.ServiceName = servicenName;
            this.Mapper = mapper;
        }
        public string ServiceName { get; private set; }
        public IProxyMapper Mapper { get; private set; }
    }
}
