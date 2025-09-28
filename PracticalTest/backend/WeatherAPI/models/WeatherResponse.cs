namespace WeatherAPI.Models
{
    public class WeatherResponse
    {
        public string City { get; set; } = default!;
        public string Country { get; set; } = default!;
        public DateTime UtcTime { get; set; }

        public double WindSpeed { get; set; }
        public int WindDirection { get; set; }
        public int Visibility { get; set; }
        public string SkyCondition { get; set; } = default!;

        public double TemperatureF { get; set; }
        public double TemperatureC { get; set; }

        public double DewPoint { get; set; }
        public int Humidity { get; set; }
        public int Pressure { get; set; }
    }
}
