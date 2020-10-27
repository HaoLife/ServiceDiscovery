using System;
using System.Collections.Generic;
using System.Text;

namespace Rainbow.Services.Proxy.Attributes
{
    public class HttpProxyGetAttribute : HttpProxyMethodAttribute
    {
        public HttpProxyGetAttribute(string template = "")
            : base("GET", template)
        {

        }
    }
}
