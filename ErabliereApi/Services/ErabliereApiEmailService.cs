using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;

namespace ErabliereApi.Services;

public class ErabliereApiEmailService : IEmailService
{
    private readonly EmailConfig _config;

    public ErabliereApiEmailService(IOptions<EmailConfig> config)
    {
        _config = config.Value;
    }

    public async Task SendEmailAsync(MimeMessage message, CancellationToken token)
    {
        using var smtpClient = new SmtpClient();
        await smtpClient.ConnectAsync(_config.SmtpServer, _config.SmtpPort, MailKit.Security.SecureSocketOptions.StartTls, token);
        await smtpClient.AuthenticateAsync(_config.Email, _config.Password, token);
        await smtpClient.SendAsync(message, token);
        await smtpClient.DisconnectAsync(true, token);
    }
}
