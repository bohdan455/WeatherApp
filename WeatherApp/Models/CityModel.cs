using System.ComponentModel.DataAnnotations;

namespace WeatherApp.Models
{
    public class CityModel
    {
        [Required(ErrorMessage = "Please enter a city name.")]
        public string Name { get; set; } = default!;
    }
}
