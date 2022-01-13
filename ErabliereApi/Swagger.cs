using ErabliereApi.OperationFilter;
using Microsoft.OpenApi.Models;
using System.Reflection;
using static System.Boolean;
using static System.Environment;
using static System.StringComparison;

namespace ErabliereApi;

/// <summary>
/// Classe d'extension relier à l'intégration de swagger dans l'api
/// </summary>
public static class Swagger
{
    /// <summary>
    /// Ajout de swagger
    /// </summary>
    /// <returns></returns>
    public static IServiceCollection AjouterSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "ÉrablièreAPI",
                Description = "Un API pour assembler les informations de plusieurs appareils électronique de l'érablière",
                Contact = new OpenApiContact
                {
                    Name = "Frédéric Jacques",
                    Email = string.Empty,
                    Url = new Uri("https://github.com/freddycoder"),
                },
                License = new OpenApiLicense
                {
                    Name = "MIT license",
                    Url = new Uri("https://opensource.org/licenses/MIT"),
                }
            });

            if (string.Equals(GetEnvironmentVariable("USE_AUTHENTICATION"), TrueString, OrdinalIgnoreCase))
            {
                if (string.Equals(GetEnvironmentVariable("USE_SWAGGER_AUTHORIZATIONCODE_WORKFLOW"), TrueString, OrdinalIgnoreCase))
                {
                    c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                    {
                        Type = SecuritySchemeType.OAuth2,
                        Flows = new OpenApiOAuthFlows
                        {
                            AuthorizationCode = new OpenApiOAuthFlow
                            {
                                AuthorizationUrl = new Uri(GetEnvironmentVariable("SWAGGER_AUTHORIZATION_URL") ?? throw new ArgumentNullException(paramName: "SWAGGER_AUTHORIZATION_URL", message: "Si 'USE_SWAGGER_AUTHORIZATIONCODE_WORKFLOW' est à 'true', vous devez initialiser la variable 'SWAGGER_AUTHORIZATION_URL'.")),
                                TokenUrl = new Uri(GetEnvironmentVariable("SWAGGER_TOKEN_URL") ?? throw new ArgumentNullException(paramName: "SWAGGER_TOKEN_URL", message: "Si 'USE_SWAGGER_AUTHORIZATIONCODE_WORKFLOW' est à 'true', vous devez initialiser la variable 'SWAGGER_TOKEN_URL'.")),
                                Scopes = new Dictionary<string, string>
                                {
                                        { GetEnvironmentVariable("OIDC_SCOPES") ?? throw new ArgumentNullException("OIDC_SCOPES", "Si 'USE_SWAGGER_AUTHORIZATIONCODE_WORKFLOW' est à 'true', vous devez initialiser la variable 'OIDC_SCOPES'."), "Erabliere Api scope" }                                    }
                            }
                        }
                    });
                }

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "oauth2" }
                            },
                            new[] { GetEnvironmentVariable("OIDC_SCOPES") }
                        }
                });

                c.OperationFilter<AuthorizeCheckOperationFilter>();
            }

            c.OperationFilter<ValiderIPRulesOperationFilter>();

            c.OrderActionsBy(description => description.RelativePath);

            // Set the comments path for the Swagger JSON and UI.
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            c.IncludeXmlComments(xmlPath);
        });

        return services;
    }

    /// <summary>
    /// Utiliser le middleware swagger
    /// </summary>
    /// <param name="app"></param>
    public static IApplicationBuilder UtiliserSwagger(this IApplicationBuilder app)
    {
        app.UseSwagger(c =>
        {
            c.RouteTemplate = "api/{documentName}/swagger.json";
        });

        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("v1/swagger.json", "ÉrablièreAPI V1");
            c.RoutePrefix = "api";
            c.DocumentTitle = "ÉrablièreAPI - Swagger";
            c.ConfigObject.DisplayRequestDuration = true;

            if (string.Equals(GetEnvironmentVariable("USE_AUTHENTICATION"), TrueString, OrdinalIgnoreCase))
            {
                c.OAuthAppName("ÉrablièreAPI - Swagger");
                c.OAuthClientId(GetEnvironmentVariable("OIDC_CLIENT_ID"));
                c.OAuthClientSecret(GetEnvironmentVariable("OIDC_CLIENT_PASSWORD"));
                c.OAuth2RedirectUrl(GetEnvironmentVariable("OAUTH2_REDIRECT_URL"));
                c.OAuthScopes(GetEnvironmentVariable("OIDC_SCOPES"));

                if (string.Equals(GetEnvironmentVariable("USE_SWAGGER_PKCE"), TrueString, OrdinalIgnoreCase))
                {
                    c.OAuthUsePkce();
                }
            }
        });

        return app;
    }
}
