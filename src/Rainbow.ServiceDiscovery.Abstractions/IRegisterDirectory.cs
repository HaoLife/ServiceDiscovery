using System;
using System.Collections.Generic;
using System.Text;

namespace Rainbow.ServiceDiscovery.Abstractions
{
    public interface IRegisterDirectory
    {
        void Add(ServiceEndpoint endpoint);

        IEnumerable<IServiceRegister> GetRegisters();
    }
}
