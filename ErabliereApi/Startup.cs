using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using ErabliereApi.Donnees.AutoMapper;
using ErabliereApi.Depot.Sql;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using static System.Boolean;
using static System.Environment;
using static System.StringComparison;
using ErabliereApi.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Logging;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNet.OData.Extensions;
using Newtonsoft.Json.Serialization;
using Microsoft.Identity.Web;
using Microsoft.Extensions.Configuration;
using ErabliereApi.Extensions;
using StackExchange.Profiling;
using System.Data.Common;
using Microsoft.Data.SqlClient;
using StackExchange.Profiling.SqlFormatters;

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
                
                if (string.Equals(GetEnvironmentVariable("MiniProfiler.Enable"), TrueString, OrdinalIgnoreCase))
                {
                    o.Filters.Add<MiniProfilerAsyncLogger>();
                }
            })
            .AddNewtonsoftJson(o =>
            {
                o.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                o.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            });

            services.AddOData();

            // Forwarded headers
            services.AddErabliereAPIForwardedHeaders(Configuration);

            // Authentication
            if (string.Equals(GetEnvironmentVariable("USE_AUTHENTICATION"), TrueString, OrdinalIgnoreCase))
            {
                if (GetEnvironmentVariable("AzureAD__ClientId") != null && GetEnvironmentVariable("AzureAD:ClientId") == null)
                {
                    SetEnvironmentVariable("AzureAD:ClientId", GetEnvironmentVariable("AzureAD__ClientId"));
                }

                if (GetEnvironmentVariable("AzureAD__TenantId") != null && GetEnvironmentVariable("AzureAD:TenantId") == null)
                {
                    SetEnvironmentVariable("AzureAD:TenantId", GetEnvironmentVariable("AzureAD__TenantId"));
                }

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
                    var connectionString = GetEnvironmentVariable("SQL_CONNEXION_STRING") ?? throw new InvalidOperationException("La variable d'environnement 'SQL_CONNEXION_STRING' à une valeur null.");

                    if (string.Equals(GetEnvironmentVariable("MiniProlifer.EntityFramework.Enable"), TrueString, OrdinalIgnoreCase))
                    {
                        DbConnection connection = new SqlConnection(connectionString);
                        
                        connection = new StackExchange.Profiling.Data.ProfiledDbConnection(connection, MiniProfiler.Current);

                        options.UseSqlServer(connection, o => o.EnableRetryOnFailure());
                    }
                    else
                    {
                        options.UseSqlServer(connectionString, o => o.EnableRetryOnFailure());
                    }

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

            services.AddHealthChecks();

            if (string.Equals(GetEnvironmentVariable("MiniProfiler.Enable"), TrueString, OrdinalIgnoreCase))
            {
                var profilerBuilder = services.AddMiniProfiler(o =>
                {
                    if (string.Equals(GetEnvironmentVariable("MiniProlifer.EntityFramework.Enable"), TrueString, OrdinalIgnoreCase))
                    {
                        o.SqlFormatter = new SqlServerFormatter();
                    }
                });

                if (string.Equals(GetEnvironmentVariable("MiniProlifer.EntityFramework.Enable"), TrueString, OrdinalIgnoreCase))
                {
                    profilerBuilder.AddEntityFramework();
                }
            }
        }

        /// <summary>
        /// Configure
        /// </summary>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider, ILogger<Startup> logger)
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

            if (string.Equals(GetEnvironmentVariable("MiniProfiler.Enable"), TrueString, OrdinalIgnoreCase))
            {
                app.UseMiniProfiler();
            }

            app.UseErabliereAPIForwardedHeadersRules(logger, Configuration);

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
                endpoints.MapHealthChecks("/health");
            });

            app.UseSpa(spa =>
            {
                
            });
        }
    }
}
