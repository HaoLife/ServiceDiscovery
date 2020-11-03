using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Rainbow.Services.Samples.GrpcServices;

namespace Rainbow.Services.Discovery.Samples.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GrpcController : ControllerBase
    {


        private readonly ILogger<GrpcController> logger;
        private readonly ILoadBalancer loadBalancer;
        private readonly IServiceDiscovery serviceDiscovery;

        public GrpcController(ILogger<GrpcController> logger, ILoadBalancer loadBalancer, IServiceDiscovery serviceDiscovery)
        {
            this.logger = logger;
            this.loadBalancer = loadBalancer;
            this.serviceDiscovery = serviceDiscovery;
        }

        [HttpGet]
        public HelloReply Get()
        {
            //var rng = new Random();
            //return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            //{
            //    Date = DateTime.Now.AddDays(index),
            //    TemperatureC = rng.Next(-20, 55),
            //    Summary = Summaries[rng.Next(Summaries.Length)]
            //})
            //.ToArray();



            var end = this.loadBalancer.TryGet(this.serviceDiscovery, "grpc_samples");
            if (end == null) throw new Exception("系统异常：没有找到可用的服务");

            using var channel = GrpcChannel.ForAddress(end.ToUrl());
            var client = new Greeter.GreeterClient(channel);
            var reply = client.SayHello(
                              new HelloRequest { Name = "GreeterClient" });


            return reply;

        }
    }
}
