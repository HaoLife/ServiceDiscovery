using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Rainbow.Services.Discovery.Samples.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiscoveryController : ControllerBase
    {

        private readonly ILogger<DiscoveryController> logger;
        private readonly IServiceDiscovery serviceDiscovery;

        public DiscoveryController(ILogger<DiscoveryController> logger, IServiceDiscovery serviceDiscovery)
        {
            this.logger = logger;
            this.serviceDiscovery = serviceDiscovery;
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

            var ends = serviceDiscovery.GetEndpoints("samples");
            if (!ends.Any()) return Enumerable.Empty<WeatherForecast>();
            var rng = new Random();
            var index = rng.Next(ends.Count());
            var element = ends.ElementAt(index);


            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var url = $"{element.Protocol}://{element.Host}:{element.Port}/api/WeatherForecast";
            HttpResponseMessage response = httpClient.GetAsync(url).GetAwaiter().GetResult();

            var content = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

            return JsonConvert.DeserializeObject<List<WeatherForecast>>(content);

        }
    }
}
