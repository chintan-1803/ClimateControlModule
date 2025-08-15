using DTO.WeatherObservation;
using Newtonsoft.Json.Linq;
using System.Text.Json;

var apiURL = "http://localhost/WeatherObservation/GetWeatherObservationDataByStationAsyncForLastNHours";

try
{

Dictionary<int, (string Name, int Wmo)> Stations = new Dictionary<int, (string, int)>
{
    { 1, ("Adelaide Airport", 94672) },
    { 2, ("Edinburgh", 95676) },
    { 3, ("Hindmarsh Island", 94677) },
    { 4, ("Kuitpo", 94683) }
};

    const int DefaultChoice = 1;

    Console.WriteLine("Showing default station data:\n");

    await GetWeatherObservationDataByStationAsyncForLastNHours(apiURL, Stations, DefaultChoice);

    bool exit = false;

    while (!exit)
    {
        
        Console.WriteLine("Select a weather observation station:");
        foreach (var station in Stations)
        {
            Console.WriteLine($"{station.Key}. {station.Value.Name}");
        }

        Console.WriteLine("\n Enter a number between 1 and 4");
        Console.WriteLine("If you enter an invalid number, the system will display data for Adelaide Airport.\r\nPress 0 to exit.");
        string input = Console.ReadLine();

        if (input == "0")
        {
            exit = true;
            Console.WriteLine("Exiting...");
            break;
        }

        int choice;
        if (!int.TryParse(input, out choice) || !Stations.ContainsKey(choice))
        {
            Console.WriteLine("Invalid choice! Using default station: Adelaide Airport.");
            choice = DefaultChoice;
        }

        await GetWeatherObservationDataByStationAsyncForLastNHours(apiURL, Stations, choice);

    }
}
catch (Exception ex)
{
    Console.WriteLine($"Exception: {ex.Message}");
}

static async Task GetWeatherObservationDataByStationAsyncForLastNHours(string apiURL, Dictionary<int, (string Name, int Wmo)> Stations, int choice)
{
    var selectedStation = Stations[choice];

    var requestObj = new WeatherObservationRequest
    {
        WmoNumber = selectedStation.Wmo
    };
    var client = new HttpClient();

    var jsonContent = new StringContent(
        JsonSerializer.Serialize(requestObj),
        System.Text.Encoding.UTF8,
        "application/json"
    );


    var response = await client.PostAsync(apiURL, jsonContent);

    if (response.IsSuccessStatusCode)
    {
        var responseJson = await response.Content.ReadAsStringAsync();

        //var weatherResponse = JsonSerializer.Deserialize<WeatherObservationResponse>(responseJson);

        JObject jObject = JObject.Parse(responseJson);

        WeatherObservationResponse jsonWeatherObject = jObject.ToObject<WeatherObservationResponse>() ?? new WeatherObservationResponse();

        if (jsonWeatherObject.Result == WeatherObservationResult.Success)
        {
            double avgTemperature = Math.Round((double) jsonWeatherObject.WeatherData.Sum(x => x.AirTemp) / jsonWeatherObject.WeatherData.Count(),1);
            Console.WriteLine("Station Name: " + selectedStation.Name + " Wmo Number: " + selectedStation.Wmo.ToString() + " Average Temperature: " + avgTemperature.ToString() + "\n");
        }
        else
        {

        }
    }
    else
    {
        Console.WriteLine("Something went wrong. Please try again.");
    }
}