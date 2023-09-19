namespace WeatherApp.Service.Interfaces
{
    public interface IOpenWeatherMapApiService
    {
        Task<string> SendGetRequest(string locationWithoutApiKey);
        Task<T> SendGetRequestAndParse<T>(string locationWithoutApiKey);
    }
}