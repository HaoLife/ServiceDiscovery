using System;
using System.Collections.Generic;
using System.Text;

namespace Rainbow.Services.Proxy
{
    public class NotFoundProxyException : Exception
    {
        public NotFoundProxyException()
        {

        }
        public NotFoundProxyException(string message) : base(message)
        {

        }
    }
}
