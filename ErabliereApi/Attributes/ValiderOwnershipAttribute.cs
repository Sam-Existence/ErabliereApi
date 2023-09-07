using ErabliereApi.Depot.Sql;
using ErabliereApi.Donnees;
using ErabliereApi.Donnees.Ownable;
using ErabliereApi.Extensions;
using ErabliereApi.Services;
using ErabliereApi.Services.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Distributed;

namespace ErabliereApi.Attributes;

/// <summary>
/// Vérifier si l'utilisateur à les droits d'accès sur la ressource qu'il tente d'accéder ou de modifier
/// en vérifiant le verbe http et les droits dans la table CustomerErablieres
/// </summary>
public class ValiderOwnershipAttribute : ActionFilterAttribute
{
    private readonly string _idParamName;
    private readonly Type? _levelTwoRelationType;

    /// <summary>
    /// Valider les droits d'accès
    /// </summary>
    /// <param name="idParamName">Le nom du paramètre de route pour l'id de l'érablière</param>
    /// <param name="levelTwoRelationType">Type référencé dans l'arboressence des relations</param>
    public ValiderOwnershipAttribute(string idParamName, Type? levelTwoRelationType = null)
    {
        if (levelTwoRelationType != null &&
            !levelTwoRelationType.GetInterfaces().Any(i => i == typeof(IErabliereOwnable)))
        {
            throw new ArgumentException($"The type of arg {nameof(levelTwoRelationType)} must implement {nameof(ILevelTwoOwnable<IErabliereOwnable>)}");
        }

        _idParamName = idParamName;
        _levelTwoRelationType = levelTwoRelationType;
    }

    /// <summary>
    /// Verif if the user has the necessary access right
    /// </summary>
    /// <param name="context"></param>
    /// <param name="next"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var allowAccess = true;

        var strId = context.HttpContext.Request.RouteValues[_idParamName]?.ToString();

        if (strId == null)
        {
            throw new InvalidOperationException($"Route value {_idParamName} does not exist");
        }

        var config = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();

        if (config.IsAuthEnabled()) 
        {
            var dbContext = context.HttpContext.RequestServices.GetRequiredService<ErabliereDbContext>();
            var cache = context.HttpContext.RequestServices.GetRequiredService<IDistributedCache>();

            Erabliere? erabliere = await GetErabliere(dbContext, cache, strId, context.HttpContext.RequestAborted);

            // Valider les droits d'accès sur l'érablière
            // Si l'érablière a été trouvé
            // Si l'érablière est publique et que l'accès est en lecture, l'accès est autorisé
            if (erabliere == null) 
            {
                context.Result = new NotFoundResult();
            }
            else if (erabliere != null && 
                (erabliere.IsPublic == true && context.HttpContext.Request.Method == "GET") == false)
            {
                var userService = context.HttpContext.RequestServices.GetRequiredService<IUserService>();

                var customer = await userService.GetCurrentUserWithAccessAsync(erabliere, context.HttpContext.RequestAborted);

                if (customer == null)
                {
                    throw new InvalidOperationException("Customer should exist at this point...");
                }

                if (customer.CustomerErablieres == null || customer.CustomerErablieres.Count == 0)
                {
                    allowAccess = false;
                }
                else
                {
                    var type = context.HttpContext.Request.Method switch
                    {
                        "GET" => 1,
                        "POST" => 2,
                        "PUT" => 4,
                        "DELETE" => 8,
                        _ => throw new InvalidOperationException($"Ownership not implement for HTTP Verb {context.HttpContext.Request.Method}"),
                    };

                    for (int i = 0; i < customer.CustomerErablieres.Count && allowAccess; i++)
                    {
                        var access = customer.CustomerErablieres[i].Access;

                        allowAccess = (access & type) > 0;
                    }
                }
            }
        }

        if (allowAccess)
        {
            await base.OnActionExecutionAsync(context, next);
        }
        else
        {
            context.HttpContext.Response.Headers["X-ErabliereApi-ForbidenReason"] = "Access Denied for this action on this resources";
            context.Result = new ForbidResult();
        }
    }

    private async Task<Erabliere?> GetErabliere(ErabliereDbContext context, IDistributedCache cache, string strId, CancellationToken token)
    {
        var idGuid = Guid.Parse(strId);

        if (_levelTwoRelationType != null)
        {
            var entity = await context.FindAsync(_levelTwoRelationType, new object?[] { idGuid }, token);

            if (entity == null)
            {
                return null;
            }

            var instance = entity as IErabliereOwnable;

            if (instance == null)
            {
                throw new InvalidOperationException($"type {entity.GetType().Name} cannot be convert into {nameof(IErabliereOwnable)}");
            }

            if (instance.IdErabliere.HasValue == false)
            {
                return null;
            }

            idGuid = instance.IdErabliere.Value;
        }

        var erabliere = await cache.GetAsync<Erabliere>($"Erabliere_{idGuid}", token);

        if (erabliere == null) 
        {
            erabliere = await context.Erabliere.FindAsync(new object?[] { idGuid }, token);

            if (erabliere != null) 
            {
                await cache.SetAsync($"Erabliere_{idGuid}", erabliere, token);
            }
        }

        return erabliere;
    }
}
