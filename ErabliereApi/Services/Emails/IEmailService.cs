using MimeKit;

namespace ErabliereApi.Services;

/// <summary>
/// Interface permettant d'abstraire l'envoie des email
/// </summary>
public interface IEmailService
{
    /// <summary>
    /// Envoie un courriel
    /// </summary>
    /// <param name="message"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    Task SendEmailAsync(MimeMessage message, CancellationToken token);
}
