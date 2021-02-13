using ErabliereApi.Depot;
using ErabliereApi.Donnees;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel;

namespace ErabliereApi.Controllers
{
    /// <summary>
    /// Contrôler représentant les données des dompeux
    /// </summary>
    [ApiController]
    [Route("erablieres/{id}/[controller]")]
    public class AlerteController : ControllerBase
    {
        private readonly Depot<Alerte> _depot;

        /// <summary>
        /// Constructeur par initialisation
        /// </summary>
        /// <param name="depot"></param>
        public AlerteController(Depot<Alerte> depot)
        {
            _depot = depot;
        }

        /// <summary>
        /// Liste les Alertes
        /// </summary>
        /// <param name="id">Identifiant de l'érablière</param>
        /// <response code="200">Une liste d'alerte potentiellement vide.</response>
        [HttpGet]
        public IEnumerable<Alerte> Lister([DefaultValue(0)] int id)
        {
            return _depot.Lister(b => b.IdErabliere == id);
        }

        /// <summary>
        /// Ajouter une Alerte
        /// </summary>
        /// <param name="id">L'identifiant de l'érablière</param>
        /// <param name="alerte"></param>
        /// <response code="200">L'alerte a été correctement ajouter.</response>
        /// <response code="400">L'id de la route ne concorde pas avec l'id de l'alerte à ajouter.</response>
        [HttpPost]
        public IActionResult Ajouter([DefaultValue(0)] int id, Alerte alerte)
        {
            if (id != alerte.IdErabliere)
            {
                return BadRequest("L'id de la route ne concorde pas avec l'id du baril à ajouter");
            }

            _depot.Ajouter(alerte);

            return Ok();
        }

        /// <summary>
        /// Modifier une alerte
        /// </summary>
        /// <param name="id">L'identifiant de l'érablière</param>
        /// <param name="alerte">L'alerte a modifier</param>
        /// <response code="200">L'alerte a été correctement supprimé.</response>
        /// <response code="400">L'id de la route ne concorde pas avec l'id du baril à modifier.</response>
        [HttpPut]
        public IActionResult Modifier([DefaultValue(0)] int id, Alerte alerte)
        {
            if (id != alerte.IdErabliere)
            {
                return BadRequest("L'id de la route ne concorde pas avec l'id du baril à modifier.");
            }

            _depot.Modifier(alerte);

            return Ok();
        }

        /// <summary>
        /// Supprimer une alerte
        /// </summary>
        /// <param name="id">Identifiant de l'érablière</param>
        /// <param name="alerte">L'alerte a supprimer</param>
        /// <response code="202">L'alerte a été correctement supprimé.</response>
        /// <response code="400">L'id de la route ne concorde pas avec l'id de l'alerte à supprimer.</response>
        [HttpDelete]
        public IActionResult Supprimer([DefaultValue(0)] int id, Alerte alerte)
        {
            if (id != alerte.IdErabliere)
            {
                return BadRequest("L'id de la route ne concorde pas avec l'id du baril à supprimer.");
            }

            _depot.Supprimer(alerte);

            return NoContent();
        }
    }
}
