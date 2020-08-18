using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rainbow.Services.Registery
{
    public interface IServiceRegisteryBuilder
    {
        IServiceCollection ServiceCollection { get; }
    }
}
