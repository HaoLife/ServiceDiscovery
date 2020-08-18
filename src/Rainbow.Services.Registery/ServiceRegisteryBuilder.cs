using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rainbow.Services.Registery
{
    public class ServiceRegisteryBuilder : IServiceRegisteryBuilder
    {
        public ServiceRegisteryBuilder(IServiceCollection serviceCollection)
        {
            ServiceCollection = serviceCollection;
        }

        public IServiceCollection ServiceCollection { get; }
    }
}
