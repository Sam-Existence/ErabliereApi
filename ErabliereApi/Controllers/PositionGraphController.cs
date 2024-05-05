using AutoMapper;
using AutoMapper.QueryableExtensions;
using ErabliereApi.Attributes;
using ErabliereApi.Controllers.Attributes;
using ErabliereApi.Depot.Sql;
using ErabliereApi.Donnees;
using ErabliereApi.Donnees.Action.Get;
using ErabliereApi.Donnees.Action.Post;
using ErabliereApi.Donnees.Action.Put;
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
        /// <param name="token">Token d'annulation</param>
        [HttpPost]
        [ProducesResponseType(200, Type = typeof(PositionGraph))]
        [ValiderOwnership("id")]
        public async Task<IActionResult> Ajouter(Guid id, [FromBody] PostPositionGraph donneeRecu, CancellationToken token)
        {
            if (id != donneeRecu.IdErabliere)
            {
                return BadRequest($"L'id de la route '{id}' ne concorde pas avec l'id de l'érablière dans la donnée '{donneeRecu.IdErabliere}'.");
            }
            
            donneeRecu.D = DateTimeOffset.Now;

            var entity = await _context.PositionGraph.AddAsync(_mapper.Map<PositionGraph>(donneeRecu), token);

            await _context.SaveChangesAsync(token);

            return Ok(new { id = entity.Entity.Id }); 
        }

        /// <summary>
        /// Ajouter un donnée.
        /// </summary>
        /// <param name="id">L'identifiant de l'érablière</param>
        /// <param name="idDonnee">L'id de la donnée à modifier</param>
        /// <param name="donneeRecu">La donnée à ajouter</param>
        /// <param name="token">Token d'annulation</param>
        [HttpPut("{idPosition}")]
        [ProducesResponseType(200, Type = typeof(PositionGraph))]
        [ValiderOwnership("id")]
        public async Task<IActionResult> Modifier(Guid id, int idPosition, [FromBody] PutPositionGraph donneeRecu, CancellationToken token)
        {
            if (id != donneeRecu.IdErabliere)
            {
                return BadRequest($"L'id de la route '{id}' ne concorde pas avec l'id de l'érablière dans la donnée '{donneeRecu.IdErabliere}'.");
            }

            var entity = await _context.PositionGraph.FindAsync(idPosition);

            if (entity == null)
            {
                return NotFound();
            }

            _mapper.Map(donneeRecu, entity);

            await _context.SaveChangesAsync(token);

            return Ok(new { id = entity.Id });
        }
    }
}
