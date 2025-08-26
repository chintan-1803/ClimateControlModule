
using Moq;
using Contracts.Managers;
using ClimateControlModuleBackEnd.Controllers;
namespace ClimateControlModuleTests.Controllers
{
    public class WeatherObservationControllerTests
    {
        private readonly Mock<IWeatherObservationManager> _mockManager;
        private readonly WeatherObservationController _controller;

        public WeatherObservationControllerTests()
        {
            _mockManager = new Mock<IWeatherObservationManager>();
            _controller = new WeatherObservationController(_mockManager.Object);
        }


        [Fact]
        public async Task GetWeatherObservationDataByStationAsyncForLastNHours_ReturnsExpectedResponse()
        {
            // Arrange
            var request = new DTO.WeatherObservation.WeatherObservationRequest
            {
               WmoNumber = 94672
            };
            var expectedResponse = new DTO.WeatherObservation.WeatherObservationResponse
            {
                Result = DTO.WeatherObservation.WeatherObservationResult.Success,   
            };

            _mockManager
                .Setup(m => m.GetWeatherObservationDataByStationAsyncForLastNHours(request))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.GetWeatherObservationDataByStationAsyncForLastNHours(request);

            // Assert
            Assert.Equal(expectedResponse, result);

        }
    }
}
