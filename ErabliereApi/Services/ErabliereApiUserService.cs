using ErabliereApi.Depot.Sql;
using ErabliereApi.Donnees;
using ErabliereApi.StripeIntegration;
using Microsoft.EntityFrameworkCore;

namespace ErabliereApi.Services;

public class ErabliereApiUserService : IUserService
{
    private ErabliereDbContext _context;

    public ErabliereApiUserService(ErabliereDbContext context)
    {
        _context = context;
    }

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
