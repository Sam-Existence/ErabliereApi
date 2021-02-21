using AutoMapper;
using ErabliereApi.Depot;
using ErabliereApi.Donnees;
using ErabliereApi.Donnees.Action.Post;
using ErabliereApi.Donnees.Action.Put;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">L'id de l'érablière à modifier</param>
        /// <param name="erabliere">Le donnée de l'érablière à modifier.
        ///     1. L'id doit concorder avec celui de la route.
        ///     2. L'érablière doit exister.
        ///     3. Si le nom est modifier, il ne doit pas être pris par un autre érablière.
        /// 
        /// Pour modifier l'adresse IP, vous devez entrer quelque chose. "-" pour supprimer les règles déjà existante.</param>
        /// <response code="200">L'érablière à été correctement modifié.</response>
        /// <response code="400">Une des validations des paramètres à échoué.</response>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Modifier(int id, PutErabliere erabliere)
        {
            if (id != erabliere.Id)
            {
                return BadRequest($"L'id de la route ne concorde pas avec l'id de l'érablière à modifier.");
            }

            var entity = _depot.Obtenir(id);

            if (entity == null)
            {
                return BadRequest($"L'érablière que vous tentez de modifier n'existe pas.");
            }

            if (string.IsNullOrWhiteSpace(erabliere.Nom) == false && await _depot.Contient(e => e.Id != id && e.Nom == erabliere.Nom))
            {
                return BadRequest($"L'érablière avec le nom {erabliere.Nom}");
            }

            // fin des validations

            if (string.IsNullOrWhiteSpace(erabliere.Nom) == false)
            {
                entity.Nom = erabliere.Nom;
            }

            if (string.IsNullOrWhiteSpace(erabliere.IpRules) == false)
            {
                entity.IpRule = erabliere.IpRules;
            }

            _depot.Modifier(entity);

            return Ok();
        }
    }
}
