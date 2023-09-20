namespace WeatherApp.Models
{
    public class WeatherModel
    {
        public double Temperature { get; set; }

        public double FeelsLikeTemperature { get; set; }

        public double MinimumTemperature { get; set; }

        public double MaximumTemperature { get; set; }

        public int Pressure { get; set; }

        public int Humidity { get; set; }

        public double WindSpeed { get; set; }

        public double RainVolume { get; set; }

        public string Location { get; set; } = default!;
    }
}
