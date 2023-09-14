using System.Security.Claims;
using ErabliereApi.Authorization;
using ErabliereApi.Extensions;

internal class UsersUtils
{
    /// <summary>
    /// Permet d'obtenir le nom unique de l'utilisateur.
    /// 
    /// Si l'utilisateur est authentifié, on utilise les claims pour obtenir le nom unique.
    /// Par ordre d'importance, la claim unique_name est cherché, puis preferred_username.
    /// 
    /// Si l'utilisateur n'est pas authentifié, mais on utilise un clé d'api, va chercher
    /// le nom unique de l'utilisateur dans le contexte d'autorisation.
    /// </summary>
    /// <param name="scope"></param>
    /// <param name="user"></param>
    /// <returns></returns>
    internal static string? GetUniqueName(IServiceScope scope, ClaimsPrincipal? user)
    {
        if (user?.Identity?.IsAuthenticated == true)
        {
            var uniqueName = user.FindFirst("unique_name")?.Value ?? "";

            if (string.IsNullOrWhiteSpace(uniqueName))
            {
                uniqueName = user.FindFirst("preferred_username")?.Value ?? "";
            }

            return uniqueName;
        }

        var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();

        if (config.StripeIsEnabled())
        {
            var apiKeyAuthContext = scope.ServiceProvider.GetRequiredService<ApiKeyAuthorizationContext>();

            return apiKeyAuthContext?.Customer?.UniqueName;
        }

        return null;
    }
}