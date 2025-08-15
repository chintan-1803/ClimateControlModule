using System.Text.Json;
using DTO.WeatherObservation;

Console.WriteLine("Hello, World!");

var apiURL = "http://localhost/WeatherObservation/GetWeatherObservationDataByStation";


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
        var weatherResponse = JsonSerializer.Deserialize<WeatherObservationResponse>(responseJson);

        //Console.WriteLine($"Station: {weatherResponse?.StationName}, Temperature: {weatherResponse?.Temperature}");
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