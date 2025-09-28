using WeatherAPI.Models;
using WeatherAPI.Repositories;

namespace WeatherAPI.Services;

public class CountriesService : ICountriesService
{
    private readonly ILogger<CountriesService> _logger;
    private readonly ICountryRepository _countryRepository;

    public CountriesService(ILogger<CountriesService> logger, ICountryRepository countryRepository)
    {
        _logger = logger;
        _countryRepository = countryRepository;
    }
    
    public Task<IEnumerable<Country>> GetCountry()
    {
        var list = _countryRepository.GetAll()
                                     .Select(c => new Country { Code = c.Code, Name = c.Name });

        return Task.FromResult(list);
    }

    public Task<IEnumerable<string>> GetCities(string countryCode)
    {
        var cities = _countryRepository.GetCities(countryCode);
        return Task.FromResult(cities);
    }
}
