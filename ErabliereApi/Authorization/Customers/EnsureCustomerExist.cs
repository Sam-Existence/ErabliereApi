using ErabliereApi.Depot.Sql;
using Microsoft.EntityFrameworkCore;

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
            var dbContext = context.RequestServices.GetRequiredService<ErabliereDbContext>();

            var uniqueName = context.User.FindFirst("unique_name")?.Value ?? "";

            if (!(await dbContext.Customers.AnyAsync(c => c.UniqueName == uniqueName)))
            {
                await dbContext.Customers.AddAsync(new Donnees.Customer
                {
                    Email = uniqueName,
                    UniqueName = uniqueName,
                    Name = context.User.FindFirst("name")?.Value ?? "",
                    CreationTime = DateTimeOffset.Now
                }, context.RequestAborted);

                await dbContext.SaveChangesAsync(context.RequestAborted);
            }
        }

        await next(context);
    }
}
