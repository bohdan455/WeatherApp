using Microsoft.AspNetCore.Mvc;
using Moq;
using WeatherApp.Controllers;
using WeatherApp.Models;
using WeatherApp.Service.Interfaces;

namespace WeatherApp.Tests.Controllers
{
    public class HomeControllerTests
    {
        private readonly Mock<IWeatherService> _weatherServiceMock;
        private readonly HomeController _controller;

        public HomeControllerTests()
        {
            _weatherServiceMock = new Mock<IWeatherService>();
            _controller = new HomeController(_weatherServiceMock.Object);
        }

        [Fact]
        public async Task CityName_ValidModelState_CallsWeatherService()
        {
            // Arrange
            var city = new CityModel { Name = "New York" };

            // Act
            var result = await _controller.CityName(city);

            // Assert
            _weatherServiceMock.Verify(service => service.GetWeatherInformationByCity(city.Name), Times.Once);
            Assert.IsType<ViewResult>(result);
            Assert.Equal("WeatherInformation", (result as ViewResult).ViewName);
        }

        [Fact]
        public async Task ZipCode_ValidModelState_CallsWeatherService()
        {
            // Arrange
            var zipCode = new ZipCodeModel { Code = "12345" };

            // Act
            var result = await _controller.ZipCode(zipCode);

            // Assert
            _weatherServiceMock.Verify(service => service.GetWeatherInformationByZipCode(zipCode.Code), Times.Once);
            Assert.IsType<ViewResult>(result);
            Assert.Equal("WeatherInformation", (result as ViewResult).ViewName);
        }

        [Fact]
        public async Task CityName_InvalidModelState_ReturnsIndexView()
        {
            // Arrange
            var city = new CityModel { Name = "" };
            _controller.ModelState.AddModelError("Name", "Name is required");

            // Act
            var result = await _controller.CityName(city);

            // Assert
            _weatherServiceMock.Verify(service => service.GetWeatherInformationByCity(It.IsAny<string>()), Times.Never);
            Assert.IsType<ViewResult>(result);
            Assert.Equal("Index", (result as ViewResult).ViewName);
        }

        [Fact]
        public async Task ZipCode_InvalidModelState_ReturnsIndexView()
        {
            // Arrange
            var zipCode = new ZipCodeModel { Code = "" };
            _controller.ModelState.AddModelError("Code", "Code is required");

            // Act
            var result = await _controller.ZipCode(zipCode);

            // Assert
            _weatherServiceMock.Verify(service => service.GetWeatherInformationByZipCode(It.IsAny<string>()), Times.Never);
            Assert.IsType<ViewResult>(result);
            Assert.Equal("Index", (result as ViewResult).ViewName);
        }
    }

}
