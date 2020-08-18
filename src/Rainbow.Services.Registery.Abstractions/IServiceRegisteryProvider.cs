using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rainbow.Services.Registery
{
    public interface IServiceRegisteryProvider
    {

        void Load();

        void Register(IServiceApplication application);

        void Deregister(IServiceApplication application);

        IChangeToken GetReloadToken();
    }
}
