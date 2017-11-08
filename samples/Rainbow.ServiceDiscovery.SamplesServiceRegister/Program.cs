using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Rainbow.ServiceDiscovery.Abstractions;

namespace Rainbow.ServiceDiscovery.SamplesServiceRegister
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .ConfigureServices((context, services) =>
                {
                    var app = context.Configuration.GetSection("service");
                    var zk = context.Configuration.GetSection("service:zookeeper");

                    var serviceDiscovery = new ServiceDiscoveryBuilder()
                        .AddZookeeper(new Zookeeper.ZookeeperServiceDiscoveryOptions() { Connection = "72.21.252.250:2181", SessionTimeout = TimeSpan.Parse("00:00:30") }
                            , (builder) =>
                            {
                                builder.SubscriberDirectory.Add("wp_test");
                                //builder.ProxyMapper.Service("wp_test").Map<>
                            })
                        .Build();
                    services.AddSingleton<IServiceDiscovery>(serviceDiscovery);
                })
                .Build();
    }
}
