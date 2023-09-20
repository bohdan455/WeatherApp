using Microsoft.EntityFrameworkCore;
using WeatherApp.DataAccess;
using WeatherApp.DataAccess.Entities;
using WeatherApp.Dto;
using WeatherApp.Service.Interfaces;

namespace WeatherApp.Service
{
    public class WeatherDatabaseService : IWeatherDatabaseService
    {
        private readonly ApplicationDbContext _context;

        public WeatherDatabaseService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<WeatherInformation?> TryGetWeatherInformationByCity(string city)
        {
            var result = await _context.Cities.Include(c => c.WeatherInformation)
                .FirstOrDefaultAsync(c => c.Name == city);

            return result?.WeatherInformation;
        }

        public async Task<WeatherInformation?> TryGetWeatherInformationByZipCode(string zipCode)
        {
            var result = await _context.ZipCodes.Include(zc => zc.WeatherInformation)
                .FirstOrDefaultAsync(zc => zc.Code == zipCode);

            return result?.WeatherInformation;
        }

        public async Task SaveWeatherInformationToDb(WeatherInformationDto weatherInformation)
        {
            var weatherInformationToDb = await _context.WeatherInformations.Include(wi => wi.Cities).FirstOrDefaultAsync(wi => wi.DefaultLocationName == weatherInformation.DefaultLocationName);

            weatherInformationToDb ??= new WeatherInformation();
            AssignDtoToDb(weatherInformationToDb, weatherInformation);

            await SetZipCodeToWeatherInformationWhenItIsNotNull(weatherInformationToDb, weatherInformation.ZipCode);

            await SetCityToWeatherInformationWhenItIsNotNull(weatherInformationToDb, weatherInformation.City);

            _context.WeatherInformations.Update(weatherInformationToDb);
            await _context.SaveChangesAsync();
        }

        public async Task<List<WeatherInformation>> GetAll()
        {
            return await _context.WeatherInformations.Include(wi => wi.GeoInfromation).ToListAsync();
        }

        private async Task SetZipCodeToWeatherInformationWhenItIsNotNull(WeatherInformation weatherInformationToDb, string? zipCode)
        {
            if (zipCode != null)
            {
                var zipCodeToDb = await _context.ZipCodes.FirstOrDefaultAsync(zc => zc.Code == zipCode);
                zipCodeToDb ??= new ZipCode
                {
                    Code = zipCode,
                };

                weatherInformationToDb.ZipCode = zipCodeToDb;
            }
        }

        private async Task SetCityToWeatherInformationWhenItIsNotNull(WeatherInformation weatherInformationToDb, string? city)
        {
            if (city != null)
            {
                var cityToDb = await _context.Cities.FirstOrDefaultAsync(c => c.Name == city);
                cityToDb ??= new City
                {
                    Name = city
                };

                weatherInformationToDb.Cities.Add(cityToDb);
            }
        }

        private void AssignDtoToDb(WeatherInformation weatherInformationToDb, WeatherInformationDto weatherInformationDto)
        {
            weatherInformationToDb.Temperature = weatherInformationDto.Temperature;
            weatherInformationToDb.Humidity = weatherInformationDto.Humidity;
            weatherInformationToDb.Pressure = weatherInformationDto.Pressure;
            weatherInformationToDb.WindSpeed = weatherInformationDto.WindSpeed;
            weatherInformationToDb.GeoInfromation = new GeoInfromation
            {
                Lat = weatherInformationDto.Lat,
                Lon = weatherInformationDto.Lon
            };
            weatherInformationToDb.DefaultLocationName = weatherInformationDto.DefaultLocationName;
            weatherInformationToDb.FeelsLikeTemperature = weatherInformationDto.FeelsLikeTemperature;
            weatherInformationToDb.MaximumTemperature = weatherInformationDto.MaximumTemperature;
            weatherInformationToDb.MinimumTemperature = weatherInformationDto.MinimumTemperature;
            weatherInformationToDb.RainVolume = weatherInformationDto.RainVolume;
        }
    }
}
