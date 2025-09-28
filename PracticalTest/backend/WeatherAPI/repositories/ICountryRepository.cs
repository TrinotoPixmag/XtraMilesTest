using WeatherAPI.Models;

namespace WeatherAPI.Repositories
{
    public interface ICountryRepository
    {
        IEnumerable<Country> GetAll();
        IEnumerable<string> GetCities(string countryCodeOrName);
    }
}
