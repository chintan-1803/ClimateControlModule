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
        public List<WeatherObservation> MapWeatherObservationsForLast72Hours(List<WeatherObservationReceivedFromApi> weatherObservationReceivedList)
        {
            if (weatherObservationReceivedList == null || !weatherObservationReceivedList.Any())
            {
                return new List<WeatherObservation>();
            }

            DateTime lastThreeDaysData = DateTime.Now.AddHours(-24);

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
            }).Where(x=>x.LocalDateTimeFull >= lastThreeDaysData).ToList(); // Filter for last three days data



        }
    }
}
