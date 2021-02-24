using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ErabliereApi.Identity
{
    /// <summary>
    /// Classe non utilisé, début des travaux
    /// </summary>
    public static class IdentityExtension
    {
        public static IServiceCollection AjouterIdentityServer4(this IServiceCollection services)
        {
            services.AddIdentityServer()
                    .AddInMemoryIdentityResources(Config.IdentityResources)
                    .AddInMemoryApiScopes(Config.ApiScopes)
                    .AddInMemoryClients(Config.Clients);

            return services;
        }

        public static IApplicationBuilder UtiliserIdentityServer4(this IApplicationBuilder app)
        {
            app.UseIdentityServer();

            return app;
        }
    }
}
