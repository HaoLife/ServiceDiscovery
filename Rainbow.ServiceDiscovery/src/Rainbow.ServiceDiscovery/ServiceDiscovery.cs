using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rainbow.ServiceDiscovery
{
    public class ServiceDiscovery : IServiceDiscovery
    {
        private readonly ServiceDiscoveryOptions _options;
        private readonly IServiceLoadBalancing _serviceLoadBalancing;
        private readonly IServiceProxyGenerator _serviceProxyGenerator;
        public ServiceDiscovery(ServiceDiscoveryOptions options, IServiceLoadBalancing serviceLoadBalancing, IServiceProxyGenerator serviceProxyGenerator)
        {
            this._options = options;
            this._serviceLoadBalancing = serviceLoadBalancing;
            this._serviceProxyGenerator = serviceProxyGenerator;
        }

        public virtual ServiceEndpoint GetService(string serviceName)
        {
            return this._serviceLoadBalancing.Find(serviceName);
        }

        public virtual T GetProxy<T>()
        {
            var mapping = this._options.ProxyMapper.GetMappings().Where(a => a.MapType == typeof(T)).FirstOrDefault();
            if (mapping == null) throw new Exception("没有可映射的服务" + typeof(T).Name);

            var point = this.GetService(mapping.ServiceName);
            //创建代理服务
            return _serviceProxyGenerator.CreateServiceProxy<T>(point);
        }
    }
}
