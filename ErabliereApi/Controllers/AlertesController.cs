using ErabliereApi.Depot.Sql;
using ErabliereApi.Donnees;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ErabliereApi.Controllers;

/// <summary>
/// Contrôler pour interagir avec les alertes
/// </summary>
[ApiController]
[Route("Erablieres/{id}/[controller]")]
[Authorize]
public class AlertesController : ControllerBase
{
    private readonly ErabliereDbContext _depot;

    /// <summary>
    /// Constructeur par initialisation
    /// </summary>
    /// <param name="depot"></param>
    public AlertesController(ErabliereDbContext depot)
    {
        _depot = depot;
    }

    /// <summary>
    /// Liste les Alertes
    /// </summary>
    /// <param name="id">Identifiant de l'érablière</param>
    /// <param name="token">Jeton d'annulation de la tâche</param>
    /// <remarks>Les valeurs numérique sont en dixième de leurs unitées respective.</remarks>
    /// <response code="200">Une liste d'alerte potentiellement vide.</response>
    [HttpGet]
    public async Task<IEnumerable<Alerte>> Lister(Guid id, CancellationToken token)
    {
        return await _depot.Alertes.AsNoTracking().Where(b => b.IdErabliere == id).ToArrayAsync(token);
    }

    /// <summary>
    /// Ajouter une Alerte
    /// </summary>
    /// <remarks>Chaque valeur numérique est en dixième. Donc pour représenter 1 degré celcius, il faut inscrire 10.</remarks>
    /// <param name="id">L'identifiant de l'érablière</param>
    /// <param name="alerte">Les paramètres de l'alerte</param>
    /// <param name="token">Jeton d'annulation de la tâche</param>
    /// <response code="200">L'alerte a été correctement ajouter.</response>
    /// <response code="400">L'id de la route ne concorde pas avec l'id de l'alerte à ajouter.</response>
    [HttpPost]
    public async Task<IActionResult> Ajouter(Guid id, Alerte alerte, CancellationToken token)
    {
        if (id != alerte.IdErabliere)
        {
            return BadRequest("L'id de la route ne concorde pas avec l'id de l'alerte à ajouter");
        }

        var entity = await _depot.Alertes.AddAsync(alerte, token);

        await _depot.SaveChangesAsync(token);

        return Ok(entity.Entity);
    }

    /// <summary>
    /// Modifier une alerte
    /// </summary>
    /// <param name="id">L'identifiant de l'érablière</param>
    /// <param name="alerte">L'alerte a modifier</param>
    /// <param name="token">Jeton d'annulation de la tâche</param>
    /// <response code="200">L'alerte a été correctement supprimé.</response>
    /// <response code="400">L'id de la route ne concorde pas avec l'id du baril à modifier.</response>
    [HttpPut]
    public async Task<IActionResult> Modifier(Guid id, Alerte alerte, CancellationToken token)
    {
        if (id != alerte.IdErabliere)
        {
            return BadRequest("L'id de la route ne concorde pas avec l'id de l'alerte à modifier.");
        }

        var entity = _depot.Update(alerte);

        await _depot.SaveChangesAsync(token);

        return Ok(entity.Entity);
    }

    /// <summary>
    /// Activer une alerte
    /// </summary>
    /// <param name="id">L'id de l'érablière</param>
    /// <param name="alerte">L'id de l'alerte</param>
    /// <param name="token">Le jeton d'annulation</param>
    /// <returns>Ok dans le cas d'un succès. NotFound avec l'alerte reçu qui n'a pas été trouvé.</returns>
    [HttpPut]
    public async Task<IActionResult> Activer(Guid id, Alerte alerte, CancellationToken token)
    {
        if (id != alerte.IdErabliere)
        {
            return BadRequest("L'id de la route ne concorde pas avec l'id de l'alerte à activer.");
        }

        var entity = await _depot.Alertes.FindAsync(new object?[] { alerte.Id }, cancellationToken: token);

        if (entity is not null)
        {
            entity.IsEnable = true;

            await _depot.SaveChangesAsync(token);

            return Ok();
        }

        return NotFound(alerte);
    }

    /// <summary>
    /// Désactiver une alerte
    /// </summary>
    /// <param name="id">L'id de l'érablière</param>
    /// <param name="alerte">L'id de l'alerte</param>
    /// <param name="token">Le jeton d'annulation</param>
    /// <returns>Ok dans le cas d'un succès. NotFound avec l'alerte reçu qui n'a pas été trouvé.</returns>
    [HttpPut]
    public async Task<IActionResult> Desactiver(Guid id, Alerte alerte, CancellationToken token)
    {
        if (id != alerte.IdErabliere)
        {
            return BadRequest("L'id de la route ne concorde pas avec l'id de l'alerte à désactiver.");
        }

        var entity = await _depot.Alertes.FindAsync(new object?[] { alerte.Id }, cancellationToken: token);

        if (entity is not null)
        {
            entity.IsEnable = false;

            await _depot.SaveChangesAsync(token);

            return Ok();
        }

        return NotFound(alerte);
    }

    /// <summary>
    /// Supprimer une alerte
    /// </summary>
    /// <param name="id">Identifiant de l'érablière</param>
    /// <param name="alerte">L'alerte à supprimer</param>
    /// <param name="token">Jeton d'annulation de la tâche</param>
    /// <response code="204">L'alerte a été correctement supprimé.</response>
    /// <response code="400">L'id de la route ne concorde pas avec l'id de l'alerte à supprimer.</response>
    [HttpDelete]
    public async Task<IActionResult> Supprimer(Guid id, Alerte alerte, CancellationToken token)
    {
        if (id != alerte.IdErabliere)
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
    /// <param name="id">Identifiant de l'érablière</param>
    /// <param name="idAlerte">L'id de l'alerte à supprimer</param>
    /// <param name="token">Jeton d'annulation de la tâche</param>
    /// <response code="204">L'alerte a été correctement supprimé.</response>
    /// <response code="400">L'id de la route ne concorde pas avec l'id de l'alerte à supprimer.</response>
    [HttpDelete("{idAlerte}")]
    public async Task<IActionResult> Supprimer(Guid id, Guid idAlerte, CancellationToken token)
    {
        var alerte = await _depot.Alertes.FindAsync(new object?[] { idAlerte }, cancellationToken: token);

        if (alerte == null)
        {
            return NoContent();
        }

        if (id != alerte.IdErabliere)
        {
            return BadRequest("L'id de la route ne concorde pas avec l'id de l'alerte à supprimer.");
        }

        _depot.Remove(alerte);

        await _depot.SaveChangesAsync(token);

        return NoContent();
    }
}
