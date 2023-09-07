using ErabliereApi.Donnees;
using ErabliereApi.Donnees.Action.NonHttp;

namespace ErabliereApi.Services.Users;

/// <summary>
/// Service permettant d'abstraire les interactions avec des utilisateurs
/// </summary>
public interface IUserService
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="customerId"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    Task<Stripe.Customer> StripeGetAsync(string customerId, CancellationToken token);

    /// <summary>
    /// Créer un utilisateur (<see cref="Customer"/>) 
    /// </summary>
    /// <param name="customer"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    Task<Customer> GetOrCreateCustomerAsync(Customer customer, CancellationToken token);
    
    /// <summary>
    /// Permet d'obtenir une instance de <see cref="Customer" /> avec les droits
    /// d'accès concernant l'érablière en paramètre.
    /// </summary>
    /// <param name="erabliere"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    Task<CustomerOwnershipAccess?> GetCurrentUserWithAccessAsync(Erabliere erabliere, CancellationToken token);

    /// <summary>
    /// Permet de modifier un customer pour appliquer les informations stripe.
    /// L'action se veut idempotent, si les données sont déjà appliqué, rien ne sera changé.
    /// </summary>
    /// <param name="customer"></param>
    /// <param name="stripeId"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    Task UpdateEnsureStripeInfoAsync(Customer customer, string stripeId, CancellationToken token);
}