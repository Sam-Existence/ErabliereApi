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
    [Route("[controller]/[action]")]
    public class DonneesController : ControllerBase
    {
        private readonly Dépôt<Donnee> dépôt;

        /// <summary>
        /// Constructeur par initlisation
        /// </summary>
        /// <param name="dépôt"></param>
        public DonneesController(Dépôt<Donnee> dépôt)
        {
            this.dépôt = dépôt;
        }

        /// <summary>
        /// Lister les données
        /// </summary>
        /// <returns>Liste des données</returns>
        [HttpGet]
        public IEnumerable<Donnee> Lister()
        {
            return dépôt.Lister();
        }

        /// <summary>
        /// Ajouter un donnée
        /// </summary>
        /// <param name="donnee"></param>
        [HttpPost]
        public void Ajouter(Donnee donnee)
        {
            dépôt.Ajouter(donnee);
        }
    }
}
