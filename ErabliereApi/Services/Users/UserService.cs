using AutoMapper;
using AutoMapper.QueryableExtensions;
using ErabliereApi.Authorization;
using ErabliereApi.Depot.Sql;
using ErabliereApi.Donnees;
using ErabliereApi.Donnees.Action.NonHttp;
using Microsoft.EntityFrameworkCore;
using Stripe;

namespace ErabliereApi.Services.Users;

/// <summary>
/// Implémentation de <see cref="IUserService" /> selon la logique interne du projet ErabliereApi
/// </summary>
public class UserService : IUserService
{
    private readonly ErabliereDbContext _context;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ApiKeyAuthorizationContext _apiKeyAuthorizationContext;
    private readonly IConfiguration _config;

    /// <summary>
    /// Constructeur par initialisation
    /// </summary>
    /// <param name="context"></param>
    /// <param name="serviceScope"></param>
    /// <param name="apiKeyAuthorizationContext"></param>
    /// <param name="config"></param>
    public UserService(
        ErabliereDbContext context, 
        IServiceScopeFactory serviceScope, 
        ApiKeyAuthorizationContext apiKeyAuthorizationContext,
        IConfiguration config)
    {
        _context = context;
        _scopeFactory = serviceScope;
        _apiKeyAuthorizationContext = apiKeyAuthorizationContext;
        _config = config;
    }

    /// <inheritdoc />
    public async Task<Donnees.Customer> GetOrCreateCustomerAsync(Donnees.Customer customer, CancellationToken token)
    {
        if (customer.UniqueName == null)
        {
            throw new InvalidOperationException("The customer instance must have a UniqueName");
        }

        var customerDb = customer.StripeId != null ?
            await _context.Customers.FirstOrDefaultAsync(c => c.StripeId == customer.StripeId) :
            await _context.Customers.FirstOrDefaultAsync(c => c.UniqueName == customer.UniqueName, token);

        if (customerDb == null)
        {
            customerDb = await _context.Customers.FirstOrDefaultAsync(c => c.Email == customer.Email);

            if (customerDb != null) 
            {
                return customerDb;
            }
        }

        if (customerDb == null)
        {
            var entity = await _context.Customers.AddAsync(customer, token);

            await _context.SaveChangesAsync(token);

            return entity.Entity;
        }

        return customerDb;
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
            if (_apiKeyAuthorizationContext.Authorize)
            {
                uniqueName = _apiKeyAuthorizationContext.Customer?.UniqueName;
            }

            if (uniqueName == null)
            {
                throw new InvalidOperationException("Customer should not be null here ...");
            }
        }

        var idErabliere = erabliere.Id.Value;
        var query = _context.Customers.AsNoTracking()
                                     .Where(c => c.UniqueName == uniqueName)
                                     .ProjectTo<CustomerOwnershipAccess>(_fetchCustomerOwnershipAccessMap, new { idErabliere });

        return await query.SingleOrDefaultAsync(token);
    }

    /// <inheritdoc />
    public async Task UpdateEnsureStripeInfoAsync(Donnees.Customer customer, string stripeId, CancellationToken token)
    {
        customer.StripeId = stripeId;
        if (!customer.AccountType.Contains("Stripe.Customer"))
        {
            if (customer.AccountType.Length == 0)
            {
                customer.AccountType = "Stripe.Customer";
            }
            else
            {
                customer.AccountType = string.Concat(customer.AccountType, "Stripe.Customer");
            }
        }
        await _context.SaveChangesAsync(token);
    }

    /// <inheritdoc />
    public async Task<Stripe.Customer> StripeGetAsync(string customerId, CancellationToken token)
    {
        var testMode = _config.GetValue<string>("ErabliereApiUserService.TestMode");

        if (testMode == "true")
        {
            return new Stripe.Customer
            {
                Email = "john@doe.com",
                Id = "cus_2PXvRa6ztL96bV",
                Name = "John Doe"
            };
        }

        var service = new CustomerService();

        return await service.GetAsync(customerId, cancellationToken: token);
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
