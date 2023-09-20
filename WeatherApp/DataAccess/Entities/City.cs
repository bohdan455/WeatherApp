using System.ComponentModel.DataAnnotations;

namespace WeatherApp.DataAccess.Entities
{
    public class City
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; } = default!;

        public WeatherInformation WeatherInformation { get; set; } = default!;
    }
}
