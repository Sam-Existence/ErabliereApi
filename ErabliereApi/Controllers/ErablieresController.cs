using AutoMapper;
using ErabliereApi.Depot;
using ErabliereApi.Donnees;
using ErabliereApi.Donnees.Action.Post;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ErabliereApi.Controllers
{
    /// <summary>
    /// Contrôleur pour gérer les érablières
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class ErablieresController : ControllerBase
    {
        private readonly Depot<Erabliere> _depot;
        private readonly IMapper _mapper;

        /// <summary>
        /// Constructeur par initialisation
        /// </summary>
        /// <param name="dépôt">Dépôt de donnée des érablières</param>
        /// <param name="mapper">mapper de donnée</param>
        public ErablieresController(Depot<Erabliere> dépôt, IMapper mapper)
        {
            _depot = dépôt;
            _mapper = mapper;
        }

        /// <summary>
        /// Liste les érablières
        /// </summary>
        /// <returns>Une liste d'érablière</returns>
        [HttpGet]
        public IEnumerable<Erabliere> Lister()
        {
            return _depot.Lister();
        }

        /// <summary>
        /// Créer une érablière
        /// </summary>
        /// <param name="erablieres">L'érablière à créer</param>
        /// <response code="200">L'érablière a été correctement ajouté</response>
        /// <response code="400">Le nom de l'érablière est null ou vide ou un érablière avec le nom reçu existe déjà.</response>
        [HttpPost]
        public async Task<IActionResult> Ajouter(PostErabliere erablieres)
        {
            if (string.IsNullOrWhiteSpace(erablieres.Nom))
            {
                return BadRequest($"Le nom de l'érablière ne peut pas être vide.");
            }
            if (await _depot.Contient(e => e.Nom == erablieres.Nom))
            {
                return BadRequest($"L'érablière nommé '{erablieres.Nom}' existe déjà");
            }

            _depot.Ajouter(_mapper.Map<Erabliere>(erablieres));

            return Ok();
        }
    }
}
