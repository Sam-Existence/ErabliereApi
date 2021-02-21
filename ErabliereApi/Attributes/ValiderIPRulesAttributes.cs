using ErabliereApi.Depot;
using ErabliereApi.Donnees;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            var id = context.ActionArguments["id"];

            var depot = context.HttpContext.RequestServices.GetService(typeof(Depot<Erabliere>)) as Depot<Erabliere> ?? throw new InvalidProgramException($"Il doit y avoir un type enregistrer pour {typeof(Depot<Erabliere>)}.");

            var erabliere = depot.Obtenir(id);
            
            if (string.IsNullOrWhiteSpace(erabliere?.IpRule) == false && erabliere.IpRule != "-")
            {
                if (string.Equals(erabliere.IpRule, context.HttpContext.Connection.RemoteIpAddress.ToString(), StringComparison.OrdinalIgnoreCase) == false)
                {
                    context.ModelState.AddModelError("IP", $"L'adresse IP est différente de l'adresse ip aloué pour créer des alimentations à cette érablière. L'adresse IP reçu est {context.HttpContext.Connection.RemoteIpAddress}.");
                }
            }
        }
    }
}
