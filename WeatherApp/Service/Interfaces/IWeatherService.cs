using WeatherApp.Dto;
using WeatherApp.Models;

namespace WeatherApp.Service.Interfaces
{
    public interface IWeatherService
    {
        Task<WeatherModel> GetWeatherInformationByCity(string city);
        Task<WeatherModel> GetWeatherInformationByZipCode(string zipCode);
        Task<WeatherModel> GetWeatherInformationFromApi(GeoInformationModel geoInformation);
        WeatherInformationDto ParseToDto(WeatherModel weatherModel, GeoInformationModel geoInformationModel);
    }
}