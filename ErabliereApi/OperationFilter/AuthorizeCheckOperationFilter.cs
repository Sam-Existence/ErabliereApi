using ErabliereApi.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using static System.Boolean;
using static System.StringComparison;

namespace ErabliereApi.OperationFilter;

/// <summary>
/// Ajout des indication pour les actions possédant l'attribut Authorize.
/// L'attribue sera déduit soit de la méthode ou de la classe.
/// </summary>
public class AuthorizeCheckOperationFilter : IOperationFilter
{
    private readonly IConfiguration _configuration;

    /// <summary>
    /// Constructeur
    /// </summary>
    /// <param name="configuration"></param>
    public AuthorizeCheckOperationFilter(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    /// <inheritdoc />
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var hasAuthorize = context.MethodInfo.DeclaringType?.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any() == true ||
                           context.MethodInfo.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any();

        var oneAuthMethodEnabled =
            string.Equals(_configuration["USE_AUTHENTICATION"], TrueString, OrdinalIgnoreCase) ||
            _configuration.StripeIsEnabled();

        if (hasAuthorize && oneAuthMethodEnabled)
        {
            operation.Responses.Add("401", new OpenApiResponse { Description = "Unauthorized" });
            operation.Responses.Add("403", new OpenApiResponse { Description = "Forbidden" });

            operation.Security = new List<OpenApiSecurityRequirement>();

            if (string.Equals(_configuration["USE_AUTHENTICATION"], TrueString, OrdinalIgnoreCase))
            {
                operation.Security.Add(new OpenApiSecurityRequirement
                {
                    [
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Id = "oauth2",
                                Type = ReferenceType.SecurityScheme
                            }
                        }
                    ] = new[] { "api1" }
                });
            }

            if (_configuration.StripeIsEnabled())
            {
                operation.Security.Add(new OpenApiSecurityRequirement
                {
                    [
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Id = "ApiKey",
                                Type = ReferenceType.SecurityScheme
                            }
                        }
                    ] = new[] { "api1" }
                });
            }
        }
    }
}
