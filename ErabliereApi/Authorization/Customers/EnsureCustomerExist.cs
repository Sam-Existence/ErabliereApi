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
            var uniqueName = UsersUtils.GetUniqueName(context.RequestServices.CreateScope(), context.User);

            if (uniqueName == null)
            {
                throw new InvalidOperationException("User is authenticated but no unique name was found");
            }

            if (uniqueName == "") 
            {
                throw new InvalidOperationException("User is authenticated but unique name is empty");
            }

            var cache = context.RequestServices.GetRequiredService<IDistributedCache>();

            var customer = await cache.GetAsync<Customer>($"Customer_{uniqueName}", context.RequestAborted);

            if (customer == null) 
            {
                var dbContext = context.RequestServices.GetRequiredService<ErabliereDbContext>();

                if (!(await dbContext.Customers.AnyAsync(c => c.UniqueName == uniqueName)))
                {
                    // Cas spécial ou l'utilisateur aurait été créé précédement
                    // et le uniqueName est vide.
                    if (!(await dbContext.Customers.AnyAsync(c => c.UniqueName == "", context.RequestAborted))) 
                    {
                        var cust = await dbContext.Customers.SingleAsync(c => c.UniqueName == "", context.RequestAborted);

                        cust.UniqueName = uniqueName;

                        if (!cust.AccountType.Contains("AzureAD", StringComparison.OrdinalIgnoreCase))
                        {
                            cust.AccountType = string.Concat(cust.AccountType, ',', "AzureAD");
                        }

                        await dbContext.SaveChangesAsync(context.RequestAborted);

                        await cache.SetAsync($"Customer_{uniqueName}", cust, context.RequestAborted);
                    }
                    else
                    {
                        var customerEntity = await dbContext.Customers.AddAsync(new Donnees.Customer
                        {
                            Email = uniqueName,
                            UniqueName = uniqueName,
                            Name = context.User.FindFirst("name")?.Value ?? "",
                            AccountType = "AzureAD",
                            CreationTime = DateTimeOffset.Now
                        }, context.RequestAborted);

                        await dbContext.SaveChangesAsync(context.RequestAborted);

                        customer = customerEntity.Entity;

                        await cache.SetAsync($"Customer_{uniqueName}", customer, context.RequestAborted);
                    }
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
