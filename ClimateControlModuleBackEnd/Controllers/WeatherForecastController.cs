using Contracts.Managers;
using DTO.WeatherObservation;
using Microsoft.AspNetCore.Mvc;

namespace ClimateControlModuleBackEnd.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherObservationController : ControllerBase
    {
        private readonly IWeatherObservationManager _manager;
        public WeatherObservationController(IWeatherObservationManager manager)
        {
            // Constructor logic if needed
            this._manager = manager;
        }

        // This endpoint retrieves weather observation data by WMO number for last N hours only -- Just take post method for adding more parameters in future
        [HttpPost("GetWeatherObservationDataByStationAsyncForLastNHours")]
        public async Task<WeatherObservationResponse> GetWeatherObservationDataByStationAsyncForLastNHours(WeatherObservationRequest objRequest)
        {
            var response =  await _manager.GetWeatherObservationDataByStationAsyncForLastNHours(objRequest);
            return response; 
        }
    }
}
