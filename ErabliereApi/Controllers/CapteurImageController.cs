using AutoMapper;
using AutoMapper.QueryableExtensions;
using ErabliereApi.Attributes;
using ErabliereApi.Depot.Sql;
using ErabliereApi.Donnees;
using ErabliereApi.Donnees.Action.Get;
using ErabliereApi.Donnees.Action.Post;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.EntityFrameworkCore;

namespace ErabliereApi.Controllers
{
    /// <summary>
    /// Controller représentant les capteurs d'image
    /// </summary>
    /// 
    [Route("Erablieres/{id}/[controller]")]
    [ApiController]
    [Authorize]
    public class CapteurImageController : ControllerBase
    {
        private readonly ErabliereDbContext _depot;
        private readonly IMapper _mapper;

        /// <summary>
        /// Constructeur par initialisation
        /// </summary>
        /// <param name="depot">Le dépôt des capteurs d'image</param>
        /// <parsam name="mapper">Interface de mapping entre les données</param>
        public CapteurImageController(ErabliereDbContext depot, IMapper mapper)
        {
            _depot = depot;
            _mapper = mapper;
        }

        /// <summary>
        /// Lister les capteurs d'image
        /// </summary>
        /// <param name="id">L'identifiant de l'érablière</param>
        /// <param name="token">Le jeton d'annulation de la requête</param>
        /// <response code="200">Les capteurs d'image ont correctement été récupérés.</response>
        [HttpGet]
        [ValiderOwnership("id")]
        [EnableQuery]
        public async Task<IEnumerable<GetCapteurImage>> Lister(Guid id, string? filtreNom, CancellationToken token)
        {
            return await _depot.CapteurImage.AsNoTracking()
                                .Where(b => b.IdErabliere == id && 
                                        (filtreNom == null || (b.Nom != null && b.Nom.Contains(filtreNom) == true)))
                                .ProjectTo<GetCapteurImage>(_mapper.ConfigurationProvider)
                                .ToArrayAsync(token);
        }


        /// <summary>
        /// Ajouter un capteur d'image
        /// </summary>
        /// <param name="id">L'identifiant de l'érablière</param>
        /// <param name="capteur">Le capteur a ajouter</param>
        /// <param name="token">Le jeton d'annulation de la requête</param>
        /// <response code="200">Le capteur a été correctement ajouté.</response>
        [HttpPost]
        [ValiderOwnership("id")]
        public async Task<IActionResult> Ajouter(Guid id, PostCapteurImage capteur, CancellationToken token)
        {
            var capteurImage = _mapper.Map<CapteurImage>(capteur);

            capteurImage.Ordre = _depot.CapteurImage.Where(c => id == c.IdErabliere).Count();
            capteurImage.IdErabliere = id;
            if(string.IsNullOrEmpty(capteurImage.MotDePasse))
            {
                capteurImage.MotDePasse = null;
            }
            if(string.IsNullOrEmpty(capteurImage.MotDePasse))
            {
                capteurImage.Identifiant = null;
            }

            var entity = await _depot.CapteurImage.AddAsync(capteurImage, token);

            
            

            await _depot.SaveChangesAsync(token);

            return Ok(new { id = entity.Entity.Id });
        }
    }
}
