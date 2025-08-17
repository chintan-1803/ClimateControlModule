// Publishing Command to have a single file executable for Windows x64 platform -- no rumtime dependencies 
//dotnet publish -c Release -r win-x64 --self-contained true /p:PublishSingleFile = true / p:IncludeAllContentForSelfExtract = true




using DTO.WeatherObservation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using System.Text.Json;

try
{
    var serviceCollection = new ServiceCollection();

    var configuration = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .Build();

    var apiURL = configuration["WeatherApiSettings:ClimateControlModuleApiUrl"];

    if (apiURL != null)
    {
        Dictionary<int, (string Name, int Wmo)> Stations = new Dictionary<int, (string, int)>
        {
            { 1, ("Adelaide Airport", 94672) },
            { 2, ("Edinburgh", 95676) },
            { 3, ("Hindmarsh Island", 94677) },
            { 4, ("Kuitpo", 94683) }
        };

        const int DefaultChoice = 1;

        Console.WriteLine("Showing Default Station Data:\n");

        await GetWeatherObservationDataByStationAsyncForLastNHours(apiURL, Stations, DefaultChoice);

        bool exit = false;

        while (!exit)
        {

            Console.WriteLine("Select a weather observation station:\n");
            foreach (var station in Stations)
            {
                Console.WriteLine($"{station.Key}. {station.Value.Name}");
            }

            Console.WriteLine("\n");

            Console.WriteLine("Enter a number between 1 and 4");
            Console.WriteLine("If you enter an invalid number, the system will display data for default Adelaide Airport.\r\nPress 0 to exit.");
            string input = Console.ReadLine();

            Console.WriteLine("\n");

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
    else
    {
        Console.WriteLine("URL not found.Please check your settings file");
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

        JObject jObject = JObject.Parse(responseJson);

        WeatherObservationResponse jsonWeatherObject = jObject.ToObject<WeatherObservationResponse>() ?? new WeatherObservationResponse();

        switch (jsonWeatherObject.Result)
        {
            case WeatherObservationResult.Success:
                double avgTemperature = Math.Round((double)jsonWeatherObject.WeatherData.Sum(x => x.AirTemp) / jsonWeatherObject.WeatherData.Count(), 1);
                Console.WriteLine("Station Name: " + selectedStation.Name + " Wmo Number: " + selectedStation.Wmo.ToString() + " Average Temperature: " + avgTemperature.ToString() + "\n");

                Console.Write("Do you want to see the full JSON data? (y/n): ");
                var input = Console.ReadLine()?.Trim().ToLower();

                if (input == "y" || input == "yes")
                {
                    Console.WriteLine("\nFull JSON Data:\n");
                    Console.WriteLine(JsonSerializer.Serialize(jsonWeatherObject.WeatherData));
                    Console.WriteLine(); // extra line for spacing
                }
                break;
            case WeatherObservationResult.WmoNumberNotFound:
                Console.WriteLine("WMO number not found. Please try again.");
                break;
            case WeatherObservationResult.Error:
                Console.WriteLine("An error occurred while processing your request. Please try again.");
                break;
        }
    }
    else
    {
        Console.WriteLine("Something went wrong. Please try again.");
    }
}