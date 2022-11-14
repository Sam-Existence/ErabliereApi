using System.Security.Claims;
using ErabliereApi.Authorization;
using ErabliereApi.Extensions;

internal class UsersUtils
{
    internal static string? GetUniqueName(IServiceScope scope, ClaimsPrincipal? user)
    {
        if (user?.Identity?.IsAuthenticated == true)
        {
            return user.FindFirst("unique_name")?.Value ?? "";
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