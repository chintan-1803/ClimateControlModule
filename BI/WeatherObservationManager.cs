using Contracts.Managers;
using DTO.WeatherObservation;
using System.Net.Http;
using System.Net.Http.Headers;

namespace BI
{
    public class WeatherObservationManager : IWeatherObservationManager
    {
        public WeatherObservationManager()
        {

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
                await NewMethod(objRequest);

                response.Result = WeatherObservationResult.Success;

            }

            await Task.Delay(1000); // Simulate async operation -- Need to replace with actual data retrieval logic






            return response;
        }

        private static async Task NewMethod(WeatherObservationRequest objRequest)
        {
            HttpClient _httpClient = new HttpClient();

            _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) " + "AppleWebKit/537.36 (KHTML, like Gecko) " + "Chrome/115.0.0.0 Safari/537.36");

            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var url = $"https://www.bom.gov.au/fwo/IDS60901/IDS60901.{objRequest.WmoNumber}.json";

            var responseMessage = await _httpClient.GetAsync(url);
            responseMessage.EnsureSuccessStatusCode();

            string json = await responseMessage.Content.ReadAsStringAsync();
        }
    }
}
