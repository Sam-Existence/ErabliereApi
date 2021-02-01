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
    public class DompeuxController
    {
        private readonly Dépôt<Dompeux> dépôt;

        /// <summary>
        /// Constructeur par initialisation
        /// </summary>
        /// <param name="dépôt"></param>
        public DompeuxController(Dépôt<Dompeux> dépôt)
        {
            this.dépôt = dépôt;
        }

        /// <summary>
        /// Lister les dompeux
        /// </summary>
        /// <returns>Liste des dompeux</returns>
        [HttpGet]
        public IEnumerable<Dompeux> Lister()
        {
            return dépôt.Lister();
        }

        /// <summary>
        /// Ajouter un dompeux
        /// </summary>
        /// <param name="donnee"></param>
        [HttpPost]
        public void Ajouter(Dompeux donnee)
        {
            dépôt.Ajouter(donnee);
        }

        /// <summary>
        /// Modifier un dompeux
        /// </summary>
        /// <param name="donnee"></param>
        [HttpPut]
        public void Modifier(Dompeux donnee)
        {
            dépôt.Modifier(donnee);
        }

        /// <summary>
        /// Supprimer un dompeux
        /// </summary>
        /// <param name="donnee"></param>
        [HttpDelete]
        public void Supprimer(Dompeux donnee)
        {
            dépôt.Supprimer(donnee);
        }
    }
}
