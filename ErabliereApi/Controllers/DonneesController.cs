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
    /// Contrôler représentant les données reçu par l'automate principale
    /// </summary>
    [ApiController]
    [Route("erablieres/{id}/[controller]")]
    public class DonneesController : ControllerBase
    {
        private readonly Depot<Donnee> dépôt;

        /// <summary>
        /// Constructeur par initlisation
        /// </summary>
        /// <param name="dépôt"></param>
        public DonneesController(Depot<Donnee> dépôt)
        {
            this.dépôt = dépôt;
        }

        /// <summary>
        /// Lister les données
        /// </summary>
        /// <param name="dd">Date de début</param>
        /// <param name="df">Date de début</param>
        /// <param name="q">Quantité de donnée demander</param>
        /// <param name="t">Trie</param>
        /// <param name="ordre">Doit être croissant "c" ou decroissant "d". Par défaut "c"</param>
        /// <returns>Liste des données</returns>
        [HttpGet]
        public IEnumerable<Donnee> Lister([DefaultValue("0")] int id, DateTime? dd, DateTime? df, int? q, string? o = "c")
        {
            IEnumerable<Donnee> query = dépôt.Lister(d => d.IdErabliere == id &&
                                                    (dd == null || d.D >= dd) &&
                                                    (df == null || d.D <= df));

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
        /// Ajouter un donnée
        /// </summary>
        /// <param name="id">L'identifiant de l'érablière</param>
        /// <param name="donnee">La donnée à ajouter</param>
        [HttpPost]
        public IActionResult Ajouter([DefaultValue(0)] int id, Donnee donnee)
        {
            if (id != donnee.IdErabliere)
            {
                return BadRequest($"L'id de la route '{id}' ne concorde pas avec l'id de l'érablière dans la donnée '{donnee.IdErabliere}'.");
            }

            if (donnee.D == default || donnee.D.Equals(DateTime.MinValue))
            {
                donnee.D = DateTime.Now;
            }

            dépôt.Ajouter(donnee);

            return Ok();
        }
    }
}
