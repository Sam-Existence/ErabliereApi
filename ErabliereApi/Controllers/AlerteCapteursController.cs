using AutoMapper;
using ErabliereApi.Attributes;
using ErabliereApi.Depot.Sql;
using ErabliereApi.Donnees;
using ErabliereApi.Donnees.Action.Get;
using ErabliereApi.Donnees.Action.Put;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ErabliereApi.Controllers;

/// <summary>
/// Contrôler pour interagir avec les alertes des capteurs
/// </summary>
[ApiController]
[Route("Capteurs/{id}/[controller]")]
[Authorize]
public class AlerteCapteursController : ControllerBase
{
    private readonly ErabliereDbContext _depot;
    private readonly IMapper _mapper;

    /// <summary>
    /// Constructeur par initialisation
    /// </summary>
    public AlerteCapteursController(ErabliereDbContext depot, IMapper mapper)
    {
        _depot = depot;
        _mapper = mapper;
    }

    /// <summary>
    /// Liste les alertes d'un capteur
    /// </summary>
    /// <param name="id">Identifiant du capteur</param>
    /// <param name="token">Jeton d'annulation de la tâche</param>
    /// <param name="additionnalProperties">Propriétés additionnelles à ajouter à la réponse</param>
    /// <remarks>Les valeurs numérique sont en dixième de leurs unitées respective.</remarks>
    /// <response code="200">Une liste d'alerte potentiellement vide.</response>
    [HttpGet]
    [ValiderOwnership("id", typeof(Capteur))]
    public async Task<IEnumerable<AlerteCapteur>> Lister(Guid id, CancellationToken token, [FromQuery] bool additionnalProperties)
    {
        var alertes = await _depot.AlerteCapteurs.AsNoTracking().Where(b => b.IdCapteur == id).ToArrayAsync(token);

        if (additionnalProperties)
        {
            alertes = _mapper.Map<GetAlerteCapteur[]>(alertes);
        }

        return alertes;
    }

    /// <summary>
    /// Lister les alertes des capteurs d'une erablière
    /// </summary>
    /// <param name="id">L'id de l'érablière</param>
    /// <param name="additionnalProperties">Propriétés additionnel</param>
    /// <returns></returns>
    [Route("/Erablieres/{id}/AlertesCapteur")]
    [HttpGet]
    [ValiderOwnership("id")]
    public async Task<IEnumerable<AlerteCapteur>> ListerAlerteCapteurErabliere(Guid id, [FromQuery] bool additionnalProperties)
    {
#nullable disable
        var alertesCapteurs = await _depot.AlerteCapteurs.AsNoTracking().Where(b => b.Capteur.IdErabliere == id).ToArrayAsync();
#nullable enable

        if (additionnalProperties)
        {
            alertesCapteurs = _mapper.Map<GetAlerteCapteur[]>(alertesCapteurs);
        }

        return alertesCapteurs;
    }

    /// <summary>
    /// Ajouter une Alerte.
    /// Séparer les adresse courriel par des ; pour saisir plusieurs adresses.
    /// </summary>
    /// <remarks>Chaque valeur numérique est en dixième. Donc pour représenter 1 degré celcius, il faut inscrire 10.</remarks>
    /// <param name="id">L'identifiant de l'érablière</param>
    /// <param name="alerte">Les paramètres de l'alerte</param>
    /// <param name="token">Jeton d'annulation de la tâche</param>
    /// <response code="200">L'alerte a été correctement ajouter.</response>
    /// <response code="400">L'id de la route ne concorde pas avec l'id de l'alerte à ajouter.</response>
    /// <response code="400">Le capteur n'existe pas</response>
    [HttpPost]
    [ValiderOwnership("id", typeof(Capteur))]
    public async Task<IActionResult> Ajouter(Guid id, AlerteCapteur alerte, CancellationToken token)
    {
        if (id != alerte.IdCapteur)
        {
            return BadRequest("L'id de la route ne concorde pas avec l'id du capteur à ajouter");
        }

        if (await (_depot.Capteurs.FindAsync(new object?[] { id }, cancellationToken: token)) == null)
        {
            return BadRequest("Le capteur n'existe pas");
        }

        if (!alerte.DC.HasValue)
        {
            alerte.DC = DateTime.Now;
        }

        var entity = await _depot.AlerteCapteurs.AddAsync(alerte, token);

        await _depot.SaveChangesAsync(token);

        return Ok(entity.Entity);
    }

    /// <summary>
    /// Modifier une alerte
    /// </summary>
    /// <param name="id">L'identifiant du capteur</param>
    /// <param name="alerte">L'alerte a modifier</param>
    /// <param name="additionnalProperties">Propriété additionnel, tel que les adresse couriels dans une liste</param>
    /// <param name="token">Jeton d'annulation de la tâche</param>
    /// <response code="200">L'alerte a été correctement supprimé.</response>
    /// <response code="400">L'id de la route ne concorde pas avec l'id du baril à modifier.</response>
    [HttpPut]
    [ValiderOwnership("id", typeof(Capteur))]
    public async Task<IActionResult> Modifier(Guid id, PutAlerteCapteur alerte, bool? additionnalProperties, CancellationToken token)
    {
        if (id != alerte.IdCapteur)
        {
            return BadRequest("L'id de la route ne concorde pas avec l'id de l'alerte à modifier.");
        }

        var entity = _depot.Update(_mapper.Map<AlerteCapteur>(alerte));

        await _depot.SaveChangesAsync(token);

        if (additionnalProperties == true) {
            return Ok(_mapper.Map<GetAlerteCapteur>(entity.Entity));
        }

        return Ok(entity.Entity);
    }

    /// <summary>
    /// Activer une alerte capteur
    /// </summary>
    /// <param name="id">L'identifiant du capteur</param>
    /// <param name="idAlerte">L'id de l'alerte</param>
    /// <param name="alerte">L'alerte a modifier</param>
    /// <param name="token">Jeton d'annulation de la tâche</param>
    /// <response code="200">L'alerte a été correctement supprimé.</response>
    /// <response code="400">L'id de la route ne concorde pas avec l'id du baril à modifier.</response>
    [HttpPut]
    [Route("{idAlerte}/[action]")]
    [ValiderOwnership("id", typeof(Capteur))]
    public async Task<IActionResult> Activer(Guid id, Guid idAlerte, PutAlerteCapteur alerte, CancellationToken token)
    {
        if (id != alerte.IdCapteur)
        {
            return BadRequest("L'id du capteur de la route ne concorde pas avec l'id du capteur de l'alerte à activer.");
        }
        if (idAlerte != alerte.Id)
        {
            return BadRequest("L'id de l'alerte ne concorde pas avec l'id de l'alerte à modifier.");
        }

        var entity = await _depot.AlerteCapteurs.FindAsync(new object?[] { alerte.Id }, cancellationToken: token);

        if (entity is not null && entity.IdCapteur == id)
        {
            entity.IsEnable = true;

            await _depot.SaveChangesAsync(token);

            return Ok();
        }

        return NotFound();
    }

    /// <summary>
    /// Activer une alerte capteur
    /// </summary>
    /// <param name="id">L'identifiant du capteur</param>
    /// <param name="idAlerte">L'id de l'alerte</param>
    /// <param name="alerte">L'alerte a modifier</param>
    /// <param name="token">Jeton d'annulation de la tâche</param>
    /// <response code="200">L'alerte a été correctement supprimé.</response>
    /// <response code="400">L'id de la route ne concorde pas avec l'id du baril à modifier.</response>
    [HttpPut]
    [Route("{idAlerte}/[action]")]
    [ValiderOwnership("id", typeof(Capteur))]
    public async Task<IActionResult> Desactiver(Guid id, Guid idAlerte, PutAlerteCapteur alerte, CancellationToken token)
    {
        if (id != alerte.IdCapteur)
        {
            return BadRequest("L'id du capteur de la route ne concorde pas avec l'id du capteur de l'alerte à désactiver.");
        }
        if (idAlerte != alerte.Id)
        {
            return BadRequest("L'id de l'alerte de la route ne concorde pas avec l'id de l'alerte à désactiver.");
        }

        var entity = await _depot.AlerteCapteurs.FindAsync(new object?[] { idAlerte }, cancellationToken: token);

        if (entity is not null && entity.IdCapteur == id)
        {
            entity.IsEnable = false;

            await _depot.SaveChangesAsync(token);

            return Ok();
        }

        return NotFound();
    }

    /// <summary>
    /// Supprimer une alerte
    /// </summary>
    /// <param name="id">Identifiant du capteur</param>
    /// <param name="alerte">L'alerte à supprimer</param>
    /// <param name="token">Jeton d'annulation de la tâche</param>
    /// <response code="204">L'alerte a été correctement supprimé.</response>
    /// <response code="400">L'id de la route ne concorde pas avec l'id de l'alerte à supprimer.</response>
    [HttpDelete]
    [ValiderOwnership("id", typeof(Capteur))]
    public async Task<IActionResult> Supprimer(Guid id, AlerteCapteur alerte, CancellationToken token)
    {
        if (id != alerte.IdCapteur)
        {
            return BadRequest("L'id de la route ne concorde pas avec l'id de l'alerte à supprimer.");
        }

        _depot.Remove(alerte);

        await _depot.SaveChangesAsync(token);

        return NoContent();
    }

    /// <summary>
    /// Supprimer une alerte
    /// </summary>
    /// <param name="id">Identifiant du capteur</param>
    /// <param name="idAlerte">L'id de l'alerte à supprimer</param>
    /// <param name="token">Jeton d'annulation de la tâche</param>
    /// <response code="204">L'alerte a été correctement supprimé.</response>
    /// <response code="400">L'id de la route ne concorde pas avec l'id de l'alerte à supprimer.</response>
    [HttpDelete("{idAlerte}")]
    [ValiderOwnership("id", typeof(Capteur))]
    public async Task<IActionResult> Supprimer(Guid id, Guid idAlerte, CancellationToken token)
    {
        var alerte = await _depot.AlerteCapteurs.FindAsync(new object?[] { idAlerte }, cancellationToken: token);

        if (alerte == null)
        {
            return NoContent();
        }

        if (id != alerte.IdCapteur)
        {
            return BadRequest("L'id de la route ne concorde pas avec l'id de l'alerte à supprimer.");
        }

        _depot.Remove(alerte);

        await _depot.SaveChangesAsync(token);

        return NoContent();
    }
}
