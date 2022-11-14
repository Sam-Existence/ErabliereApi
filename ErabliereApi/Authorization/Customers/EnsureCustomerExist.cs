using ErabliereApi.Depot.Sql;
using ErabliereApi.Donnees;
using ErabliereApi.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;

namespace ErabliereApi.Authorization.Customers;

/// <summary>
/// Lorsqu'un utilisateur est authentifier avec un jeton Bearer, il faut assurer
/// que l'utilisateur existe en BD afin de faire fonctionner le modèle de propriété
/// (ownership) des données.
/// </summary>
public class EnsureCustomerExist : IMiddleware
{
    /// <inheritdoc />
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        if (context.User?.Identity?.IsAuthenticated == true)
        {
            var uniqueName = context.User.FindFirst("unique_name")?.Value ?? "";

            var cache = context.RequestServices.GetRequiredService<IDistributedCache>();

            var customer = await cache.GetAsync<Customer>($"Customer_{uniqueName}", context.RequestAborted);

            if (customer == null) 
            {
                var dbContext = context.RequestServices.GetRequiredService<ErabliereDbContext>();

                if (!(await dbContext.Customers.AnyAsync(c => c.UniqueName == uniqueName)))
                {
                    var customerEntity = await dbContext.Customers.AddAsync(new Donnees.Customer
                    {
                        Email = uniqueName,
                        UniqueName = uniqueName,
                        Name = context.User.FindFirst("name")?.Value ?? "",
                        CreationTime = DateTimeOffset.Now
                    }, context.RequestAborted);

                    await dbContext.SaveChangesAsync(context.RequestAborted);

                    customer = customerEntity.Entity;

                    await cache.SetAsync($"Customer_{uniqueName}", customer, context.RequestAborted);
                }
                else 
                {
                    customer = await dbContext.Customers.FirstAsync(c => c.UniqueName == uniqueName, context.RequestAborted);

                    await cache.SetAsync($"Customer_{uniqueName}", customer, context.RequestAborted);
                }
            }
            else 
            {
                // Log the was found in cache
                var logger = context.RequestServices.GetRequiredService<ILogger<EnsureCustomerExist>>();

                logger.LogDebug("Customer {Customer} was found in cache", customer);
            }
        }

        await next(context);
    }
}
