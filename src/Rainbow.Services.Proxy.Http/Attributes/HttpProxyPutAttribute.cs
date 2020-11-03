using System;
using System.Collections.Generic;
using System.Text;

namespace Rainbow.Services.Proxy.Http.Attributes
{
    public class HttpProxyPutAttribute : HttpProxyMethodAttribute
    {
        public HttpProxyPutAttribute(string template = null)
            : this("application/json", template)
        {

        }
        public HttpProxyPutAttribute(string contentType, string template = null)
            : base("PUT", contentType, template)
        {

        }
    }
}
