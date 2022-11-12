using AutoMapper;
using AutoMapper.QueryableExtensions;
using ErabliereApi.Attributes;
using ErabliereApi.Depot.Sql;
using ErabliereApi.Donnees;
using ErabliereApi.Donnees.Action.Delete;
using ErabliereApi.Donnees.Action.Get;
using ErabliereApi.Donnees.Action.Post;
using ErabliereApi.Donnees.Action.Put;
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
public class CapteursController : ControllerBase
{
    private readonly ErabliereDbContext _depot;
    private readonly IMapper _mapper;

    /// <summary>
    /// Constructeur par initialisation
    /// </summary>
    /// <param name="depot">Le dépôt des barils</param>
    /// <param name="mapper">Interface de mapping entre les données</param>
    public CapteursController(ErabliereDbContext depot, IMapper mapper)
    {
        _depot = depot;
        _mapper = mapper;
    }

    /// <summary>
    /// Liste les capteurs
    /// </summary>
    /// <param name="id">Identifiant de l'érablière</param>
    /// <param name="filtreNom">Permet de filtrer les capteurs recherché selon leur nom</param>
    /// <param name="token">Le jeton d'annulation</param>
    /// <response code="200">Une liste de capteurs.</response>
    [HttpGet]
    [ValiderOwnership("id")]
    public async Task<IEnumerable<GetCapteurs>> Lister(Guid id, string? filtreNom, CancellationToken token)
    {
        return await _depot.Capteurs.AsNoTracking()
                            .Where(b => b.IdErabliere == id &&
                                    (filtreNom == null || (b.Nom != null && b.Nom.Contains(filtreNom) == true)))
                            .ProjectTo<GetCapteurs>(_mapper.ConfigurationProvider)
                            .ToArrayAsync(token);
    }

    /// <summary>
    /// Obtenir un capteur
    /// </summary>
    /// <param name="id">Identifiant de l'érablière</param>
    /// <param name="idCapteur">Id du capteur présent dans la route</param>
    /// <param name="token">Le jeton d'annulation</param>
    /// <response code="200">Une liste de capteurs.</response>
    [HttpGet("{idCapteur}")]
    [ProducesResponseType(200, Type = typeof(GetCapteurs))]
    [ValiderOwnership("id")]
    public async Task<IActionResult> Lister(Guid id, Guid idCapteur, CancellationToken token)
    {
        var capteur = await _depot.Capteurs.AsNoTracking()
                            .Where(b => b.IdErabliere == id && b.Id == idCapteur)
                            .ProjectTo<GetCapteurs>(_mapper.ConfigurationProvider)
                            .FirstOrDefaultAsync(token);

        if (capteur == null)
        {
            return NotFound();
        }

        return Ok(capteur);
    }

    /// <summary>
    /// Ajouter un capteur
    /// </summary>
    /// <param name="id">L'identifiant de l'érablière</param>
    /// <param name="capteur">Le capteur a ajouter</param>
    /// <param name="token">Le jeton d'annulation de la requête</param>
    /// <response code="200">Le capteur a été correctement ajouté.</response>
    /// <response code="400">L'id de la route ne concorde pas avec l'id du capteur à ajouter.</response>
    [HttpPost]
    [ValiderOwnership("id")]
    public async Task<IActionResult> Ajouter(Guid id, PostCapteur capteur, CancellationToken token)
    {
        if (id != capteur.IdErabliere)
        {
            return BadRequest("L'id de la route n'est pas le même que l'id de l'érablière dans les données du capteur à ajouter");
        }

        if (capteur.Id != null && await _depot.Capteurs.AnyAsync(c => c.Id == capteur.Id))
        {
            return BadRequest($"Le capteur avec l'id '{capteur.Id}' exite déjà");
        }

        if (capteur.DC == null)
        {
            capteur.DC = DateTimeOffset.Now;
        }

        var entity = await _depot.Capteurs.AddAsync(_mapper.Map<Capteur>(capteur), token);

        await _depot.SaveChangesAsync(token);

        return Ok(new { id = entity.Entity.Id });
    }

    /// <summary>
    /// Modifier un capteur
    /// </summary>
    /// <param name="id">L'identifiant de l'érablière</param>
    /// <param name="capteur">Le capteur a modifier</param>
    /// <param name="token">Le jeton d'annulation</param>
    /// <response code="200">Le capteur a été correctement supprimé.</response>
    /// <response code="400">L'id de la route ne concorde pas avec l'id du capteur à modifier.</response>
    [HttpPut]
    [ValiderOwnership("id")]
    public async Task<IActionResult> Modifier(Guid id, PutCapteur capteur, CancellationToken token)
    {
        if (id != capteur.IdErabliere)
        {
            return BadRequest("L'id de la route ne concorde pas avec l'id de l'érablière possédant le capteur à modifier.");
        }

        var capteurEntity = await _depot.Capteurs.FindAsync(capteur.Id);

        if (capteurEntity == null || capteurEntity.IdErabliere != capteur.IdErabliere)
        {
            return BadRequest("Le capteur à modifier n'existe pas.");
        }

        if (capteur.AfficherCapteurDashboard.HasValue)
        {
            capteurEntity.AfficherCapteurDashboard = capteur.AfficherCapteurDashboard ?? false;
        }

        if (capteur.AjouterDonneeDepuisInterface.HasValue)
        {
            capteurEntity.AjouterDonneeDepuisInterface = capteur.AjouterDonneeDepuisInterface.Value;
        }

        if (capteur.DC.HasValue)
        {
            capteurEntity.DC = capteur.DC;
        }

        if (string.IsNullOrWhiteSpace(capteur.Nom) == false)
        {
            capteurEntity.Nom = capteur.Nom;
        }

        _depot.Update(capteurEntity);

        await _depot.SaveChangesAsync(token);

        return Ok();
    }

    /// <summary>
    /// Supprimer un capteur
    /// </summary>
    /// <param name="id">Identifiant de l'érablière</param>
    /// <param name="capteur">Le capteur a supprimer</param>
    /// <param name="token">Le jeton d'annulation</param>
    /// <response code="202">Le capteur a été correctement supprimé.</response>
    /// <response code="400">L'id de la route ne concorde pas avec l'id du capteur à supprimer.</response>
    [HttpDelete]
    [ValiderOwnership("id")]
    public async Task<IActionResult> Supprimer(Guid id, DeleteCapteur capteur, CancellationToken token)
    {
        if (id != capteur.IdErabliere)
        {
            return BadRequest("L'id de la route ne concorde pas avec l'id du baril à supprimer.");
        }

        var capteurEntity = await _depot.Capteurs
            .FirstOrDefaultAsync(x => x.Id == capteur.Id && x.IdErabliere == capteur.IdErabliere, token);

        if (capteurEntity != null)
        {
            _depot.Remove(capteurEntity);

            await _depot.SaveChangesAsync(token);
        }

        return NoContent();
    }
}
