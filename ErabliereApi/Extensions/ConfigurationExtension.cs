using System.ComponentModel;
using System.Text;

namespace ErabliereApi.Extensions;

/// <summary>
/// Méthode d'extension de <see cref="IConfiguration"/>
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
}
