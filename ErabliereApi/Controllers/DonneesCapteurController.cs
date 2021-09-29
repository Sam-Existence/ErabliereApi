using AutoMapper;
using AutoMapper.QueryableExtensions;
using ErabliereApi.Depot.Sql;
using ErabliereApi.Donnees;
using ErabliereApi.Donnees.Action.Post;
using ErabliereApi.Donnees.Generic;
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
    /// Contrôler représentant les données des dompeux
    /// </summary>
    [ApiController]
    [Route("Capteurs/{id}/[controller]")]
    [Authorize]
    public class DonneesCapteurController : ControllerBase
    {
        private readonly ErabliereDbContext _depot;
        private readonly IMapper _mapper;

        /// <summary>
        /// Constructeur par initialisation
        /// </summary>
        /// <param name="depot">Le dépôt des barils</param>
        /// <param name="mapper">Interface de mapping entre les objets</param>
        public DonneesCapteurController(ErabliereDbContext depot, IMapper mapper)
        {
            _depot = depot;
            _mapper = mapper;
        }

        /// <summary>
        /// Liste les DonneesCapteur
        /// </summary>
        /// <param name="id">Identifiant du capteur</param>
        /// <param name="ddr">Date de la dernière données reçu. Permet au client d'optimiser le nombres de données reçu.</param>
        /// <param name="dd">Date de début</param>
        /// <param name="df">Date de début</param>
        /// <response code="200">Une liste de DonneesCapteur.</response>
        [HttpGet]
        public async Task<IEnumerable<GetDonneesCapteur>> Lister(int id,
                                                                 [FromHeader(Name = "x-ddr")] DateTimeOffset? ddr,
                                                                 DateTimeOffset? dd, 
                                                                 DateTimeOffset? df)
        {
            var donnees = await _depot.DonneesCapteur.AsNoTracking()
                                .Where(b => b.IdCapteur == id &&
                                            (ddr == null || b.D >= ddr) &&
                                            (dd == null || b.D >= dd) &&
                                            (df == null || b.D <= df))
                                .OrderBy(b => b.D)
                                .ProjectTo<GetDonneesCapteur>(_mapper.ConfigurationProvider)
                                .ToArrayAsync();

            if (donnees.Length > 0)
            {
                if (ddr.HasValue)
                {
                    HttpContext.Response.Headers.Add("x-ddr", ddr.Value.ToString());
                }
                HttpContext.Response.Headers.Add("x-dde", donnees[^1].D.ToString());
            }

            return donnees;
        }

        /// <summary>
        /// Liste les données de plusieurs capteurs DonneesCapteur
        /// </summary>
        /// <param name="ids">Identifiant des capteurs</param>
        /// <param name="ddr">Date de la dernière données reçu. Permet au client d'optimiser le nombres de données reçu.</param>
        /// <param name="dd">Date de début</param>
        /// <param name="df">Date de début</param>
        /// <response code="200">Une liste Tupple avec l'id du catpeur et la liste des DonneesCapteur.</response>
        [HttpGet]
        [Route("/DonneesCapteur/Grape")]
        public async IAsyncEnumerable<Pair<int, IEnumerable<GetDonneesCapteur>>> ListerPlusieurs(
                                                    [FromQuery] string ids,
                                                    [FromHeader(Name = "x-ddr")] DateTimeOffset? ddr,
                                                    DateTimeOffset? dd,
                                                    DateTimeOffset? df)
        {
            foreach (var idstr in ids.Split(';'))
            {
                var id = int.Parse(idstr);

                yield return new Pair<int, IEnumerable<GetDonneesCapteur>>(id, await Lister(id, ddr, dd, df));
            }
        }

        /// <summary>
        /// Ajouter un capteur
        /// </summary>
        /// <param name="id">L'identifiant de l'érablière</param>
        /// <param name="donneeCapteur">Le capteur a ajouter</param>
        /// <response code="200">Le capteur a été correctement ajouté.</response>
        /// <response code="400">L'id de la route ne concorde pas avec l'id du capteur à ajouter.</response>
        [HttpPost]
        public async Task<IActionResult> Ajouter(int id, PostDonneeCapteur donneeCapteur)
        {
            if (id != donneeCapteur.IdCapteur)
            {
                return BadRequest("L'id de la route ne concorde pas avec l'id du capteur à ajouter");
            }

            if (donneeCapteur.D == null)
            {
                donneeCapteur.D = DateTimeOffset.Now;
            }

            await _depot.DonneesCapteur.AddAsync(_mapper.Map<DonneeCapteur>(donneeCapteur));

            await _depot.SaveChangesAsync();

            return Ok();
        }

        /// <summary>
        /// Modifier un capteur
        /// </summary>
        /// <param name="id">L'identifiant de l'érablière</param>
        /// <param name="capteur">Le capteur a modifier</param>
        /// <response code="200">Le capteur a été correctement supprimé.</response>
        /// <response code="400">L'id de la route ne concorde pas avec l'id du capteur à modifier.</response>
        [HttpPut]
        public async Task<IActionResult> Modifier(int id, DonneeCapteur capteur)
        {
            if (id != capteur.IdCapteur)
            {
                return BadRequest("L'id de la route ne concorde pas avec l'id du capteur à modifier.");
            }

            _depot.Update(capteur);

            await _depot.SaveChangesAsync();

            return Ok();
        }

        /// <summary>
        /// Supprimer un capteur
        /// </summary>
        /// <param name="id">Identifiant de l'érablière</param>
        /// <param name="capteur">Le capteur a supprimer</param>
        /// <response code="202">Le capteur a été correctement supprimé.</response>
        /// <response code="400">L'id de la route ne concorde pas avec l'id du capteur à supprimer.</response>
        [HttpDelete]
        public async Task<IActionResult> Supprimer(int id, DonneeCapteur capteur)
        {
            if (id != capteur.IdCapteur)
            {
                return BadRequest("L'id de la route ne concorde pas avec l'id du baril à supprimer.");
            }

            _depot.Remove(capteur);

            await _depot.SaveChangesAsync();

            return NoContent();
        }
    }
}
