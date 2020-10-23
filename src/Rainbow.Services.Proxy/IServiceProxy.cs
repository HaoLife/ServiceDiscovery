using System;
using System.Collections.Generic;
using System.Text;

namespace Rainbow.Services.Proxy
{
    public interface IServiceProxy
    {
        T Create<T>();
    }
}
