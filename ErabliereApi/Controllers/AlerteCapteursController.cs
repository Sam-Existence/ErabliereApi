﻿using ErabliereApi.Depot.Sql;
using ErabliereApi.Donnees;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

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

    /// <summary>
    /// Constructeur par initialisation
    /// </summary>
    /// <param name="depot"></param>
    public AlerteCapteursController(ErabliereDbContext depot)
    {
        _depot = depot;
    }

    /// <summary>
    /// Liste les alertes d'un capteur
    /// </summary>
    /// <param name="id">Identifiant de l'érablière</param>
    /// <param name="token">Jeton d'annulation de la tâche</param>
    /// <remarks>Les valeurs numérique sont en dixième de leurs unitées respective.</remarks>
    /// <response code="200">Une liste d'alerte potentiellement vide.</response>
    [HttpGet]
    public async Task<IEnumerable<AlerteCapteur>> Lister(Guid id, CancellationToken token)
    {
        return await _depot.AlerteCapteurs.AsNoTracking().Where(b => b.IdCapteur == id).ToArrayAsync(token);
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
    /// <response code="400">Le capteur n'existe pas</response>
    [HttpPost]
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

        var entity = await _depot.AlerteCapteurs.AddAsync(alerte, token);

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
    public async Task<IActionResult> Modifier(Guid id, AlerteCapteur alerte, CancellationToken token)
    {
        if (id != alerte.IdCapteur)
        {
            return BadRequest("L'id de la route ne concorde pas avec l'id de l'alerte à modifier.");
        }

        var entity = _depot.Update(alerte);

        await _depot.SaveChangesAsync(token);

        return Ok(entity.Entity);
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
    /// <param name="id">Identifiant de l'érablière</param>
    /// <param name="idAlerte">L'id de l'alerte à supprimer</param>
    /// <param name="token">Jeton d'annulation de la tâche</param>
    /// <response code="204">L'alerte a été correctement supprimé.</response>
    /// <response code="400">L'id de la route ne concorde pas avec l'id de l'alerte à supprimer.</response>
    [HttpDelete("{idAlerte}")]
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