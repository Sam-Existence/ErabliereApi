﻿using ErabliereApi.Depot;
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
        /// <param name="id">Identifiant de l'érablière</param>
        /// <returns>Liste des dompeux</returns>
        [HttpGet]
        public IEnumerable<Dompeux> Lister([DefaultValue(0)] int id, DateTime? dd, DateTime? df)
        {
            return dépôt.Lister(d => d.IdErabliere == id &&
                               ((dd != null) ? d.T >= dd : true) &&
                               ((df != null) ? d.T <= df : true));
        }

        /// <summary>
        /// Ajouter un dompeux
        /// </summary>
        /// <param name="id">L'identifiant de l'érablière</param>
        /// <param name="donnee"></param>
        [HttpPost]
        public IActionResult Ajouter([DefaultValue(0)] int id, Dompeux donnee)
        {
            if (id != donnee.IdErabliere)
            {
                return BadRequest("L'id de la route ne concorde pas avec l'id du dompeux");
            }

            dépôt.Ajouter(donnee);

            return Ok();
        }

        /// <summary>
        /// Modifier un dompeux
        /// </summary>
        /// <param name="id">Identifiant de l'érablière</param>
        /// <param name="donnee">Le dompeux à ajouter</param>
        [HttpPut]
        public IActionResult Modifier([DefaultValue(0)] int id, Dompeux donnee)
        {
            if (id != donnee.IdErabliere)
            {
                return BadRequest("L'id de la route ne concorde pas avec l'id du dompeux");
            }

            dépôt.Modifier(donnee);

            return Ok();
        }

        /// <summary>
        /// Supprimer un dompeux
        /// </summary>
        /// <param name="id">L'identifiant de l'érablière</param>
        /// <param name="donnee">Le dompeux a supprimer</param>
        [HttpDelete]
        public IActionResult Supprimer([DefaultValue(0)] int id, Dompeux donnee)
        {
            if (id != donnee.IdErabliere)
            {
                return BadRequest("L'id de la route ne concorde pas avec l'id du dompeux");
            }

            dépôt.Supprimer(donnee);

            return NoContent();
        }
    }
}
