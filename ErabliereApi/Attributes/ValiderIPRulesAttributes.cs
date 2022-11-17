using ErabliereApi.Depot.Sql;
using ErabliereApi.Donnees;
using ErabliereApi.Extensions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Distributed;

namespace ErabliereApi.Attributes;

/// <summary>
/// Attribue permettant de restraindre les accès a un seul adresse IP selon l'id enregistrer dans les données de l'érablière.
/// </summary>
public class ValiderIPRulesAttribute : ActionFilterAttribute
{
    /// <summary>
    /// Contructeur par initialisation.
    /// </summary>
    /// <param name="order">Ordre d'exectuion des action filter</param>
    public ValiderIPRulesAttribute(int order = int.MinValue)
    {
        Order = order;
    }

    /// <inheritdoc />
    public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var id = context.ActionArguments["id"];

        var cache = context.HttpContext.RequestServices.GetRequiredService<IDistributedCache>();

        var erabliere = await cache.GetAsync<Erabliere>($"Erabliere_{id}", context.HttpContext.RequestAborted);

        if (erabliere == null) 
        {
            var depot = context.HttpContext.RequestServices.GetRequiredService<ErabliereDbContext>();

            erabliere = await depot.Erabliere.FindAsync(new object?[] { id }, context.HttpContext.RequestAborted);

            if (erabliere != null) 
            {
                await cache.SetAsync($"Erabliere_{id}", erabliere, context.HttpContext.RequestAborted);
            }
        }
        

        if (string.IsNullOrWhiteSpace(erabliere?.IpRule) == false && erabliere.IpRule != "-")
        {
            var ip = GetClientIp(context);

            if (context.ModelState.ContainsKey("X-Real-IP") == false &&
                erabliere.IpRule.Split(';').All(eIp => string.Equals(eIp, ip, StringComparison.OrdinalIgnoreCase) == false))
            {
                context.ModelState.AddModelError("IP", $"L'adresse IP est différente de l'adresse ip aloué pour créer des alimentations à cette érablière. L'adresse IP reçu est {ip}.");
            }
        }

        await next();
    }

    /// <summary>
    /// Permet d'extraire l'id de l'applant.
    /// Si l'entête X-Real-IP est présent, cette entête sera utilisé. 
    /// Sinon l'adresse ip sera retourner depuis la propriété RemoteIpAddress.
    /// </summary>
    /// <remarks>
    /// La présence de plus de une entête X-Real-IP ajoutera une erreur dans 
    /// le ModelState.
    /// </remarks>
    /// <param name="context"></param>
    /// <returns></returns>
    private static string GetClientIp(ActionExecutingContext context)
    {
        if (context.HttpContext.Request.Headers.ContainsKey("X-Real-IP"))
        {
            var ips = context.HttpContext.Request.Headers["X-Real-IP"];

            if (ips.Count == 1)
            {
                return ips.ToString();
            }

            else
            {
                context.ModelState.AddModelError("X-Real-IP", "Une seule entête 'X-Real-IP' doit être trouvé dans la requête.");
                return "";
            }
        }

        return context.HttpContext.Connection.RemoteIpAddress?.ToString() ?? throw new InvalidOperationException("Aucune adresse ip distante trouvé.");
    }

    private static void DebugHeaders(ActionExecutingContext context)
    {
        Console.WriteLine($"Debug headers connection id : {context.HttpContext.Connection.Id}");

        foreach (var header in context.HttpContext.Request.Headers)
        {
            foreach (var value in header.Value)
            {
                Console.WriteLine($"{header.Key}\t\t\t: {value}");
            }
        }

        Console.WriteLine("---");
    }
}
