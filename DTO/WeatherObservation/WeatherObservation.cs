using Newtonsoft.Json;

namespace DTO.WeatherObservation
{
    public class WeatherObservationReceivedFromApi
    {
        [JsonProperty("wmo")]
        public int Wmo { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("local_date_time_full")]
        public string LocalDateTimeFull { get; set; }

        [JsonProperty("air_temp")]
        public double AirTemp { get; set; }

        [JsonProperty("dewpt")]
        public double DewPt { get; set; }

        [JsonProperty("press")]
        public double Pressure { get; set; }
    }


    public class WeatherObservation
    {
        [JsonProperty("wmo")]
        public int Wmo { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("local_date_time_full")]
        public DateTime LocalDateTimeFull { get; set; }

        [JsonProperty("air_temp")]
        public double AirTemp { get; set; }

        [JsonProperty("dewpt")]
        public double DewPt { get; set; }

        [JsonProperty("press")]
        public double Pressure { get; set; }
    }
    public class WeatherObservationRequest
    {
        public int WmoNumber { get; set; }
    }
    public class WeatherObservationResponse
    {
        public List<WeatherObservation> WeatherData { get; set; } = new List<WeatherObservation>(); 
        public WeatherObservationResult Result { get; set; }
    }

    public enum WeatherObservationResult
    { 
        Success  = 1,
        WmoNumberNotFound = 2,
        Error = 3
    }
}
