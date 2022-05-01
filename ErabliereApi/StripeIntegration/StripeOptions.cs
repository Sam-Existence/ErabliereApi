namespace ErabliereApi;

/// <summary>
/// Classe d'option pour l'intégration Stripe
/// </summary>
public class StripeOptions
{
    /// <summary>
    /// Indique si stripe est activé
    /// </summary>
    public bool IsEnabled { get; set; }

    /// <summary>
    /// La clé d'api secret de Stripe
    /// </summary>
    public string? ApiKey { get; set; }

    /// <summary>
    /// L'url de redirection lors de succès de checkout
    /// </summary>
    public string? SuccessUrl { get; set; }

    /// <summary>
    /// L'url de redirection lors de échec de checkout
    /// </summary>
    public string? CancelUrl { get; set; }

    /// <summary>
    /// L'id stripe du plan de base
    /// </summary>
    public string? BasePlanPriceId { get; set; }
    public string WebhookSecret { get; internal set; }
    public string WebhookSiginSecret { get; internal set; }
}