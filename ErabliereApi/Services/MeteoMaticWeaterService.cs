
using ErabliereApi.Services.MeteoMaticModels;

namespace ErabliereApi.Services;

public class MeteoMaticWeaterService : IWeaterService
{
    public async ValueTask<string> GetLocationCodeAsync(string postalCode)
    {
        using var http = new HttpClient();

        var response = await http.GetFromJsonAsync<MeteoMaticLocationResponse>($"https://geocoder.meteomatics.com/api/v1/geocoder/direct/?location={postalCode}&language=en&limit=8");

        var geo = response?.result.FirstOrDefault()?.geometry;

        if (geo == null)
        {
            return "";
        }

        return $"{geo.lat},{geo.lng}";
    }

    public ValueTask<object?> GetWeatherForecastAsync(string location, string lang)
    {
        throw new NotImplementedException();
    }
}
