using ErabliereApi.Depot;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using AutoMapper;
using ErabliereApi.Donnees.Action.Post;
using ErabliereApi.Donnees;

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

            services.AjouterSwagger();

            if (string.Equals(Environment.GetEnvironmentVariable("USE_CORS"), bool.TrueString, StringComparison.OrdinalIgnoreCase))
            {
                services.AddCors();
            }

            services.AddAutoMapper(config =>
            {
                config.CreateMap<PostErabliere, Erabliere>();
                config.CreateMap<PostDonnee, Donnee>();
            });

            services.AddSingleton(typeof(Depot<>), typeof(DepotMemoire<>));
        }

        /// <summary>
        /// Configure
        /// </summary>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

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
