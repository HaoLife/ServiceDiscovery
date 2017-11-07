using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rainbow.ServiceDiscovery
{
    /// <summary>
    /// 服务节点
    /// </summary>
    public class ServiceEndpoint
    {
        public ServiceEndpoint(string name, Endpoint endpoint)
            : this(name, "1.0", endpoint)
        {

        }
        public ServiceEndpoint(string name, string version, Endpoint endpoint)
        {
            this.Name = name;
            this.Version = version;
            this.Endpoint = endpoint;
        }


        public ServiceEndpoint(string name, Uri uri)
            : this(name, "1.0", Endpoint.ToEndpoint(uri))
        {
        }


        public string Name { get; set; }
        public string Version { get; set; }
        public Endpoint Endpoint { get; set; }

        public override string ToString()
        {
            return string.Format("{0}[{1}]_{2}", this.Name, this.Version, this.Endpoint);
        }
    }
}
