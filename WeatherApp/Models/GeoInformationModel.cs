using System.Text.Json.Serialization;

namespace WeatherApp.Models
{
    public class GeoInformationModel
    {
        [JsonPropertyName("lat")]
        public decimal Lat { get; set; }

        [JsonPropertyName("lon")]
        public decimal Lon { get; set; }
    }
}
