using System;
using System.Collections.Generic;
using System.Text;

namespace Rainbow.Services.Proxy.Attributes
{
    public class HttpProxyPostAttribute : HttpProxyMethodAttribute
    {
        public HttpProxyPostAttribute(string template = null)
            : base("POST", template)
        {

        }
    }
}
