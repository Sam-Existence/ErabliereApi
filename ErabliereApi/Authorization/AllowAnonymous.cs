using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ErabliereApi.Authorization
{
    /// <summary>
    /// Classe utiliser quand la varaible d'environnement USE_AUTHENTICATION est à "false".
    /// </summary>
    public class AllowAnonymous : IAuthorizationHandler
    {
        /// <inheritdoc />
        public Task HandleAsync(AuthorizationHandlerContext context)
        {
            foreach (IAuthorizationRequirement requirement in context.PendingRequirements.ToList())
            {
                context.Succeed(requirement);
            }
            
            return Task.CompletedTask;
        }
    }
}
