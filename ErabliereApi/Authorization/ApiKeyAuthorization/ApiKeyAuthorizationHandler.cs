using Microsoft.AspNetCore.Authorization;

namespace ErabliereApi.Authorization;

/// <summary>
/// Classe utiliser quand la fonction Stripe est activé dans l'api.
/// Authorise les clients qui ont une clé d'api valide en regardant l'état de ApiKeyAuthrozationContext.
/// </summary>
public class ApiKeyAuthrizationHandler : IAuthorizationHandler
{
    private readonly IHttpContextAccessor _accessor;

    /// <summary>
    /// Classe par défaut de cette classe singleton
    /// </summary>
    public ApiKeyAuthrizationHandler(IHttpContextAccessor accessor)
    {
        _accessor = accessor;
    }

    /// <inheritdoc />
    public Task HandleAsync(AuthorizationHandlerContext context)
    {
        var authContext = _accessor.HttpContext?.RequestServices.GetRequiredService<ApiKeyAuthorizationContext>();

        if (authContext != null && authContext.Authorize) 
        {
            foreach (var requirement in context.PendingRequirements)
            {
                context.Succeed(requirement);
            }
        }
        
        return Task.CompletedTask;
    }
}
