namespace ErabliereApi;

/// <summary>
/// Modèle pour la configuration pour l'envoie de SMS. 
/// Utilisé pour la fonctionnalité d'alertage
/// </summary>
public class SMSConfig
{
    /// <summary>
    /// Le numéro de l'expéditeur
    /// </summary>
    public string? Numero { get; set; }

    /// <summary>
    /// Account SID d'application Twilio
    /// </summary>
    public string? AccountSid { get; set; }

    /// <summary>
    /// Auth token d'application Twilio
    /// </summary>
    public string? AuthToken { get; set; }

    /// <summary>
    /// Indique si la config est bien remplie
    /// </summary>
    public bool IsConfigured => Numero != null &&
                                AccountSid != null &&
                                AuthToken != null;
}