using BI.Maps;
using BI.Settings;
using BI.Utilities;
using Contracts.Managers;
using DTO.WeatherObservation;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.Options;
namespace BI
{
    public class WeatherObservationManager : IWeatherObservationManager
    {
        private readonly IHelper _helper;
        private readonly WeatherObservationMap _weatherObservationMap;
        private readonly BOMApiSettings _bomApiSettings;
        public WeatherObservationManager(IHelper helper,IOptions<BOMApiSettings> options)
        {
            _bomApiSettings = options.Value;
            _helper = helper;
            _weatherObservationMap = new WeatherObservationMap();
        }
        public async Task<WeatherObservationResponse> GetWeatherObservationDataByStationAsyncForLastNHours(WeatherObservationRequest objRequest)
        {
            var response = new WeatherObservationResponse
            {
                Result = WeatherObservationResult.Error
            };

            if(objRequest == null || objRequest.WmoNumber <= 0)
            {
                response.Result = WeatherObservationResult.WmoNumberNotFound;
                return response;
            }
            else
            {
                string url  = string.Format(_bomApiSettings.Url, objRequest.WmoNumber);
                HttpResponseMessage responseMessage = await _helper.SendRequestToExternalApi(Method.Get, url);

                responseMessage.EnsureSuccessStatusCode();

                string jsonWeatherData = await responseMessage.Content.ReadAsStringAsync();

                var jsonWeatherObject =  JObject.Parse(jsonWeatherData);

                var dataArray = jsonWeatherObject["observations"]?["data"] as JArray;

                var data = dataArray?.ToObject<List<WeatherObservationReceivedFromApi>>() ?? new List<WeatherObservationReceivedFromApi>();

                // All logic to map the data from DTO to the response DTO in map file to keep the manager clean
                response.WeatherData = _weatherObservationMap.MapWeatherObservationsForLastNHours(data);

                response.Result = WeatherObservationResult.Success;
            }
            return response;
        }
    }
}
