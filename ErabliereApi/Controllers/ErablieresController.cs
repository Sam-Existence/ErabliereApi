using ErabliereApi.Depot;
using ErabliereApi.Donnees;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace ErabliereApi.Controllers
{
    /// <summary>
    /// Contrôleur pour gérer les érablières
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class ErablieresController : ControllerBase
    {
        private readonly Depot<Erablieres> _dépôt;

        /// <summary>
        /// Constructeur par initialisation
        /// </summary>
        /// <param name="dépôt">Dépôt de donnée des érablières</param>
        public ErablieresController(Depot<Erablieres> dépôt)
        {
            _dépôt = dépôt;
        }

        /// <summary>
        /// Liste les érablières
        /// </summary>
        /// <returns>Une liste d'érablière</returns>
        [HttpGet]
        public IEnumerable<Erablieres> Lister()
        {
            return _dépôt.Lister();
        }

        /// <summary>
        /// Créer une érablière
        /// </summary>
        /// <param name="érablières">L'érablière à créer</param>
        [HttpPost]
        public IActionResult Ajouter(Erablieres érablières)
        {
            if (érablières.Id != null && _dépôt.Contient(érablières.Id))
            {
                return BadRequest($"L'érablière avec l'ID {érablières.Id} existe déjà");
            }
            if (érablières.Id == null && string.IsNullOrWhiteSpace(érablières.Nom))
            {
                return BadRequest($"Le nom de l'érablière ne peut pas être vide.");
            }
            if (_dépôt.Contient(e => e.Nom == érablières.Nom))
            {
                return BadRequest($"L'érablière nommé {érablières} existe déjà");
            }

            _dépôt.Ajouter(érablières);

            return Ok();
        }
    }
}
