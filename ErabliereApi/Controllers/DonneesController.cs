using ErabliereApi.Depot;
using ErabliereApi.Donnees;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

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
        public IEnumerable<Donnee> Lister(int id)
        {
            return dépôt.Lister(d => d.IdÉrablière == id);
        }

        /// <summary>
        /// Ajouter un donnée
        /// </summary>
        /// <param name="donnee"></param>
        [HttpPost]
        public IActionResult Ajouter(int id, Donnee donnee)
        {
            if (id != donnee.IdÉrablière)
            {
                return BadRequest($"L'id de la route '{id}' ne concorde pas avec l'id de l'érablière dans la donnée '{donnee.IdÉrablière}'.");
            }

            dépôt.Ajouter(donnee);

            return Ok();
        }
    }
}
