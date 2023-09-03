using AutoMapper;
using AutoMapper.QueryableExtensions;
using ErabliereApi.Depot.Sql;
using ErabliereApi.Donnees;
using ErabliereApi.Donnees.Action.Get;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;

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
}
