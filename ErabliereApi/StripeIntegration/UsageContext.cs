using System.Collections.Concurrent;

/// <summary>
/// Context de l'utilisation de l'API pour les utilisateurs avec des clé d'API
/// </summary>
public class UsageContext
{
    /// <summary>
    /// Les utilisations
    /// </summary>
    public ConcurrentQueue<Usage> Usages = new ConcurrentQueue<Usage>();
}