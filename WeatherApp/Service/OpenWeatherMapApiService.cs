using WeatherApp.Service.Interfaces;

namespace WeatherApp.Service
{
    public class OpenWeatherMapApiService : IOpenWeatherMapApiService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string? _apiKey;

        public OpenWeatherMapApiService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _apiKey = configuration["OpenWeatherMapApiKey"];
        }

        public async Task<T> SendGetRequestAndParse<T>(string locationWithoutApiKey)
        {
            var responce = await GetResponce(locationWithoutApiKey);
            if(responce.IsSuccessStatusCode)
            {
                var geoInformation = await responce.Content.ReadFromJsonAsync<T>();
                return geoInformation ?? throw new ArgumentException("No information found.");
            }
            else
            {
                throw new ArgumentException("No information found.");
            }
        }

        public async Task<string> SendGetRequest(string locationWithoutApiKey)
        {
            var responce = await GetResponce(locationWithoutApiKey);
            if (responce.IsSuccessStatusCode)
            {
                var geoInformation = await responce.Content.ReadAsStringAsync();
                return geoInformation ?? throw new ArgumentException("No information found.");
            }
            else
            {
                throw new ArgumentException("No information found.");
            }
        }

        private async Task<HttpResponseMessage> GetResponce(string locationWithoutApiKey)
        {
            using var client = _httpClientFactory.CreateClient("OpenWeather");
            var responce = await client.GetAsync($"{locationWithoutApiKey}&appid={_apiKey}");
            return responce;
        }
    }
}
