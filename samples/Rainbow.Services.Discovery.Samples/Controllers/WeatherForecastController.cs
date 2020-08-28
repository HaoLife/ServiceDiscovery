using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Rainbow.ServiceDiscovery.Proxy;
using Rainbow.Services.Discovery.Samples.Services;

namespace Rainbow.Services.Discovery.Samples.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IServiceProxy serviceProxy;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IServiceProxy serviceProxy)
        {
            _logger = logger;
            this.serviceProxy = serviceProxy;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            //var rng = new Random();
            //return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            //{
            //    Date = DateTime.Now.AddDays(index),
            //    TemperatureC = rng.Next(-20, 55),
            //    Summary = Summaries[rng.Next(Summaries.Length)]
            //})
            //.ToArray();
            var list = serviceProxy.Create<IWeatherForecastService>().Get();
            return list;
        }
    }
}
