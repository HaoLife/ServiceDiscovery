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
using Rainbow.ServiceDiscovery.Zookeeper;

namespace Rainbow.ServiceDiscovery.SamplesServiceRegister
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("hosting.json", optional: true)
                .AddCommandLine(args)
                .Build();

            return WebHost.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(builder =>
                {
                    builder.AddJsonFile("services.json", false, true);
                })
                .ConfigureServices((context, services) =>
                {
                    services.AddDiscovery(context.Configuration.GetSection("Service:Application"), builder =>
                    {
                        builder
                            .AddZookeeper(context.Configuration.GetSection("service:Zookeeper"))
                            .AddMemory(context.Configuration.GetSection("service:memory"));
                    });
                })
                .UseStartup<Startup>()
                .UseConfiguration(config)
                .Build();

        }
    }
}
