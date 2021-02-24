using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace ErabliereApi
{
    /// <summary>
    /// Classe d'extension relier à l'intégration de swagger dans l'api
    /// </summary>
    public static class SwaggerServiceCollectionExtension
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

                //c.AddSecurityDefinition("oauth", new OpenApiSecurityScheme
                //{
                //    Flows = new OpenApiOAuthFlows
                //    {

                //    }
                //});

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
                c.PreSerializeFilters.Add((swagger, httpReq) =>
                {
                    var serverUrl = Environment.GetEnvironmentVariable("SWAGGER_SERVER_URL");

                    if (string.IsNullOrWhiteSpace(serverUrl)) 
                    {
                        serverUrl = $"{httpReq.Scheme}://{httpReq.Host.Value}";
                    }

                    swagger.Servers = new List<OpenApiServer> { new OpenApiServer { Url = serverUrl } };
                });
            });

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("v1/swagger.json", "ÉrablièreAPI V1");
                c.RoutePrefix = "api";
                c.DocumentTitle = "ÉrablièreAPI - Swagger";
            });

            return app;
        }
    }
}
