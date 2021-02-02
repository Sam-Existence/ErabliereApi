using ErabliereApi.Depot;
using ErabliereApi.Donnees;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;

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
        /// <param name="id">Identifiant de l'érablière</param>
        /// <returns>Liste des barils</returns>
        [HttpGet]
        public IEnumerable<Baril> Lister([DefaultValue(0)] int id)
        {
            return dépôt.Lister(b => b.IdÉrablière == id);
        }

        /// <summary>
        /// Ajouter un baril
        /// </summary>
        /// <param name="id">L'identifiant de l'érablière</param>
        /// <param name="donnee"></param>
        [HttpPost]
        public IActionResult Ajouter([DefaultValue(0)] int id, Baril donnee)
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
        /// <param name="id">L'identifiant de l'érablière</param>
        /// <param name="donnee">Le baril a modifier</param>
        [HttpPut]
        public IActionResult Modifier([DefaultValue(0)] int id, Baril donnee)
        {
            if (id != donnee.IdÉrablière)
            {
                return BadRequest("L'id de la route ne concorde pas avec l'id du baril à ajouter");
            }

            dépôt.Modifier(donnee);

            return Ok();
        }

        /// <summary>
        /// Supprimer un baril
        /// </summary>
        /// <param name="id">Identifiant de l'érablière</param>
        /// <param name="donnee">Le baril a supprimer</param>
        [HttpDelete]
        public IActionResult Supprimer([DefaultValue(0)] int id, Baril donnee)
        {
            if (id != donnee.IdÉrablière)
            {
                return BadRequest("L'id de la route ne concorde pas avec l'id du baril à ajouter");
            }

            dépôt.Supprimer(donnee);

            return NoContent();
        }
    }
}
