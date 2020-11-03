using System;
using System.Collections.Generic;
using System.Text;

namespace Rainbow.Services.Proxy.Http.Attributes
{
    public class HttpProxyGetAttribute : HttpProxyMethodAttribute
    {
        public HttpProxyGetAttribute(string template = "")
            : this("application/x-www-form-urlencoded", template)
        {

        }

        public HttpProxyGetAttribute(string contentType, string template = "")
            : base("GET", contentType, template)
        {

        }
    }
}
