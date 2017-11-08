using Rainbow.ServiceDiscovery.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Rainbow.ServiceDiscovery.Zookeeper
{
    public class ZookeeperRegisterDirectory : IRegisterDirectory
    {
        private readonly ZookeeperServiceDiscoverySource _source;
        private readonly List<IServiceRegister> _serviceRegisters;

        public ZookeeperRegisterDirectory(ZookeeperServiceDiscoverySource source)
        {
            this._source = source;
            this._serviceRegisters = new List<IServiceRegister>();
        }

        public void Add(ServiceEndpoint endpoint)
        {
            if (_serviceRegisters.Any(a => a.ServiceName == endpoint.Name))
            {
                throw new Exception("重复添加注册节点:" + endpoint.Name);
            }
            var register = new ZookeeperServiceRegister(this._source.Client, endpoint);
            this._serviceRegisters.Add(register);
        }

        public IEnumerable<IServiceRegister> GetRegisters()
        {
            return this._serviceRegisters;
        }
    }
}
