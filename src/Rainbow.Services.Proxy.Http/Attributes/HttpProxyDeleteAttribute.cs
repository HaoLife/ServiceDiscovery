using System;
using System.Collections.Generic;
using System.Text;

namespace Rainbow.Services.Proxy.Attributes
{
    public class HttpProxyDeleteAttribute : HttpProxyMethodAttribute
    {
        public HttpProxyDeleteAttribute(string template = null)
            : base("DELETE", template)
        {

        }
    }
}
