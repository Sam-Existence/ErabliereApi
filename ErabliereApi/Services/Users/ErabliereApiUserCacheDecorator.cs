using System.Text.Json;
using ErabliereApi.Donnees;
using ErabliereApi.Donnees.Action.NonHttp;
using ErabliereApi.Extensions;
using Microsoft.Extensions.Caching.Distributed;

namespace ErabliereApi.Services.Users;

/// <summary>
/// Décorateur pour le cache de l'utilisateur
/// </summary>
public class ErabliereApiUserCacheDecorator : IUserService
{
    private readonly IUserService _userService;
    private readonly IDistributedCache _cache;
    private readonly IServiceScopeFactory _scopeFactory;

    /// <summary>
    /// Constructeur
    /// </summary>
    public ErabliereApiUserCacheDecorator(IUserService userService, IDistributedCache cache, IServiceScopeFactory scopeFactory)
    {
        _userService = userService;
        _cache = cache;
        _scopeFactory = scopeFactory;
    }

    /// <inheritdoc />
    public Task CreateCustomerAsync(Donnees.Customer customer, CancellationToken token)
    {
        return _userService.CreateCustomerAsync(customer, token);
    }

    /// <inheritdoc />
    public async Task<CustomerOwnershipAccess?> GetCurrentUserWithAccessAsync(Erabliere erabliere, CancellationToken token)
    {
        if (erabliere.Id.HasValue == false)
        {
            throw new InvalidOperationException("Cannot get user acces of an erabliere with Id null");
        }

        using var scope = _scopeFactory.CreateScope();

        var user = scope.ServiceProvider.GetRequiredService<IHttpContextAccessor>().HttpContext?.User;

        string? uniqueName = UsersUtils.GetUniqueName(scope, user);
        
        if (uniqueName == null)
        {
            throw new InvalidOperationException("Customer should not be null here ...");
        }

        var customerWithAccess = await _cache.GetAsync<CustomerOwnershipAccess>($"CustomerWithAccess_{uniqueName}_{erabliere.Id}", token);

        if (customerWithAccess == null)
        {
            customerWithAccess = await _userService.GetCurrentUserWithAccessAsync(erabliere, token);

            if (customerWithAccess != null)
            {
                try 
                {
                    await _cache.SetAsync<CustomerOwnershipAccess>($"CustomerWithAccess_{uniqueName}_{erabliere.Id}", customerWithAccess, token);
                }
                catch (Exception e) 
                {
                    var logger = scope.ServiceProvider.GetRequiredService<ILogger<ErabliereApiUserCacheDecorator>>();

                    logger.LogWarning($"CustomerWithAccess {uniqueName} cannot be set in cache, error: {e.Message}");
                    logger.LogDebug(JsonSerializer.Serialize(customerWithAccess));
                }
            }
        }
        else 
        {
            // Log that the CustomerWithAccess was found in cache
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<ErabliereApiUserCacheDecorator>>();

            logger.LogDebug($"CustomerWithAccess {uniqueName} was found in cache");
        }

        return customerWithAccess;
    }
}
