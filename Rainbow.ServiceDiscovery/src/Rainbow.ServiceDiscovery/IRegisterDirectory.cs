using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rainbow.ServiceDiscovery
{
    public interface IRegisterDirectory
    {
        void Add(ServiceEndpoint endpoint);

        IEnumerable<IServiceRegister> GetRegisters();
    }
}
