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

        // This endpoint retrieves weather observation data by WMO number for last 72 hours only -- Just take post method for adding more parameters in future
        [HttpPost("GetWeatherObservationDataByStationAsyncForLast72Hours")]
        public async Task<WeatherObservationResponse> GetWeatherObservationDataByStationAsyncForLast72Hours(WeatherObservationRequest objRequest)
        {
            var response =  await _manager.GetWeatherObservationDataByStationAsyncForLast72Hours(objRequest);
            return response; 
        }
    }
}
