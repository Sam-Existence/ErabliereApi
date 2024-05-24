using ErabliereApi.Depot.Sql;
using ErabliereApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ErabliereApi.Controllers;

/// <summary>
/// Contrôler pour interagir avec les alertes des capteurs
/// </summary>
[ApiController]
[Route("Erablieres/{id}/[controller]")]
[Authorize]
public class WeatherForecastController
{
    private readonly ErabliereDbContext _context;
    private readonly WeatherService _weatherService;

    /// <summary>
    /// Constructeur
    /// </summary>
    public WeatherForecastController(ErabliereDbContext context, WeatherService weatherService)
    {
        _context = context;
        _weatherService = weatherService;
    }

    /// <summary>
    /// Obtenir les prévisions météo pour une érablière
    /// </summary>
    /// <param name="id">Identifiant de l'érablière</param>
    /// <param name="lang">Paramètre de langue, fr-ca par défaut.</param>
    /// <returns>Prévisions météo</returns>
    /// <response code="200">Prévisions météo</response>
    /// <response code="401">Non autorisé</response>
    /// <response code="404">Érablière non trouvée</response>
    /// <response code="500">Erreur interne du serveur</response>
    [HttpGet]
    [ProducesResponseType(200, Type = typeof(WeatherForecastResponse))]
    public async Task<IActionResult> GetWeatherForecast(Guid id, string lang = "fr-ca")
    {
        // Résoudre l'érablière
        var erabliere = await _context.Erabliere.FindAsync(id);

        // Vérifier si l'érablière existe
        if (erabliere == null)
        {
            return new NotFoundResult();
        }

        if (string.IsNullOrWhiteSpace(erabliere.CodePostal))
        {
            return new BadRequestObjectResult("L'érablière n'a pas de code postal");
        }

        var locationCode = await _weatherService.GetLocationCodeAsync(erabliere.CodePostal);

        if (string.IsNullOrWhiteSpace(locationCode))
        {
            return new BadRequestObjectResult("Impossible de trouver le code de localisation");
        }

        var weatherForecast = await _weatherService.GetWeatherForecastAsync(locationCode, lang);

        if (weatherForecast == null)
        {
            return new BadRequestObjectResult("Impossible de trouver les prévisions météo");
        }

        return new OkObjectResult(weatherForecast);
    }

    /// <summary>
    /// Obtenir les prévisions météo pour les prochaines heures
    /// </summary>
    /// <param name="id">Identifiant de l'érablière</param>
    /// <param name="lang">Paramètre de langue, fr-ca par défaut.</param>
    /// <returns>Prévisions météo</returns>
    /// <response code="200">Prévisions météo</response>
    /// <response code="401">Non autorisé</response>
    /// <response code="404">Érablière non trouvée</response>
    [HttpGet("Hourly")]
    [ProducesResponseType(200)]
    public async Task<IActionResult> GetHourlyWeatherForecast(Guid id, string lang = "fr-ca")
    {
        // Résoudre l'érablière
        var erabliere = await _context.Erabliere.FindAsync(id);

        // Vérifier si l'érablière existe
        if (erabliere == null)
        {
            return new NotFoundResult();
        }

        if (string.IsNullOrWhiteSpace(erabliere.CodePostal))
        {
            return new BadRequestObjectResult("L'érablière n'a pas de code postal");
        }

        var locationCode = await _weatherService.GetLocationCodeAsync(erabliere.CodePostal);

        if (string.IsNullOrWhiteSpace(locationCode))
        {
            return new BadRequestObjectResult("Impossible de trouver le code de localisation");
        }

        var weatherForecast = await _weatherService.GetHoulyForecastAsync(locationCode, lang);

        if (weatherForecast == null)
        {
            return new BadRequestObjectResult("Impossible de trouver les prévisions météo");
        }

        return new OkObjectResult(weatherForecast);
    }
}
