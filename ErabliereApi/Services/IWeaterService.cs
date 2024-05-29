using ErabliereApi.Services.AccuWeatherModels;

namespace ErabliereApi.Services;

public interface IWeaterService
{
    ValueTask<string> GetLocationCodeAsync(string postalCode);
    ValueTask<WeatherForecastResponse?> GetWeatherForecastAsync(string location, string lang);
    ValueTask<HourlyWeatherForecastResponse[]?> GetHoulyForecastAsync(string location, string lang);
}
