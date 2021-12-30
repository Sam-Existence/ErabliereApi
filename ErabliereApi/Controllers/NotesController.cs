using AutoMapper;
using ErabliereApi.Depot.Sql;
using ErabliereApi.Donnees;
using ErabliereApi.Donnees.Action.Post;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ErabliereApi.Controllers
{
    /// <summary>
    /// Contrôler représentant les données des notes
    /// </summary>
    [ApiController]
    [Route("erablieres/{id}/[controller]")]
    [Authorize]
    public class NotesController : ControllerBase
    {
        private readonly ErabliereDbContext _depot;
        private readonly IMapper _mapper;

        /// <summary>
        /// Constructeur par défaut
        /// </summary>
        /// <param name="depot"></param>
        /// <param name="mapper"></param>
        public NotesController(ErabliereDbContext depot, IMapper mapper)
        {
            _depot = depot;
            _mapper = mapper;
        }

        /// <summary>
        /// Lister les notes avec les fonctionnalité de OData
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [EnableQuery]
        public IQueryable<Note> Lister(Guid id)
        {
            return _depot.Notes.AsNoTracking().Where(n => n.IdErabliere == id);
        }

        /// <summary>
        /// Action permettant d'ajouter une note
        /// </summary>
        /// <param name="id"></param>
        /// <param name="postNote"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Ajouter(Guid id, PostNote postNote, CancellationToken token)
        {
            if (id != postNote.IdErabliere)
            {
                return BadRequest("L'id de la route ne concorde pas avec l'id du baril à ajouter");
            }

            if (postNote.Created == null)
            {
                postNote.Created = DateTimeOffset.Now;
            }

            if (postNote.NoteDate == null)
            {
                postNote.NoteDate = DateTimeOffset.Now;
            }

            var entite = await _depot.Notes.AddAsync(_mapper.Map<Note>(postNote), token);

            await _depot.SaveChangesAsync(token);

            return Ok(entite);
        }
    }
}
