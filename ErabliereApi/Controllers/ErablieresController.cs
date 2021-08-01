using AutoMapper;
using AutoMapper.QueryableExtensions;
using ErabliereApi.Attributes;
using ErabliereApi.Depot.Sql;
using ErabliereApi.Donnees;
using ErabliereApi.Donnees.Action.Delete;
using ErabliereApi.Donnees.Action.Get;
using ErabliereApi.Donnees.Action.Post;
using ErabliereApi.Donnees.Action.Put;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ErabliereApi.Controllers
{
    /// <summary>
    /// Contrôleur pour gérer les érablières
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class ErablieresController : ControllerBase
    {
        private readonly ErabliereDbContext _context;
        private readonly IMapper _mapper;

        /// <summary>
        /// Constructeur par initialisation
        /// </summary>
        /// <param name="context">Classe de contexte pour accéder à la BD</param>
        /// <param name="mapper">mapper de donnée</param>
        public ErablieresController(ErabliereDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Liste les érablières
        /// </summary>
        /// <returns>Une liste d'érablière</returns>
        [HttpGet]
        [EnableQuery]
        public IEnumerable<Erabliere> Lister()
        {
            return _context.Erabliere.AsNoTracking();
        }

        /// <summary>
        /// Obtenir les données pour la page principale d'ErabliereIU
        /// </summary>
        /// <returns>Une liste d'érablière</returns>
        [HttpGet("[action]")]
        public async Task<IEnumerable<GetErabliereDashboard>> Dashboard(DateTimeOffset? dd, DateTimeOffset? df, DateTimeOffset? ddr)
        {
            var dashboardData = await _context.Erabliere.AsNoTracking()
                .ProjectTo<GetErabliereDashboard>(_dashboardMapper, new { dd, df, ddr })
                .ToArrayAsync();

            return dashboardData;
        }

        private static readonly IConfigurationProvider _dashboardMapper = new MapperConfiguration(config =>
        {
            DateTimeOffset? ddr = default;
            DateTimeOffset? dd = default;
            DateTimeOffset? df = default;

            config.CreateMap<Erabliere, GetErabliereDashboard>()
                  .ForMember(d => d.Donnees, o => o.MapFrom(e => e.Donnees.Where(d => (ddr == null || d.D > ddr) &&
                                                                                      (dd == null || d.D >= dd) &&
                                                                                      (df == null || d.D <= df))))
                  .ForMember(d => d.Dompeux, o => o.MapFrom(e => e.Dompeux.Where(d => (ddr == null || d.T > ddr) &&
                                                                                      (dd == null || d.T >= dd) &&
                                                                                      (df == null || d.T <= df))))
                  .ForMember(d => d.Capteurs, o => o.MapFrom(e => e.Capteurs.Where(c => c.AfficherCapteurDashboard == true)))
                  .ReverseMap();

            config.CreateMap<Capteur, GetCapteursAvecDonnees>()
                  .ForMember(c => c.Donnees, o => o.MapFrom(e => e.DonneesCapteur.Where(d => (ddr == null || d.D > ddr) &&
                                                                                             (dd == null || d.D >= dd) &&
                                                                                             (df == null || d.D <= df))))
                  .ReverseMap();

            config.CreateMap<DonneeCapteur, GetDonneesCapteur>()
                  .ReverseMap();

            config.CreateMap<Dompeux, GetDompeux>()
                  .ReverseMap();

            config.CreateMap<Donnee, GetDonnee>()
                  .ReverseMap();

            config.CreateMap<Baril, GetBaril>()
                  .ReverseMap();
        });

        /// <summary>
        /// Créer une érablière
        /// </summary>
        /// <param name="erablieres">L'érablière à créer</param>
        /// <response code="200">L'érablière a été correctement ajouté</response>
        /// <response code="400">
        /// Le nom de l'érablière dépasse les 50 caractères, est null ou vide ou un érablière avec le nom reçu existe déjà.
        /// </response>
        [HttpPost]
        public async Task<ActionResult> Ajouter(PostErabliere erablieres)
        {
            if (string.IsNullOrWhiteSpace(erablieres.Nom))
            {
                return BadRequest($"Le nom de l'érablière ne peut pas être vide.");
            }
            if (await _context.Erabliere.AnyAsync(e => e.Nom == erablieres.Nom))
            {
                return BadRequest($"L'érablière nommé '{erablieres.Nom}' existe déjà");
            }

            await _context.Erabliere.AddAsync(_mapper.Map<Erabliere>(erablieres));

            await _context.SaveChangesAsync();

            return Ok();
        }

        /// <summary>
        /// Modifier une érablière
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
        [ValiderIPRules]
        public async Task<IActionResult> Modifier(int id, PutErabliere erabliere)
        {
            if (id != erabliere.Id)
            {
                return BadRequest($"L'id de la route ne concorde pas avec l'id de l'érablière à modifier.");
            }

            var entity = _context.Erabliere.Find(id);

            if (entity == null)
            {
                return BadRequest($"L'érablière que vous tentez de modifier n'existe pas.");
            }

            if (string.IsNullOrWhiteSpace(erabliere.Nom) == false && await _context.Erabliere.AnyAsync(e => e.Id != id && e.Nom == erabliere.Nom))
            {
                return BadRequest($"L'érablière avec le nom {erabliere.Nom}");
            }

            // fin des validations

            if (string.IsNullOrWhiteSpace(erabliere.Nom) == false)
            {
                entity.Nom = erabliere.Nom;
            }

            if (string.IsNullOrWhiteSpace(erabliere.IpRule) == false)
            {
                entity.IpRule = erabliere.IpRule;
            }

            if (erabliere.IndiceOrdre.HasValue)
            {
                entity.IndiceOrdre = erabliere.IndiceOrdre;
            }

            if (erabliere.AfficherSectionBaril.HasValue)
            {
                entity.AfficherSectionBaril = erabliere.AfficherSectionBaril;
            }

            if (erabliere.AfficherSectionDompeux.HasValue)
            {
                entity.AfficherSectionDompeux = erabliere.AfficherSectionDompeux;
            }

            if (erabliere.AfficherTrioDonnees.HasValue)
            {
                entity.AfficherTrioDonnees = erabliere.AfficherTrioDonnees;
            }

            _context.Erabliere.Update(entity);

            await _context.SaveChangesAsync();

            return Ok();
        }

        /// <summary>
        /// Supprimer une érablière
        /// </summary>
        /// <param name="id">L'identifiant de l'érablière</param>
        /// <param name="erabliere">L'érablière a supprimer</param>
        [HttpDelete("{id}")]
        [ValiderIPRules]
        [ProducesResponseType(204)]
        public async Task<IActionResult> Supprimer(int id, DeleteErabliere<int> erabliere)
        {
            if (id != erabliere.Id)
            {
                return BadRequest("L'id de la route ne concorde pas avec l'id de la donnée");
            }

            var entity = await _context.Erabliere.FindAsync(erabliere.Id);

            _context.Remove(entity);

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
