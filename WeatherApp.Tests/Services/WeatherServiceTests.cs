using Moq;
using WeatherApp.Models;
using WeatherApp.Service.Interfaces;
using WeatherApp.Service;

namespace WeatherApp.Tests.Services
{
    public class WeatherServiceTests
    {
        private readonly WeatherService _weatherService;
        private readonly Mock<IGeoInformationService> _geoInformationServiceMock;
        private readonly Mock<IOpenWeatherMapApiService> _openWeatherMapApiServiceMock;

        public WeatherServiceTests()
        {
            _geoInformationServiceMock = new Mock<IGeoInformationService>();
            _openWeatherMapApiServiceMock = new Mock<IOpenWeatherMapApiService>();
            _weatherService = new WeatherService(_geoInformationServiceMock.Object, _openWeatherMapApiServiceMock.Object);
        }

        [Fact]
        public async Task GetWeatherInformationByCity_ReturnsValidData()
        {
            // Arrange
            var geoInformation = new GeoInformationModel { Lat = 42.3601, Lon = -71.0589 };
            _geoInformationServiceMock.Setup(x => x.GetGeoInformationByCity("Boston")).ReturnsAsync(geoInformation);

            var weatherJson = @"{
            ""coord"":{""lon"":-71.06,""lat"":42.36},
            ""weather"":[{""id"":800,""main"":""Clear"",""description"":""clear sky"",""icon"":""01d""}],
            ""base"":""stations"",
            ""main"":{""temp"":290.25,""feels_like"":288.54,""temp_min"":289.26,""temp_max"":291.48,""pressure"":1025,""humidity"":53},
            ""visibility"":10000,
            ""wind"":{""speed"":4.1,""deg"":330},
            ""clouds"":{""all"":1},
            ""dt"":1633068112,
            ""sys"":{""type"":2,""id"":2009954,""country"":""US"",""sunrise"":1633033126,""sunset"":1633072141},
            ""timezone"":-14400,
            ""id"":4930956,
            ""name"":""Boston"",
            ""cod"":200
        }";
            _openWeatherMapApiServiceMock.Setup(x => x.SendGetRequest($"/data/2.5/weather?lat={geoInformation.Lat}&lon={geoInformation.Lon}")).ReturnsAsync(weatherJson);

            var expected = new WeatherModel
            {
                Temperature = 290.25,
                FeelsLikeTemperature = 288.54,
                MinimumTemperature = 289.26,
                MaximumTemperature = 291.48,
                Pressure = 1025,
                Humidity = 53,
                WindSpeed = 4.1,
                RainVolume = 0,
                Location = "Boston"
            };

            // Act
            var result = await _weatherService.GetWeatherInformationByCity("Boston");

            // Assert
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

        [Fact]
        public async Task GetWeatherInformationByZipCode_ReturnsValidData()
        {
            // Arrange
            var geoInformation = new GeoInformationModel {Lat = 42.3601, Lon = -71.0589 };
            _geoInformationServiceMock.Setup(x => x.GetGeoInformationByZipCode("1000AA")).ReturnsAsync(geoInformation);

            var weatherJson = @"{
            ""coord"":{""lon"":4.9,""lat"":52.38},
            ""weather"":[{""id"":803,""main"":""Clouds"",""description"":""broken clouds"",""icon"":""04d""}],
            ""base"":""stations"",
            ""main"":{""temp"":282.26,""feels_like"":280.56,""temp_min"":281.49,""temp_max"":283.4,""pressure"":1021,""humidity"":87},
            ""visibility"":10000,
            ""wind"":{""speed"":2.57,""deg"":220},
            ""clouds"":{""all"":75},
            ""dt"":1633067024,
            ""sys"":{""type"":2,""id"":2012453,""country"":""NL"",""sunrise"":1633036821,""sunset"":1633075168},
            ""timezone"":7200,
            ""id"":2759794,
            ""name"":""Amsterdam"",
            ""cod"":200
        }";
            _openWeatherMapApiServiceMock.Setup(x => x.SendGetRequest($"/data/2.5/weather?lat={geoInformation.Lat}&lon={geoInformation.Lon}")).ReturnsAsync(weatherJson);

            var expected = new WeatherModel
            {
                Temperature = 282.26,
                FeelsLikeTemperature = 280.56,
                MinimumTemperature = 281.49,
                MaximumTemperature = 283.4,
                Pressure = 1021,
                Humidity = 87,
                WindSpeed = 2.57,
                RainVolume = 0,
                Location = "Amsterdam"
            };

            // Act
            var result = await _weatherService.GetWeatherInformationByZipCode("1000AA");

            // Assert
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

        [Fact]
        public async Task GetWeatherInformation_ThrowsArgumentExceptionWhenCityNotFound()
        {
            // Arrange
            _geoInformationServiceMock.Setup(x => x.GetGeoInformationByCity("NotACity")).ReturnsAsync(() => null);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _weatherService.GetWeatherInformationByCity("NotACity"));
        }

        [Fact]
        public async Task GetWeatherInformation_ThrowsArgumentExceptionWhenZipCodeNotFound()
        {
            // Arrange
            _geoInformationServiceMock.Setup(x => x.GetGeoInformationByZipCode("0000ZZ")).ReturnsAsync(() => null);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _weatherService.GetWeatherInformationByZipCode("0000ZZ"));
        }
    }
}
