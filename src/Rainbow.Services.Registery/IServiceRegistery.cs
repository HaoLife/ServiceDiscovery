using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text;

namespace Rainbow.Services.Registery
{
    public interface IServiceRegistery
    {
        IServiceApplication Application { get; }
        IEnumerable<IServiceRegisteryProvider> Providers { get; }

        void Register();

        void Deregister();
    }
}
