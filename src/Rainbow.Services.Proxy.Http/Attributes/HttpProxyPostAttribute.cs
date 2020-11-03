using System;
using System.Collections.Generic;
using System.Text;

namespace Rainbow.Services.Proxy.Http.Attributes
{
    public class HttpProxyPostAttribute : HttpProxyMethodAttribute
    {

        public HttpProxyPostAttribute(string template = null)
            : this("application/json", template)
        {

        }
        public HttpProxyPostAttribute(string contentType, string template = null)
            : base("PUT", contentType, template)
        {

        }
    }
}
