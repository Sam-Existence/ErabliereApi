using AutoMapper;
using ErabliereApi.Depot;
using ErabliereApi.Donnees;
using ErabliereApi.Donnees.Action.Post;
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
        private readonly Depot<Erabliere> _dépôt;

        /// <summary>
        /// Constructeur par initialisation
        /// </summary>
        /// <param name="dépôt">Dépôt de donnée des érablières</param>
        /// <param name="mapper">mapper de donnée</param>
        public ErablieresController(Depot<Erabliere> dépôt, IMapper mapper)
        {
            _dépôt = dépôt;
        }

        /// <summary>
        /// Liste les érablières
        /// </summary>
        /// <returns>Une liste d'érablière</returns>
        [HttpGet]
        public IEnumerable<Erabliere> Lister()
        {
            return _dépôt.Lister();
        }

        /// <summary>
        /// Créer une érablière
        /// </summary>
        /// <param name="érablières">L'érablière à créer</param>
        [HttpPost]
        public IActionResult Ajouter(PostErabliere érablières)
        {
            if (string.IsNullOrWhiteSpace(érablières.Nom))
            {
                return BadRequest($"Le nom de l'érablière ne peut pas être vide.");
            }
            if (_dépôt.Contient(e => e.Nom == érablières.Nom))
            {
                return BadRequest($"L'érablière nommé {érablières} existe déjà");
            }

            _dépôt.Ajouter(_mapper.Map<Erabliere>(érablières));

            return Ok();
        }
    }
}
