using WeatherApp.Models;
using WeatherApp.Service.Interfaces;

namespace WeatherApp.BackgroundServices
{
    public class WeatherInformationCheckerService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<WeatherInformationCheckerService> _logger;

        public WeatherInformationCheckerService(IServiceProvider serviceProvider,
            ILogger<WeatherInformationCheckerService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            const int hours = 10;
            const int millisecondsInHour = 3600000;
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await RefreshWeatherInformation();
                }
                catch (Exception e)
                {
                    _logger.LogError("Error in background service: {e}", e.ToString());
                }
                _logger.LogInformation("Weather information checked");
                await Task.Delay(hours * millisecondsInHour, stoppingToken);
            }
        }

        private async Task RefreshWeatherInformation()
        {
            using var scope = _serviceProvider.CreateScope();
            var weatherDatabaseService = scope.ServiceProvider.GetRequiredService<IWeatherDatabaseService>();
            var weatherService = scope.ServiceProvider.GetRequiredService<IWeatherService>();
            var weatherInformations = await weatherDatabaseService.GetAll();
            foreach (var weatherInformation in weatherInformations)
            {
                var geoInformation = new GeoInformationModel
                {
                    Lat = weatherInformation.GeoInfromation.Lat,
                    Lon = weatherInformation.GeoInfromation.Lon,
                };

                var weatherModel = await weatherService.GetWeatherInformationFromApi(geoInformation);
                var weatherDto = weatherService.ParseToDto(weatherModel, geoInformation);
                await weatherDatabaseService.SaveWeatherInformationToDb(weatherDto);
            }
        }
    }
}
