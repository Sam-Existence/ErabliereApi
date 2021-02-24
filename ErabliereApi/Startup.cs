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
            services.AddControllers();

            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            });

            services.AjouterSwagger();

            if (string.Equals(Environment.GetEnvironmentVariable("USE_CORS"), bool.TrueString, StringComparison.OrdinalIgnoreCase))
            {
                services.AddCors();
            }

            services.AddAutoMapper(config =>
            {
                config.CreateMap<Dompeux, GetDompeux>();

                config.CreateMap<PostErabliere, Erabliere>();
                config.CreateMap<PostDonnee, Donnee>();
            });

            if (string.Equals(Environment.GetEnvironmentVariable("USE_SQL"), bool.FalseString, StringComparison.OrdinalIgnoreCase))
            {
                services.AddSingleton(typeof(Depot<>), typeof(DepotMemoire<>));
            }
            else
            {
                services.AddTransient(typeof(Depot<>), typeof(DepotDbContext<>));

                services.AddDbContext<ErabliereDbContext>(options =>
                {
                    options.UseSqlServer(Environment.GetEnvironmentVariable("SQL_CONNEXION_STRING") ?? throw new InvalidOperationException("La variable d'environnement 'SQL_CONNEXION_STRING' à une valeur null."));
                    
                    if (string.Equals(Environment.GetEnvironmentVariable("LOG_SQL"), "Console", StringComparison.OrdinalIgnoreCase))
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
            if (string.Equals(Environment.GetEnvironmentVariable("USE_SQL"), bool.TrueString, StringComparison.OrdinalIgnoreCase) &&
                string.Equals(Environment.GetEnvironmentVariable("SQL_USE_STARTUP_MIGRATION"), bool.TrueString, StringComparison.OrdinalIgnoreCase))
            {
                var database = serviceProvider.GetRequiredService<ErabliereDbContext>();

                database.Database.Migrate();
            }

            if (env.IsDevelopment())
            {
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

            if (string.Equals(Environment.GetEnvironmentVariable("USE_CORS"), bool.TrueString, StringComparison.OrdinalIgnoreCase))
            {
                app.UseCors(option =>
                {
                    option.WithHeaders(Environment.GetEnvironmentVariable("CORS_HEADERS")?.Split(','));
                    option.WithMethods(Environment.GetEnvironmentVariable("CORS_METHODS")?.Split(','));
                    option.WithOrigins(Environment.GetEnvironmentVariable("CORS_ORIGINS")?.Split(','));
                });
            }

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapSwagger("api/{documentName}/swagger.json");
            });
        }
    }
}
