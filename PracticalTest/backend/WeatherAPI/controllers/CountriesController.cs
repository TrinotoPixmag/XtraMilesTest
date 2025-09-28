using Microsoft.AspNetCore.Mvc;
using WeatherAPI.Services;

namespace WeatherAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CountriesController : ControllerBase
{
    private readonly ICountriesService _countriesService;

    public CountriesController(ICountriesService countriesService)
    {
        _countriesService = countriesService;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        try
        {
            var countries = await _countriesService.GetCountry();
            return Ok(countries);
        }
        catch (HttpRequestException ex)
        {
            return StatusCode(502, new { error = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }

    [HttpGet("{countryCode}/cities")]
    public async Task<IActionResult> GetCities(string countryCode)
    {
        var cities = await _countriesService.GetCities(countryCode);
        if (!cities.Any())
            return NotFound(new { message = $"No cities found for country code '{countryCode}'" });

        return Ok(cities);
    }
}
