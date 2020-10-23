using System;
using System.Collections.Generic;
using System.Text;

namespace Rainbow.Services.Registery
{
    public interface IServiceRegisteryProvider
    {
        void Register();

        void Deregister();
    }
}
