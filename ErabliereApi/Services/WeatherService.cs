using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;

/// <summary>
/// Service pour interagir avec les prévisions météo
/// </summary>
public class WeatherService
{
    private readonly HttpClient _httpClient;
    private readonly IDistributedCache _cache;
    private readonly ILogger<WeatherService> _logger;
    private readonly string? AccuWeatherApiKey;

    /// <summary>
    /// Constructeur
    /// </summary>
    public WeatherService(
        IDistributedCache memoryCache,
        ILogger<WeatherService> logger,
        IConfiguration configuration)
    {
        _httpClient = new HttpClient();
        _cache = memoryCache;
        _logger = logger;
        AccuWeatherApiKey = configuration["AccuWeatherApiKey"];
    }

    /// <summary>
    /// Obtenir le code de localisation à partir d'un code postal
    /// </summary>
    public async ValueTask<string> GetLocationCodeAsync(string postalCode)
    {
        var cacheKey = $"WeatherService.PostalCode.{postalCode}";
        var locationCode = await _cache.GetStringAsync(cacheKey);
        if (!string.IsNullOrWhiteSpace(locationCode))
        {
            return locationCode;
        }

        try
        {
            string url = $"http://dataservice.accuweather.com/locations/v1/postalcodes/search?apikey={AccuWeatherApiKey}&q={postalCode}";

            HttpResponseMessage response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var responseBody = JsonSerializer.Deserialize<List<LocationResponse>>(await response.Content.ReadAsStringAsync());
            
            if (responseBody == null || responseBody.Count == 0)
            {
                return "";
            }

            var locationKey = responseBody[0].Key ?? "";

            await _cache.SetStringAsync(cacheKey, locationKey);

            return locationKey;
        }
        catch (Exception ex)
        {
            _logger.LogCritical($"Error retrieving location code: {ex.Message}");
            return "";
        }
    }

    /// <summary>
    /// Obtenir les prévisions météo à partir d'un code de localisation
    /// </summary>
    public async ValueTask<object?> GetWeatherForecastAsync(string location)
    {
        var cacheKey = $"WeatherService.GetWeatherForecast.{location}";
        var cacheValue = await _cache.GetStringAsync(cacheKey);
        if (!string.IsNullOrWhiteSpace(cacheValue))
        {
            var res = JsonSerializer.Deserialize(cacheValue, typeof(object));
            return res;
        }

        try
        {
            string url = $"http://dataservice.accuweather.com/forecasts/v1/daily/5day/{location}?apikey={AccuWeatherApiKey}";

            HttpResponseMessage response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadAsStringAsync();

            await _cache.SetStringAsync(cacheKey , responseBody, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
            });

            var res = JsonSerializer.Deserialize(responseBody, typeof(object));
            return res;
        }
        catch (Exception ex)
        {
            _logger.LogCritical($"Error retrieving weather forecast: {ex.Message}");
            return "";
        }
    }
}
