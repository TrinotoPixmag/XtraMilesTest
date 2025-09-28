using System.Net.Http.Json;
using WeatherAPI.Models;
using WeatherAPI.Dtos;

namespace WeatherAPI.Services;

public class WeatherService : IWeatherService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<WeatherService> _logger;
    private readonly WeatherConfig _config;

    public WeatherService(HttpClient httpClient, ILogger<WeatherService> logger, WeatherConfig config)
    {
        _httpClient = httpClient;
        _logger = logger;
        _config = config;
    }

    public async Task<WeatherResponse?> GetWeatherAsync(string cityName)
    {
        var url =  $"{_config.ApiEndpoint}/weather?q={cityName}&units=imperial&appid={_config.ApiKey}";
        var response = await _httpClient.GetFromJsonAsync<OpenWeatherMapResponse>(url);

        if (response == null) return null;

        double dewC = (response.Main.DewPoint - 32) * 5 / 9;

        return new WeatherResponse
        {
            City = response.Name,
            Country = response.Sys.Country,
            UtcTime = DateTimeOffset.FromUnixTimeSeconds(response.Dt).UtcDateTime,
            WindSpeed = response.Wind.Speed,
            WindDirection = response.Wind.Deg,
            Visibility = response.Visibility,
            SkyCondition = response.Weather.FirstOrDefault()?.Description ?? "unknown",
            TemperatureF = response.Main.Temp,
            TemperatureC = FahrenheitToCelsius(response.Main.Temp),
            DewPoint = Math.Round(dewC, 2),
            Humidity = response.Main.Humidity,
            Pressure = response.Main.Pressure
        };
    }
    private static double FahrenheitToCelsius(double f) => Math.Round((f - 32) * 5.0 / 9.0, 2);
}