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
using Rainbow.Services.Discovery.Samples.Services;
using Rainbow.Services.Proxy;

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

            services.AddHealthChecks();

            //services.AddRegistery(Configuration.GetSection("application"))
            //    .AddConsul(Configuration.GetSection("register:consul"));

            services.AddDiscovery()
                .AddMemory(Configuration.GetSection("discovery:memory"))
                 .AddConsul(Configuration.GetSection("discovery:consul"), true, true)
                ;

            //services.AddProxyService()

            services.AddServiceProxy()
                .AddHttp()
                .AddRollPolling()
                .AddAutoProxy()
                //.AddHttpProxy<IWeatherForecastService>("samples", "api")

                ;


            //services.AddHttpProxy()
            //    .Add<IWeatherForecastService>("samples");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime lifetime)
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

            //app.ApplicationServices.UseRegistery();
            //app.UseHealthChecks("/health");
            //lifetime.ApplicationStopped.Register(() => app.ApplicationServices.UseDeregister());

            //var ls = discovery.GetEndpoints("samples");

            //var proxy = app.ApplicationServices.GetService<IServiceProxy>();
            //var list = proxy.Create<IWeatherForecastService>().Get();
        }
    }
}
