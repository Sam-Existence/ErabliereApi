using ErabliereApi.Donnees;

namespace ErabliereApi.Services;

/// <summary>
/// Interface d'abstraction des clé d'api
/// </summary>
public interface IApiKeyService
{
    /// <summary>
    /// Créer une clé d'api pour le email donnée
    /// </summary>
    /// <param name="customer"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    Task<ApiKey> CreateApiKeyAsync(Customer customer, CancellationToken token);

    /// <summary>
    /// Hash an api key from a string
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    string HashApiKey(string key);

    /// <summary>
    /// Try hash a given key
    /// </summary>
    /// <param name="key"></param>
    /// <param name="hashApiKey"></param>
    /// <returns></returns>
    bool TryHashApiKey(string key, out string? hashApiKey);

    /// <summary>
    /// Hash an api key from a byte array
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    string HashApiKey(byte[] key);

    /// <summary>
    /// Set the subscription key for a customer
    /// </summary>
    /// <param name="customer">The ErabliereAPI Customer</param>
    /// <param name="id">The stripe subscription id</param>
    /// <param name="token"></param>
    /// <returns></returns>
    Task SetSubscriptionKeyAsync(Donnees.Customer customer, string id, CancellationToken token);
}
