using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
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
using Microsoft.OData.Edm;
using ErabliereApi.Donnees;
using Microsoft.AspNet.OData.Extensions;
using Newtonsoft.Json.Serialization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Identity.Web;
using Microsoft.Extensions.Configuration;

namespace ErabliereApi
{
    /// <summary>
    /// Classe Startup de l'api
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Configuration of the app. Use when AzureAD authentication method is used.
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Constructor of the startup class with the configuration object in parameter
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Méthodes ConfigureServices
        /// </summary>
        public void ConfigureServices(IServiceCollection services)
        {
            // contrôleur
            services.AddControllers(o =>
            {
                o.EnableEndpointRouting = false;
            })
            .AddNewtonsoftJson(o =>
            {
                o.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                o.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            });

            services.AddOData();

            // Forwarded headers
            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            });

            // Authentication
            if (string.Equals(GetEnvironmentVariable("USE_AUTHENTICATION"), TrueString, OrdinalIgnoreCase))
            {
                if (string.IsNullOrWhiteSpace(GetEnvironmentVariable("AzureAD:ClientId")) == false)
                {
                    services.AddMicrosoftIdentityWebApiAuthentication(Configuration);
                }
                else
                {
                    services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                            .AddIdentityServerAuthentication(options =>
                            {
                                options.Authority = GetEnvironmentVariable("OIDC_AUTHORITY");

                                options.ApiName = "erabliereapi";
                            });
                }
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
            if (string.Equals(GetEnvironmentVariable("USE_SQL"), TrueString, OrdinalIgnoreCase))
            {
                services.AddDbContext<ErabliereDbContext>(options =>
                {
                    options.UseSqlServer(GetEnvironmentVariable("SQL_CONNEXION_STRING") ?? throw new InvalidOperationException("La variable d'environnement 'SQL_CONNEXION_STRING' à une valeur null."));

                    if (string.Equals(GetEnvironmentVariable("LOG_SQL"), "Console", OrdinalIgnoreCase))
                    {
                        options.LogTo(Console.WriteLine, LogLevel.Information);
                    }
                });
            }
            else
            {
                services.AddDbContext<ErabliereDbContext>(options =>
                {
                    options.UseInMemoryDatabase(nameof(ErabliereDbContext));
                });
            }
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
                endpoints.EnableDependencyInjection();
                endpoints.Select().Expand().Filter().Count().MaxTop(100).OrderBy();
                endpoints.MapControllers();
            });

            app.UseSpa(spa =>
            {
                
            });
        }
    }
}
