using System.Text.Json.Serialization;

namespace WeatherApp.Models
{
    public class GeoInformationModel
    {
        [JsonPropertyName("lat")]
        public double Lat { get; set; }

        [JsonPropertyName("lon")]
        public double Lon { get; set; }
    }
}
