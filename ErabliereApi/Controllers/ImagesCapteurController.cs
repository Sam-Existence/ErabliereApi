using AutoMapper;
using AutoMapper.QueryableExtensions;
using ErabliereApi.Attributes;
using ErabliereApi.Controllers.Attributes;
using ErabliereApi.Depot.Sql;
using ErabliereApi.Donnees;
using ErabliereApi.Donnees.Action.Post;
using ErabliereApi.Donnees.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ErabliereApi.Controllers;

/// <summary>
/// Contrôler représentant les données des dompeux
/// </summary>
[ApiController]
[Route("Erablieres/{id}/[controller]")]
[Authorize]
public class ImagesCapteurController : ControllerBase
{
    private readonly IConfiguration _config;

    /// <summary>
    /// Constructeur par initialisation
    /// </summary>
    public ImagesCapteurController(IConfiguration config)
    {
        _config = config;
    }

    /// <summary>
    /// Liste les données d'un capteur
    /// </summary>
    /// <param name="id">Identifiant de l'érablière</param>
    /// <param name="take">Nombre de données à prendre</param>
    /// <param name="skip">Nombre de données à sauter</param>
    /// <param name="token">Token d'annulation</param>
    /// <response code="200">Une liste de DonneesCapteur.</response>
    [HttpGet]
    [ValiderOwnership("id")]
    public async Task<IActionResult> Lister([FromRoute] Guid? id,
                                            [FromQuery] int? take,
                                            [FromQuery] int? skip,
                                                        CancellationToken token)
    {
        using var client = new HttpClient();

        var baseUrl = _config["EmailImageObserverUrl"];

        var route = $"{baseUrl}/api/image?ownerId={id}";

        if (take.HasValue)
        {
            route += $"&take={take}";
        }

        if (skip.HasValue)
        {
            route += $"&skip={skip}";
        }

        var response = await client.GetAsync(route, token);

        var obj = await response.Content.ReadFromJsonAsync(typeof(object), token);

        return Ok(obj);
    }
}
