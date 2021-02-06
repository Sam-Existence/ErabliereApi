using ErabliereApi.Depot;
using ErabliereApi.Donnees;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;

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
        /// <returns>Liste des données</returns>
        [HttpGet]
        public IEnumerable<Donnee> Lister([DefaultValue(0)] int id, DateTime? dd, DateTime? df)
        {
            return dépôt.Lister(d => d.IdErabliere == id &&
                               ((dd != null) ? d.D >= dd : true) &&
                               ((df != null) ? d.D <= df : true));
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
