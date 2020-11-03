using System;
using Rainbow.Services.Proxy.Attributes;

namespace Rainbow.Services.Proxy.Http.Attributes
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