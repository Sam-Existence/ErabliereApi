using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;

namespace ErabliereApi.Extensions;

/// <summary>
/// Extension pour le cache distribué
/// </summary>
public static class DistributedCacheExtension 
{
    /// <summary>
    /// Obtenir un objet du cache
    /// </summary>
    public static async Task<T?> GetAsync<T>(this IDistributedCache cache, string key, CancellationToken token) where T : class 
    {
        var value = await cache.GetStringAsync(key, token);

        if (value == null) 
        {
            return null;
        }

        return JsonSerializer.Deserialize<T>(value);
    }

    /// <summary>
    /// Ajouter un objet au cache
    /// </summary>
    public static Task SetAsync<T>(this IDistributedCache cache, string key, T value, CancellationToken token) where T : class 
    {
        return cache.SetStringAsync(key, JsonSerializer.Serialize(value), token);
    }
}