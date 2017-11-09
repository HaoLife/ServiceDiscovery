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

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .ConfigureServices((context, services) =>
                {
                    var app = context.Configuration.GetSection("service");
                    var zk = context.Configuration.GetSection("service:zookeeper");
                    var appname = app.GetValue<string>("name");
                    var url = app.GetValue<string>("url");

                    ServiceEndpoint endpoint = new ServiceEndpoint(appname, new Uri(url));

                    var conn = zk.GetValue<string>("Connection");
                    var timeout = zk.GetValue<TimeSpan>("SessionTimeout");

                    var serviceDiscovery = new ServiceDiscoveryBuilder()
                        .AddZookeeper(conn, timeout, new List<string>() { appname }, endpoint)
                        .Build();
                    services.AddSingleton<IServiceDiscovery>(serviceDiscovery);
                })
                .Build();
    }
}
