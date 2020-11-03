using System;
using System.Collections.Generic;
using System.Text;

namespace Rainbow.Services.Proxy.Http.Attributes
{
    [AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class HttpProxyRouteAttribute : Attribute
    {
        public HttpProxyRouteAttribute(string template = "")
        {
            this.Template = template;
        }
        public string Template { get; set; }
    }
}
