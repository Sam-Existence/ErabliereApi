namespace ErabliereApi;

/// <summary>
/// Modèle pour la configuration pour l'envoie de courriel. 
/// Utilisé pour la fonctionnalité d'alertage
/// </summary>
public class EmailConfig
{
    /// <summary>
    /// L'adresse couriel de l'expéditeur
    /// </summary>
    public string? Sender { get; set; }

    /// <summary>
    /// Adresse couriel réel
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// Mot de passe du couriel de la propriété <see cref="Email" />
    /// </summary>
    public string? Password { get; set; }

    /// <summary>
    /// Adresse du server Smtp
    /// </summary>
    public string? SmtpServer { get; set; }

    /// <summary>
    /// Port du server Smpt
    /// </summary>
    public int SmtpPort { get; set; }
}
