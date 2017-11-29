using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Rainbow.ServiceDiscovery.Zookeeper;
using Rainbow.ServiceDiscovery.Abstractions;
using Consul;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace Rainbow.ServiceDiscovery.SamplesServiceRegister
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment env, IServiceDiscovery serviceDiscovery)
        {
            Configuration = configuration;
            ServiceDiscovery = serviceDiscovery;
        }

        public IConfiguration Configuration { get; }
        public IServiceDiscovery ServiceDiscovery { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
