﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using ErabliereApi.Attributes;
using ErabliereApi.Controllers.Attributes;
using ErabliereApi.Depot.Sql;
using ErabliereApi.Donnees;
using ErabliereApi.Donnees.Action.Get;
using ErabliereApi.Donnees.Action.Post;
using ErabliereApi.Donnees.Action.Put;
using ErabliereApi.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Net.Mime;

namespace ErabliereApi.Controllers;

/// <summary>
/// Contrôler représentant les données reçu par l'automate principale
/// </summary>
[ApiController]
[Route("Erablieres/{id}/[controller]")]
[Authorize]
public class DonneesController : ControllerBase
{
    private readonly ErabliereDbContext _context;
    private readonly IMapper _mapper;

    /// <summary>
    /// Constructeur par initlisation
    /// </summary>
    /// <param name="context">Classe de contexte pour accéder aux données</param>
    /// <param name="mapper">Mapper entre les modèles</param>
    public DonneesController(ErabliereDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    /// <summary>
    /// Lister les données
    /// </summary>
    /// <param name="id">L'identifiant de l'érablière</param>
    /// <param name="dd">Date de début</param>
    /// <param name="df">Date de fin</param>
    /// <param name="q">Quantité de donnée demander</param>
    /// <param name="o">Doit être croissant "c" ou decroissant "d". Par défaut "c"</param>
    /// <param name="ddr">Date de la dernière données reçu. Permet au client d'optimiser le nombres de données reçu.</param>
    /// <response code="200">Retourne une liste de données. La liste est potentiellement vide.</response>
    [ProducesResponseType(200, Type = typeof(GetDonnee))]
    [HttpGet]
    [Produces(MediaTypeNames.Application.Json, MediaTypeNames.Text.Plain, "text/json", "text/csv")]
    [ValiderOwnership("id")]
    public async Task<IActionResult> Lister(Guid id,
                                            [FromHeader(Name = "x-ddr")] DateTimeOffset? ddr,
                                            DateTimeOffset? dd,
                                            DateTimeOffset? df,
                                            int? q,
                                            string? o = "c")
    {
        IEnumerable<GetDonnee> list;

        switch (HttpContext.Request.Headers["Accept"].ToString().ToLowerInvariant())
        {
            case "text/csv":
                list = await ListerGenerique(id, ddr, dd, df, q, o);

                return File(list.AsCsvInByteArray(), "text/csv", $"{Guid.NewGuid()}.csv");
            default:
                list = await ListerGenerique(id, ddr, dd, df, q, o);

                return Ok(list);
        }
    }

    private async Task<IEnumerable<GetDonnee>> ListerGenerique(Guid id, DateTimeOffset? ddr, DateTimeOffset? dd, DateTimeOffset? df, int? q, string? o)
    {
        var query = _context.Donnees.AsNoTracking()
                                    .Where(d => d.IdErabliere == id &&
                                           (ddr == null || d.D > ddr) &&
                                           (dd == null || d.D >= dd) &&
                                           (df == null || d.D <= df));

        if (string.IsNullOrWhiteSpace(o) || o == "c")
        {
            query = query.OrderBy(d => d.D);
        }
        else if (o == "d")
        {
            query = query.OrderByDescending(d => d.D);
        }

        if (q.HasValue)
        {
            query = query.Take(q.Value);
        }

        var list = await query.ProjectTo<GetDonnee>(_mapper.ConfigurationProvider).ToArrayAsync();

        if (o == "c" && list.Length > 0)
        {
            if (ddr.HasValue)
            {
                HttpContext.Response.Headers.Append("x-ddr", ddr.Value.ToString("s", CultureInfo.InvariantCulture));
            }

            if (list[^1].D.HasValue)
            {
                HttpContext.Response.Headers.Append("x-dde", list[^1].D!.Value.ToString("s", CultureInfo.InvariantCulture));
            }
        }

        return list;
    }

    /// <summary>
    /// Ajouter un donnée.
    /// </summary>
    /// <param name="id">L'identifiant de l'érablière</param>
    /// <param name="donneeRecu">La donnée à ajouter</param>
    [HttpPost]
    [ValiderIPRules]
    [ValiderOwnership("id")]
    [TriggerAlert]
    public async Task<IActionResult> Ajouter(Guid id, [FromBody] PostDonnee donneeRecu)
    {
        if (id != donneeRecu.IdErabliere)
        {
            return BadRequest($"L'id de la route '{id}' ne concorde pas avec l'id de l'érablière dans la donnée '{donneeRecu.IdErabliere}'.");
        }

        if (donneeRecu.D == default || donneeRecu.D.Equals(DateTimeOffset.MinValue))
        {
            donneeRecu.D = DateTimeOffset.Now;
        }

        var donnePlusRecente = _context.Donnees.OrderBy(d => d.D).LastOrDefault(d => d.IdErabliere == id);

        if (donnePlusRecente != null &&
            donnePlusRecente.IdentiqueMemeLigneDeTemps(donneeRecu))
        {
            if (donnePlusRecente.D.HasValue == false || donneeRecu.D.HasValue == false)
            {
                throw new InvalidProgramException("Les dates des données ne devrait pas être null rendu à cette étape.");
            }

            var interval = donneeRecu.D.Value - donnePlusRecente.D.Value;

            if (donnePlusRecente.Iddp != null)
            {
                donnePlusRecente.D = donneeRecu.D;

                if (donnePlusRecente.PI.HasValue == false)
                {
                    if (interval.Milliseconds > donnePlusRecente.PI)
                    {
                        donnePlusRecente.PI = (int)interval.TotalSeconds;
                    }
                }
                else
                {
                    donnePlusRecente.PI = (int)interval.TotalSeconds;
                }

                donnePlusRecente.Nboc++;

                _context.Donnees.Update(donnePlusRecente);

                await _context.SaveChangesAsync();
            }
            else
            {
                var donnee = _mapper.Map<Donnee>(donneeRecu);

                donnee.Iddp = donnePlusRecente.Id;

                donnee.PI = (int)interval.TotalSeconds;

                _context.Donnees.Add(donnee);

                await _context.SaveChangesAsync();
            }
        }
        else
        {
            _context.Donnees.Add(_mapper.Map<Donnee>(donneeRecu));

            await _context.SaveChangesAsync();
        }

        return Ok();
    }

    /// <summary>
    /// Action permettant d'importer une liste de données
    /// </summary>
    /// <param name="id"></param>
    /// <param name="donnees"></param>
    /// <param name="token"></param>
    /// <returns>Le nombre d'entré enregistré dans la base de données</returns>
    [HttpPost]
    [ValiderIPRules]
    [ValiderOwnership("id")]
    [Route("Importer")]
    public async Task<IActionResult> Importer(Guid id, Donnee[] donnees, CancellationToken token)
    {
        await _context.AddRangeAsync(donnees.Select(d =>
        {
            if (d.IdErabliere == null)
            {
                d.IdErabliere = id;
            }

            return donnees;
        }), cancellationToken: token);

        var nbStateEntrySaved = await _context.SaveChangesAsync(token);

        return Ok(nbStateEntrySaved);
    }

    /// <summary>
    /// Modifier un dompeux
    /// </summary>
    /// <param name="id">Identifiant de l'érablière</param>
    /// <param name="idDonnee">L'id de la donnée à modifier</param>
    /// <param name="donnee">Le dompeux à ajouter</param>
    /// <param name="token">Le token d'annulation</param>
    [HttpPut("{idDonnee}")]
    [ValiderIPRules]
    [ValiderOwnership("id")]
    public async Task<IActionResult> Modifier(Guid id, Guid idDonnee, [FromBody] PutDonnee donnee, CancellationToken token)
    {
        if (id != donnee.IdErabliere)
        {
            return BadRequest("L'id de la route ne concorde pas avec l'id de l'érablière.");
        }

        if (idDonnee != donnee.Id)
        {
            return BadRequest("L'id de la donnée dans la route ne concorde pas avec l'id la donnée dans le body.");
        }

        var entity = await _context.Donnees.FindAsync(new object?[] { donnee.Id }, token);

        if (entity == null)
        {
            return BadRequest($"La donnée que vous tentez de modifier n'existe pas.");
        }

        if (entity.IdErabliere != donnee.IdErabliere)
        {
            return BadRequest($"L'id de l'érablière de la donnée trouvé ne concorde pas avec l'id de l'érablière de la route.");
        }

        // fin des validations

        if (donnee.V.HasValue)
        {
            entity.V = donnee.V;
        }

        if (donnee.T.HasValue)
        {
            entity.T = donnee.T;
        }

        if (donnee.NB.HasValue)
        {
            entity.NB = donnee.NB;
        }

        _context.Donnees.Update(entity);

        await _context.SaveChangesAsync();

        return Ok();
    }

    /// <summary>
    /// Supprimer un dompeux
    /// </summary>
    /// <param name="id">L'identifiant de l'érablière</param>
    /// <param name="idDonnee">L'id de la donnée à supprimer</param>
    /// <param name="donnee">Le dompeux a supprimer</param>
    [HttpDelete("{idDonnee}")]
    [ValiderIPRules]
    [ValiderOwnership("id")]
    public async Task<IActionResult> Supprimer(Guid id, Guid idDonnee, Donnee donnee)
    {
        if (id != donnee.IdErabliere)
        {
            return BadRequest("L'id de la route ne concorde pas avec l'id de la donnée.");
        }

        if (idDonnee != donnee.Id)
        {
            return BadRequest("L'id de la donnée dans la route ne concorde pas avec l'id la donnée dans le body.");
        }

        var entity = await _context.Donnees.FindAsync(donnee.Id);

        if (entity == null)
        {
            return BadRequest($"La donnée que vous tentez de supprimer n'existe pas.");
        }

        if (entity.IdErabliere != donnee.IdErabliere)
        {
            return BadRequest($"L'id de l'érablière de la donnée trouvé ne concorde pas avec l'id de l'érablière de la route.");
        }

        _context.Donnees.Remove(entity);

        await _context.SaveChangesAsync();

        return NoContent();
    }
}
