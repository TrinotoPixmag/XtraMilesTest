namespace WeatherAPI.Services;
using WeatherAPI.Models;

public interface ICountriesService
{
    Task<IEnumerable<Country>> GetCountry();
    Task<IEnumerable<string>> GetCities(string countryCode);
}
