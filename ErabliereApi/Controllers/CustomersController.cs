﻿using AutoMapper;
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
    [ProducesResponseType(200, Type = typeof(List<GetCustomer>))]
    public async Task<List<GetCustomer>> GetCustomers(CancellationToken token)
    {
        var customers = await _context.Customers.ProjectTo<GetCustomer>(_mapper.ConfigurationProvider)
                                                .ToListAsync(token);

        // Masquer avec des * certains caractères des adresses courriel
        foreach (var customer in customers.Where(c => !string.IsNullOrEmpty(c.Email) && c.Email.Contains('@')))
        {
            if (customer.Email == null) {
                continue;
            }

            var email = customer.Email.Split('@');
            var name = email[0];
            var domain = email[1];

            var nameLength = name.Length;
            var nameToHide = nameLength / 2;
            var nameStart = name.Substring(0, nameToHide);
            var nameEnd = name.Substring(nameToHide, nameLength - nameToHide);

            customer.Email = $"{nameStart}{new string('*', nameEnd.Length)}@{domain}";
        }

        // Masquer avec des * certains caractères des noms unique
        foreach (var customer in customers.Where(c => !string.IsNullOrEmpty(c.UniqueName) && c.UniqueName.Contains('@')))
        {
            if (customer.UniqueName == null) {
                continue;
            }

            var email = customer.UniqueName.Split('@');
            var name = email[0];
            var domain = email[1];

            var nameLength = name.Length;
            var nameToHide = nameLength / 2;
            var nameStart = name.Substring(0, nameToHide);
            var nameEnd = name.Substring(nameToHide, nameLength - nameToHide);

            customer.UniqueName = $"{nameStart}{new string('*', nameEnd.Length)}@{domain}";
        }

        return customers;
    }

    /// <summary>
    /// Point de terminaison pour l'administration des utilisateurs
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [EnableQuery]
    [Route("/admin/customers")]
    [Authorize(Roles = "administrateur", Policy = "TenantIdPrincipal")]
    public IQueryable<Customer> GetCustomersAdmin()
    {
        return _context.Customers;
    }

    /// <summary>
    /// Point de terminaison pour modifier un utilisateur
    /// </summary>
    /// <returns></returns>
    [HttpPut]
    [Route("/admin/customers/{id}")]
    [ProducesResponseType(200, Type = typeof(GetCustomer))]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [Authorize(Roles = "administrateur", Policy = "TenantIdPrincipal")]
    public async Task<IActionResult> PutCustomer(Guid id, PutCustomer putCustomer, CancellationToken token)
    {
        if (string.IsNullOrWhiteSpace(putCustomer.Name))
        {
            return BadRequest("Le nom ne doit pas être vide");
        }
        if (id != putCustomer.Id)
        {
            return BadRequest("L'id de l'utilisateur dans la route ne concorde pas avec l'id dans le corps du message.");
        }

        var entity = await _context.Customers.FindAsync([id], token);

        if (entity != null && entity.Id == id)
        {
            entity.Name = putCustomer.Name;

            await _context.SaveChangesAsync(token);

            return Ok(entity);
        }
        else
        {
            return NotFound();
        }
    }

    /// <summary>
    /// Point de terminaison d'administration pour 
    /// récupérer les accès d'un utilisateur
    /// </summary>
    /// <param name="id">Id de l'utilisateur</param>
    /// <param name="token"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("/admin/customers/{id}/customeraccess")]
    [Authorize(Roles = "administrateur", Policy = "TenantIdPrincipal")]
    [ProducesResponseType(200, Type = typeof(GetCustomerAccess))]
    public async Task<IActionResult> GetAdminCustomerAccess(Guid id, CancellationToken token)
    {
        var customer = await _context.Customers.FindAsync([id], cancellationToken: token);

        if (customer == null)
        {
            return NotFound();
        }

        var erablieres = await _context.CustomerErablieres.AsNoTracking()
            .Where(c => c.IdCustomer == id)
            .ProjectTo<GetCustomerAccess>(_mapper.ConfigurationProvider)
            .ToArrayAsync(token);

        return Ok(erablieres);
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
    [Authorize(Roles = "administrateur", Policy = "TenantIdPrincipal")]
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
    [Authorize(Roles = "administrateur", Policy = "TenantIdPrincipal")]
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
}
