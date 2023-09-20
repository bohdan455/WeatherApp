using WeatherApp.DataAccess.Entities;
using WeatherApp.Dto;

namespace WeatherApp.Service.Interfaces
{
    public interface IWeatherDatabaseService
    {
        Task<List<WeatherInformation>> GetAll();
        Task SaveWeatherInformationToDb(WeatherInformationDto weatherInformation);
        Task<WeatherInformation?> TryGetWeatherInformationByCity(string city);
        Task<WeatherInformation?> TryGetWeatherInformationByZipCode(string zipCode);
    }
}