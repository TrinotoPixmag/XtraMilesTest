using WeatherAPI.Models;

namespace WeatherAPI.Repositories
{
    public class InMemoryCountryRepository : ICountryRepository
    {
        private readonly List<Country> _countries;

        public InMemoryCountryRepository()
        {
            _countries = SeedCountries();
        }

        public IEnumerable<Country> GetAll() => _countries;

        public Country? GetByCode(string code) =>
            _countries.FirstOrDefault(c => string.Equals(c.Code, code, StringComparison.OrdinalIgnoreCase));

        public Country? GetByName(string name) =>
            _countries.FirstOrDefault(c => string.Equals(c.Name, name, StringComparison.OrdinalIgnoreCase));

        public IEnumerable<string> GetCities(string countryCodeOrName)
        {
            var country = GetByCode(countryCodeOrName) ?? GetByName(countryCodeOrName);
            return country?.Cities ?? Enumerable.Empty<string>();
        }

        private static List<Country> SeedCountries() =>
            new()
            {
                new Country
                {
                    Code = "US",
                    Name = "United States",
                    Cities = new List<string> { "New York", "Los Angeles", "Chicago", "Houston", "Phoenix" }
                },
                new Country
                {
                    Code = "GB",
                    Name = "United Kingdom",
                    Cities = new List<string> { "London", "Manchester", "Birmingham", "Leeds", "Glasgow" }
                },
                new Country
                {
                    Code = "JP",
                    Name = "Japan",
                    Cities = new List<string> { "Tokyo", "Osaka", "Kyoto", "Nagoya", "Sapporo" }
                },
                new Country
                {
                    Code = "ID",
                    Name = "Indonesia",
                    Cities = new List<string> { "Jakarta", "Bandung", "Surabaya", "Yogyakarta", "Bali" }
                },
                new Country
                {
                    Code = "AU",
                    Name = "Australia",
                    Cities = new List<string> { "Sydney", "Melbourne", "Brisbane", "Perth", "Adelaide" }
                }
            };
    }
}
