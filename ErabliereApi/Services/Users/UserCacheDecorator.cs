using System.Text.Json;
using ErabliereApi.Authorization;
using ErabliereApi.Donnees;
using ErabliereApi.Donnees.Action.NonHttp;
using ErabliereApi.Extensions;
using Microsoft.Extensions.Caching.Distributed;

namespace ErabliereApi.Services.Users;

/// <summary>
/// Décorateur pour le cache de l'utilisateur
/// </summary>
public class UserCacheDecorator : IUserService
{
    private readonly IUserService _userService;
    private readonly IDistributedCache _cache;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ApiKeyAuthorizationContext _apiKeyAuthorization;

    /// <summary>
    /// Constructeur
    /// </summary>
    public UserCacheDecorator(
        IUserService userService, 
        IDistributedCache cache, 
        IServiceScopeFactory scopeFactory,
        ApiKeyAuthorizationContext apiKeyAuthorization)
    {
        _userService = userService;
        _cache = cache;
        _scopeFactory = scopeFactory;
        _apiKeyAuthorization = apiKeyAuthorization;
    }

    /// <inheritdoc />
    public Task<Customer> GetOrCreateCustomerAsync(Donnees.Customer customer, CancellationToken token)
    {
        return _userService.GetOrCreateCustomerAsync(customer, token);
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
            // Try to find customer from the ApiKey, if any
            if (_apiKeyAuthorization.Authorize)
            {
                uniqueName = _apiKeyAuthorization.Customer?.UniqueName;
            }

            if (uniqueName == null)
            {
                throw new InvalidOperationException("Customer should not be null here ...");
            }
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
                    var logger = scope.ServiceProvider.GetRequiredService<ILogger<UserCacheDecorator>>();

                    logger.LogWarning($"CustomerWithAccess {uniqueName} cannot be set in cache, error: {e.Message}");
                    logger.LogDebug(JsonSerializer.Serialize(customerWithAccess));
                }
            }
        }
        else 
        {
            // Log that the CustomerWithAccess was found in cache
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<UserCacheDecorator>>();

            logger.LogDebug($"CustomerWithAccess {uniqueName} was found in cache");
        }

        return customerWithAccess;
    }

    /// <inheritdoc />
    public Task UpdateEnsureStripeInfoAsync(Customer customer, string stripeId, CancellationToken token)
    {
        return _userService.UpdateEnsureStripeInfoAsync(customer, stripeId, token);
    }

    /// <inheritdoc />
    public Task<Stripe.Customer> StripeGetAsync(string customerId, CancellationToken token)
    {
        return _userService.StripeGetAsync(customerId, token);
    }
}
