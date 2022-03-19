using ErabliereApi.Donnees.AutoMapper;
using ErabliereApi.Depot.Sql;
using Microsoft.EntityFrameworkCore;
using static System.Boolean;
using static System.StringComparison;
using ErabliereApi.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Logging;
using IdentityServer4.AccessTokenValidation;
using Microsoft.Identity.Web;
using ErabliereApi.Extensions;
using StackExchange.Profiling;
using System.Data.Common;
using Microsoft.Data.SqlClient;
using StackExchange.Profiling.SqlFormatters;
using Microsoft.AspNetCore.OData;
using System.Text.Json.Serialization;
using System.Text.Json;
using Prometheus;
using ErabliereApi.HealthCheck;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace ErabliereApi;

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
            if (string.Equals(Configuration["MiniProfiler.Enable"], TrueString, OrdinalIgnoreCase))
            {
                o.Filters.Add<MiniProfilerAsyncLogger>();
            }
        })
        .AddOData(o =>
        {
            o.Select().Filter().OrderBy().SetMaxTop(100).Expand();
        })
        .AddJsonOptions(c =>
        {
            c.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
            c.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        });

        // Forwarded headers
        services.AddErabliereAPIForwardedHeaders(Configuration);

        // Authentication
        if (string.Equals(Configuration["USE_AUTHENTICATION"], TrueString, OrdinalIgnoreCase))
        {
            if (Configuration["AzureAD__ClientId"] != null && Configuration["AzureAD:ClientId"] == null)
            {
                Configuration["AzureAD:ClientId"] = Configuration["AzureAD__ClientId"];
            }

            if (Configuration["AzureAD__TenantId"] != null && Configuration["AzureAD:TenantId"] == null)
            {
                Configuration["AzureAD:TenantId"] = Configuration["AzureAD__TenantId"];
            }

            if (string.IsNullOrWhiteSpace(Configuration["AzureAD:ClientId"]) == false)
            {
                services.AddMicrosoftIdentityWebApiAuthentication(Configuration);
            }
            else
            {
                services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                        .AddIdentityServerAuthentication(options =>
                        {
                            options.Authority = Configuration["OIDC_AUTHORITY"];

                            options.ApiName = "erabliereapi";
                        });
            }
        }
        else
        {
            services.AddSingleton<IAuthorizationHandler, AllowAnonymous>();
        }

        // Swagger
        services.AjouterSwagger(Configuration);

        // Cors
        if (string.Equals(Configuration["USE_CORS"], TrueString, OrdinalIgnoreCase))
        {
            services.AddCors();
        }

        // Automapper
        services.AjouterAutoMapperErabliereApiDonnee();

        // Database
        if (string.Equals(Configuration["USE_SQL"], TrueString, OrdinalIgnoreCase))
        {
            services.AddDbContext<ErabliereDbContext>(options =>
            {
                var connectionString = Configuration["SQL_CONNEXION_STRING"] ?? throw new InvalidOperationException("La variable d'environnement 'SQL_CONNEXION_STRING' à une valeur null.");

                if (string.Equals(Configuration["MiniProlifer.EntityFramework.Enable"], TrueString, OrdinalIgnoreCase))
                {
                    DbConnection connection = new SqlConnection(connectionString);

                    connection = new StackExchange.Profiling.Data.ProfiledDbConnection(connection, MiniProfiler.Current);

                    options.UseSqlServer(connection, o => o.EnableRetryOnFailure());
                }
                else
                {
                    options.UseSqlServer(connectionString, o => o.EnableRetryOnFailure());
                }

                if (string.Equals(Configuration["LOG_SQL"], "Console", OrdinalIgnoreCase))
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

        // HealthCheck
        services.AddHealthChecks()
                .AddCheck<MemoryUsageCheck>(nameof(MemoryUsageCheck), HealthStatus.Degraded, new[]
                {
                    "live",
                    "memory"
                });

        // MiniProfiler
        if (string.Equals(Configuration["MiniProfiler.Enable"], TrueString, OrdinalIgnoreCase))
        {
            var profilerBuilder = services.AddMiniProfiler(o =>
            {
                if (string.Equals(Configuration["MiniProlifer.EntityFramework.Enable"], TrueString, OrdinalIgnoreCase))
                {
                    o.SqlFormatter = new SqlServerFormatter();
                }
            });

            if (string.Equals(Configuration["MiniProlifer.EntityFramework.Enable"], TrueString, OrdinalIgnoreCase))
            {
                profilerBuilder.AddEntityFramework();
            }
        }

        // Prometheus
        services.AddSingleton<CollectorRegistry>(Metrics.DefaultRegistry);
    }

    /// <summary>
    /// Configure
    /// </summary>
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider, ILogger<Startup> logger)
    {
        if (string.Equals(Configuration["USE_SQL"], TrueString, OrdinalIgnoreCase) &&
            string.Equals(Configuration["SQL_USE_STARTUP_MIGRATION"], TrueString, OrdinalIgnoreCase))
        {
            var database = serviceProvider.GetRequiredService<ErabliereDbContext>();

            database.Database.Migrate();
        }

        if (env.IsDevelopment())
        {
            IdentityModelEventSource.ShowPII = true;
            app.UseDeveloperExceptionPage();
        }

        if (string.Equals(Configuration["MiniProfiler.Enable"], TrueString, OrdinalIgnoreCase))
        {
            app.UseMiniProfiler();
        }

        app.UseErabliereAPIForwardedHeadersRules(logger, Configuration);

        app.UseDefaultFiles();
        app.UseStaticFiles();

        app.UtiliserSwagger(Configuration);

        app.UseRouting();

        if (string.Equals(Configuration["USE_CORS"], TrueString, OrdinalIgnoreCase))
        {
            app.UseCors(option =>
            {
                option.WithHeaders(Configuration["CORS_HEADERS"]?.Split(',') ?? new[] { "*" });
                option.WithMethods(Configuration["CORS_METHODS"]?.Split(',') ?? new[] { "*" });
                option.WithOrigins(Configuration["CORS_ORIGINS"]?.Split(',') ?? new[] { "*" });
            });
        }

        if (string.Equals(Configuration["USE_AUTHENTICATION"], TrueString, OrdinalIgnoreCase))
        {
            app.UseAuthentication();
        }
        app.UseAuthorization();

        app.UseMetricServer(registry: serviceProvider.GetRequiredService<CollectorRegistry>());
        app.UseHttpMetrics();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapHealthChecks("/health", new HealthCheckOptions
            {
                Predicate = r => r.Tags.Contains("live") == false
            });
            endpoints.MapHealthChecks("/live", new HealthCheckOptions
            {
                Predicate = r => r.Tags.Contains("live")
            });
        });

        app.UseSpa(spa =>
        {
            
        });
    }
}
