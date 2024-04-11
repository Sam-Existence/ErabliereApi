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
using ErabliereApi.StripeIntegration;
using ErabliereApi.Services;
using ErabliereApi.Authorization.Customers;
using ErabliereApi.Services.Users;
using ErabliereApi.Middlewares;
using ErabliereApi.Models;
using ErabliereApi.Donnees;
using MailKit.Net.Smtp;
using MailKit;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.Extensions.Options;
using ErabliereApi.Services.Emails;
using ErabliereApi.Services.SMS;
using ErabliereApi.ControllerFeatureProviders;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using ErabliereApi.Authorization.Policies.Requirements;
using ErabliereApi.Authorization.Policies.Handlers;

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
        .ConfigureApplicationPartManager(manager => 
        {
            // This code is used to scan for controller using the StripeIntegrationToggleFiltrer
            // which is going to control if the stripe controller must be enabled or disabled
            manager.FeatureProviders.Clear();
            manager.FeatureProviders.Add(new ErabliereAPIControllerFeatureProvider(Configuration));
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
        services.AddTransient<IUserService, UserService>()
                .Decorate<IUserService, UserCacheDecorator>()
                .AddHttpContextAccessor();
        if (Configuration.IsAuthEnabled())
        {
            Console.WriteLine("Authentication enabled.");

            if (Configuration["AzureAD__ClientId"] != null && Configuration["AzureAD:ClientId"] == null)
            {
                Configuration["AzureAD:ClientId"] = Configuration["AzureAD__ClientId"];
            }

            if (Configuration["AzureAD__TenantId"] != null && Configuration["AzureAD:TenantId"] == null)
            {
                Configuration["AzureAD:TenantId"] = Configuration["AzureAD__TenantId"];
            }

            if (Configuration["AzureAD__ClientSecret"] != null && Configuration["AzureAD:ClientSecret"] == null)
            {
                Configuration["AzureAD:ClientSecret"] = Configuration["AzureAD__ClientSecret"];
            }

            if (string.IsNullOrWhiteSpace(Configuration["AzureAD:ClientId"]) == false)
            {
                services.AddSingleton<IAuthorizationHandler, TenantIdHandler>();
                services.AddAuthorization(options =>
                {
                    options.AddPolicy("TenantIdPrincipal", policy =>
                    {
                        policy.Requirements.Add(new TenantIdRequirement(Configuration["AzureAD:TenantIdPrincipal"] ?? ""));
                    });
                });

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

            services.AddTransient<EnsureCustomerExist>();
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
            Console.WriteLine("CORS enabled. Services Added.");
            services.AddCors();
        }

        // Automapper
        services.AjouterAutoMapperErabliereApiDonnee(config => 
            {
                StripeIntegration.AutoMapperExtension.AddCustomersApiKeyMappings(config);

                config.CreateMap<PostNoteMultipart, Note>()
                  .ForMember(d => d.File, o => o.Ignore())
                  .ReverseMap()
                  .ForMember(d => d.File, o => o.Ignore());
            }
        );

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

            }, contextLifetime: ServiceLifetime.Singleton, 
               optionsLifetime: ServiceLifetime.Transient);
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

        // Stripe
        services.AddScoped<ApiKeyAuthorizationContext>();
        if (Configuration.StripeIsEnabled())
        {
            services.Configure<StripeOptions>(o =>
            {
                o.ApiKey = Configuration["Stripe.ApiKey"];
                o.SuccessUrl = Configuration["Stripe.SuccessUrl"];
                o.CancelUrl = Configuration["Stripe.CancelUrl"];
                o.BasePlanPriceId = Configuration["Stripe.BasePlanPriceId"];
                o.WebhookSecret = Configuration["Stripe.WebhookSecret"];
                o.WebhookSiginSecret = Configuration["Stripe.WebhookSiginSecret"];
            });

            services.AddTransient<ICheckoutService, StripeCheckoutService>()
                    .AddTransient<IApiKeyService, ApiApiKeyService>()
                    .AddTransient<ApiKeyMiddleware>();

            // Authorization
            services.AddSingleton<IAuthorizationHandler, ApiKeyAuthrizationHandler>();

            // Context and usage reccorder
            services.AddSingleton<UsageContext>();
        }

        // Email
        services.AddTransient<ErabliereApiEmailService>();
        services.AddTransient<MSGraphEmailService>();
        services.AddTransient<IEmailService>(sp =>
        {
            var o = sp.GetRequiredService<IOptions<EmailConfig>>().Value;

            if (o.UseMSGraphAPI == true)
            {
                return sp.GetRequiredService<MSGraphEmailService>();
            }

            return sp.GetRequiredService<ErabliereApiEmailService>();
        });
        services.AddSingleton<ISmtpClient, SmtpClient>();
        services.AddSingleton<IProtocolLogger>(sp =>
        {
            if (Configuration.IsDevelopment())
            {
                return new ProtocolLogger(Console.OpenStandardOutput());
            }
            else
            {
                return new ProtocolLogger(Stream.Null);
            }
        });
        services.AddSingleton<SmtpClient>(sp =>
        {
           return new SmtpClient(sp.GetRequiredService<IProtocolLogger>());
        });
        services.Configure<EmailConfig>(o =>
        {
            var path = Configuration["EMAIL_CONFIG_PATH"];

            if (string.IsNullOrWhiteSpace(path))
            {
                Console.WriteLine("La variable d'environment 'EMAIL_CONFIG_PATH' ne possédant pas de valeur, les configurations de courriel ne seront pas désérialisé.");
            }
            else
            {
                try
                {
                    var v = File.ReadAllText(path);

                    var deserializedConfig = JsonSerializer.Deserialize<EmailConfig>(v);

                    if (deserializedConfig != null)
                    {
                        o.Sender = deserializedConfig.Sender;
                        o.Email = deserializedConfig.Email;
                        o.Password = deserializedConfig.Password;
                        o.TenantId = deserializedConfig.TenantId;
                        o.SmtpServer = deserializedConfig.SmtpServer;
                        o.SmtpPort = deserializedConfig.SmtpPort;
                        o.UseMSGraphAPI = deserializedConfig.UseMSGraphAPI;
                    }
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine("Erreur en désérialisant les configurations de l'email. La fonctionnalité des alertes ne pourra pas être utilisé.");
                    Console.Error.WriteLine(e.Message);
                    Console.Error.WriteLine(e.StackTrace);
                }
            }
        });

        // SMS
        services.AddTransient<TwilioSMSService>();
        services.AddTransient<ISMSService, TwilioSMSService>();
        services.Configure<SMSConfig>(o =>
        {
            var path = Configuration["SMS_CONFIG_PATH"];

            if (string.IsNullOrWhiteSpace(path))
            {
                Console.WriteLine("La variable d'environment 'SMS_CONFIG_PATH' ne possédant pas de valeur, les configurations de SMS ne seront pas désérialisé.");
            }
            else
            {
                try
                {
                    var v = File.ReadAllText(path);

                    var deserializedConfig = JsonSerializer.Deserialize<SMSConfig>(v);

                    if (deserializedConfig != null)
                    {
                        o.Numero = deserializedConfig.Numero;
                        o.AccountSid = deserializedConfig.AccountSid;
                        o.AuthToken = deserializedConfig.AuthToken;
                    }
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine("Erreur en désérialisant les configurations du SMS. La fonctionnalité des alertes ne pourra pas être utilisé.");
                    Console.Error.WriteLine(e.Message);
                    Console.Error.WriteLine(e.StackTrace);
                }
            }
        });


        // Distributed cache
        if (string.Equals(Configuration["USE_DISTRIBUTED_CACHE"], TrueString, OrdinalIgnoreCase))
        {
            Console.WriteLine("Distributed cache enabled. Using Redis " + Configuration["REDIS_CONNEXION_STRING"]);
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = Configuration["REDIS_CONNEXION_STRING"];
            });
        }
        else 
        {
            services.AddDistributedMemoryCache();
        }

        // ChaosEngineering
        if (Configuration.IsChaosEngineeringEnabled()) 
        {
            services.AddSingleton<ChaosEngineeringMiddleware>();
        }

        // Weather Service
        services.AddTransient<WeatherService>();
    }

    /// <summary>
    /// Configure
    /// </summary>
    public void Configure(
        IApplicationBuilder app, 
        IWebHostEnvironment env, 
        IServiceProvider serviceProvider)
    {
        var logger = serviceProvider.GetRequiredService<ILogger<Startup>>();

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

        app.UseRouting();

        if (string.Equals(Configuration["USE_CORS"], TrueString, OrdinalIgnoreCase))
        {
            Console.WriteLine("CORS enabled. Middleware Added.");

            app.UseCors(option =>
            {
                option.WithHeaders(Configuration["CORS_HEADERS"]?.Split(',') ?? new[] { "*" });
                option.WithMethods(Configuration["CORS_METHODS"]?.Split(',') ?? new[] { "*" });
                option.WithOrigins(Configuration["CORS_ORIGINS"]?.Split(',') ?? new[] { "*" });
            });
        }

        if (Configuration.StripeIsEnabled())
        {
            app.UseMiddleware<ApiKeyMiddleware>();
        }

        if (Configuration.IsAuthEnabled())
        {
            app.UseAuthentication();
        }
        app.UseAuthorization();

        app.UseMetricServer(registry: serviceProvider.GetRequiredService<CollectorRegistry>());
        app.UseHttpMetrics();

        if (Configuration.IsAuthEnabled())
        {
            app.UseMiddleware<EnsureCustomerExist>();
        }

        if (Configuration.IsChaosEngineeringEnabled())
        {
            if (env.IsProduction()) 
            {
                logger.LogWarning("Chaos engineering is enabled in production. This is not recommended.");
            }
            logger.LogInformation("Chaos engineering is enabled with a probability of {0}%", Configuration["ChaosEngineeringPercent"]);
            
            app.UseMiddleware<ChaosEngineeringMiddleware>();
        }

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

        app.UtiliserSwagger(Configuration);

        app.UseSpa(spa =>
        {
            
        });
    }
}
