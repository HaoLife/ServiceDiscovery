using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rainbow.Services.Discovery.Samples.Services
{
    public interface IWeatherForecastService
    {
        List<WeatherForecast> Get();
    }
}
