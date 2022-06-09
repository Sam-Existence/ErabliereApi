using AutoMapper;
using ErabliereApi.Depot.Sql;
using ErabliereApi.Donnees;
using ErabliereApi.Donnees.Action.Post;
using ErabliereApi.Donnees.Action.Put;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.EntityFrameworkCore;

namespace ErabliereApi.Controllers;

/// <summary>
/// Contrôler représentant les données des notes
/// </summary>
[ApiController]
[Route("Erablieres/{id}/[controller]")]
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
    [ProducesResponseType(200, Type = typeof(Note))]
    public async Task<IActionResult> Ajouter(Guid id, PostNote postNote, CancellationToken token)
    {
        if (id != postNote.IdErabliere)
        {
            return BadRequest("L'id de la route ne concorde pas avec l'érablière possédant la note");
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

        return Ok(entite.Entity);
    }

    /// <summary>
    /// Action permettant de modifier note
    /// </summary>
    /// <param name="id">L'id de l'érablière</param>
    /// <param name="noteId">L'id de la note</param>
    /// <param name="putNote"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    [HttpPut("{noteId}")]
    [ProducesResponseType(200, Type = typeof(Note))]
    public async Task<IActionResult> Modifier(Guid id, Guid noteId, PutNote putNote, CancellationToken token)
    {
        if (id != putNote.IdErabliere)
        {
            return BadRequest("L'id de la route ne concorde pas avec l'érablière possédant la note");
        }
        if (noteId != putNote.Id)
        {
            return BadRequest("L'id de la note dans la route ne concorde pas avec l'id de la note dans le corps du message.");
        }

        var entity = await _depot.Notes.FindAsync(new object?[] { noteId }, token);

        if (entity != null && entity.IdErabliere == id)
        {
            if (putNote.FileExtension != null)
            {
                entity.FileExtension = putNote.FileExtension;
            }

            await _depot.SaveChangesAsync(token);

            return Ok(entity);
        }
        else
        {
            return NotFound();
        }
    }

    /// <summary>
    /// Action permettant de supprimer une note
    /// </summary>
    /// <param name="id">L'id de l'érablière</param>
    /// <param name="noteId">L'id de la note</param>
    /// <param name="token"></param>
    /// <returns></returns>
    [HttpDelete("{noteId}")]
    [ProducesResponseType(200, Type = typeof(Note))]
    public async Task<IActionResult> Supprimer(Guid id, Guid noteId, CancellationToken token)
    {
        var entity = await _depot.Notes.FindAsync(new object?[] { noteId }, token);

        if (entity != null && entity.IdErabliere == id)
        {
            _depot.Notes.Remove(entity);

            await _depot.SaveChangesAsync(token);

            return Ok(entity);
        }
        else
        {
            return NotFound();
        }
    }
}
