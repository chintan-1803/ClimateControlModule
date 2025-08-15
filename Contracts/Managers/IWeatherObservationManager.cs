
using DTO.WeatherObservation;

namespace Contracts.Managers
{
    public interface IWeatherObservationManager
    {
        /// <summary>
        /// Gets the weather observation data by WMO number.
        /// </summary>
        /// <param name="wmoNumber">The WMO number of the weather station.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the weather observation response.</returns>
        Task<WeatherObservationResponse> GetWeatherObservationDataByStationAsyncForLastNHours(WeatherObservationRequest objRequest);
    }
}
