using System;
using System.Collections.Generic;
using System.Text;

namespace Rainbow.Services.Proxy.Http.Attributes
{

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter, AllowMultiple = false, Inherited = true)]
    public class HttpProxyQueryAttribute : Attribute
    {
        public string Name { get; set; }
    }
}
