using AutoMapper;
using AutoMapper.QueryableExtensions;
using ErabliereApi.Depot.Sql;
using ErabliereApi.Donnees;
using ErabliereApi.Donnees.Action.Get;
using ErabliereApi.Donnees.Action.Post;
using ErabliereApi.Donnees.Action.Put;
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
    [Route("erablieres/{id}/[controller]")]
    [Authorize]
    public class CapteursController : ControllerBase
    {
        private readonly ErabliereDbContext _depot;
        private readonly IMapper _mapper;

        /// <summary>
        /// Constructeur par initialisation
        /// </summary>
        /// <param name="depot">Le dépôt des barils</param>
        /// <param name="mapper">Interface de mapping entre les données</param>
        public CapteursController(ErabliereDbContext depot, IMapper mapper)
        {
            _depot = depot;
            _mapper = mapper;
        }

        /// <summary>
        /// Liste les capteurs
        /// </summary>
        /// <param name="id">Identifiant de l'érablière</param>
        /// <param name="filtreNom">Permet de filtrer les capteurs recherché selon leur nom</param>
        /// <response code="200">Une liste de capteurs.</response>
        [HttpGet]
        public async Task<IEnumerable<GetCapteurs>> Lister(int id, string? filtreNom)
        {
            return await _depot.Capteurs.AsNoTracking()
                                .Where(b => b.IdErabliere == id &&
                                       (filtreNom == null || (b.Nom != null && b.Nom.Contains(filtreNom) == true)))
                                .ProjectTo<GetCapteurs>(_mapper.ConfigurationProvider)
                                .ToArrayAsync();
        }

        /// <summary>
        /// Ajouter un capteur
        /// </summary>
        /// <param name="id">L'identifiant de l'érablière</param>
        /// <param name="capteur">Le capteur a ajouter</param>
        /// <response code="200">Le capteur a été correctement ajouté.</response>
        /// <response code="400">L'id de la route ne concorde pas avec l'id du capteur à ajouter.</response>
        [HttpPost]
        public async Task<IActionResult> Ajouter(int id, PostCapteur capteur)
        {
            if (id != capteur.IdErabliere)
            {
                return BadRequest("L'id de la route n'est pas le même que l'id de l'érablière dans les données du capteur à ajouter");
            }

            if (capteur.DC == null)
            {
                capteur.DC = DateTimeOffset.Now;
            }

            await _depot.Capteurs.AddAsync(_mapper.Map<Capteur>(capteur));

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
        public async Task<IActionResult> Modifier(int id, PutCapteur capteur)
        {
            if (id != capteur.IdErabliere)
            {
                return BadRequest("L'id de la route ne concorde pas avec l'id du baril à modifier.");
            }

            var capteurEntity = await _depot.Capteurs.FindAsync(capteur.Id);

            if (capteurEntity == null || capteurEntity.IdErabliere != capteur.IdErabliere) 
            {
                return BadRequest("Le capteur à modifier n'existe pas.");
            }

            if (capteur.AfficherCapteurDashboard.HasValue)
            {
                capteurEntity.AfficherCapteurDashboard = capteur.AfficherCapteurDashboard;
            }

            if (capteur.DC.HasValue) 
            {
                capteurEntity.DC = capteur.DC;
            }

            if (string.IsNullOrWhiteSpace(capteur.Nom) == false) 
            {
                capteurEntity.Nom = capteur.Nom;
            }

            _depot.Update(capteurEntity);

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
        public async Task<IActionResult> Supprimer(int id, Capteur capteur)
        {
            if (id != capteur.IdErabliere)
            {
                return BadRequest("L'id de la route ne concorde pas avec l'id du baril à supprimer.");
            }

            _depot.Remove(capteur);

            await _depot.SaveChangesAsync();

            return NoContent();
        }
    }
}
