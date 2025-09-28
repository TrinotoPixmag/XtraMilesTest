using Microsoft.AspNetCore.Mvc;
using WeatherAPI.Services;

namespace WeatherAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WeatherController : ControllerBase
{
    private readonly IWeatherService _weatherService;

    public WeatherController(IWeatherService weatherService)
    {
        _weatherService = weatherService;
    }

    [HttpGet("{cityName}")]
    public async Task<IActionResult> GetWeather(string cityName)
    {
        try
        {
            var weather = await _weatherService.GetWeatherAsync(cityName);
            if (weather == null)
                return NotFound();

            return Ok(weather);
        }
        catch (HttpRequestException ex)
        {
            return StatusCode(502, ex.Message);
        }
        catch (Exception)
        {
            return StatusCode(500);
        }
    }
}
