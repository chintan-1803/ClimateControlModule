using Contracts.Managers;
using DTO.WeatherObservation;

namespace BI
{
    public class WeatherObservationManager : IWeatherObservationManager
    {
        private readonly HttpClient _httpClient;

        public WeatherObservationManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
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
                // Example: third-party API endpoint
                var url = $"fwo/IDS60901/IDS60901.{objRequest.WmoNumber}.json";

                var responseMessage = await _httpClient.GetAsync(url);
                responseMessage.EnsureSuccessStatusCode();

                string json = await responseMessage.Content.ReadAsStringAsync();
                response.Result = WeatherObservationResult.Success;

            }

                await Task.Delay(1000); // Simulate async operation -- Need to replace with actual data retrieval logic






            return response;
        }
    }
}
