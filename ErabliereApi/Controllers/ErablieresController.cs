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

    /// <summary>
    /// Constructeur par initialisation
    /// </summary>
    /// <param name="context">Classe de contexte pour accéder à la BD</param>
    /// <param name="mapper">mapper de donnée</param>
    /// <param name="config">Permet d'accéder au configuration de l'api</param>
    public ErablieresController(ErabliereDbContext context, IMapper mapper, IConfiguration config)
    {
        _context = context;
        _mapper = mapper;
        _config = config;
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
    [EnableQuery(MaxTop = TakeErabliereNbMax)]
    [AllowAnonymous]
    public async Task<IQueryable<Erabliere>> ListerAsync([FromQuery] bool my, CancellationToken token)
    {
        var query = _context.Erabliere.AsNoTracking();

        if (_config.IsAuthEnabled() &&
            User.Identity?.IsAuthenticated == false)
        {
            query = query.Where(e => e.IsPublic == true);
        }
        else if (my)
        {
            var isAuth = await IsAuthenticatedAsync(token);

            if (isAuth.Item1 == true && isAuth.Item3 != null)
            {
                Guid?[] erablieresOwned
                    = await _context.CustomerErablieres
                    .AsNoTracking()
                    .Where(c => c.IdCustomer == isAuth.Item3.Id)
                    .Select(c => c.IdErabliere)
                    .ToArrayAsync(token);

                query = query.Where(e => erablieresOwned.Contains(e.Id));
            }
        }

        HttpContext.Response.Headers.Add("X-ErabliereTotal", (await query.CountAsync(token)).ToString());

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
    /// Obtenir les données pour la page principale d'ErabliereIU
    /// </summary>
    /// <returns>Une liste d'érablière</returns>
    [HttpGet("[action]")]
    [ProducesResponseType(200, Type = typeof(GetErabliereDashboard[]))]
    public async Task<IActionResult> Dashboard(DateTimeOffset? dd, DateTimeOffset? df, DateTimeOffset? ddr, CancellationToken token, int nbErabliere = TakeErabliereNbMax)
    {
        if (nbErabliere < 0)
        {
            return BadRequest($"Le paramètre {nbErabliere} ne peut pas être négatif.");
        }

        if (dd == null && df == null)
        {
            dd = DateTime.Now - TimeSpan.FromHours(12);
        }

        var dashboardData = await _context.Erabliere.AsNoTracking()
            .ProjectTo<GetErabliereDashboard>(_dashboardMapper, new { dd, df, ddr })
            .OrderBy(e => e.IndiceOrdre)
            .ThenBy(e => e.Nom)
            .Take(nbErabliere)
            .ToArrayAsync(token);

        return Ok(dashboardData);
    }

    private static readonly AutoMapper.IConfigurationProvider _dashboardMapper = new MapperConfiguration(config =>
    {
        DateTimeOffset? ddr = default;
        DateTimeOffset? dd = default;
        DateTimeOffset? df = default;

        config.CreateMap<Erabliere, GetErabliereDashboard>()
              .ForMember(d => d.Donnees, o => o.MapFrom(e => e.Donnees.Where(d => (ddr == null || d.D > ddr) &&
                                                                                  (dd == null || d.D >= dd) &&
                                                                                  (df == null || d.D <= df))))
              .ForMember(d => d.Dompeux, o => o.MapFrom(e => e.Dompeux.Where(d => (ddr == null || d.T > ddr) &&
                                                                                  (dd == null || d.T >= dd) &&
                                                                                  (df == null || d.T <= df))))
              .ForMember(d => d.Capteurs, o => o.MapFrom(e => e.Capteurs.Where(c => c.AfficherCapteurDashboard == true)))
              .ReverseMap();

        config.CreateMap<Capteur, GetCapteursAvecDonnees>()
              .ForMember(c => c.Donnees, o => o.MapFrom(e => e.DonneesCapteur.Where(d => (ddr == null || d.D > ddr) &&
                                                                                         (dd == null || d.D >= dd) &&
                                                                                         (df == null || d.D <= df))))
              .ReverseMap();

        config.CreateMap<DonneeCapteur, GetDonneesCapteur>()
              .ReverseMap();

        config.CreateMap<Dompeux, GetDompeux>()
              .ReverseMap();

        config.CreateMap<Donnee, GetDonnee>()
              .ReverseMap();

        config.CreateMap<Baril, GetBaril>()
              .ReverseMap();
    });

    /// <summary>
    /// Créer une érablière
    /// </summary>
    /// <param name="postErabliere">L'érablière à créer</param>
    /// <param name="token">Le jeton d'annulation de la requête http</param>
    /// <response code="200">L'érablière a été correctement ajouté</response>
    /// <response code="400">
    /// Le nom de l'érablière dépasse les 50 caractères, est null ou vide ou un érablière avec le nom reçu existe déjà.
    /// </response>
    [HttpPost]
    public async Task<ActionResult> Ajouter(PostErabliere postErabliere, CancellationToken token)
    {
        if (string.IsNullOrWhiteSpace(postErabliere.Nom))
        {
            return BadRequest($"Le nom de l'érablière ne peut pas être vide.");
        }
        if (await _context.Erabliere.AnyAsync(e => e.Nom == postErabliere.Nom, token))
        {
            return BadRequest($"L'érablière nommé '{postErabliere.Nom}' existe déjà");
        }

        var erabliere = _mapper.Map<Erabliere>(postErabliere);

        var (isAuthenticate, authType, customer) = await IsAuthenticatedAsync(token);

        if (isAuthenticate)
        {
            erabliere.IsPublic = false;
        }
        else
        {
            erabliere.IsPublic = true;
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
            var unique_name = User.FindFirst("unique_name")?.Value ?? "";

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

        _context.Erabliere.Update(entity);

        await _context.SaveChangesAsync();

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
        }

        return NoContent();
    }
}
