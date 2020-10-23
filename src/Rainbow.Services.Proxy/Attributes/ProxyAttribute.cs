using System;

namespace Rainbow.Services.Proxy.Attributes
{
    [AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public class ProxyAttribute : Attribute
    {
        public ProxyAttribute(string service, string providerName, string prefix = null, string loadBalancer = null)
        {
            this.Service = service;
            this.ProviderName = providerName;
            this.Prefix = prefix;
            this.LoadBalancer = loadBalancer;
        }

        public string Service { get; set; }
        public string ProviderName { get; set; }
        public string LoadBalancer { get; set; }
        public string Prefix { get; set; }
    }
}