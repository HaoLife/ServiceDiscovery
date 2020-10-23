using System;
using System.Collections.Generic;
using System.Text;

namespace Rainbow.Services.Registery
{
    public interface IServiceApplication
    {
        string Name { get; }
        string Host { get; }
        string Path { get; }
        int Port { get; }
        string Protocol { get; }

    }
}
