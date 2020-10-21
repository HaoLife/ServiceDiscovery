using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Rainbow.Services.Registery;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseRegistery(this IApplicationBuilder app, IHostApplicationLifetime lifetime = null)
        {
            var registery = app.ApplicationServices.GetRequiredService<IServiceRegistery>();
            registery.Register();

            lifetime?.ApplicationStopped.Register(() => registery.Deregister());
            return app;
        }

    }
}
