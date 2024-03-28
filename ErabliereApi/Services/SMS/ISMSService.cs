using MimeKit;

namespace ErabliereApi.Services;

/// <summary>
/// Interface permettant d'abstraire l'envoie des SMS
/// </summary>
public interface ISMSService
{
    /// <summary>
    /// Envoie un SMS
    /// </summary>
    /// <param name="message"></param>
    /// <param name="destinataire"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    Task SendSMSAsync(string message, string destinataire, CancellationToken token);
}
