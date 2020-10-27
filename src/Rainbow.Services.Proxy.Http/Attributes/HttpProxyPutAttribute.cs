using System;
using System.Collections.Generic;
using System.Text;

namespace Rainbow.Services.Proxy.Attributes
{
    public class HttpProxyPutAttribute : HttpProxyMethodAttribute
    {
        public HttpProxyPutAttribute(string template = null)
            : base("PUT", template)
        {

        }
    }
}
