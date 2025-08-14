using Microsoft.AspNetCore.Mvc;

namespace ClimateControlModuleBackEnd.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherObservationController : ControllerBase
    {
        [HttpPost(Name = "GetWeatherObservationDataByStation")]
        public IEnumerable<WeatherForecast> GetWeatherObservationDataByStation()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
