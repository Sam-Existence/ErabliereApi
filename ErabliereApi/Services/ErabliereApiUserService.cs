using ErabliereApi.Depot.Sql;
using ErabliereApi.Donnees;
using Microsoft.EntityFrameworkCore;

namespace ErabliereApi.Services;

/// <summary>
/// Implémentation de <see cref="IUserService" /> selon la logique interne du projet ErabliereApi
/// </summary>
public class ErabliereApiUserService : IUserService
{
    private ErabliereDbContext _context;

    /// <summary>
    /// Constructeur par initialisation
    /// </summary>
    /// <param name="context"></param>
    public ErabliereApiUserService(ErabliereDbContext context)
    {
        _context = context;
    }

    /// <inheritdoc />
    public async Task CreateCustomerAsync(Customer customer, CancellationToken token)
    {
        var customerExist = await _context.Customers.AnyAsync(c => c.Email == customer.Email, token);

        if (!customerExist)
        {
            var entity = await _context.Customers.AddAsync(customer, token);

            await _context.SaveChangesAsync(token);
        }
    }
}
