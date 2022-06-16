using ErabliereApi.Depot.Sql;
using ErabliereApi.Donnees;
using Microsoft.EntityFrameworkCore;

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
    public async Task CreateCustomerAsync(Customer customer, CancellationToken token)
    {
        using var scope = _scopeFactory.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<ErabliereDbContext>();

        var customerExist = await context.Customers.AnyAsync(c => c.Email == customer.Email, token);

        if (!customerExist)
        {
            var entity = await context.Customers.AddAsync(customer, token);

            await context.SaveChangesAsync(token);
        }
    }
}
