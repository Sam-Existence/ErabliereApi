using ErabliereApi.Depot.Sql;
using ErabliereApi.Donnees;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace ErabliereApi.Controllers
{
    /// <summary>
    /// Contrôler représentant les données des dompeux
    /// </summary>
    [ApiController]
    [Route("erablieres/{id}/[controller]")]
    [Authorize]
    public class CapteursController : ControllerBase
    {
        private readonly ErabliereDbContext _depot;

        /// <summary>
        /// Constructeur par initialisation
        /// </summary>
        /// <param name="depot">Le dépôt des barils</param>
        public CapteursController(ErabliereDbContext depot)
        {
            _depot = depot;
        }

        /// <summary>
        /// Liste les capteurs
        /// </summary>
        /// <param name="id">Identifiant de l'érablière</param>
        /// <param name="filtreNom">Permet de filtrer les capteurs recherché selon leur nom</param>
        /// <response code="200">Une liste de capteurs.</response>
        [HttpGet]
        public async Task<IEnumerable<Capteur>> Lister([DefaultValue(0)] int id, string? filtreNom)
        {
            return await _depot.Capteurs.AsNoTracking()
                                .Where(b => b.IdErabliere == id &&
                                       (filtreNom == null || (b.Nom != null && b.Nom.Contains(filtreNom, StringComparison.OrdinalIgnoreCase) == true)))
                                .ToArrayAsync();
        }

        /// <summary>
        /// Ajouter un capteur
        /// </summary>
        /// <param name="id">L'identifiant de l'érablière</param>
        /// <param name="capteur">Le capteur a ajouter</param>
        /// <response code="200">Le capteur a été correctement ajouté.</response>
        /// <response code="400">L'id de la route ne concorde pas avec l'id du capteur à ajouter.</response>
        [HttpPost]
        public async Task<IActionResult> Ajouter([DefaultValue(0)] int id, Capteur capteur)
        {
            if (id != capteur.IdErabliere)
            {
                return BadRequest("L'id de la route ne concorde pas avec l'id du baril à ajouter");
            }

            await _depot.Capteurs.AddAsync(capteur);

            await _depot.SaveChangesAsync();

            return Ok();
        }

        /// <summary>
        /// Modifier un capteur
        /// </summary>
        /// <param name="id">L'identifiant de l'érablière</param>
        /// <param name="capteur">Le capteur a modifier</param>
        /// <response code="200">Le capteur a été correctement supprimé.</response>
        /// <response code="400">L'id de la route ne concorde pas avec l'id du capteur à modifier.</response>
        [HttpPut]
        public async Task<IActionResult> Modifier([DefaultValue(0)] int id, Capteur capteur)
        {
            if (id != capteur.IdErabliere)
            {
                return BadRequest("L'id de la route ne concorde pas avec l'id du baril à modifier.");
            }

            _depot.Update(capteur);

            await _depot.SaveChangesAsync();

            return Ok();
        }

        /// <summary>
        /// Supprimer un capteur
        /// </summary>
        /// <param name="id">Identifiant de l'érablière</param>
        /// <param name="capteur">Le capteur a supprimer</param>
        /// <response code="202">Le capteur a été correctement supprimé.</response>
        /// <response code="400">L'id de la route ne concorde pas avec l'id du capteur à supprimer.</response>
        [HttpDelete]
        public async Task<IActionResult> Supprimer([DefaultValue(0)] int id, Capteur capteur)
        {
            if (id != capteur.IdErabliere)
            {
                return BadRequest("L'id de la route ne concorde pas avec l'id du baril à supprimer.");
            }

            _depot.Remove(capteur);

            await _depot.SaveChangesAsync();

            return NoContent();
        }
    }
}
