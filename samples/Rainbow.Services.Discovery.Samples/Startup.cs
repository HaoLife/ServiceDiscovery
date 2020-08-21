using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Rainbow.ServiceDiscovery.Proxy;
using Rainbow.Services.Discovery.Samples.Services;

namespace Rainbow.Services.Discovery.Samples
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddRegistery(Configuration.GetSection("application"))
                .AddConsul(Configuration.GetSection("register:consul"));

            services.AddDiscovery()
                //.AddMemory(Configuration.GetSection("discovery:memory"))
                .AddConsul(Configuration.GetSection("discovery:consul"));

            //services.AddProxyService()

            services.AddServiceProxy()
                .AddHttp();


            services.AddHttpProxy()
                .Add<IWeatherForecastService>("samples");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceDiscovery discovery, IHostApplicationLifetime lifetime)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseRegisteryAndHealth("/health");

            var ls = discovery.GetEndpoints("samples");

            lifetime.ApplicationStopped.Register(() => app.UseDeregister());

            var proxy = app.ApplicationServices.GetService<IServiceProxy>();
            var list = proxy.Create<IWeatherForecastService>().Get();
        }
    }
}
