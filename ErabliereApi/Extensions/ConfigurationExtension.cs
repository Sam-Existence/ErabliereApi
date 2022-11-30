using static System.Boolean;
using static System.StringComparison;

namespace ErabliereApi.Extensions;

/// <summary>
/// M�thode d'extension de <see cref="IConfiguration"/>
/// </summary>
public static class ConfigurationExtension
{
    /// <summary>
    /// Indique si Stripe est activé
    /// </summary>
    /// <returns></returns>
    public static bool StripeIsEnabled(this IConfiguration config)
    {
        return !string.IsNullOrWhiteSpace(config["Stripe.ApiKey"]);
    }

    /// <summary>
    /// Indique si l'authentification est activée
    /// </summary>
    /// <param name="config"></param>
    /// <returns></returns>
    public static bool IsAuthEnabled(this IConfiguration config)
    {
        return string.Equals(config["USE_AUTHENTICATION"], TrueString, OrdinalIgnoreCase);
    }

    /// <summary>
    /// Indique si le mode ChaosEngineering est activé
    /// </summary>
    /// <param name="config"></param>
    /// <returns></returns>
    public static bool IsChaosEngineeringEnabled(this IConfiguration config)
    {
        return string.Equals(config["USE_CHAOS_ENGINEERING"], TrueString, OrdinalIgnoreCase);
    }
}
