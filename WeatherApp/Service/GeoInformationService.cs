using WeatherApp.Models;
using WeatherApp.Service.Interfaces;

namespace WeatherApp.Service
{
    public class GeoInformationService : IGeoInformationService
    {
        private readonly IOpenWeatherMapApiService _openWeatherMapApiService;

        public GeoInformationService(IOpenWeatherMapApiService openWeatherMapApiService)
        {
            _openWeatherMapApiService = openWeatherMapApiService;
        }

        public async Task<GeoInformationModel> GetGeoInformationByCity(string city)
        {
            var listOfGeoInformation = await _openWeatherMapApiService.SendGetRequestAndParse<List<GeoInformationModel>>($"/geo/1.0/direct?q={city}");

            return listOfGeoInformation[0];
        }

        public async Task<GeoInformationModel> GetGeoInformationByZipCode(string zipCode)
        {
            return await _openWeatherMapApiService.SendGetRequestAndParse<GeoInformationModel>($"/geo/1.0/zip?zip={zipCode}");
        }

    }
}
