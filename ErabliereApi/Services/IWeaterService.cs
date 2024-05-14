namespace ErabliereApi.Services;

public interface IWeaterService
{
    ValueTask<string> GetLocationCodeAsync(string postalCode);
    ValueTask<object?> GetWeatherForecastAsync(string location, string lang);
}
