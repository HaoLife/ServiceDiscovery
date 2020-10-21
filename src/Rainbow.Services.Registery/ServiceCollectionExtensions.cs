using Microsoft.Extensions.Configuration;
using Rainbow.Services.Registery;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {

        public static ServiceRegisteryBuilder AddRegistery(this IServiceCollection services)
        {
            var builder = new ServiceRegisteryBuilder();
            services.AddSingleton<IServiceRegistery>((provider) => builder.Build());

            return builder;
        }


        public static ServiceRegisteryBuilder AddRegistery(this IServiceCollection services, IConfiguration configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }
            var builder = new ServiceRegisteryBuilder();
            var app = configuration.Get<ServiceApplication>();
            builder.SetApplication(app);

            services.AddSingleton<IServiceRegistery>((provider) => builder.Build());

            return builder;
        }
    }
}
