namespace WeatherAPI.Dtos
{
    public class OpenWeatherMapResponse
    {
        public List<WeatherCondition> Weather { get; set; } = new();
        public MainData Main { get; set; } = default!;
        public WindData Wind { get; set; } = default!;
        public SysData Sys { get; set; } = default!;
        public string Name { get; set; } = default!;
        public long Dt { get; set; }
        public int Visibility { get; set; }
    }

    public class WeatherCondition { public string Description { get; set; } = default!; }
    public class MainData
    {
        public double Temp { get; set; }
        public double DewPoint { get; set; }
        public int Pressure { get; set; }
        public int Humidity { get; set; }
    }
    public class WindData { 
        public double Speed { get; set; } 
        public int Deg { get; set; } 
    }
    public class SysData { 
        public string Country { get; set; } = default!; 
    }
}