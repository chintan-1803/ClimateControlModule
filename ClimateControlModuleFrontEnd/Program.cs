using DTO.WeatherObservation;
using Newtonsoft.Json.Linq;
using System.Text.Json;

Console.WriteLine("Hello, World!");

var apiURL = "http://localhost/WeatherObservation/GetWeatherObservationDataByStationAsyncForLast72Hours";


var requestObj = new WeatherObservationRequest
{
    WmoNumber = 94672 
};


using var client = new HttpClient();

var jsonContent = new StringContent(
    JsonSerializer.Serialize(requestObj),
    System.Text.Encoding.UTF8,
    "application/json"
);


try
{
    var response = await client.PostAsync(apiURL, jsonContent);

    if (response.IsSuccessStatusCode)
    {
        var responseJson = await response.Content.ReadAsStringAsync();

        //var weatherResponse = JsonSerializer.Deserialize<WeatherObservationResponse>(responseJson);

        JObject jObject = JObject.Parse(responseJson);

        WeatherObservationResponse jsonWeatherObject = jObject.ToObject<WeatherObservationResponse>() ?? new WeatherObservationResponse();

        if(jsonWeatherObject.Result == WeatherObservationResult.Success)
        {

        }
        else
        {

        }
    }
    else
    {
        //Console.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
    }


}
catch (Exception ex)
{
    Console.WriteLine($"Exception: {ex.Message}");
}