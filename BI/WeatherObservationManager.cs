using BI.Utilities;
using Contracts.Managers;
using DTO.WeatherObservation;
using Newtonsoft.Json.Linq;

namespace BI
{
    public class WeatherObservationManager : IWeatherObservationManager
    {
        private readonly IHelper _helper;
        public WeatherObservationManager(IHelper helper)
        {
            _helper = helper;
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

                //response.WeatherData = dataArray?.ToObject<List<WeatherObservationReceivedFromApi>>() ?? new List<WeatherObservationReceivedFromApi>();

                response.Result = WeatherObservationResult.Success;
            }
            return response;
        }
    }
}
