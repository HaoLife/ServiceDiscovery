//using Rainbow.ServiceDiscovery.Proxy.Http.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rainbow.Services.Discovery.Samples.Services
{
    //[Service("http://{sample}/weatherforecast)]
    public interface IWeatherForecastService
    {
        //[ProxyGet]
        List<WeatherForecast> Get();
    }
}
