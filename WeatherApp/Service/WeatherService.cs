using Newtonsoft.Json;
using WeatherApp.DataAccess.Entities;
using WeatherApp.Dto;
using WeatherApp.Models;
using WeatherApp.Service.Interfaces;

namespace WeatherApp.Service
{
    public class WeatherService : IWeatherService
    {
        private readonly IGeoInformationService _geoInformationService;
        private readonly IOpenWeatherMapApiService _openWeatherMapApiService;
        private readonly IWeatherDatabaseService _weatherDatabaseService;

        public WeatherService(IGeoInformationService geoInformationService,
            IOpenWeatherMapApiService openWeatherMapApiService,
            IWeatherDatabaseService weatherDatabaseService)
        {
            _geoInformationService = geoInformationService;
            _openWeatherMapApiService = openWeatherMapApiService;
            _weatherDatabaseService = weatherDatabaseService;
        }

        public async Task<WeatherModel> GetWeatherInformationByCity(string city)
        {
            var weatherInformation = await _weatherDatabaseService.TryGetWeatherInformationByCity(city);
            if (weatherInformation != null)
            {
                return ParseWeatherModelFromDb(weatherInformation);
            }
            else
            {
                var geoInformation = await _geoInformationService.GetGeoInformationByCity(city) ?? throw new ArgumentException("No city information found");
                var weather = await GetWeatherInformationFromApi(geoInformation);
                var weatherInformationDto = ParseToDto(weather, geoInformation);
                weatherInformationDto.City = city;
                await _weatherDatabaseService.SaveWeatherInformationToDb(weatherInformationDto);
                return weather;
            }
        }

        public async Task<WeatherModel> GetWeatherInformationByZipCode(string zipCode)
        {
            var weatherInformation = await _weatherDatabaseService.TryGetWeatherInformationByZipCode(zipCode);
            if (weatherInformation != null)
            {
                return ParseWeatherModelFromDb(weatherInformation);
            }
            else
            {
                var geoInformation = await _geoInformationService.GetGeoInformationByZipCode(zipCode) ?? throw new ArgumentException("No city information found");
                var weather = await GetWeatherInformationFromApi(geoInformation);
                var weatherInformationDto = ParseToDto(weather, geoInformation);
                weatherInformationDto.ZipCode = zipCode;
                await _weatherDatabaseService.SaveWeatherInformationToDb(weatherInformationDto);
                return weather;
            }
        }

        public WeatherInformationDto ParseToDto(WeatherModel weatherModel, GeoInformationModel geoInformationModel)
        {
            return new WeatherInformationDto
            {
                FeelsLikeTemperature = weatherModel.FeelsLikeTemperature,
                Humidity = weatherModel.Humidity,
                DefaultLocationName = weatherModel.Location,
                MaximumTemperature = weatherModel.MaximumTemperature,
                MinimumTemperature = weatherModel.MinimumTemperature,
                Pressure = weatherModel.Pressure,
                RainVolume = weatherModel.RainVolume,
                Temperature = weatherModel.Temperature,
                WindSpeed = weatherModel.WindSpeed,
                Lat = geoInformationModel.Lat,
                Lon = geoInformationModel.Lon,
            };
        }
        public async Task<WeatherModel> GetWeatherInformationFromApi(GeoInformationModel geoInformation)
        {
            var weatherJson = await _openWeatherMapApiService.SendGetRequest($"/data/2.5/weather?lat={geoInformation.Lat}&lon={geoInformation.Lon}");
            dynamic data = JsonConvert.DeserializeObject(weatherJson) ?? throw new ArgumentException("No city information found");
            return ParseWeatherModelFromJson(data);
        }

        private WeatherModel ParseWeatherModelFromJson(dynamic data)
        {
            WeatherModel weatherInfo = new()
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

        private static WeatherModel ParseWeatherModelFromDb(WeatherInformation weatherInformation)
        {
            return new WeatherModel
            {
                FeelsLikeTemperature = weatherInformation.FeelsLikeTemperature,
                Humidity = weatherInformation.Humidity,
                Location = weatherInformation.DefaultLocationName,
                MaximumTemperature = weatherInformation.MaximumTemperature,
                MinimumTemperature = weatherInformation.MinimumTemperature,
                Pressure = weatherInformation.Pressure,
                RainVolume = weatherInformation.RainVolume,
                Temperature = weatherInformation.Temperature,
                WindSpeed = weatherInformation.WindSpeed,
            };
        }

        private static double CheckIsRainVolumeNull(dynamic data)
        {
            return (data.rain != null && data.rain["1h"] != null) ? data.rain["1h"] : 0;
        }
    }
}
