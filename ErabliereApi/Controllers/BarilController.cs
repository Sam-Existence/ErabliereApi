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
    public class BarilController : ControllerBase
    {
        private readonly Depot<Baril> dépôt;

        /// <summary>
        /// Constructeur par initialisation
        /// </summary>
        /// <param name="dépôt"></param>
        public BarilController(Depot<Baril> dépôt)
        {
            this.dépôt = dépôt;
        }

        /// <summary>
        /// Liste les barils
        /// </summary>
        /// <returns>Liste des barils</returns>
        [HttpGet]
        public IEnumerable<Baril> Lister(int id)
        {
            return dépôt.Lister(b => b.IdÉrablière == id);
        }

        /// <summary>
        /// Ajouter un baril
        /// </summary>
        /// <param name="donnee"></param>
        [HttpPost]
        public IActionResult Ajouter(int id, Baril donnee)
        {
            if (id != donnee.IdÉrablière)
            {
                return BadRequest("L'id de la route ne concorde pas avec l'id du baril à ajouter");
            }

            dépôt.Ajouter(donnee);

            return Ok();
        }

        /// <summary>
        /// Modifier un baril
        /// </summary>
        /// <param name="donnee"></param>
        [HttpPut]
        public void Modifier(int id, Baril donnee)
        {
            dépôt.Modifier(donnee);
        }

        /// <summary>
        /// Supprimer un baril
        /// </summary>
        /// <param name="donnee"></param>
        [HttpDelete]
        public void Supprimer(int id, Baril donnee)
        {
            dépôt.Supprimer(donnee);
        }
    }
}
