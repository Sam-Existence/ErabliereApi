using ErabliereApi.Depot;
using ErabliereApi.Donnees;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace ErabliereApi.Controllers
{
    /// <summary>
    /// Contrôler représentant les données des dompeux
    /// </summary>
    [ApiController]
    [Route("[controller]/[action]")]
    public class BarilController
    {
        private readonly Dépôt<Baril> dépôt;

        /// <summary>
        /// Constructeur par initialisation
        /// </summary>
        /// <param name="dépôt"></param>
        public BarilController(Dépôt<Baril> dépôt)
        {
            this.dépôt = dépôt;
        }

        /// <summary>
        /// Liste les barils
        /// </summary>
        /// <returns>Liste des barils</returns>
        [HttpGet]
        public IEnumerable<Baril> Lister()
        {
            return dépôt.Lister();
        }

        /// <summary>
        /// Ajouter un baril
        /// </summary>
        /// <param name="donnee"></param>
        [HttpPost]
        public void Ajouter(Baril donnee)
        {
            dépôt.Ajouter(donnee);
        }

        /// <summary>
        /// Modifier un baril
        /// </summary>
        /// <param name="donnee"></param>
        [HttpPut]
        public void Modifier(Baril donnee)
        {
            dépôt.Modifier(donnee);
        }

        /// <summary>
        /// Supprimer un baril
        /// </summary>
        /// <param name="donnee"></param>
        [HttpDelete]
        public void Supprimer(Baril donnee)
        {
            dépôt.Supprimer(donnee);
        }
    }
}
