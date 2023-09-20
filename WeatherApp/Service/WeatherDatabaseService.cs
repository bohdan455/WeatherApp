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

            weatherInformationToDb ??= new WeatherInformation
            {
                Temperature = weatherInformation.Temperature,
                Humidity = weatherInformation.Humidity,
                Pressure = weatherInformation.Pressure,
                WindSpeed = weatherInformation.WindSpeed,
                GeoInfromation = new GeoInfromation
                {
                    Lat = weatherInformation.Lat,
                    Lon = weatherInformation.Lon
                },
                DefaultLocationName = weatherInformation.DefaultLocationName,
                FeelsLikeTemperature = weatherInformation.FeelsLikeTemperature,
                MaximumTemperature = weatherInformation.MaximumTemperature,
                MinimumTemperature = weatherInformation.MinimumTemperature,
                RainVolume = weatherInformation.RainVolume,
            };

            if (weatherInformation.ZipCode != null)
            {
                var zipCode = await _context.ZipCodes.FirstOrDefaultAsync(zc => zc.Code == weatherInformation.ZipCode);
                zipCode ??= new ZipCode
                {
                    Code = weatherInformation.ZipCode
                };

                weatherInformationToDb.ZipCode = zipCode;
            }

            if (weatherInformation.City != null)
            {
                var city = await _context.Cities.FirstOrDefaultAsync(c => c.Name == weatherInformation.City);
                city ??= new City
                {
                    Name = weatherInformation.City
                };

                weatherInformationToDb.Cities.Add(city);
            }

            _context.WeatherInformations.Update(weatherInformationToDb);
            await _context.SaveChangesAsync();
        }
    }
}
