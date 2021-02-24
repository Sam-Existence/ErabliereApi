using ErabliereApi.Depot;
using ErabliereApi.Donnees;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace ErabliereApi.Attributes
{
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
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (string.Equals(Environment.GetEnvironmentVariable("DEBUG_HEADERS"), bool.TrueString, StringComparison.OrdinalIgnoreCase))
            {
                DebugHeaders(context);
            }

            var id = context.ActionArguments["id"];

            var depot = context.HttpContext.RequestServices.GetService(typeof(Depot<Erabliere>)) as Depot<Erabliere> ?? throw new InvalidProgramException($"Il doit y avoir un type enregistrer pour {typeof(Depot<Erabliere>)}.");

            var erabliere = depot.Obtenir(id);
            
            if (string.IsNullOrWhiteSpace(erabliere?.IpRule) == false && erabliere.IpRule != "-")
            {
                var ip = GetClientIp(context);

                if (string.Equals(erabliere.IpRule, ip, StringComparison.OrdinalIgnoreCase) == false)
                {
                    context.ModelState.AddModelError("IP", $"L'adresse IP est différente de l'adresse ip aloué pour créer des alimentations à cette érablière. L'adresse IP reçu est {ip}.");
                }
            }
        }

        private string GetClientIp(ActionExecutingContext context)
        {
            if (context.HttpContext.Request.Headers.ContainsKey("X-Real-IP"))
            {
                var ips = context.HttpContext.Request.Headers["X-Real-IP"];

                if (ips.Count == 1)
                {
                    return ips;
                }

                else
                {
                    context.ModelState.AddModelError("X-Real-IP", "Une seule entête 'X-Real-IP' doit être trouvé dans la requête.");
                }
            }

            return context.HttpContext.Connection.RemoteIpAddress.ToString();
        }

        private void DebugHeaders(ActionExecutingContext context)
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
}
