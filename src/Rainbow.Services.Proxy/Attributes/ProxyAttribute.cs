using System;

namespace Rainbow.Services.Proxy.Attributes
{
    [AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public class ProxyAttribute : Attribute
    {
        public ProxyAttribute(string service, string providerName, string loadBalancer = null)
        {
            this.Service = service;
            this.ProviderName = providerName;
            this.LoadBalancer = loadBalancer;
        }

        public string Service { get; set; }
        public string ProviderName { get; set; }
        public string LoadBalancer { get; set; }
    }
}