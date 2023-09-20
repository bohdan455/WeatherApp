namespace WeatherApp.Dto
{
    public class WeatherInformationDto
    {
        public string DefaultLocationName { get; set; } = default!;

        public double Temperature { get; set; }

        public double FeelsLikeTemperature { get; set; }

        public double MinimumTemperature { get; set; }

        public double MaximumTemperature { get; set; }

        public int Pressure { get; set; }

        public int Humidity { get; set; }

        public double WindSpeed { get; set; }

        public double RainVolume { get; set; }

        public string? City { get; set; }

        public string? ZipCode { get; set; }

        public double Lat { get; set; }

        public double Lon { get; set; }

    }
}
