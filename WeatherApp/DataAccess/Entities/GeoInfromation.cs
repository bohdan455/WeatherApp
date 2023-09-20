using System.ComponentModel.DataAnnotations;

namespace WeatherApp.DataAccess.Entities
{
    public class GeoInfromation
    {
        [Key]
        public int Id { get; set; }

        public double Lat { get; set; }

        public double Lon { get; set; }
    }
}
