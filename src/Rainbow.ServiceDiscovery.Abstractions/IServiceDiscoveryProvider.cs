using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rainbow.ServiceDiscovery.Abstractions
{
    public interface IServiceDiscoveryProvider
    {
        bool TryGet(string key, out ServiceEndpoint value);
        void Load();
        IChangeToken GetReloadToken();

    }
}
