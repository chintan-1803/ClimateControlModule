using BI.Utilities;
using Contracts.Managers;
using DTO.WeatherObservation;
using System.Net.Http;
using System.Net.Http.Headers;

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

                string json = await responseMessage.Content.ReadAsStringAsync();
            }
            return response;
        }
    }
}
