namespace WeatherAPI.Services;
using WeatherAPI.Models;

public interface IWeatherService
{
    Task<WeatherResponse?> GetWeatherAsync(string cityName);
}
