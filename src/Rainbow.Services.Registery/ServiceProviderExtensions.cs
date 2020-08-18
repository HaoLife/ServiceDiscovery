using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Rainbow.Services.Registery;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceProviderExtensions
    {
        public static IApplicationBuilder UseRegistery(this IApplicationBuilder app)
        {
            var registery = app.ApplicationServices.GetRequiredService<IServiceRegistery>();
            registery.Register();

            return app;
        }


        public static IApplicationBuilder UseDeregister(this IApplicationBuilder app)
        {
            var registery = app.ApplicationServices.GetRequiredService<IServiceRegistery>();
            registery.Deregister();

            return app;
        }


        public static IApplicationBuilder UseRegisteryAndHealth(this IApplicationBuilder app, string healthPath)
        {
            var registery = app.ApplicationServices.GetRequiredService<IServiceRegistery>();
            registery.Register();

            app.Map(healthPath, builder =>
            {
                builder.Run(async context =>
                {
                    await context.Response.WriteAsync("Healthy");
                });
            });

            return app;
        }
    }
}
