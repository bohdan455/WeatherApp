using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

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
