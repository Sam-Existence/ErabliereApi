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
    [Route("erablieres/{id}/[controller]")]
    public class DompeuxController : ControllerBase
    {
        private readonly Depot<Dompeux> dépôt;

        /// <summary>
        /// Constructeur par initialisation
        /// </summary>
        /// <param name="dépôt"></param>
        public DompeuxController(Depot<Dompeux> dépôt)
        {
            this.dépôt = dépôt;
        }

        /// <summary>
        /// Lister les dompeux
        /// </summary>
        /// <returns>Liste des dompeux</returns>
        [HttpGet]
        public IEnumerable<Dompeux> Lister(int id)
        {
            return dépôt.Lister(d => d.IdÉrablière == id);
        }

        /// <summary>
        /// Ajouter un dompeux
        /// </summary>
        /// <param name="donnee"></param>
        [HttpPost]
        public IActionResult Ajouter(int id, Dompeux donnee)
        {
            if (id != donnee.IdÉrablière)
            {
                return BadRequest("L'id de la route ne concorde pas avec l'id du dompeux");
            }

            dépôt.Ajouter(donnee);

            return Ok();
        }

        /// <summary>
        /// Modifier un dompeux
        /// </summary>
        /// <param name="donnee"></param>
        [HttpPut]
        public void Modifier(int id, Dompeux donnee)
        {
            dépôt.Modifier(donnee);
        }

        /// <summary>
        /// Supprimer un dompeux
        /// </summary>
        /// <param name="donnee"></param>
        [HttpDelete]
        public void Supprimer(int id, Dompeux donnee)
        {
            dépôt.Supprimer(donnee);
        }
    }
}
