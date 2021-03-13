using ErabliereApi.OperationFilter;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using static System.Boolean;
using static System.Environment;
using static System.StringComparison;

namespace ErabliereApi
{
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
                            OpenIdConnectUrl = new Uri(GetEnvironmentVariable("OIDC_AUTHORITY") + "/.well-known/openid-configuration"),
                            Flows = new OpenApiOAuthFlows
                            {
                                AuthorizationCode = new OpenApiOAuthFlow
                                {
                                    AuthorizationUrl = new Uri(GetEnvironmentVariable("SWAGGER_AUTHORIZATION_URL") ?? throw new ArgumentNullException("Si 'USE_SWAGGER_AUTHORIZATIONCODE_WORKFLOW' est à 'true', vous devez initialiser la variable 'SWAGGER_AUTHORIZATION_URL'.")),
                                    TokenUrl = new Uri(GetEnvironmentVariable("SWAGGER_TOKEN_URL") ?? throw new ArgumentNullException("Si 'USE_SWAGGER_AUTHORIZATIONCODE_WORKFLOW' est à 'true', vous devez initialiser la variable 'SWAGGER_TOKEN_URL'.")),
                                    Scopes = new Dictionary<string, string>
                                    {
                                        { "openid", "Request an OpenID Connect ID Token" },
                                        { "offline", "A scope required when requesting refresh tokens (alias for ```offline_access```)" },
                                        { "offline_access", "A scope required when requesting refresh tokens" }
                                    }
                                }
                            }
                        });
                    }
                    if (string.Equals(GetEnvironmentVariable("USE_SWAGGER_IMPLICIT_WORKFLOW"), TrueString, OrdinalIgnoreCase))
                    {
                        c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                        {
                            Type = SecuritySchemeType.OAuth2,
                            OpenIdConnectUrl = new Uri(GetEnvironmentVariable("OIDC_AUTHORITY") + "/.well-known/openid-configuration"),
                            Flows = new OpenApiOAuthFlows
                            {
                                Implicit = new OpenApiOAuthFlow
                                {
                                    AuthorizationUrl = new Uri(GetEnvironmentVariable("SWAGGER_AUTHORIZATION_URL") ?? throw new ArgumentNullException("Si 'USE_SWAGGER_IMPLICIT_WORKFLOW' est à 'true', vous devez initialiser la variable 'SWAGGER_AUTHORIZATION_URL'.")),
                                    Scopes = new Dictionary<string, string>
                                    {
                                        { "openid", "Request an OpenID Connect ID Token" },
                                        { "offline", "A scope required when requesting refresh tokens (alias for ```offline_access```)" },
                                        { "offline_access", "A scope required when requesting refresh tokens" }
                                    }
                                }
                            }
                        });
                    }
                    if (string.Equals(GetEnvironmentVariable("USE_SWAGGER_CLIENTCREDENTIALS_WORKFLOW"), TrueString, OrdinalIgnoreCase))
                    {
                        c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                        {
                            Type = SecuritySchemeType.OAuth2,
                            OpenIdConnectUrl = new Uri(GetEnvironmentVariable("OIDC_AUTHORITY") + "/.well-known/openid-configuration"),
                            Flows = new OpenApiOAuthFlows
                            {
                                ClientCredentials = new OpenApiOAuthFlow
                                {
                                    AuthorizationUrl = new Uri(GetEnvironmentVariable("SWAGGER_AUTHORIZATION_URL") ?? throw new ArgumentNullException("Si 'USE_SWAGGER_CLIENTCREDENTIALS_WORKFLOW' est à 'true', vous devez initialiser la variable 'SWAGGER_AUTHORIZATION_URL'.")),
                                    TokenUrl = new Uri(GetEnvironmentVariable("SWAGGER_TOKEN_URL") ?? throw new ArgumentNullException("Si 'USE_SWAGGER_CLIENTCREDENTIALS_WORKFLOW' est à 'true', vous devez initialiser la variable 'SWAGGER_TOKEN_URL'.")),
                                    Scopes = new Dictionary<string, string>
                                    {
                                        { "openid", "Request an OpenID Connect ID Token" },
                                        { "offline", "A scope required when requesting refresh tokens (alias for ```offline_access```)" },
                                        { "offline_access", "A scope required when requesting refresh tokens" }
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
                            new[] { "openid", "offline", "offline_access" }
                        }
                    });

                    c.OperationFilter<AuthorizeCheckOperationFilter>();
                    c.OperationFilter<ValiderIPRulesOperationFilter>();
                }

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            services.AddSwaggerGenNewtonsoftSupport();

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
                    c.OAuthScopes("offline", "offline_access", "openid");

                    if (string.Equals(GetEnvironmentVariable("USE_SWAGGER_PKCE"), TrueString, OrdinalIgnoreCase))
                    {
                        c.OAuthUsePkce();
                    }
                }

                if (string.Equals(GetEnvironmentVariable("SWAGGER_XSRF_TOKEN_INTERCEPTOR"), TrueString, OrdinalIgnoreCase))
                {
                    c.UseRequestInterceptor("(req) => { req.headers['X-XSRF-Token'] = localStorage.getItem('xsrf-token'); return req; }");
                }
            });

            return app;
        }
    }
}
