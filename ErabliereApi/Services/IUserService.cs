using ErabliereApi.Donnees;

namespace ErabliereApi.Services;

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
}