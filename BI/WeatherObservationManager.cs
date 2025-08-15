using BI.Maps;
using BI.Utilities;
using Contracts.Managers;
using DTO.WeatherObservation;
using Newtonsoft.Json.Linq;

namespace BI
{
    public class WeatherObservationManager : IWeatherObservationManager
    {
        private readonly IHelper _helper;
        private readonly WeatherObservationMap _weatherObservationMap = null;
        public WeatherObservationManager(IHelper helper)
        {
            _helper = helper;
            _weatherObservationMap = new WeatherObservationMap();
        }
        public async Task<WeatherObservationResponse> GetWeatherObservationDataByStationAsync(WeatherObservationRequest objRequest)
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
                HttpResponseMessage responseMessage = await _helper.SendRequestToExternalApi(Method.Get, $"https://www.bom.gov.au/fwo/IDS60901/IDS60901.{objRequest.WmoNumber}.json");

                responseMessage.EnsureSuccessStatusCode();

                string jsonWeatherData = await responseMessage.Content.ReadAsStringAsync();

                var jsonWeatherObject =  JObject.Parse(jsonWeatherData);

                var dataArray = jsonWeatherObject["observations"]?["data"] as JArray;

                var data = dataArray?.ToObject<List<WeatherObservationReceivedFromApi>>() ?? new List<WeatherObservationReceivedFromApi>();

                // All logic to map the data from DTO to the response DTO in map file to keep the manager clean
                response.WeatherData = _weatherObservationMap.GetMappedWeatherData(data);

                response.Result = WeatherObservationResult.Success;
            }
            return response;
        }
    }
}
