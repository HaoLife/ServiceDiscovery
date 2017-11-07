using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rainbow.ServiceDiscovery
{
    public class ProxyMapper : IProxyMapper
    {
        private readonly List<MappingDescribe> _mappingDescribes;
        public ProxyMapper()
        {
            this._mappingDescribes = new List<MappingDescribe>();
        }

        public void Add(MappingDescribe describe)
        {
            if (this._mappingDescribes.Any(a => describe.ServiceName == a.ServiceName))
            {
                throw new Exception("重复添加订阅项");
            }
            this._mappingDescribes.Add(describe);
        }

        public IEnumerable<MappingDescribe> GetMappings()
        {
            return this._mappingDescribes;
        }
    }
}
