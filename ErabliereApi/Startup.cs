using ErabliereApi.Depot;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using ErabliereApi.Donnees.Action.Post;
using ErabliereApi.Donnees;
using ErabliereApi.Depot.Sql;
using Microsoft.EntityFrameworkCore;
using ErabliereApi.Donnees.Action.Get;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Logging;
using static System.Boolean;
using static System.Environment;
using static System.StringComparison;
using ErabliereApi.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Logging;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace ErabliereApi
{
    /// <summary>
    /// Classe Startup de l'api
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Constructeur d'initialisation
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Configuration
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Méthodes ConfigureServices
        /// </summary>
        public void ConfigureServices(IServiceCollection services)
        {
            // contrôleur
            services.AddControllers();

            // Forwarded headers
            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            });

            // Authentication
            if (string.Equals(GetEnvironmentVariable("USE_AUTHENTICATION"), TrueString, OrdinalIgnoreCase))
            {
                services.Configure<CookiePolicyOptions>(options =>
                {
                    options.CheckConsentNeeded = context => true;
                    options.MinimumSameSitePolicy = Microsoft.AspNetCore.Http.SameSiteMode.Lax;
                });

                services.AddAuthentication(options =>
                {
                    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
                })
                .AddCookie()
                .AddOpenIdConnect(o =>
                {
                    o.ClientId = GetEnvironmentVariable("OIDC_CLIENT_ID");
                    o.ClientSecret = GetEnvironmentVariable("OIDC_CLIENT_PASSWORD");
                    o.Authority = GetEnvironmentVariable("OIDC_AUTHORITY");
                    o.RequireHttpsMetadata = true;

                    o.AuthenticationMethod = OpenIdConnectRedirectBehavior.RedirectGet;
                    o.ResponseMode = OpenIdConnectResponseMode.Fragment;
                    o.ResponseType = OpenIdConnectResponseType.Code;
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
            services.AddAutoMapper(config =>
            {
                config.CreateMap<Dompeux, GetDompeux>();

                config.CreateMap<PostErabliere, Erabliere>();
                config.CreateMap<PostDonnee, Donnee>();
            });

            // Database
            if (string.Equals(GetEnvironmentVariable("USE_SQL"), FalseString, OrdinalIgnoreCase))
            {
                services.AddDbContext<ErabliereDbContext>(options =>
                {
                    options.UseInMemoryDatabase(Guid.NewGuid().ToString());
                });
            }
            else
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
                app.UseCookiePolicy();
                app.UseAuthentication();
                app.UseAuthorization();
            }
            else
            {
                app.UseAuthorization();
            }

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
