using System;
using System.Collections.Generic;
using System.Text;

namespace Rainbow.Services.Proxy
{
    public interface IServiceProxyProvider
    {
        //void Load();
        bool CanHandle(string type);

        T Create<T>(ServiceProxyDescriptor descriptor);
    }
}
