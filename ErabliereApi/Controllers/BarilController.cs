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
    public class BarilController : ControllerBase
    {
        private readonly ErabliereDbContext _depot;

        /// <summary>
        /// Constructeur par initialisation
        /// </summary>
        /// <param name="depot">Le dépôt des barils</param>
        public BarilController(ErabliereDbContext depot)
        {
            _depot = depot;
        }

        /// <summary>
        /// Liste les barils
        /// </summary>
        /// <param name="id">Identifiant de l'érablière</param>
        /// <param name="dd">Utiliser ce paramètre pour obtenir les barils avec ne date plus grande ou égal au paramètre passé.</param>
        /// <param name="df">Utiliser ce paramètre pour obtenir les barils avec une date plus petite ou égal au paramètre passé.</param>
        /// <response code="200">Une liste de baril potentiellement vide.</response>
        [HttpGet]
        public async Task<IEnumerable<Baril>> Lister([DefaultValue(0)] int id, DateTimeOffset? dd, DateTimeOffset? df)
        {
            return await _depot.Barils.AsNoTracking()
                                .Where(b => b.IdErabliere == id &&
                                       (dd == null || b.DF >= dd) &&
                                       (df == null || b.DF <= df))
                                .ToArrayAsync();
        }

        /// <summary>
        /// Ajouter un baril
        /// </summary>
        /// <param name="id">L'identifiant de l'érablière</param>
        /// <param name="baril"></param>
        /// <response code="200">Le baril a été correctement ajouter.</response>
        /// <response code="400">L'id de la route ne concorde pas avec l'id du baril à ajouter.</response>
        [HttpPost]
        public async Task<IActionResult> Ajouter([DefaultValue(0)] int id, Baril baril)
        {
            if (id != baril.IdErabliere)
            {
                return BadRequest("L'id de la route ne concorde pas avec l'id du baril à ajouter");
            }

            await _depot.Barils.AddAsync(baril);

            await _depot.SaveChangesAsync();

            return Ok();
        }

        /// <summary>
        /// Modifier un baril
        /// </summary>
        /// <param name="id">L'identifiant de l'érablière</param>
        /// <param name="baril">Le baril a modifier</param>
        /// <response code="200">Le baril a été correctement supprimé.</response>
        /// <response code="400">L'id de la route ne concorde pas avec l'id du baril à modifier.</response>
        [HttpPut]
        public async Task<IActionResult> Modifier([DefaultValue(0)] int id, Baril baril)
        {
            if (id != baril.IdErabliere)
            {
                return BadRequest("L'id de la route ne concorde pas avec l'id du baril à modifier.");
            }

            _depot.Update(baril);

            await _depot.SaveChangesAsync();

            return Ok();
        }

        /// <summary>
        /// Supprimer un baril
        /// </summary>
        /// <param name="id">Identifiant de l'érablière</param>
        /// <param name="baril">Le baril a supprimer</param>
        /// <response code="202">Le baril a été correctement supprimé.</response>
        /// <response code="400">L'id de la route ne concorde pas avec l'id du baril à supprimer.</response>
        [HttpDelete]
        public async Task<IActionResult> Supprimer([DefaultValue(0)] int id, Baril baril)
        {
            if (id != baril.IdErabliere)
            {
                return BadRequest("L'id de la route ne concorde pas avec l'id du baril à supprimer.");
            }

            _depot.Remove(baril);

            await _depot.SaveChangesAsync();

            return NoContent();
        }
    }
}
