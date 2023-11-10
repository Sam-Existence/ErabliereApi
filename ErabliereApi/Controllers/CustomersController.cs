using AutoMapper;
using AutoMapper.QueryableExtensions;
using ErabliereApi.Depot.Sql;
using ErabliereApi.Donnees;
using ErabliereApi.Donnees.Action.Get;
using ErabliereApi.Donnees.Action.Put;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.EntityFrameworkCore;

namespace ErabliereApi.Controllers;

/// <summary>
/// Contrôler permettant d'accéder au information de base sur les utilisateurs.
/// </summary>
[ApiController]
[Route("Customers")]
[Authorize]
public class CustomersController : ControllerBase
{
    private readonly ErabliereDbContext _context;
    private readonly IMapper _mapper;

    /// <summary>
    /// Constructeur avec dépendance
    /// </summary>
    /// <param name="context">La base de données</param>
    /// <param name="mapper">Le mapper</param>
    public CustomersController(ErabliereDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    /// <summary>
    /// Permet de lister les utilisateurs en exposant un minimum d'information.
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [EnableQuery(MaxExpansionDepth = 0)]
    public IQueryable<GetCustomer> GetCustomers()
    {
        return _context.Customers.ProjectTo<GetCustomer>(_mapper.ConfigurationProvider);
    }

    /// <summary>
    /// Point de terminaison pour l'administration des utilisateurs
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [EnableQuery]
    [Route("/admin/customers")]
    [Authorize(Roles = "administrateur")]
    public IQueryable<Customer> GetCustomersAdmin()
    {
        return _context.Customers;
    }

    /// <summary>
    /// Point de terminaison d'administration pour 
    /// modifier les accès d'un utilisateur
    /// </summary>
    /// <param name="id">Id de l'utilisateur</param>
    /// <param name="putAdminCustomerAccess">Les accès à modifier</param>
    /// <param name="token"></param>
    /// <returns></returns>
    [HttpPut]
    [Route("/admin/customers/{id}/customeraccess")]
    [Authorize(Roles = "administrateur")]
    public async Task<IActionResult> PutAdminCustomerAccess(Guid id, PutAdminCustomerAccess putAdminCustomerAccess, CancellationToken token)
    {
        var access = await _context.CustomerErablieres
            .FirstOrDefaultAsync(ce => ce.IdCustomer == id && ce.IdErabliere == putAdminCustomerAccess.IdErabliere, token);

        if (access == null)
        {
            // create a new access
            access = new CustomerErabliere
            {
                IdCustomer = id,
                IdErabliere = putAdminCustomerAccess.IdErabliere,
                Access = putAdminCustomerAccess.CustomerAccessLevel
            };

            await _context.AddAsync(access, token);
        }
        else 
        {
            access.Access = putAdminCustomerAccess.CustomerAccessLevel;
        }

        await _context.SaveChangesAsync(token);

        return NoContent();
    }

    /// <summary>
    /// Point de terminaison pour la suppression d'un utilisateur
    /// </summary>
    /// <param name="id">Id de l'utilisateur</param>
    /// <param name="token"></param>
    /// <returns></returns>
    [HttpDelete]
    [Route("/admin/customers/{id}")]
    [Authorize(Roles = "administrateur")]
    public async Task<IActionResult> DeleteCustomerAdmin(Guid id, CancellationToken token)
    {
        var customer = await _context.Customers
            .Include(c => c.CustomerErablieres)
            .FirstOrDefaultAsync(c => c.Id == id, token);

        if (customer == null)
        {
            return NoContent();
        }

        _context.Remove(customer);

        await _context.SaveChangesAsync(token);

        return NoContent();
    }

    /// <summary>
    /// Point de terminaison pour la suppression d'un utilisateur
    /// </summary>
    /// <param name="id">Id de l'utilisateur</param>
    /// <param name="token"></param>
    /// <returns></returns>
    [HttpDelete]
    [Route("/admin/customeraccess/{id}")]
    [Authorize(Roles = "administrateur")]
    public async Task<IActionResult> DeleteCustomerAccessAdmin(Guid id, CancellationToken token)
    {
        var customer = await _context.CustomerErablieres
            .FirstOrDefaultAsync(c => c.Id == id, token);

        if (customer == null)
        {
            return NoContent();
        }

        _context.Remove(customer);

        await _context.SaveChangesAsync(token);

        return NoContent();
    }
}
