using AutoMapper;
using AutoMapper.QueryableExtensions;
using ErabliereApi.Attributes;
using ErabliereApi.Controllers.Attributes;
using ErabliereApi.Depot.Sql;
using ErabliereApi.Donnees;
using ErabliereApi.Donnees.Action.Get;
using ErabliereApi.Donnees.Action.Post;
using ErabliereApi.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Graph.Models.CallRecords;
using Microsoft.Identity.Client;
using System.Net.Mime;

namespace ErabliereApi.Controllers
{
    /// <summary>
    /// Controller pour la gestion des positions des graphiques
    /// </summary>
    [ApiController]
    [Route("Erablieres/{id}/[controller]")]
    [Authorize]
    public class PositionGraphController : ControllerBase
    {
        private readonly ErabliereDbContext _context;
        private readonly IMapper _mapper;

        /// <summary>
        /// Constructeur par initlisation
        /// </summary>
        /// <param name="context">Classe de contexte pour accéder aux données</param>
        /// <param name="mapper">Mapper entre les modèles</param>
        public PositionGraphController(ErabliereDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Lister les donne 
        /// </summary>
        /// <param name="id">Identifiant de l'érablière</param>
        /// <response code="200">Une liste de position potentiellement vide.</response>
        [ProducesResponseType(200, Type = typeof(GetPositonGraph))]
        [HttpGet]
        [Produces(MediaTypeNames.Application.Json, MediaTypeNames.Text.Plain, "text/json", "text/csv")]
        [ValiderOwnership("id")]
        public async Task<IActionResult> Lister(Guid id)
        {
            IEnumerable<GetPositonGraph> list;

            switch (HttpContext.Request.Headers["Accept"].ToString().ToLowerInvariant())
            {
                case "text/csv":
                    list = await ListerGenerique(id);

                    return File(list.AsCsvInByteArray(), "text/csv", $"{Guid.NewGuid()}.csv");
                default:
                    list = await ListerGenerique(id);

                    return Ok(list);
            }
        }

        private async Task<IEnumerable<GetPositonGraph>> ListerGenerique(Guid id)
        {
            var query = await _context.PositionGraph.AsNoTracking().Where(x => x.IdErabliere == id)
                .ProjectTo<GetPositonGraph>(_mapper.ConfigurationProvider).ToArrayAsync();
            
            return query;
        }

        /// <summary>
        /// Ajouter un donnée.
        /// </summary>
        /// <param name="id">L'identifiant de l'érablière</param>
        /// <param name="donneeRecu">La donnée à ajouter</param>
        /// <response code="200">La position a été correctement ajouté.</response>
        /// <response code="400">L'id de l'erabliere ne concorde pas avec l'id donne.</response>
        [HttpPost]
        [ValiderIPRules]
        [ValiderOwnership("id")]
        [TriggerAlert]
        public async Task<IActionResult> Ajouter(Guid id, [FromBody] PostPositionGraph donneeRecu)
        {
            if (id != donneeRecu.IdErabliere)
            {
                return BadRequest($"L'id de la route '{id}' ne concorde pas avec l'id de l'érablière dans la donnée '{donneeRecu.IdErabliere}'.");
            }

            if (donneeRecu.D == default || donneeRecu.D.Equals(DateTimeOffset.MinValue))
            {
                donneeRecu.D = DateTimeOffset.Now;
            }

            var entity = await _context.PositionGraph.AddAsync(_mapper.Map<PositionGraph>(donneeRecu));

            await _context.SaveChangesAsync();

            return Ok(new { id = entity.Entity.Id }); 
        }
    }
}
