using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using MimeKit;

namespace ErabliereApi.Services;

/// <summary>
/// Implementation de <see cref="IEmailService" /> envoyant des courriel utilisant
/// <see cref="IOptions{EmailConfig}" /> and <see cref="SmtpClient" />
/// </summary>
public class ErabliereApiEmailService : IEmailService
{
    private readonly EmailConfig _config;
    private readonly IConfiguration _configuration;
    private readonly ILogger<ErabliereApiEmailService> _logger;
    private readonly ISmtpClient _smtpClient;

    /// <summary>
    /// Constructeur par initlaisation
    /// </summary>
    /// <param name="config"></param>
    /// <param name="configuration"></param>
    /// <param name="logger"></param>
    /// <param name="smtpClient"></param>
    public ErabliereApiEmailService(
        IOptions<EmailConfig> config,
        IConfiguration configuration,
        ILogger<ErabliereApiEmailService> logger,
        ISmtpClient smtpClient)
    {
        _config = config.Value;
        _configuration = configuration;
        _logger = logger;
        _smtpClient = smtpClient;
    }

    /// <inheritdoc />
    public async Task SendEmailAsync(MimeMessage message, CancellationToken token)
    {
        if (_smtpClient.IsConnected == false)
        {
            await _smtpClient.ConnectAsync(_config.SmtpServer, _config.SmtpPort, SecureSocketOptions.StartTls, token);
        }

        if (string.IsNullOrWhiteSpace(_config.Password))
        {
            await OAuthAuthenticateAsync(token);
        }
        else
        {
            await _smtpClient.AuthenticateAsync(_config.Email, _config.Password, token);
        }

        await _smtpClient.SendAsync(message, token);
        await _smtpClient.DisconnectAsync(true, token);
    }

    private async Task OAuthAuthenticateAsync(CancellationToken token)
    {
        var clientId = _configuration.GetValue<string>("AzureAD:ClientId");
        var clientSecret = _configuration.GetValue<string>("AzureAD:ClientSecret");
        var tenantId = _config.TenantId;

        var confidentialClientApplication = ConfidentialClientApplicationBuilder.Create(clientId)
            .WithAuthority($"https://login.microsoftonline.com/{tenantId}/v2.0")
            .WithClientSecret(clientSecret)
            .Build();

        var scopes = new string[] {
            // For IMAP and POP3, use the following scope
            //"https://ps.outlook.com/.default"

            // For SMTP, use the following scope
            "https://outlook.office365.com/.default"
        };

        var authToken = await confidentialClientApplication.AcquireTokenForClient(scopes).ExecuteAsync(token);

        var sasl = new SaslMechanismOAuth2(_config.Email, authToken.AccessToken);

        try
        {
            await _smtpClient.AuthenticateAsync(sasl, token);
        }
        catch
        {
            _logger.LogWarning($"Authentication unsuccessful with token: {authToken.AccessToken}");
            throw;
        }
    }
}
