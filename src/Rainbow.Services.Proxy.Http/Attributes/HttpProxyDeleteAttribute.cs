using System;
using System.Collections.Generic;
using System.Text;

namespace Rainbow.Services.Proxy.Http.Attributes
{
    public class HttpProxyDeleteAttribute : HttpProxyMethodAttribute
    {

        public HttpProxyDeleteAttribute(string template = "")
            : this("application/json", template)
        {

        }

        public HttpProxyDeleteAttribute(string contentType, string template = "")
            : base("GET", contentType, template)
        {

        }
    }
}
