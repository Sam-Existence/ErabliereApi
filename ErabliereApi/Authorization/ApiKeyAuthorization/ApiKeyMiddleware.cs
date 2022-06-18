using ErabliereApi.Depot.Sql;
using ErabliereApi.Services;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Text.Json;

namespace ErabliereApi.Authorization;

/// <summary>
/// Middleware pour la gestion d'autorisation des clé d'api
/// </summary>
public class ApiKeyMiddleware : IMiddleware
{
    const string XApiKeyHeader = "X-ErabliereApi-ApiKey";

    /// <summary>
    /// Execute the logic needed to track usage of api keys
    /// </summary>
    /// <param name="context"></param>
    /// <param name="next"></param>
    /// <returns></returns>
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        bool authorizeRequest = true;

        if (context.Request.Headers.TryGetValue(XApiKeyHeader, out var apiKey))
        {
            var apiKeyService = context.RequestServices.GetRequiredService<IApiKeyService>();

            authorizeRequest = apiKeyService.TryHashApiKey(apiKey, out var hashkey);

            var dbContext = context.RequestServices.GetRequiredService<ErabliereDbContext>();

            var apiKeyEntity = await dbContext.ApiKeys.FirstOrDefaultAsync(k => k.Key == hashkey);

            var now = DateTimeOffset.Now;

            if (apiKeyEntity != null &&
                (apiKeyEntity.RevocationTime == null || now < apiKeyEntity.RevocationTime) &&
                (apiKeyEntity.DeletionTime == null || now < apiKeyEntity.CreationTime) &&
                apiKeyEntity.DeletionTime == null)
            {
                var checkoutService = context.RequestServices.GetRequiredService<ICheckoutService>();

                await checkoutService.ReccordUsageAsync(apiKeyEntity);

                context.RequestServices.GetRequiredService<ApiKeyAuthorizationContext>().Authorize = true;
            }
            else
            {
                authorizeRequest = false;
            }
        }

        if (authorizeRequest)
        {
            await next(context);
        }
        else
        {
            context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
            context.Response.Headers.Add("X-ErabliereApi-ApiKey-Reason", "Something is wrong with the token");
        }
    }
}
