using System;
using System.Collections.Generic;
using System.Text;

namespace Rainbow.ServiceDiscovery.Proxy.Http
{
    public interface IHttpServiceInvoke
    {
        object Handle(ServiceInvokeContext context);
    }
}
