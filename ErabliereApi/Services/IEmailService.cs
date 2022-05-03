using MimeKit;

namespace ErabliereApi.Services;

public interface IEmailService
{
    Task SendEmailAsync(MimeMessage message, CancellationToken token);
}
