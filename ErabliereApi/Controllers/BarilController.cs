using ErabliereApi.Depot;
using ErabliereApi.Depot.Sql;
using ErabliereApi.Donnees;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace ErabliereApi.Controllers
{
    /// <summary>
    /// Contrôler représentant les données des dompeux
    /// </summary>
    [ApiController]
    [Route("erablieres/{id}/[controller]")]
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
        public IEnumerable<Baril> Lister([DefaultValue(0)] int id, DateTimeOffset? dd, DateTimeOffset? df)
        {
            return _depot.Barils.AsNoTracking()
                                .Where(b => b.IdErabliere == id &&
                                       (dd == null || b.DF >= dd) &&
                                       (df == null || b.DF <= df));
        }

        /// <summary>
        /// Ajouter un baril
        /// </summary>
        /// <param name="id">L'identifiant de l'érablière</param>
        /// <param name="donnee"></param>
        /// <response code="200">Le baril a été correctement ajouter.</response>
        /// <response code="400">L'id de la route ne concorde pas avec l'id du baril à ajouter.</response>
        [HttpPost]
        public IActionResult Ajouter([DefaultValue(0)] int id, Baril donnee)
        {
            if (id != donnee.IdErabliere)
            {
                return BadRequest("L'id de la route ne concorde pas avec l'id du baril à ajouter");
            }

            _depot.Barils.Add(donnee);

            _depot.SaveChanges();

            return Ok();
        }

        /// <summary>
        /// Modifier un baril
        /// </summary>
        /// <param name="id">L'identifiant de l'érablière</param>
        /// <param name="donnee">Le baril a modifier</param>
        /// <response code="200">Le baril a été correctement supprimé.</response>
        /// <response code="400">L'id de la route ne concorde pas avec l'id du baril à modifier.</response>
        [HttpPut]
        public IActionResult Modifier([DefaultValue(0)] int id, Baril donnee)
        {
            if (id != donnee.IdErabliere)
            {
                return BadRequest("L'id de la route ne concorde pas avec l'id du baril à modifier.");
            }

            _depot.Update(donnee);

            _depot.SaveChanges();

            return Ok();
        }

        /// <summary>
        /// Supprimer un baril
        /// </summary>
        /// <param name="id">Identifiant de l'érablière</param>
        /// <param name="donnee">Le baril a supprimer</param>
        /// <response code="202">Le baril a été correctement supprimé.</response>
        /// <response code="400">L'id de la route ne concorde pas avec l'id du baril à supprimer.</response>
        [HttpDelete]
        public IActionResult Supprimer([DefaultValue(0)] int id, Baril donnee)
        {
            if (id != donnee.IdErabliere)
            {
                return BadRequest("L'id de la route ne concorde pas avec l'id du baril à supprimer.");
            }

            _depot.Remove(donnee);

            _depot.SaveChanges();

            return NoContent();
        }
    }
}
