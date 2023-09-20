using Moq;
using WeatherApp.Models;
using WeatherApp.Service;
using WeatherApp.Service.Interfaces;

namespace WeatherApp.Tests.Services
{
    public class GeoInformationServiceTests
    {
        private readonly GeoInformationService _geoInformationService;
        private readonly Mock<IOpenWeatherMapApiService> _openWeatherMapApiServiceMock;

        public GeoInformationServiceTests()
        {
            _openWeatherMapApiServiceMock = new Mock<IOpenWeatherMapApiService>();
            _geoInformationService = new GeoInformationService(_openWeatherMapApiServiceMock.Object);
        }

        [Fact]
        public async Task GetGeoInformationByCity_ReturnsValidGeoInformationModel()
        {
            // Arrange
            var expected = new GeoInformationModel
            {
                Lat = 42.3601,
                Lon = -71.0589,
            };

            _openWeatherMapApiServiceMock
                .Setup(x => x.SendGetRequestAndParse<List<GeoInformationModel>>("/geo/1.0/direct?q=Boston"))
                .ReturnsAsync(new List<GeoInformationModel> { expected });

            // Act
            var result = await _geoInformationService.GetGeoInformationByCity("Boston");

            // Assert
            Assert.Equal(expected.Lat, result.Lat);
            Assert.Equal(expected.Lon, result.Lon);
        }

        [Fact]
        public async Task GetGeoInformationByZipCode_ReturnsValidGeoInformationModel()
        {
            // Arrange
            var expected = new GeoInformationModel
            {
                Lat = 42.3601,
                Lon = -71.0589,
            };

            _openWeatherMapApiServiceMock
                .Setup(x => x.SendGetRequestAndParse<GeoInformationModel>("/geo/1.0/zip?zip=02108"))
                .ReturnsAsync(expected);

            // Act
            var result = await _geoInformationService.GetGeoInformationByZipCode("02108");

            // Assert
            Assert.Equal(expected.Lat, result.Lat);
            Assert.Equal(expected.Lon, result.Lon);
        }
    }
}
