using ErabliereApi.Attributes;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ErabliereApi.OperationFilter;

/// <summary>
/// Ajout des indication pour les actions possédant l'attribut Authorize.
/// L'attribue sera déduit soit de la méthode ou de la classe.
/// </summary>
public class ValiderIPRulesOperationFilter : IOperationFilter
{
    /// <inheritdoc />
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var hasIPRulesValidation = context.MethodInfo.DeclaringType?.GetCustomAttributes(true).OfType<ValiderIPRulesAttribute>().Any() == true ||
                                   context.MethodInfo.GetCustomAttributes(true).OfType<ValiderIPRulesAttribute>().Any();

        if (hasIPRulesValidation)
        {
            operation.Responses.Add("400", new OpenApiResponse
            {
                Description = "Bad Request - IP Invalide"
            });
        }
    }
}
