using WeatherApp.Service;
using WeatherApp.Service.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IWeatherService, WeatherService>();
builder.Services.AddScoped<IGeoInformationService, GeoInformationService>();
builder.Services.AddScoped<IOpenWeatherMapApiService, OpenWeatherMapApiService>();
builder.Services.AddHttpClient("OpenWeather",config =>
{
    const string openWeatherUrl = "https://api.openweathermap.org";

    config.BaseAddress = new Uri(openWeatherUrl);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
