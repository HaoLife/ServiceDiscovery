using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rainbow.ServiceDiscovery
{
    public interface IServiceRegisterFactory
    {
        IServiceRegister CreateRegister(ServiceEndpoint endpoint);
    }
}
