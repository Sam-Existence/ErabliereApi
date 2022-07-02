using AutoMapper;
using AutoMapper.QueryableExtensions;
using ErabliereApi.Authorization;
using ErabliereApi.Depot.Sql;
using ErabliereApi.Donnees;
using ErabliereApi.Donnees.Action.NonHttp;
using ErabliereApi.Extensions;
using Microsoft.EntityFrameworkCore;
using Stripe;
using System.Security.Claims;

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
    public async Task<CustomerOwnershipAccess?> GetCurrentUserWithAccessAsync(Erabliere erabliere)
    {
        if (erabliere.Id.HasValue == false)
        {
            throw new InvalidOperationException("Cannot get user acces of an erabliere with Id null");
        }

        using var scope = _scopeFactory.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<ErabliereDbContext>();

        var user = scope.ServiceProvider.GetRequiredService<IHttpContextAccessor>().HttpContext?.User;
        
        string? uniqueName = GetUniqueName(scope, user);

        if (uniqueName == null)
        {
            throw new InvalidOperationException("Customer should not be null here ...");
        }

        var idErabliere = erabliere.Id.Value;
        var query = context.Customers.AsNoTracking()
                                     .Where(c => c.UniqueName == uniqueName)
                                     .ProjectTo<CustomerOwnershipAccess>(_fetchCustomerOwnershipAccessMap, new { idErabliere });

        return await query.SingleOrDefaultAsync();
    }

    private static string? GetUniqueName(IServiceScope scope, ClaimsPrincipal? user)
    {
        if (user?.Identity?.IsAuthenticated == true)
        {
            return user.FindFirst("unique_name")?.Value ?? "";
        }

        var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();

        if (config.StripeIsEnabled())
        {
            var apiKeyAuthContext = scope.ServiceProvider.GetRequiredService<ApiKeyAuthorizationContext>();

            return apiKeyAuthContext?.Customer?.UniqueName;
        }

        return null;
    }

    private static readonly AutoMapper.IConfigurationProvider _fetchCustomerOwnershipAccessMap = new MapperConfiguration(config =>
    {
        Guid idErabliere = Guid.Empty;

#nullable disable
        config.CreateMap<Donnees.Customer, CustomerOwnershipAccess>()
              .ForMember(c => c.CustomerErablieres, o => o.MapFrom(cbd => cbd.CustomerErablieres.Where(cbde => cbde.IdErabliere == idErabliere)));
#nullable enable

        config.CreateMap<CustomerErabliere, CustomerErabliereOwnershipAccess>();
    });
}
