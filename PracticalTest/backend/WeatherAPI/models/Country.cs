namespace WeatherAPI.Models
{
    public class Country
    {
        public string Code { get; set; } = default!;
        public string Name { get; set; } = default!;
        public List<string> Cities { get; set; } = new();
    }
}
