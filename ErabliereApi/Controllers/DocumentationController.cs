using AutoMapper;
using ErabliereApi.Depot.Sql;
using ErabliereApi.Donnees.Action.Post;
using ErabliereApi.Donnees;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.OData.Query;
using ErabliereApi.Attributes;
using ErabliereApi.Donnees.Action.Put;

namespace ErabliereApi.Controllers;

/// <summary>
/// Contrôler représentant les documentations
/// </summary>
[ApiController]
[Route("Erablieres/{id}/[controller]")]
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
    [ValiderOwnership("id")]
    public IQueryable<Documentation> Lister(Guid id)
    {
        return _depot.Documentation.AsNoTracking().Where(d => d.IdErabliere == id);
    }

    /// <summary>
    /// Récupère la quantité de documentation
    /// </summary>
    /// <returns></returns>
    [HttpGet("Quantite")]
    [ProducesResponseType(200, Type = typeof(int))]
    public async Task<IActionResult> Compter(Guid id, CancellationToken token)
    {

        return Ok(await _depot.Documentation.CountAsync(d => d.IdErabliere == id, token));
    }

    /// <summary>
    /// Action permettant de télécharger le fichier relié à la documentation
    /// </summary>
    /// <param name="id">id de l'érablière</param>
    /// <param name="idDocumentation">id de la documentation</param>
    /// <param name="token"></param>
    /// <returns></returns>
    [HttpGet("{idDocumentation}/Fichier")]
    [ProducesResponseType(200, Type = typeof(FileStreamResult))]
    [ProducesResponseType(204)]
    [ValiderOwnership("id")]
    public async Task<IActionResult> TelechargerFichier(Guid id, Guid idDocumentation, CancellationToken token)
    {
        var documentation = await _depot.Documentation.FindAsync([idDocumentation], token);

        if (documentation == null)
        {
            return NotFound();
        }

        if (documentation.IdErabliere != id)
        {
            return BadRequest("Le document n'apportient pas à l'érbière indiqué");
        }

        if (documentation.File == null)
        {
            return NoContent();
        }

        var stream = new MemoryStream(documentation.File);

        return File(stream, "application/octet-stream", documentation.Title + '.' + documentation.FileExtension);
    }

    /// <summary>
    /// Action permettant d'ajouter une documentation
    /// </summary>
    /// <param name="id"></param>
    /// <param name="postDocumentation"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType(200, Type = typeof(Documentation))]
    [ValiderOwnership("id")]
    public async Task<IActionResult> Ajouter(Guid id, PostDocumentation postDocumentation, CancellationToken token)
    {
        if (id != postDocumentation.IdErabliere)
        {
            return BadRequest("L'id de la route ne concorde pas avec l'id de l'érablière possédant la note");
        }

        if (postDocumentation.Created == null)
        {
            postDocumentation.Created = DateTimeOffset.Now;
        }

        var entite = await _depot.Documentation.AddAsync(_mapper.Map<Documentation>(postDocumentation), token);

        await _depot.SaveChangesAsync(token);

        return Ok(entite.Entity);
    }

    /// <summary>
    /// Action permettant de modifier une documentation
    /// </summary>
    /// <param name="id">id de l'érablière</param>
    /// <param name="idDocumentation">id de la documentation</param>
    /// <param name="putDocumentation">nouvelle informations sur la documentation</param>
    /// <param name="token"></param>
    /// <returns></returns>
    [HttpPut("{idDocumentation}")]
    [ProducesResponseType(200, Type = typeof(Documentation))]
    [ValiderOwnership("id")]
    public async Task<IActionResult> Modifier(Guid id, Guid idDocumentation, PutDocumentation putDocumentation, CancellationToken token)
    {
        if (id != putDocumentation.IdErabliere)
        {
            return BadRequest("L'id de la route ne concorde pas avec l'id de l'érablière possédant la dcumentation");
        }

        if (idDocumentation != putDocumentation.Id)
        {
            return BadRequest("L'id de la route ne concorde pas avec l'id de la documentation");
        }

        var documentation = await _depot.Documentation.FindAsync([idDocumentation], token);

        if (documentation == null)
        {
            return NotFound();
        }

        if (putDocumentation.Title != null) 
        {
            documentation.Title = putDocumentation.Title;
        }

        if (putDocumentation.Text != null) 
        {
            documentation.Text = putDocumentation.Text;
        }

        if (putDocumentation.FileExtension != null) 
        {
            documentation.FileExtension = putDocumentation.FileExtension;
        }

        await _depot.SaveChangesAsync(token);

        return Ok(documentation);
    } 

    /// <summary>
    /// Action permettant de supprimer un document
    /// </summary>
    /// <param name="id">id de l'érablière</param>
    /// <param name="idDocumentation">id du document</param>
    /// <param name="token"></param>
    /// <returns></returns>
    [HttpDelete("{idDocumentation}")]
    [ProducesResponseType(204)]
    [ValiderOwnership("id")]
    public async Task<IActionResult> Supprimer(Guid id, Guid idDocumentation, CancellationToken token)
    {
        var documentation = await _depot.Documentation.FindAsync([idDocumentation], token);

        if (documentation == null)
        {
            return NotFound();
        }

        _depot.Documentation.Remove(documentation);

        await _depot.SaveChangesAsync(token);

        return NoContent();
    }
}
