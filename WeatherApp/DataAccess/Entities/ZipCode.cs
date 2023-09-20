using System.ComponentModel.DataAnnotations;

namespace WeatherApp.DataAccess.Entities
{
    public class ZipCode
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(255)]
        public string Code { get; set; } = default!;

        public WeatherInformation WeatherInformation { get; set; } = default!;
    }
}
