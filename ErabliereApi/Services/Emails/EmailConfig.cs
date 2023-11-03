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
    /// Laisser vide pour utiliser l'authentification OAuth.
    /// Utiliser pour l'authentification basic.
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

    /// <summary>
    /// Indicate if email is configure.
    /// Return true if email and sender are not null
    /// </summary>
    public bool IsConfigured => Email != null && 
                                Sender != null;

    /// <summary>
    /// Indique le tenent, nécessaire pour l'authentication oauth
    /// </summary>
    public string? TenantId { get; set; }

    /// <summary>
    /// Indiquate if the MSGraph service should be use to send email
    /// </summary>
    public bool? UseMSGraphAPI { get; set; }
}
