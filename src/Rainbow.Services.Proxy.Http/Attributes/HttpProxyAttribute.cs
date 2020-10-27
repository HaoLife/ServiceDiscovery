using System;
using Rainbow.Services.Proxy.Http;

namespace Rainbow.Services.Proxy.Attributes
{

    [AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public class HttpProxyAttribute : ProxyAttribute
    {
        public HttpProxyAttribute(string service, string loadBalancer = null)
            : base(service, ProxyDefaults.ProviderName, loadBalancer)
        {
        }

    }
}