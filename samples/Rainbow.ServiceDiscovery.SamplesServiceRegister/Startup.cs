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
            //var appName = Configuration["service:appName"];
            //var url = Configuration["service:url"];
            //var connection = Configuration["service:zookeeperConnection"];
            //ServiceEndpoint app = new ServiceEndpoint(appName, new Uri(url));

            //IZookeeperRegistryClient zkClient = new ZookeeperRegistryClient(connection, TimeSpan.Parse("00:00:10"));

            //IServiceEndpointStore store = new MemoryServiceEndpointStore();
            //IServiceRegisterFactory registerFactory = new ZookeeperServiceRegisterFactory(zkClient);
            //IServiceSubscriberFactory subscriberFactory = new ZookeeperServiceSubscriberFactory(zkClient, store);
            //IServiceProxyGenerator serviceProxyGenerator = new HttpDynamicServiceProxyGenerator();
            //var options = new ServiceDiscoveryOptions();
            //options.ProxyMapper = new ProxyMapper();
            //options.SubscriberDirectory = new SubscriberDirectory(subscriberFactory);
            //options.RegisterDirectory = new RegisterDirectory(registerFactory);


            //options.RegisterDirectory.Add(app);
            //options.SubscriberDirectory.Subscribe(appName);

            ////options.ProxyMapper.Service("wp_test").Map<IValuesService>();




            //IServiceDiscoveryStarter starter = new ServiceDiscoveryStarter(options);
            //starter.Bootup();

            //IServiceLoadBalancing loadBalancing = new PollingServiceLoadBalancing(options.SubscriberDirectory);

            //IServiceDiscovery serviceDiscovery = new ServiceDiscovery(options, loadBalancing, serviceProxyGenerator);
            //var point = serviceDiscovery.GetService("wp_test");

            //var proxy = serviceDiscovery.GetProxy<IValuesService>();

            //.ConfigureServices(services =>
            //{
            //    var serviceDiscovery = new ServiceDiscoveryBuilder()
            //        .Build();
            //    services.AddSingleton<IServiceDiscovery>(serviceDiscovery);
            //})

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
