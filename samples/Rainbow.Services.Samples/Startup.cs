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
using Rainbow.Services.Registery;
using Rainbow.Services.Registery.Consul;

namespace Rainbow.Services.Samples
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

            //var app = new ServiceApplication();
            //Configuration.GetSection("application").Bind(app);

            //var options = new ConsulServiceRegisteryOptions();
            //Configuration.GetSection("register:consul").Bind(options);

            //var builder = new ServiceRegisteryBuilder();
            //builder.SetApplication(app);
            //builder.Add(new ConsulServiceRegisterySource(options));

            //builder.Build();


            //services.AddRegistery(Configuration.GetSection("application"), (builder) =>
            //{
            //    builder
            //        .AddConsul(Configuration.GetSection("register:consul"));
            //});

            //services.AddRegistery((builder) =>
            //{
            //    builder.SetApplication(Configuration.GetSection("application"))
            //        .AddConsul(Configuration.GetSection("register:consul"));
            //});

            services.AddRegistery(Configuration.GetSection("application"))
                .AddConsul(Configuration.GetSection("register:consul"));
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
            app.UseHealthChecks("/health");

            app.UseRegistery(lifetime);


            //lifetime.ApplicationStopped.Register(() => app.ApplicationServices.UseDeregister());

        }
    }
}
