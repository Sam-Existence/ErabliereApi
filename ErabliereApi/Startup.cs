using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using ErabliereApi.Donnees.AutoMapper;
using ErabliereApi.Depot.Sql;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Logging;
using static System.Boolean;
using static System.Environment;
using static System.StringComparison;
using ErabliereApi.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Logging;
using IdentityServer4.AccessTokenValidation;
using System.Text.Json.Serialization;
using Microsoft.OData.Edm;
using ErabliereApi.Donnees;
using Microsoft.Net.Http.Headers;
using System.Linq;
using Microsoft.AspNetCore.OData;
using Microsoft.OData.ModelBuilder;

namespace ErabliereApi
{
    /// <summary>
    /// Classe Startup de l'api
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// M�thodes ConfigureServices
        /// </summary>
        public void ConfigureServices(IServiceCollection services)
        {
            // contrôleur
            services.AddControllers(o =>
            {
                o.EnableEndpointRouting = false;
            })
            .AddJsonOptions(o =>
            {
                o.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            });

            // Forwarded headers
            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            });

            // Authentication
            if (string.Equals(GetEnvironmentVariable("USE_AUTHENTICATION"), TrueString, OrdinalIgnoreCase))
            {
                services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                        .AddIdentityServerAuthentication(options =>
                        {
                            options.Authority = GetEnvironmentVariable("OIDC_AUTHORITY");
                            
                            options.ApiName = "erabliereapi";
                        });
            }
            else
            {
                services.AddSingleton<IAuthorizationHandler, AllowAnonymous>();
            }

            // Swagger
            services.AjouterSwagger();

            // Cors
            if (string.Equals(GetEnvironmentVariable("USE_CORS"), TrueString, OrdinalIgnoreCase))
            {
                services.AddCors();
            }

            // Automapper
            services.AjouterAutoMapperErabliereApiDonnee();

            // Database
            if (string.Equals(GetEnvironmentVariable("USE_SQL"), FalseString, OrdinalIgnoreCase))
            {
                services.AddDbContext<ErabliereDbContext>(options =>
                {
                    options.UseInMemoryDatabase(nameof(ErabliereDbContext));
                });
            }
            else
            {
                services.AddDbContext<ErabliereDbContext>(options =>
                {
                    options.UseSqlServer(GetEnvironmentVariable("SQL_CONNEXION_STRING") ?? throw new InvalidOperationException("La variable d'environnement 'SQL_CONNEXION_STRING' � une valeur null."));

                    if (string.Equals(GetEnvironmentVariable("LOG_SQL"), "Console", OrdinalIgnoreCase))
                    {
                        options.LogTo(Console.WriteLine, LogLevel.Information);
                    }
                });
            }

            services.AddOData(o => 
            {
                o.Select();
                o.Filter();
                o.Expand();
                o.Filter();
                o.OrderBy();
                o.Count();
                o.MaxTop = 100;
                o.AddModel("odata", GetEdmModel());
            });
        }

        /// <summary>
        /// Configure
        /// </summary>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            if (string.Equals(GetEnvironmentVariable("USE_SQL"), TrueString, OrdinalIgnoreCase) &&
                string.Equals(GetEnvironmentVariable("SQL_USE_STARTUP_MIGRATION"), TrueString, OrdinalIgnoreCase))
            {
                var database = serviceProvider.GetRequiredService<ErabliereDbContext>();

                database.Database.Migrate();
            }

            if (env.IsDevelopment())
            {
                IdentityModelEventSource.ShowPII = true;
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseForwardedHeaders();
            }

            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UtiliserSwagger();

            app.UseRouting();

            if (string.Equals(GetEnvironmentVariable("USE_CORS"), TrueString, OrdinalIgnoreCase))
            {
                app.UseCors(option =>
                {
                    option.WithHeaders(GetEnvironmentVariable("CORS_HEADERS")?.Split(','));
                    option.WithMethods(GetEnvironmentVariable("CORS_METHODS")?.Split(','));
                    option.WithOrigins(GetEnvironmentVariable("CORS_ORIGINS")?.Split(','));
                });
            }

            if (string.Equals(GetEnvironmentVariable("USE_AUTHENTICATION"), TrueString, OrdinalIgnoreCase))
            {
                app.UseAuthentication();
            }
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSpa(spa =>
            {
                
            });
        }

        private IEdmModel GetEdmModel()
        {
            var builder = new ODataConventionModelBuilder();

            builder.EntitySet<Alerte>(nameof(ErabliereDbContext.Alertes));
            builder.EntitySet<Baril>(nameof(ErabliereDbContext.Barils));
            builder.EntitySet<Capteur>(nameof(ErabliereDbContext.Capteurs));
            builder.EntitySet<Dompeux>(nameof(ErabliereDbContext.Dompeux));
            builder.EntitySet<Donnee>(nameof(ErabliereDbContext.Donnees));
            builder.EntitySet<DonneeCapteur>(nameof(ErabliereDbContext.DonneesCapteur));
            builder.EntitySet<Erabliere>(nameof(ErabliereDbContext.Erabliere));

            return builder.GetEdmModel();
        }
    }
}
