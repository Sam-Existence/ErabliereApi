namespace ErabliereApi.StripeIntegration;

/// <summary>
/// Classe représentant une utilisation avec une clé d'API
/// </summary>
public class Usage
{
    /// <summary>
    /// SubscriptionId de la clé d'API
    /// </summary>
    public string SubscriptionId { get; set; } = "";

    /// <summary>
    /// Quantité d'utilisation
    /// </summary>
    public int Quantite { get; set; }
}