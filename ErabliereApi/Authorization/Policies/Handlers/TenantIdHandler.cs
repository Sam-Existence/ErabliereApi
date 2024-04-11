using ErabliereApi.Authorization.Policies.Requirements;
using Microsoft.AspNetCore.Authorization;

namespace ErabliereApi.Authorization.Policies.Handlers;
/// <summary>
/// Handler pour une politique qui s'assure que le tenantId de l'utilisateur est celui du tenant principal.
/// Si ce dernier n'est pas défini, le tenantId est toujours accepté.
/// </summary>
public class TenantIdHandler : AuthorizationHandler<TenantIdRequirement>
{
    /// <inheritdoc />
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, TenantIdRequirement requirement)
    {
        if (string.IsNullOrWhiteSpace(requirement.TenantId))
        {
            context.Succeed(requirement);
        }
        else
        {
            var tenantIdClaim = context.User.FindFirst(c => c.Type == "http://schemas.microsoft.com/identity/claims/tenantid");

            if (tenantIdClaim == null || tenantIdClaim.Value == requirement.TenantId)
            {
                context.Succeed(requirement);
            }
        }
        
        return Task.CompletedTask;
    }
}