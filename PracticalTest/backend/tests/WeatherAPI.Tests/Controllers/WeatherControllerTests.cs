using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WeatherAPI.Controllers;
using WeatherAPI.Models;
using WeatherAPI.Services;
using Xunit;

namespace WeatherAPI.Tests.Controllers
{
    public class WeatherControllerTests
    {
        [Fact]
        public async Task GetWeather_ReturnsOk_WhenServiceReturnsData()
        {
            // Arrange
            var mockService = new Mock<IWeatherService>();
            mockService.Setup(s => s.GetWeatherAsync("London"))
                .ReturnsAsync(new WeatherResponse
                {
                    City = "London",
                    Country = "GB",
                    TemperatureC = 20,
                    TemperatureF = 68,
                    SkyCondition = "clear"
                });

            var controller = new WeatherController(mockService.Object);

            // Act
            var result = await controller.GetWeather("London");

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var value = Assert.IsType<WeatherResponse>(okResult.Value);
            Assert.Equal("London", value.City);
            Assert.Equal("GB", value.Country);
        }

        [Fact]
        public async Task GetWeather_ReturnsNotFound_WhenServiceReturnsNull()
        {
            // Arrange
            var mockService = new Mock<IWeatherService>();
            mockService.Setup(s => s.GetWeatherAsync("Nowhere"))
                .ReturnsAsync((WeatherResponse?)null);

            var controller = new WeatherController(mockService.Object);

            // Act
            var result = await controller.GetWeather("Nowhere");

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task GetWeather_Returns500_OnException()
        {
            // Arrange
            var mockService = new Mock<IWeatherService>();
            mockService.Setup(s => s.GetWeatherAsync("Paris"))
                .ThrowsAsync(new Exception("API error"));

            var controller = new WeatherController(mockService.Object);

            // Act
            IActionResult result;
            try
            {
                result = await controller.GetWeather("Paris");
            }
            catch (Exception ex)
            {
                Assert.Equal("API error", ex.Message);
                return;
            }
            Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(500, ((StatusCodeResult)result).StatusCode);
        }
    }
}
