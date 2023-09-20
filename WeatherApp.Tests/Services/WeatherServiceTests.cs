using Moq;
using WeatherApp.Models;
using WeatherApp.Service.Interfaces;
using WeatherApp.Service;
using WeatherApp.DataAccess.Entities;
using WeatherApp.Dto;

namespace WeatherApp.Tests.Services
{
    public class WeatherServiceTests
    {
        private readonly WeatherModel expected = new WeatherModel
        {
            Temperature = 25,
            FeelsLikeTemperature = 30,
            MinimumTemperature = 20,
            MaximumTemperature = 30,
            Pressure = 101,
            Humidity = 80,
            WindSpeed = 10,
            RainVolume = 0,
            Location = "New York"
        };

        private readonly Mock<IGeoInformationService> geoInfoServiceMock = new Mock<IGeoInformationService>();
        private readonly Mock<IOpenWeatherMapApiService> openWeatherMapApiServiceMock = new Mock<IOpenWeatherMapApiService>();
        private readonly Mock<IWeatherDatabaseService> weatherDbServiceMock = new Mock<IWeatherDatabaseService>();
        private readonly WeatherService weatherService;

        public WeatherServiceTests()
        {
            weatherService = new WeatherService(geoInfoServiceMock.Object, openWeatherMapApiServiceMock.Object, weatherDbServiceMock.Object);
        }

        [Fact]
        public async Task GetWeatherInformationByCity_ShouldReturnWeatherFromDb_WhenAvailable()
        {
            // Arrange
            weatherDbServiceMock.Setup(s => s.TryGetWeatherInformationByCity("New York"))
                .ReturnsAsync(new WeatherInformation { Temperature = 25 });

            // Act
            var result = await weatherService.GetWeatherInformationByCity("New York");

            // Assert
            weatherDbServiceMock.Verify(s => s.TryGetWeatherInformationByCity("New York"), Times.Once);
            geoInfoServiceMock.Verify(s => s.GetGeoInformationByCity(It.IsAny<string>()), Times.Never);
            openWeatherMapApiServiceMock.Verify(s => s.SendGetRequest(It.IsAny<string>()), Times.Never);
            Assert.Equal(25, result.Temperature);
        }

        [Fact]
        public async Task GetWeatherInformationByCity_ShouldReturnWeatherFromApi_WhenNotAvailableInDb()
        {
            // Arrange
            weatherDbServiceMock.Setup(s => s.TryGetWeatherInformationByCity("Paris"))
                .ReturnsAsync((WeatherInformation)null); // Return null from DB
            geoInfoServiceMock.Setup(s => s.GetGeoInformationByCity("Paris"))
                .ReturnsAsync(new GeoInformationModel { Lat = 48.86, Lon = 2.35 }); // Return geo info for Paris
            openWeatherMapApiServiceMock.Setup(s => s.SendGetRequest("/data/2.5/weather?lat=48.86&lon=2.35"))
                .ReturnsAsync("{\"main\":{\"temp\":25,\"feels_like\":30,\"temp_min\":20,\"temp_max\":30,\"pressure\":101,\"humidity\":80},\"wind\":{\"speed\":10},\"name\":\"New York\",\"rain\":{\"1h\":0}}"); // Return weather data for Paris

            // Act
            var result = await weatherService.GetWeatherInformationByCity("Paris");

            // Assert
            weatherDbServiceMock.Verify(s => s.TryGetWeatherInformationByCity("Paris"), Times.Once);
            geoInfoServiceMock.Verify(s => s.GetGeoInformationByCity("Paris"), Times.Once);
            openWeatherMapApiServiceMock.Verify(s => s.SendGetRequest("/data/2.5/weather?lat=48.86&lon=2.35"), Times.Once);
            weatherDbServiceMock.Verify(s => s.SaveWeatherInformationToDb(It.IsAny<WeatherInformationDto>()), Times.Once);
            AssertWeatherModel(result);
        }

        [Fact]
        public async Task GetWeatherInformationByCity_ShouldThrowException_WhenNoGeoInfoFound()
        {
            // Arrange
            weatherDbServiceMock.Setup(s => s.TryGetWeatherInformationByCity("Invalid City"))
                .ReturnsAsync((WeatherInformation)null); // Return null from DB
            geoInfoServiceMock.Setup(s => s.GetGeoInformationByCity("Invalid City"))
                .ReturnsAsync((GeoInformationModel)null); // Return null for geo info

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => weatherService.GetWeatherInformationByCity("Invalid City"));
        }

        [Fact]
        public async Task GetWeatherInformationByZipCode_ShouldReturnWeatherFromDb_WhenAvailable()
        {
            // Arrange
            weatherDbServiceMock.Setup(s => s.TryGetWeatherInformationByZipCode("000"))
                .ReturnsAsync(new WeatherInformation { Temperature = 25 });

            // Act
            var result = await weatherService.GetWeatherInformationByZipCode("000");

            // Assert
            weatherDbServiceMock.Verify(s => s.TryGetWeatherInformationByZipCode("000"), Times.Once);
            geoInfoServiceMock.Verify(s => s.GetGeoInformationByZipCode(It.IsAny<string>()), Times.Never);
            openWeatherMapApiServiceMock.Verify(s => s.SendGetRequest(It.IsAny<string>()), Times.Never);
            Assert.Equal(25, result.Temperature);
        }

        [Fact]
        public async Task GetWeatherInformationByZipCode_ShouldReturnWeatherFromApi_WhenNotAvailableInDb()
        {
            // Arrange
            weatherDbServiceMock.Setup(s => s.TryGetWeatherInformationByZipCode("000"))
                .ReturnsAsync((WeatherInformation)null); // Return null from DB
            geoInfoServiceMock.Setup(s => s.GetGeoInformationByZipCode("000"))
                .ReturnsAsync(new GeoInformationModel { Lat = 48.86, Lon = 2.35 }); // Return geo info for Paris
            openWeatherMapApiServiceMock.Setup(s => s.SendGetRequest("/data/2.5/weather?lat=48.86&lon=2.35"))
                .ReturnsAsync("{\"main\":{\"temp\":25,\"feels_like\":30,\"temp_min\":20,\"temp_max\":30,\"pressure\":101,\"humidity\":80},\"wind\":{\"speed\":10},\"name\":\"New York\",\"rain\":{\"1h\":0}}"); // Return weather data for Paris

            // Act
            var result = await weatherService.GetWeatherInformationByZipCode("000");

            // Assert
            weatherDbServiceMock.Verify(s => s.TryGetWeatherInformationByZipCode("000"), Times.Once);
            geoInfoServiceMock.Verify(s => s.GetGeoInformationByZipCode("000"), Times.Once);
            openWeatherMapApiServiceMock.Verify(s => s.SendGetRequest("/data/2.5/weather?lat=48.86&lon=2.35"), Times.Once);
            weatherDbServiceMock.Verify(s => s.SaveWeatherInformationToDb(It.IsAny<WeatherInformationDto>()), Times.Once);
            AssertWeatherModel(result);
        }

        [Fact]
        public async Task GetWeatherInformationByZipCode_ShouldThrowException_WhenNoGeoInfoFound()
        {
            // Arrange
            weatherDbServiceMock.Setup(s => s.TryGetWeatherInformationByZipCode("Invalid ZipCode"))
                .ReturnsAsync((WeatherInformation)null); // Return null from DB
            geoInfoServiceMock.Setup(s => s.GetGeoInformationByZipCode("Invalid ZipCode"))
                .ReturnsAsync((GeoInformationModel)null); // Return null for geo info

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => weatherService.GetWeatherInformationByZipCode("Invalid ZipCode"));
        }

        private void AssertWeatherModel(WeatherModel result)
        {
            Assert.Equal(expected.Temperature, result.Temperature);
            Assert.Equal(expected.FeelsLikeTemperature, result.FeelsLikeTemperature);
            Assert.Equal(expected.MinimumTemperature, result.MinimumTemperature);
            Assert.Equal(expected.MaximumTemperature, result.MaximumTemperature);
            Assert.Equal(expected.Pressure, result.Pressure);
            Assert.Equal(expected.Humidity, result.Humidity);
            Assert.Equal(expected.WindSpeed, result.WindSpeed);
            Assert.Equal(expected.RainVolume, result.RainVolume);
            Assert.Equal(expected.Location, result.Location);
        }
    }
}
