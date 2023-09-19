using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WeatherApp.Models;
using WeatherApp.Service.Interfaces;

namespace WeatherApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IWeatherService _weatherService;

        public HomeController(IWeatherService weatherService)
        {
            _weatherService = weatherService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> CityName(CityModel city)
        {
            if (IsModelStateNotValid())
            {
                return View("Index");
            }
            try
            {
                var weather = await _weatherService.GetWeatherInformationByCity(city.Name);
                return View("WeatherInformation", weather);
            }
            catch (ArgumentException)
            {
                ModelState.AddModelError("City", "Not found");
                return View("Index");
            }
        }

        public async Task<IActionResult> ZipCode(ZipCodeModel zipCode)
        {
            if (IsModelStateNotValid())
            {
                return View("Index");
            }

            try
            {
                var weather = await _weatherService.GetWeatherInformationByZipCode(zipCode.Code);
                return View("WeatherInformation", weather);
            }
            catch (ArgumentException)
            {
                ModelState.AddModelError("ZipCode", "Not found");
                return View("Index");
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private bool IsModelStateNotValid()
        {
            return !ModelState.IsValid;
        }
    }
}