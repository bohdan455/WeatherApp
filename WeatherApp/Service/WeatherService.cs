using Newtonsoft.Json;
using WeatherApp.Models;
using WeatherApp.Service.Interfaces;

namespace WeatherApp.Service
{
    public class WeatherService : IWeatherService
    {
        private readonly IGeoInformationService _geoInformationService;
        private readonly IOpenWeatherMapApiService _openWeatherMapApiService;

        public WeatherService(IGeoInformationService geoInformationService,IOpenWeatherMapApiService openWeatherMapApiService)
        {
            _geoInformationService = geoInformationService;
            _openWeatherMapApiService = openWeatherMapApiService;
        }

        public async Task<WeatherModel> GetWeatherInformationByCity(string city)
        {
            var geoInformation = await _geoInformationService.GetGeoInformationByCity(city);
            return await GetWeatherInformation(geoInformation);
        }

        public async Task<WeatherModel> GetWeatherInformationByZipCode(string zipCode)
        {
            var geoInfromation = await _geoInformationService.GetGeoInformationByZipCode(zipCode);
            return await GetWeatherInformation(geoInfromation);
        }

        private async Task<WeatherModel> GetWeatherInformation(GeoInformationModel geoInformation)
        {
            var weatherJson = await _openWeatherMapApiService.SendGetRequest($"/data/2.5/weather?lat={geoInformation.Lat}&lon={geoInformation.Lon}");
            dynamic data = JsonConvert.DeserializeObject(weatherJson) ?? throw new ArgumentException("No city information found");
            return ParseWeatherModel(data);
        }

        private WeatherModel ParseWeatherModel(dynamic data)
        {
            WeatherModel weatherInfo = new WeatherModel
            {
                Temperature = data.main.temp,
                FeelsLikeTemperature = data.main.feels_like,
                MinimumTemperature = data.main.temp_min,
                MaximumTemperature = data.main.temp_max,
                Pressure = data.main.pressure,
                Humidity = data.main.humidity,
                WindSpeed = data.wind.speed,
                RainVolume = CheckIsRainVolumeNull(data),
                Location = data.name
            };

            return weatherInfo;
        }

        private double CheckIsRainVolumeNull(dynamic data)
        {
            return (data.rain != null && data.rain["1h"] != null) ? data.rain["1h"] : 0;
        }
    }
}
