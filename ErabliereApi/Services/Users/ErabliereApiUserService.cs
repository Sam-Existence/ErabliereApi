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

namespace ErabliereApi.Services.Users;

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
    public async Task<CustomerOwnershipAccess?> GetCurrentUserWithAccessAsync(Erabliere erabliere, CancellationToken token)
    {
        if (erabliere.Id.HasValue == false)
        {
            throw new InvalidOperationException("Cannot get user acces of an erabliere with Id null");
        }

        using var scope = _scopeFactory.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<ErabliereDbContext>();

        var user = scope.ServiceProvider.GetRequiredService<IHttpContextAccessor>().HttpContext?.User;
        
        string? uniqueName = UsersUtils.GetUniqueName(scope, user);

        if (uniqueName == null)
        {
            throw new InvalidOperationException("Customer should not be null here ...");
        }

        var idErabliere = erabliere.Id.Value;
        var query = context.Customers.AsNoTracking()
                                     .Where(c => c.UniqueName == uniqueName)
                                     .ProjectTo<CustomerOwnershipAccess>(_fetchCustomerOwnershipAccessMap, new { idErabliere });

        return await query.SingleOrDefaultAsync(token);
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
