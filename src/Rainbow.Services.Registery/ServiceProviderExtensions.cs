using Microsoft.Extensions.DependencyInjection;
using Rainbow.Services.Registery;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceProviderExtensions
    {
        public static IServiceProvider UseRegistery(this IServiceProvider provider)
        {
            var registery = provider.GetRequiredService<IServiceRegistery>();
            registery.Register();

            return provider;
        }


        public static IServiceProvider UseDeregister(this IServiceProvider provider)
        {
            var registery = provider.GetRequiredService<IServiceRegistery>();
            registery.Deregister();

            return provider;
        }

    }
}
