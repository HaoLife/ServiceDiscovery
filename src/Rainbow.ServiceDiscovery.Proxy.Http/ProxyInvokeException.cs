using System;
using System.Collections.Generic;
using System.Text;

namespace Rainbow.ServiceDiscovery.Proxy.Http
{
    public class ProxyInvokeException : Exception
    {
        public ProxyInvokeException()
        {

        }
        public ProxyInvokeException(string message) : base(message)
        {

        }
    }
}
