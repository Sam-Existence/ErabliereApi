using AutoMapper;
using AutoMapper.QueryableExtensions;
using ErabliereApi.Attributes;
using ErabliereApi.Authorization;
using ErabliereApi.Depot.Sql;
using ErabliereApi.Donnees;
using ErabliereApi.Donnees.Action.Delete;
using ErabliereApi.Donnees.Action.Get;
using ErabliereApi.Donnees.Action.Post;
using ErabliereApi.Donnees.Action.Put;
using ErabliereApi.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Localization;

namespace ErabliereApi.Controllers;

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
    private readonly IConfiguration _config;
    private readonly IDistributedCache _cache;
    private readonly IServiceProvider _serviceProvider;
    private readonly IStringLocalizer<ErablieresController> _localizer;

    /// <summary>
    /// Constructeur par initialisation
    /// </summary>
    /// <param name="context">Classe de contexte pour accéder à la BD</param>
    /// <param name="mapper">mapper de donnée</param>
    /// <param name="config">Permet d'accéder au configuration de l'api</param>
    /// <param name="cache">Cache distribué</param>
    /// <param name="serviceProvider">Service scope</param>
    /// <param name="localizer">Localisateur de ressource</param>
    public ErablieresController(
        ErabliereDbContext context, 
        IMapper mapper, 
        IConfiguration config, 
        IDistributedCache cache,
        IServiceProvider serviceProvider,
        IStringLocalizer<ErablieresController> localizer)
    {
        _context = context;
        _mapper = mapper;
        _config = config;
        _cache = cache;
        _serviceProvider = serviceProvider;
        _localizer = localizer;
    }

    private const int TakeErabliereNbMax = 20;

    /// <summary>
    /// Liste les érablières
    /// </summary>
    /// <param name="my">
    /// Indique si les érablière retourné seront ceux ayant un lien 
    /// d'appartenance à l'usager authentifier.
    /// </param>
    /// <param name="token">Jeton d'annulation de la requête</param>
    /// <returns>Une liste d'érablière</returns>
    [HttpGet]
    [SecureEnableQuery(MaxTop = TakeErabliereNbMax)]
    [AllowAnonymous]
    public async Task<IQueryable<GetErabliere>> ListerAsync([FromQuery] bool my, CancellationToken token)
    {
        var query = _context.Erabliere.AsNoTracking().ProjectTo<GetErabliere>(_mapper.ConfigurationProvider);

        if (_config.IsAuthEnabled() &&
            (await IsAuthenticatedAsync(token)).Item1 == false)
        {
            query = query.Where(e => e.IsPublic == true);
        }
        else if (my)
        {
            var (isAuthenticate, authType, customer) = await IsAuthenticatedAsync(token);

            if (isAuthenticate == true && customer != null)
            {
                Guid?[] erablieresOwned
                    = await _context.CustomerErablieres
                    .AsNoTracking()
                    .Where(c => c.IdCustomer == customer.Id)
                    .Select(c => c.IdErabliere)
                    .ToArrayAsync(token);

                query = query.Where(e => erablieresOwned.Contains(e.Id));
            }
        }

        HttpContext.Response.Headers.Append("X-ErabliereTotal", (await query.CountAsync(token)).ToString());

        if (!HttpContext.Request.Query.TryGetValue("$orderby", out _))
        {
            query = query.OrderBy(e => e.IndiceOrdre).ThenBy(e => e.Nom);
        }

        if (!HttpContext.Request.Query.TryGetValue("$filter", out _))
        {
            if (HttpContext.Request.Query.TryGetValue("$top", out var top))
            {
                if (int.TryParse(top, out var topApply) && topApply > TakeErabliereNbMax)
                {
                    query = query.Take(TakeErabliereNbMax);
                }
            }
            else
            {
                query = query.Take(TakeErabliereNbMax);
            }
        }

        return query;
    }

    /// <summary>
    /// Point de terminaison pour l'administration des érablières
    /// </summary>
    /// <returns>Une liste d'érablières</returns>
    /// <response code="200">Les érablières ont été correctement récupérées</response>
    [HttpGet]
    [EnableQuery]
    [Route("/Admin/Erablieres")]
    [Authorize(Roles = "administrateur", Policy = "TenantIdPrincipal")]
    [ProducesResponseType(200, Type = typeof(Erabliere[]))]
    public async Task<IActionResult> GetErablieresAdmin(CancellationToken token)
    {
        return Ok(await _context.Erabliere.AsNoTracking().ToArrayAsync(token));
    }

    /// <summary>
    /// Obtenir les accès des utilisateurs à une érablière
    /// </summary>
    /// <response code="200">Les droits d'accès de l'érablère demandé</response>
    /// <response code="400">L'érablière demandé n'existe pas</response>
    [HttpGet("{id}/[action]")]
    [ValiderIPRules]
    [ValiderOwnership("id")]
    [ProducesResponseType(200, Type = typeof(GetCustomerAccess))]
    public async Task<IActionResult> CustomersAccess(Guid id, CancellationToken token)
    {
        var erabliere = await _context.Erabliere.FindAsync([id], cancellationToken: token);

        if (erabliere == null)
        {
            return NotFound();
        }

        var customers = await _context.CustomerErablieres.AsNoTracking()
            .Where(c => c.IdErabliere == id)
            .ProjectTo<GetCustomerAccess>(_mapper.ConfigurationProvider)
            .ToArrayAsync(token);

        return Ok(customers);
    }

    /// <summary>
    /// Créer une érablière
    /// </summary>
    /// <param name="postErabliere">L'érablière à créer</param>
    /// <param name="token">Le jeton d'annulation de la requête http</param>
    /// <response code="200">L'érablière a été correctement ajouté</response>
    /// <response code="400">
    /// Le nom de l'érablière dépasse les 50 caractères, est null ou vide ou un érablière avec le nom reçu existe déjà.
    /// </response>
    /// <response code="409">La clé primaire de l'érablière existe déjà</response>
    [HttpPost]
    public async Task<IActionResult> Ajouter(PostErabliere postErabliere, CancellationToken token)
    {
        if (string.IsNullOrWhiteSpace(postErabliere.Nom))
        {
            ModelState.AddModelError(nameof(postErabliere.Nom), _localizer["NomVide"]);

            return BadRequest(new ValidationProblemDetails(ModelState));
        }
        if (await _context.Erabliere.AnyAsync(e => e.Nom == postErabliere.Nom, token))
        {
            ModelState.AddModelError(nameof(postErabliere.Nom), string.Format(_localizer["NomExiste"], postErabliere.Nom));

            return BadRequest(new ValidationProblemDetails(ModelState));
        }
        if (postErabliere.Id != null) 
        {
            var e = await _context.Erabliere.FindAsync([postErabliere.Id], token);

            if (e != null) 
            {
                ModelState.AddModelError(nameof(postErabliere.Id), string.Format(_localizer["IdExiste"], postErabliere.Id));
        
                return Conflict(new ValidationProblemDetails(ModelState));
            }
        }

        var erabliere = _mapper.Map<Erabliere>(postErabliere);

        var (isAuthenticate, authType, customer) = await IsAuthenticatedAsync(token);

        if (isAuthenticate)
        {
            erabliere.IsPublic = false;

            if (postErabliere.IsPublic != erabliere.IsPublic)
            {
                erabliere.IsPublic = postErabliere.IsPublic;
            }
        }
        else
        {
            erabliere.IsPublic = true;
            
            if (postErabliere.IsPublic != erabliere.IsPublic)
            {
                ModelState.AddModelError("IsPublic", _localizer["EnforceIsPublic"]);

                return BadRequest(new ValidationProblemDetails(ModelState));
            }
        }

        var entity = await _context.Erabliere.AddAsync(erabliere, token);

        if (isAuthenticate)
        {
            if (customer != null)
            {
                await _context.CustomerErablieres.AddAsync(new CustomerErabliere
                {
                    Access = 15,
                    IdCustomer = customer.Id,
                    IdErabliere = entity.Entity.Id
                }, token);
            }
            else
            {
                throw new InvalidOperationException("The user is authenticated, but there is no customer...");
            }
        }

        await _context.SaveChangesAsync(token);

        return Ok(new { id = entity.Entity.Id });
    }

    private async Task<(bool, string, Customer?)> IsAuthenticatedAsync(CancellationToken token)
    {
        if (User?.Identity?.IsAuthenticated == true)
        {
            var unique_name = UsersUtils.GetUniqueName(_serviceProvider.CreateScope(), User);

            var customer = await _context.Customers.SingleAsync(c => c.UniqueName == unique_name, token);

            return (true, "Bearer", customer);
        }

        if (_config.StripeIsEnabled())
        {
            var apiKeyAuthContext = HttpContext?.RequestServices.GetRequiredService<ApiKeyAuthorizationContext>();

            return (apiKeyAuthContext?.Authorize == true, "ApiKey", apiKeyAuthContext?.Customer);
        }

        return (false, "", null);
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
    [ValiderOwnership("id")]
    public async Task<IActionResult> Modifier(Guid id, PutErabliere erabliere)
    {
        if (id != erabliere.Id)
        {
            return BadRequest($"L'id de la route ne concorde pas avec l'id de l'érablière à modifier.");
        }

        var entity = await _context.Erabliere.FindAsync(id);

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

        if (erabliere.CodePostal != null)
        {
            entity.CodePostal = erabliere.CodePostal;
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

        if (erabliere.IsPublic.HasValue)
        {
            entity.IsPublic = erabliere.IsPublic.Value;
        }

        _context.Erabliere.Update(entity);

        await _context.SaveChangesAsync();

        await _cache.RemoveAsync($"Erabliere_{id}");

        return Ok();
    }

    /// <summary>
    /// Action permettant de creer/modifier les droits d'accès d'un utilisateur 
    /// à une érablière.
    /// </summary>
    /// <param name="id">L'id de l'érablière</param>
    /// <param name="customerErabliere">Les informations sur les droits d'accès</param>
    /// <param name="token">Le token d'annulation</param>
    /// <response code="200">Les droits d'accès ont été correctement modifié.</response>
    /// <response code="400">Une des validations des paramètres à échoué.</response>
    /// <response code="404">L'érablière n'existe pas.</response>
    [HttpPut("{id}/CustomerErabliere")]
    [ValiderIPRules]
    [ValiderOwnership("id")]
    [ProducesResponseType(200)]
    public async Task<IActionResult> Modifier(Guid id, PutCustomerErabliere customerErabliere, CancellationToken token)
    {
        if (id != customerErabliere.IdErabliere)
        {
            return BadRequest("L'id de la route ne concorde pas avec l'id dans le corps du message");
        }

        var erabliere = await _context.Erabliere.FindAsync(new object[] { id }, token);

        if (erabliere == null)
        {
            return NotFound();
        }

        if (customerErabliere.CustomerErablieres != null)
        {
            for (int i = 0; i < customerErabliere.CustomerErablieres.Count; i++)
            {
                var action = customerErabliere.CustomerErablieres[i];

                switch (action.Action)
                {
                    case PutSingleCustomerErabliereAction.Create:
                        if (await _context.CustomerErablieres.AnyAsync(
                            c => c.IdCustomer == action.IdCustomer && c.IdErabliere == id, token))
                        {
                            return BadRequest($"L'utilisateur avec l'id {action.IdCustomer} a déjà un droit d'accès à l'érablière avec l'id {id}.");
                        }

                        await _context.CustomerErablieres.AddAsync(new CustomerErabliere
                        {
                            Access = action.Access,
                            IdCustomer = action.IdCustomer,
                            IdErabliere = id
                        }, token);
                        break;
                    case PutSingleCustomerErabliereAction.Edit:
                        var customerErabliereToEdit = await _context.CustomerErablieres.SingleAsync(c => c.IdCustomer == action.IdCustomer && c.IdErabliere == id, token);
                        customerErabliereToEdit.Access = action.Access;
                        break;
                    case PutSingleCustomerErabliereAction.Delete:
                        var customerErabliereToDelete = await _context.CustomerErablieres.SingleAsync(c => c.IdCustomer == action.IdCustomer && c.IdErabliere == id, token);
                        _context.CustomerErablieres.Remove(customerErabliereToDelete);
                        break;
                    default:
                        throw new InvalidOperationException($"L'action {action.Action} n'est pas supporté.");
                }

                await _context.SaveChangesAsync(token);

                // Get the unique name of the user to delete
                var userToDelete = await _context.Customers.FindAsync(new object?[] { action.IdCustomer }, token);

                if (userToDelete != null) 
                {
                    await _cache.RemoveAsync($"CustomerWithAccess_{userToDelete.UniqueName}_{id}");
                }
            }
        }

        return Ok();
    }

    /// <summary>
    /// Supprimer une érablière
    /// </summary>
    /// <param name="id">L'identifiant de l'érablière</param>
    /// <param name="erabliere">L'érablière a supprimer</param>
    [HttpDelete("{id}")]
    [ValiderIPRules]
    [ValiderOwnership("id")]
    [ProducesResponseType(204)]
    public async Task<IActionResult> Supprimer(Guid id, DeleteErabliere<Guid> erabliere)
    {
        if (id != erabliere.Id)
        {
            return BadRequest("L'id de la route ne concorde pas avec l'id de la donnée");
        }

        var entity = await _context.Erabliere.FindAsync(erabliere.Id);

        if (entity != null)
        {
            _context.Remove(entity);

            await _context.SaveChangesAsync();

            await _cache.RemoveAsync($"Erabliere_{id}");
        }

        return NoContent();
    }

    /// <summary>
    /// Supprimer une érablière en tant qu'administrateur
    /// </summary>
    /// <param name="id">L'identifiant de l'érablière</param>
    /// <param name="token">Un jeton d'annulation</param>
    /// <response code="204">L'érablière a été correctement supprimée</response>
    /// <response code="404">L'érablière n'a pas été trouvée</response>
    [HttpDelete]
    [Route("/Admin/Erablieres/{id}")]
    [Authorize(Roles = "administrateur", Policy = "TenantIdPrincipal")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> DeleteErablieresAdmin(Guid id, CancellationToken token)
    {
        var entity = await _context.Erabliere.Include(e => e.CustomerErablieres).FirstOrDefaultAsync(e => e.Id == id, token);

        if (entity != null)
        {
            _context.Remove(entity);

            await _context.SaveChangesAsync(token);

            await _cache.RemoveAsync($"Erabliere_{id}", token);

            return NoContent();
        }

        return NotFound();
    }

    /// <summary>
    /// Supprimer les droits d'accès d'un utilisateur à une érablière
    /// </summary>
    /// <param name="id">L'identifiant de l'érablière</param>
    /// <param name="idCustomer">L'id du client</param>
    /// <param name="token">Le token d'annulation</param>
    [HttpDelete("{id}/Customer/{idCustomer}/Access/")]
    [ValiderIPRules]
    [ValiderOwnership("id")]
    [ProducesResponseType(204)]
    public async Task<IActionResult> SupprimerCustomerErabliere(Guid id, Guid idCustomer, CancellationToken token)
    {
        var entity = await _context.CustomerErablieres.FindAsync(new object?[] { idCustomer, id }, token);

        if (entity != null && entity.IdErabliere == id)
        {
            // Valider que l'utilisateur n'est pas en train de supprimer son propre droit d'accès
            var authInfo = await IsAuthenticatedAsync(token);

            if (entity.IdCustomer != authInfo.Item3?.Id)
            {
                // Get the unique name of the user to delete
                var userToDelete = await _context.Customers.FindAsync(new object?[] { entity.IdCustomer }, token);

                if (userToDelete != null) 
                {
                    await _cache.RemoveAsync($"CustomerWithAccess_{userToDelete.UniqueName}_{id}");
                }

                _context.Remove(entity);

                await _context.SaveChangesAsync(token);
            }
            else 
            {
                return BadRequest("Vous ne pouvez pas supprimer votre propre droit d'accès.");
            }
        }

        return NoContent();
    }
}
