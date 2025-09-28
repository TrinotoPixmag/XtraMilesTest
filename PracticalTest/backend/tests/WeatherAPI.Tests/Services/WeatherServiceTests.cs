using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Moq.Protected;
using WeatherAPI.Services;
using WeatherAPI.Models;
using WeatherAPI.Dtos;
using Xunit;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace WeatherAPI.Tests.Services
{
    public class WeatherServiceTests
    {
        private WeatherService CreateService(HttpClient client)
        {
            var logger = new Mock<ILogger<WeatherService>>().Object;
            var config = new WeatherConfig { ApiKey = "dummy", ApiEndpoint = "http://test" };
            return new WeatherService(client, logger, config);
        }

        [Fact]
        public void FahrenheitToCelsius_WorksCorrectly()
        {
            var c = WeatherService.FahrenheitToCelsius(212);
            Assert.Equal(100, c, 1);
        }

        [Fact]
        public async Task GetWeatherAsync_ReturnsResponse_OnSuccess()
        {
            var fakeApiResponse = new OpenWeatherMapResponse
            {
                Name = "London",
                Dt = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                Sys = new SysData { Country = "GB" },
                Wind = new WindData { Speed = 5, Deg = 200 },
                Main = new MainData { Temp = 68, DewPoint = 40, Pressure = 1012, Humidity = 60 },
                Visibility = 10000,
                Weather = new List<WeatherCondition>
                {
                    new WeatherCondition { Description = "clear sky" }
                }
            };

            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(JsonSerializer.Serialize(fakeApiResponse))
                });

            var httpClient = new HttpClient(handlerMock.Object);
            var svc = CreateService(httpClient);

            // Act
            var result = await svc.GetWeatherAsync("London");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("London", result.City);
            Assert.Equal("GB", result.Country);
            Assert.Equal("clear sky", result.SkyCondition);
        }

        [Fact]
        public async Task GetWeatherAsync_ReturnsNull_OnInvalidResponse()
        {
            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("null")
                });

            var httpClient = new HttpClient(handlerMock.Object);
            var svc = CreateService(httpClient);

            var result = await svc.GetWeatherAsync("Nowhere");

            Assert.Null(result);
        }
    }
}
