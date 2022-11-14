using ErabliereApi.Donnees;
using ErabliereApi.Donnees.Action.NonHttp;

namespace ErabliereApi.Services.Users;

/// <summary>
/// Service permettant d'abstraire les interactions avec des utilisateurs
/// </summary>
public interface IUserService
{
    /// <summary>
    /// Créer un utilisateur (<see cref="Customer"/>) 
    /// </summary>
    /// <param name="customer"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    Task CreateCustomerAsync(Customer customer, CancellationToken token);
    
    /// <summary>
    /// Permet d'obtenir une instance de <see cref="Customer" /> avec les droits
    /// d'accès concernant l'érablière en paramètre.
    /// </summary>
    /// <param name="erabliere"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    Task<CustomerOwnershipAccess?> GetCurrentUserWithAccessAsync(Erabliere erabliere, CancellationToken token);
}