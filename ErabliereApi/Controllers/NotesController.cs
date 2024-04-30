using AutoMapper;
using ErabliereApi.Attributes;
using ErabliereApi.Depot.Sql;
using ErabliereApi.Donnees;
using ErabliereApi.Donnees.Action.Post;
using ErabliereApi.Donnees.Action.Put;
using ErabliereApi.Extensions;
using ErabliereApi.Models;
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
    /// <param name="id">Id de l'érablière</param>
    /// <returns></returns>
    [HttpGet]
    [EnableQuery]
    [ValiderOwnership("id")]
    public IQueryable<Note> Lister(Guid id)
    {
        return _depot.Notes.AsNoTracking().Include(n => n.Rappel).Where(n => n.IdErabliere == id);
    }

    /// <summary>
    /// Obetenir l'image d'une note
    /// </summary>
    /// <param name="id">Id de l'érablière</param>
    /// <param name="noteId">Id de la note</param>
    /// <param name="token"></param>
    /// <returns></returns>
    /// <response code="200">Retourne l'image de la note</response>
    /// <response code="404">La note n'existe pas</response>
    /// <response code="500">Erreur interne</response>
    /// <response code="400">L'érablière ne possède pas la note</response>
    /// <response code="401">Non autorisé</response>
    /// <response code="403">Interdit</response>
    [HttpGet("{noteId}/Image")]
    [ProducesResponseType(200, Type = typeof(FileStreamResult))]
    [ValiderOwnership("id")]
    public async Task<IActionResult> ObtenirImage(Guid id, Guid noteId, CancellationToken token)
    {
        var note = await _depot.Notes.FindAsync([noteId], token);

        if (note == null)
        {
            return NotFound();
        }

        if (note.IdErabliere != id)
        {
            return BadRequest("L'érablière ne possède pas la note");
        }

        if (note.File == null)
        {
            return NoContent();
        }
        
        Response.Headers.Append("Cache-Control", "private, max-age=2592000");

        return File(note.File, $"image/{note.FileExtension ?? "jpg"}");
    }

    /// <summary>
    /// Récupère la quantité de notes
    /// </summary>
    /// <returns></returns>
    [HttpGet("Quantite")]
    [ProducesResponseType(200, Type = typeof(int))]
    public async Task<IActionResult> Compter(Guid id, CancellationToken token)
    {
        return Ok(await _depot.Notes.CountAsync(n => n.IdErabliere == id, token));
    }

    /// <summary>
    /// Action permettant d'ajouter une note
    /// </summary>
    /// <param name="id">Id de l'érablière</param>
    /// <param name="postNote"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType(200, Type = typeof(Note))]
    [ValiderOwnership("id")]
    public async Task<IActionResult> Ajouter(Guid id, PostNote postNote, CancellationToken token)
    {
        if (id != postNote.IdErabliere)
        {
            return BadRequest("L'id de la route ne concorde pas avec l'érablière possédant la note");
        }

        if (postNote.File != null && !postNote.IsValidBase64())
        {
            return BadRequest("Le fichier n'est pas en base64 valide");
        }

        if (postNote.Created == null)
        {
            postNote.Created = DateTimeOffset.Now;
        }

        if (postNote.NoteDate == null)
        {
            postNote.NoteDate = DateTimeOffset.Now;
        }

        var note = _mapper.Map<Note>(postNote);

        // Creer un rappel si le rappel est présent
        if (postNote.Rappel != null)
        {
            var allowedPeriodiciteValues = new[] { "annuel", "mensuel", "hebdo", "quotidien" };
            if (!allowedPeriodiciteValues.Contains(postNote.Rappel.Periodicite))
            {
                return BadRequest("Periodicité invalide. Doit être : Annuel, Mensuel, Hebdo, Quotidien");
            }

            note.Rappel = new Rappel
            {
                IdErabliere = postNote.IdErabliere,
                DateRappel = postNote.Rappel.DateRappel,
                Periodicite = postNote.Rappel.Periodicite,

                NoteId = note.Id
            };
        }

        var entite = await _depot.Notes.AddAsync(note, token);

        await _depot.SaveChangesAsync(token);

        entite.Entity.File = null;

        return Ok(entite.Entity);
    }

    /// <summary>
    /// Action permettant de créer une note en utilisant Content-Type: multipart/form-data
    /// </summary>
    /// <param name="id">Id de l'érablière</param>
    /// <param name="token"></param>
    /// <param name="postNoteMultipart"></param>
    /// <returns></returns>
    [HttpPost("multipart")]
    [ProducesResponseType(200, Type = typeof(PostNoteMultipartResponse))]
    [ValiderOwnership("id")]
    public async Task<IActionResult> AjouterMultipart(Guid id, CancellationToken token, [FromForm] PostNoteMultipart postNoteMultipart)
    {
        if (postNoteMultipart.File == null) 
        {
            return BadRequest("Le fichier est manquant");
        }

        var note = _mapper.Map<Note>(postNoteMultipart);

        note.File = await ToByteArray(postNoteMultipart.File, token);

        var entite = await _depot.Notes.AddAsync(note, token);

        await _depot.SaveChangesAsync(token);

        return Ok(_mapper.Map<PostNoteMultipartResponse>(entite.Entity));
    }

    private async Task<byte[]> ToByteArray(IFormFile file, CancellationToken token)
    {
        using var ms = new MemoryStream();

        await file.CopyToAsync(ms, token);

        return ms.ToArray();
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
    [ValiderOwnership("id")]
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

        var entity = await _depot.Notes.FindAsync([noteId], token);

        if (entity != null && entity.IdErabliere == id)
        {
            if (putNote.FileExtension != null)
            {
                entity.FileExtension = putNote.FileExtension;
            }

            if (putNote.NoteDate != null)
            {
                entity.NoteDate = putNote.NoteDate;
            }

            if (putNote.Text != null)
            {
                entity.Text = putNote.Text;
            }

            if (putNote.Title != null)
            {
                entity.Title = putNote.Title;
            }

            // Update rappel si le rappel est présent
            if (putNote.Rappel != null)
            {
                var allowedPeriodiciteValues = new[] { "annuel", "mensuel", "hebdo", "quotidien" };
                if (!allowedPeriodiciteValues.Contains(putNote.Rappel.Periodicite))
                {
                    return BadRequest("Periodicité invalide. Valeurs acceptées: annuel, mensuel, hebdo, quotidien");
                }

                var rappel = await _depot.Rappels.FirstOrDefaultAsync(r => r.NoteId == entity.Id, token);
                if (rappel != null)
                {
                    rappel.DateRappel = putNote.Rappel.DateRappel;
                    rappel.Periodicite = putNote.Rappel.Periodicite;
                }
                else
                {
                    entity.Rappel = new Rappel
                    {
                        NoteId = entity.Id,
                        IdErabliere = id,
                        DateRappel = putNote.Rappel.DateRappel,
                        Periodicite = putNote.Rappel.Periodicite
                    };
                }
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
    [ValiderOwnership("id")]
    public async Task<IActionResult> Supprimer(Guid id, Guid noteId, CancellationToken token)
    {
        var entity = await _depot.Notes.FindAsync([noteId], token);

        if (entity != null && entity.IdErabliere == id)
        {
            _depot.Notes.Remove(entity);

            await _depot.SaveChangesAsync(token);

            entity.File = null;

            return Ok(entity);
        }
        else
        {
            return NotFound();
        }
    }
}
