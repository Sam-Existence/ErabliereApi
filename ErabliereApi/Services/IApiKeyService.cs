using ErabliereApi.Donnees;

namespace ErabliereApi.Services;

public interface IApiKeyService
{
    Task<ApiKey> CreateApiKey(string email, CancellationToken token);

    string HashApiKey(string key);

    string HashApiKey(byte[] key);
}
