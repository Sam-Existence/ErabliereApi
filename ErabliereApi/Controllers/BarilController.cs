using ErabliereApi.Depot;
using ErabliereApi.Donnees;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace ErabliereApi.Controllers
{
    /// <summary>
    /// Contrôler représentant les données des dompeux
    /// </summary>
    [ApiController]
    [Route("erablieres/{id}/[controller]")]
    public class BarilController : ControllerBase
    {
        private readonly Depot<Baril> depot;

        /// <summary>
        /// Constructeur par initialisation
        /// </summary>
        /// <param name="dépôt">Le dépôt des barils</param>
        public BarilController(Depot<Baril> dépôt)
        {
            this.depot = dépôt;
        }

        /// <summary>
        /// Liste les barils
        /// </summary>
        /// <param name="id">Identifiant de l'érablière</param>
        /// <response code="200">Une liste de baril potentiellement vide.</response>
        [HttpGet]
        public IEnumerable<Baril> Lister([DefaultValue(0)] int id, DateTime? dd, DateTime? df)
        {
            return depot.Lister(b => b.IdErabliere == id &&
                               ((dd != null) ? b.DF >= dd : true) &&
                               ((df != null) ? b.DF <= df : true));
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

            depot.Ajouter(donnee);

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

            depot.Modifier(donnee);

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

            depot.Supprimer(donnee);

            return NoContent();
        }
    }
}
