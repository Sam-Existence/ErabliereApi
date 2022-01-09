using Microsoft.AspNetCore.Authorization;
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
            foreach (var requirement in context.PendingRequirements)
            {
                context.Succeed(requirement);
            }
            
            return Task.CompletedTask;
        }
    }
}
