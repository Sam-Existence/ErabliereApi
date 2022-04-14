using ErabliereApi.OperationFilter;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;
using static System.Boolean;
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
    public static IServiceCollection AjouterSwagger(this IServiceCollection services, IConfiguration config)
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

            if (string.Equals(config["USE_AUTHENTICATION"], TrueString, OrdinalIgnoreCase))
            {
                if (string.Equals(config["USE_SWAGGER_AUTHORIZATIONCODE_WORKFLOW"], TrueString, OrdinalIgnoreCase))
                {
                    c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                    {
                        Type = SecuritySchemeType.OAuth2,
                        Flows = new OpenApiOAuthFlows
                        {
                            AuthorizationCode = new OpenApiOAuthFlow
                            {
                                AuthorizationUrl = new Uri(config["SWAGGER_AUTHORIZATION_URL"] ?? throw new ArgumentNullException(paramName: "SWAGGER_AUTHORIZATION_URL", message: "Si 'USE_SWAGGER_AUTHORIZATIONCODE_WORKFLOW' est à 'true', vous devez initialiser la variable 'SWAGGER_AUTHORIZATION_URL'.")),
                                TokenUrl = new Uri(config["SWAGGER_TOKEN_URL"] ?? throw new ArgumentNullException(paramName: "SWAGGER_TOKEN_URL", message: "Si 'USE_SWAGGER_AUTHORIZATIONCODE_WORKFLOW' est à 'true', vous devez initialiser la variable 'SWAGGER_TOKEN_URL'.")),
                                Scopes = new Dictionary<string, string>
                                {
                                        { config["OIDC_SCOPES"] ?? throw new ArgumentNullException("OIDC_SCOPES", "Si 'USE_SWAGGER_AUTHORIZATIONCODE_WORKFLOW' est à 'true', vous devez initialiser la variable 'OIDC_SCOPES'."), "Erabliere Api scope" }                                    }
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
                            new[] { config["OIDC_SCOPES"] }
                        }
                });

                c.OperationFilter<AuthorizeCheckOperationFilter>();
            }

            c.OperationFilter<ValiderIPRulesOperationFilter>();

            c.OrderActionsBy(description => GetSortKey(description));

            // Set the comments path for the Swagger JSON and UI.
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            c.IncludeXmlComments(xmlPath);
        });

        return services;
    }

    private static string GetSortKey(ApiDescription description)
    {
        var sb = new StringBuilder();

        sb.Append(description.HttpMethod switch
        {
            "GET" => "1",
            "POST" => "2",
            "PUT" => "3",
            "DELETE" => "4",
            _ => "9"
        });

        return sb.ToString();
    }

    /// <summary>
    /// Utiliser le middleware swagger
    /// </summary>
    public static IApplicationBuilder UtiliserSwagger(this IApplicationBuilder app, IConfiguration config)
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
            c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);

            if (string.Equals(config["USE_SWAGGER_DARK_THEME"], TrueString, OrdinalIgnoreCase))
            {
                c.InjectStylesheet("/swagger/swagger-custom.css");
            }

            if (string.Equals(config["USE_AUTHENTICATION"], TrueString, OrdinalIgnoreCase))
            {
                c.OAuthAppName("ÉrablièreAPI - Swagger");
                c.OAuthClientId(config["OIDC_CLIENT_ID"]);
                c.OAuthClientSecret(config["OIDC_CLIENT_PASSWORD"]);
                c.OAuth2RedirectUrl(config["OAUTH2_REDIRECT_URL"]);
                c.OAuthScopes(config["OIDC_SCOPES"]);

                if (string.Equals(config["USE_SWAGGER_PKCE"], TrueString, OrdinalIgnoreCase))
                {
                    c.OAuthUsePkce();
                }
            }
        });

        return app;
    }
}
