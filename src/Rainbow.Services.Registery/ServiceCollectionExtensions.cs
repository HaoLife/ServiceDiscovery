using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rainbow.Services.Registery;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceRegisteryBuilder AddRegistery(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<ServiceRegisteryApplication>(configuration);

            services.AddSingleton<IServiceRegistery, ServiceRegistery>();

            var builder = new ServiceRegisteryBuilder(services);

            return builder;
        }

    }
}
