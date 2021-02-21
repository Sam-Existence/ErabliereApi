using ErabliereApi.Attributes;
using ErabliereApi.Depot;
using ErabliereApi.Donnees;
using Microsoft.AspNetCore.Mvc;
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
    public class DompeuxController : ControllerBase
    {
        private readonly Depot<Dompeux> _depot;

        /// <summary>
        /// Constructeur par initialisation
        /// </summary>
        /// <param name="dépôt">Le dépôt des dompeux</param>
        public DompeuxController(Depot<Dompeux> dépôt)
        {
            _depot = dépôt;
        }

        /// <summary>
        /// Lister les dompeux
        /// </summary>
        /// <param name="id">Identifiant de l'érablière</param>
        /// <param name="dd">La date de début</param>
        /// <param name="df">La date de fin</param>
        /// <param name="q">La quantité de donnée retourné</param>
        /// <param name="o">L'ordre des dompeux par date d'occurence. "c" = croissant, "d" = décoissant. </param>
        /// <response code="200">Une liste avec les dompeux. La liste est potentiellement vide.</response>
        [HttpGet]
        public IEnumerable<Dompeux> Lister([DefaultValue(0)] int id, DateTimeOffset? dd, DateTimeOffset? df, int? q, string? o = "c")
        {
            var query = _depot.Lister(d => d.IdErabliere == id &&
                                     (dd == null || d.T >= dd) &&
                                     (df == null || d.T <= df));

            if (o == "d")
            {
                query = query.OrderByDescending(d => d);
            }
            
            if (q.HasValue)
            {
                query = query.Take(q.Value);
            }

            return query;
        }

        /// <summary>
        /// Ajouter un dompeux
        /// </summary>
        /// <param name="id">L'identifiant de l'érablière</param>
        /// <param name="donnee"></param>
        [HttpPost]
        [ValiderIPRules]
        public IActionResult Ajouter([DefaultValue(0)] int id, Dompeux donnee)
        {
            if (id != donnee.IdErabliere)
            {
                return BadRequest("L'id de la route ne concorde pas avec l'id du dompeux");
            }

            if (donnee.T == default || donnee.T.Equals(DateTimeOffset.MinValue))
            {
                donnee.T = DateTimeOffset.Now;
            }

            _depot.Ajouter(donnee);

            return Ok();
        }

        /// <summary>
        /// Modifier un dompeux
        /// </summary>
        /// <param name="id">Identifiant de l'érablière</param>
        /// <param name="donnee">Le dompeux à ajouter</param>
        [HttpPut]
        [ValiderIPRules]
        public IActionResult Modifier([DefaultValue(0)] int id, Dompeux donnee)
        {
            if (id != donnee.IdErabliere)
            {
                return BadRequest("L'id de la route ne concorde pas avec l'id du dompeux");
            }

            _depot.Modifier(donnee);

            return Ok();
        }

        /// <summary>
        /// Supprimer un dompeux
        /// </summary>
        /// <param name="id">L'identifiant de l'érablière</param>
        /// <param name="donnee">Le dompeux a supprimer</param>
        [HttpDelete]
        [ValiderIPRules]
        public IActionResult Supprimer([DefaultValue(0)] int id, Dompeux donnee)
        {
            if (id != donnee.IdErabliere)
            {
                return BadRequest("L'id de la route ne concorde pas avec l'id du dompeux");
            }

            _depot.Supprimer(donnee);

            return NoContent();
        }
    }
}
