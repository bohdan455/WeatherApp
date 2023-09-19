using WeatherApp.Models;

namespace WeatherApp.Service.Interfaces
{
    public interface IGeoInformationService
    {
        Task<GeoInformationModel> GetGeoInformationByCity(string city);
        Task<GeoInformationModel> GetGeoInformationByZipCode(string zipCode);
    }
}