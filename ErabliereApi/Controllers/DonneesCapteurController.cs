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
using System.Globalization;

namespace ErabliereApi.Controllers;

/// <summary>
/// Contrôler représentant les données des dompeux
/// </summary>
[ApiController]
[Route("Capteurs/{id}/[controller]")]
[Authorize]
public class DonneesCapteurController : ControllerBase
{
    private readonly ErabliereDbContext _depot;
    private readonly IMapper _mapper;

    /// <summary>
    /// Constructeur par initialisation
    /// </summary>
    /// <param name="depot">Le dépôt des barils</param>
    /// <param name="mapper">Interface de mapping entre les objets</param>
    public DonneesCapteurController(ErabliereDbContext depot, IMapper mapper)
    {
        _depot = depot;
        _mapper = mapper;
    }

    /// <summary>
    /// Liste les données d'un capteur
    /// </summary>
    /// <param name="id">Identifiant du capteur</param>
    /// <param name="ddr">Date de la dernière données reçu. Permet au client d'optimiser le nombres de données reçu.</param>
    /// <param name="dd">Date de début</param>
    /// <param name="df">Date de fin</param>
    /// <response code="200">Une liste de DonneesCapteur.</response>
    [HttpGet]
    [ValiderOwnership("id", typeof(Capteur))]
    public async Task<IEnumerable<GetDonneesCapteur>> Lister(Guid id,
                                                             [FromHeader(Name = "x-ddr")] DateTimeOffset? ddr,
                                                             DateTimeOffset? dd,
                                                             DateTimeOffset? df)
    {
        var donnees = await _depot.DonneesCapteur.AsNoTracking()
                            .Where(b => b.IdCapteur == id &&
                                        (ddr == null || b.D >= ddr) &&
                                        (dd == null || b.D >= dd) &&
                                        (df == null || b.D <= df))
                            .OrderBy(b => b.D)
                            .ProjectTo<GetDonneesCapteur>(_mapper.ConfigurationProvider)
                            .ToArrayAsync();

        if (donnees.Length > 0)
        {
            if (ddr.HasValue)
            {
                HttpContext.Response.Headers.Append("x-ddr", ddr.Value.ToString("s", CultureInfo.InvariantCulture));
            }

            if (donnees[^1].D.HasValue)
            {
                HttpContext.Response.Headers.Append("x-dde", donnees[^1].D!.Value.ToString("s", CultureInfo.InvariantCulture));
            }
        }

        return donnees;
    }

    /// <summary>
    /// Liste les données de plusieurs capteurs capteurs
    /// </summary>
    /// <param name="ids">Identifiant des capteurs</param>
    /// <param name="ddr">Date de la dernière données reçu. Permet au client d'optimiser le nombres de données reçu.</param>
    /// <param name="dd">Date de début</param>
    /// <param name="df">Date de fin</param>
    /// <response code="200">Une liste Tupple avec l'id du catpeur et la liste des DonneesCapteur.</response>
    [HttpGet]
    [Route("/DonneesCapteur/Grape")]
    [ValiderOwnership("id", typeof(Capteur))]
    public async IAsyncEnumerable<Pair<Guid, IEnumerable<GetDonneesCapteur>>> ListerPlusieurs(
                                                [FromQuery] string ids,
                                                [FromHeader(Name = "x-ddr")] DateTimeOffset? ddr,
                                                DateTimeOffset? dd,
                                                DateTimeOffset? df)
    {
        foreach (var idstr in ids.Split(';'))
        {
            var id = Guid.Parse(idstr);

            yield return new Pair<Guid, IEnumerable<GetDonneesCapteur>>(id, await Lister(id, ddr, dd, df));
        }
    }

    /// <summary>
    /// Ajouter une données d'un capteur
    /// </summary>
    /// <param name="id">L'identifiant du capteurs</param>
    /// <param name="donneeCapteur">Le capteur a ajouter</param>
    /// <response code="200">Le capteur a été correctement ajouté.</response>
    /// <response code="400">L'id de la route ne concorde pas avec l'id du capteur à ajouter.</response>
    [HttpPost]
    [TriggerAlertV2]
    [ValiderOwnership("id", typeof(Capteur))]
    public async Task<IActionResult> Ajouter(Guid id, PostDonneeCapteur donneeCapteur)
    {
        if (id != donneeCapteur.IdCapteur)
        {
            return BadRequest("L'id de la route ne concorde pas avec l'id du capteur à ajouter");
        }

        if (donneeCapteur.D == null)
        {
            donneeCapteur.D = DateTimeOffset.Now;
        }

        await _depot.DonneesCapteur.AddAsync(_mapper.Map<DonneeCapteur>(donneeCapteur));

        await _depot.SaveChangesAsync();

        return Ok();
    }

    /// <summary>
    /// Modifier une données d'un capteur
    /// </summary>
    /// <param name="id">L'identifiant du capteur</param>
    /// <param name="capteur">Le capteur a modifier</param>
    /// <response code="200">Le capteur a été correctement supprimé.</response>
    /// <response code="400">L'id de la route ne concorde pas avec l'id du capteur à modifier.</response>
    [HttpPut]
    [ValiderOwnership("id", typeof(Capteur))]
    public async Task<IActionResult> Modifier(Guid id, DonneeCapteur capteur)
    {
        if (id != capteur.IdCapteur)
        {
            return BadRequest("L'id de la route ne concorde pas avec l'id du capteur à modifier.");
        }

        _depot.Update(capteur);

        await _depot.SaveChangesAsync();

        return Ok();
    }

    /// <summary>
    /// Supprimer une données d'un capteur
    /// </summary>
    /// <param name="id">Identifiant du capteur</param>
    /// <param name="capteur">Le capteur a supprimer</param>
    /// <response code="202">Le capteur a été correctement supprimé.</response>
    /// <response code="400">L'id de la route ne concorde pas avec l'id du capteur à supprimer.</response>
    [HttpDelete]
    [ValiderOwnership("id", typeof(Capteur))]
    public async Task<IActionResult> Supprimer(Guid id, DonneeCapteur capteur)
    {
        if (id != capteur.IdCapteur)
        {
            return BadRequest("L'id de la route ne concorde pas avec l'id du baril à supprimer.");
        }

        _depot.Remove(capteur);

        await _depot.SaveChangesAsync();

        return NoContent();
    }
}
