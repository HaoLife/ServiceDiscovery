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

namespace Rainbow.ServiceDiscovery.SamplesServiceRegister
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
            //var config = new ConsulClientConfiguration()
            //{
            //    Address = new Uri("http://127.0.0.1:8500")
            //};

            //using (var client = new ConsulClient(config))
            //{
            //    var result = client.KV.Keys("service/").GetAwaiter().GetResult();
            //}


            services.AddDiscovery(Configuration.GetSection("Service:Application"), builder =>
            {
                builder.AddZookeeper(Configuration.GetSection("service:Zookeeper"));
            });

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
