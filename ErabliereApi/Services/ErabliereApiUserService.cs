using ErabliereApi.Authorization;
using ErabliereApi.Depot.Sql;
using ErabliereApi.Donnees;
using ErabliereApi.Extensions;
using Microsoft.EntityFrameworkCore;
using Stripe;

namespace ErabliereApi.Services;

/// <summary>
/// Implémentation de <see cref="IUserService" /> selon la logique interne du projet ErabliereApi
/// </summary>
public class ErabliereApiUserService : IUserService
{
    private readonly IServiceScopeFactory _scopeFactory;

    /// <summary>
    /// Constructeur par initialisation
    /// </summary>
    /// <param name="scopeFactory"></param>
    public ErabliereApiUserService(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    /// <inheritdoc />
    public async Task CreateCustomerAsync(Donnees.Customer customer, CancellationToken token)
    {
        if (customer.UniqueName == null)
        {
            throw new InvalidOperationException("The customer instance must have a UniqueName");
        }

        using var scope = _scopeFactory.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<ErabliereDbContext>();

        var customerExist = await context.Customers.AnyAsync(c => c.UniqueName == customer.UniqueName, token);

        if (!customerExist)
        {
            var entity = await context.Customers.AddAsync(customer, token);

            await context.SaveChangesAsync(token);
        }
    }

    /// <inheritdoc />
    public async Task<Donnees.Customer?> GetCurrentUserWithAccessAsync(Erabliere erabliere)
    {
        string? uniqueName = default;

        using var scope = _scopeFactory.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<ErabliereDbContext>();

        var user = scope.ServiceProvider.GetRequiredService<IHttpContextAccessor>().HttpContext?.User;

        if (user?.Identity?.IsAuthenticated == true)
        {
            uniqueName = user.FindFirst("unique_name")?.Value ?? "";
        }

        var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();

        if (config.StripeIsEnabled())
        {
            var apiKeyAuthContext = scope.ServiceProvider.GetRequiredService<ApiKeyAuthorizationContext>();

            uniqueName = apiKeyAuthContext?.Customer?.UniqueName;
        }

        if (uniqueName == null)
        {
            throw new InvalidOperationException("Customer should not be bul here ...");
        }

        var query = context.Customers.AsNoTracking()
#nullable disable
                         .Include(c => c.CustomerErablieres.Where(ce => ce.IdErabliere == erabliere.Id))
#nullable enable
                         .Where(c => c.UniqueName == uniqueName);

        return await query.SingleOrDefaultAsync();
    }
}
