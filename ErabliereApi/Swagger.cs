﻿using ErabliereApi.Extensions;
using ErabliereApi.OperationFilter;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;
using System.Text.Json;
using static System.Boolean;
using static System.StringComparison;

namespace ErabliereApi;

/// <summary>
/// Classe d'extension relier à l'intégration de swagger dans l'api
/// </summary>
public static class Swagger
{
    static string? SwaggerJsonCache = null;

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
                    Name = config["SWAGGER_CONTACT_NAME"] ?? string.Empty,
                    Email = config["SWAGGER_CONTACT_EMAIL"] ?? string.Empty,
                    Url = new Uri("https://github.com/ErabliereApi"),
                },
                License = new OpenApiLicense
                {
                    Name = "Apache-2.0",
                    Url = new Uri("https://opensource.org/licenses/Apache-2.0"),
                }
            });

            if (config.IsAuthEnabled())
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
                                    { config["OIDC_SCOPES"] ?? throw new ArgumentNullException("OIDC_SCOPES", "Si 'USE_SWAGGER_AUTHORIZATIONCODE_WORKFLOW' est à 'true', vous devez initialiser la variable 'OIDC_SCOPES'."), "Erabliere Api scope" }                                   
                                }
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
            }
            
            if (config.StripeIsEnabled()) 
            {
                c.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
                {
                    Name = "X-ErabliereApi-ApiKey",
                    Type = SecuritySchemeType.ApiKey,
                    In = ParameterLocation.Header
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "ApiKey" }
                        },
                        new string[] { }
                    }
                });
            }

            c.OperationFilter<AuthorizeCheckOperationFilter>(config);

            c.OperationFilter<ValiderIPRulesOperationFilter>();
            c.OperationFilter<ODataOperationFilter>();

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
        // add a middleware that will save the first swagger document in cache and then return it for subsequent request
        app.Use(async (context, next) => {
            if (context.Request.Path.StartsWithSegments("/api/v1/swagger.json"))
            {
                if (SwaggerJsonCache != null)
                {
                    context.Response.StatusCode = 200;
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync(SwaggerJsonCache ?? "");
                    return;
                }
                else {
                    // create a memory stream to store the response
                    var originalBodyStream = context.Response.Body;
                    using var responseBody = new MemoryStream();
                    context.Response.Body = responseBody;
                    await next(context);
                    if (context.Response.StatusCode == 200 && 
                        context.Request.Path.StartsWithSegments("/api/v1/swagger.json"))
                    {
                        // save the response in cache
                        responseBody.Seek(0, SeekOrigin.Begin);
                        var reader = new StreamReader(responseBody);
                        var swaggerJson = await reader.ReadToEndAsync();
                        SwaggerJsonCache = swaggerJson;
                    }
                    // copy the response stream to the original stream
                    context.Response.Body.Seek(0, SeekOrigin.Begin);
                    await context.Response.Body.CopyToAsync(originalBodyStream);
                    context.Response.Body = originalBodyStream;
                    return;
                }
            }
            await next(context);
        });

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
            c.EnableTryItOutByDefault();

            if (string.Equals(config["USE_SWAGGER_DARK_THEME"], TrueString, OrdinalIgnoreCase))
            {
                c.InjectStylesheet("/swagger/swagger-custom.css");
            }

            if (config.IsAuthEnabled())
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
