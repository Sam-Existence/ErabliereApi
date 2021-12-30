using AutoMapper;
using ErabliereApi.Depot.Sql;
using ErabliereApi.Donnees.Action.Post;
using ErabliereApi.Donnees;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Threading;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.OData.Query;

namespace ErabliereApi.Controllers
{
    /// <summary>
    /// Contrôler représentant les documentations
    /// </summary>
    [ApiController]
    [Route("erablieres/{id}/[controller]")]
    [Authorize]
    public class DocumentationController : ControllerBase
    {
        private readonly ErabliereDbContext _depot;
        private readonly IMapper _mapper;

        /// <summary>
        /// Constructeur par défaut
        /// </summary>
        /// <param name="depot"></param>
        /// <param name="mapper"></param>
        public DocumentationController(ErabliereDbContext depot, IMapper mapper)
        {
            _depot = depot;
            _mapper = mapper;
        }

        /// <summary>
        /// Lister la documentation avec les fonctionnalité de OData
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [EnableQuery]
        public IQueryable<Documentation> Lister(Guid id)
        {
            return _depot.Documentation.AsNoTracking().Where(d => d.IdErabliere == id);
        }

        /// <summary>
        /// Action permettant d'ajouter une documentation
        /// </summary>
        /// <param name="id"></param>
        /// <param name="postDocumentation"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Ajouter(Guid id, PostDocumentation postDocumentation, CancellationToken token)
        {
            if (id != postDocumentation.IdErabliere)
            {
                return BadRequest("L'id de la route ne concorde pas avec l'id du baril à ajouter");
            }

            if (postDocumentation.Created == null)
            {
                postDocumentation.Created = DateTimeOffset.Now;
            }

            var entite = await _depot.Documentation.AddAsync(_mapper.Map<Documentation>(postDocumentation), token);

            await _depot.SaveChangesAsync(token);

            return Ok(entite);
        }
    }
}
