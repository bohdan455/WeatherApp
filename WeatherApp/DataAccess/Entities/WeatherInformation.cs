using System.ComponentModel.DataAnnotations;

namespace WeatherApp.DataAccess.Entities
{
    public class WeatherInformation
    {
        [Required]
        public int Id { get; set; }

        [MaxLength(255)]
        public string DefaultLocationName { get; set; } = default!;

        public double Temperature { get; set; }

        public double FeelsLikeTemperature { get; set; }

        public double MinimumTemperature { get; set; }

        public double MaximumTemperature { get; set; }

        public int Pressure { get; set; }

        public int Humidity { get; set; }

        public double WindSpeed { get; set; }

        public double RainVolume { get; set; }

        public City City { get; set; } = default!;

        public int CityId { get; set; }

        public ZipCode? ZipCode { get; set; }

        public int? ZipCodeId { get; set; }

        public GeoInfromation GeoInfromation { get; set; } = default!;

        public int GeoInfromationId { get; set; }
    }
}
