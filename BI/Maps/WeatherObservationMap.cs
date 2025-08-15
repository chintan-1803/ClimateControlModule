using DTO.WeatherObservation;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI.Maps
{
    public class WeatherObservationMap
    {
        public List<WeatherObservation> GetMappedWeatherData(List<WeatherObservationReceivedFromApi> weatherObservationReceivedList)
        {
            if (weatherObservationReceivedList == null || !weatherObservationReceivedList.Any())
            {
                return new List<WeatherObservation>();
            }

            return weatherObservationReceivedList.Select(item => new WeatherObservation
            {
                Wmo = item.Wmo,
                Name = item.Name,
                LocalDateTimeFull = DateTime.ParseExact(
                                    item.LocalDateTimeFull,    // string from JSON
                                    "yyyyMMddHHmmss",          // exact format of your string
                                    CultureInfo.InvariantCulture),
                AirTemp = item.AirTemp,
                DewPt = item.DewPt,
                Pressure = item.Pressure
            }).ToList();
        }
    }
}
