using System.ComponentModel.DataAnnotations;

namespace WeatherApp.Models
{
    public class ZipCodeModel
    {
        [Required(ErrorMessage = "Please enter a zip code.")]
        public string Code { get; set; } = default!;
    }
}
