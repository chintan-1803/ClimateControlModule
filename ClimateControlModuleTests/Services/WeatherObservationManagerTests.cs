using BI.Utilities;
using BI;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace ClimateControlModuleTests.Services
{
    public class WeatherObservationManagerTests
    {
        private readonly Mock<IHelper> _mockHelper;

        private readonly WeatherObservationManager _manager;

        public WeatherObservationManagerTests()
        {
            _mockHelper = new Mock<IHelper>();
            var mockOptions = Options.Create(new BI.Settings.BOMApiSettings { Url = "https://api.weather.com/v1/location/{0}:4:AU/observations/current.json?apiKey=YOUR_API_KEY" });
            _manager = new WeatherObservationManager(_mockHelper.Object, mockOptions);
        }

        [Fact]
        public async Task GetWeatherObservationDataByStationAsyncForLastNHours_ShouldReturn_WmoNumberNotFound_WhenRequestIsInvalid()
        {
            // Arrange
            var request = new DTO.WeatherObservation.WeatherObservationRequest
            {
                WmoNumber = 0 // Invalid WmoNumber
            };
            // Act
            var result = await _manager.GetWeatherObservationDataByStationAsyncForLastNHours(request);

            // Assert
            Assert.Equal(DTO.WeatherObservation.WeatherObservationResult.WmoNumberNotFound, result.Result);
        }


        [Fact]
        public async Task GetWeatherObservationDataByStationAsyncForLastNHours_ShouldReturn_Success_WhenApiReturnsData()
        {

            // Arrange
            var request = new DTO.WeatherObservation.WeatherObservationRequest
            {
                WmoNumber = 94672 // Valid WmoNumber
            };

            var mockApiResponse = new HttpResponseMessage(System.Net.HttpStatusCode.OK)
            {
                Content = new StringContent(@"{
                    'observations': {
                        'data': [
                            {
                                'wmo': 94672,
                                'name': 'Sample Station',
                                'local_date_time_full': '2023-10-01T12:00:00+10:00',
                                'air_temp': 22.5,
                                'dewpt': 15.0,
                                'press': 1013.2
                            }
                        ]
                    }
                }")
            };


            _mockHelper
                .Setup(h => h.SendRequestToExternalApi(Method.Get, It.IsAny<string>()))
                .ReturnsAsync(mockApiResponse);

            // Act
            var result = await _manager.GetWeatherObservationDataByStationAsyncForLastNHours(request);


            // Assert
            Assert.Equal(DTO.WeatherObservation.WeatherObservationResult.Success, result.Result);
        }

        [Fact]
        public async Task GetWeatherObservationDataByStationAsyncForLastNHours_ShouldReturn_Error_WhenApiThrowsException()
        {
            // Arrange
            var request = new DTO.WeatherObservation.WeatherObservationRequest
            {
                WmoNumber = 94672 // Valid WmoNumber
            };
         
            _mockHelper
                .Setup(h => h.SendRequestToExternalApi(Method.Get, It.IsAny<string>()))
                .ThrowsAsync(new Exception("API Error"));
            // Act
            var result = await _manager.GetWeatherObservationDataByStationAsyncForLastNHours(request);
            // Assert
            Assert.Equal(DTO.WeatherObservation.WeatherObservationResult.Error, result.Result);
        }

    }
}
