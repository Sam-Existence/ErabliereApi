using Azure.Identity;
using Microsoft.Extensions.Options;
using Microsoft.Graph;
using Microsoft.Graph.Models;
using MimeKit;

namespace ErabliereApi.Services.Emails;

/// <summary>
/// Implementation of <see cref="IEmailService" /> using MSGraph
/// </summary>
public class MSGraphEmailService : IEmailService
{
    private readonly IConfiguration _config;
    private readonly EmailConfig _emailConfig;

    /// <summary>
    /// Constructeur par défaut
    /// </summary>
    /// <param name="config"></param>
    /// <param name="emailConfig"></param>
    public MSGraphEmailService(
        IConfiguration config,
        IOptions<EmailConfig> emailConfig)
    {
        _config = config;
        _emailConfig = emailConfig.Value;
    }

    /// <inheritdoc />
    public async Task SendEmailAsync(MimeMessage mimeMessage, CancellationToken token)
    {
        string? tenantId = _emailConfig.TenantId;
        string? clientId = _config.GetValue<string>("AzureAD:ClientId");
        string? clientSecret = _config.GetValue<string>("AzureAD:ClientSecret");

        ClientSecretCredential credential = new(tenantId, clientId, clientSecret);
        GraphServiceClient graphClient = new(credential);

        Message message = new()
        {
            Subject = mimeMessage.Subject,
            Body = new ItemBody
            {
                ContentType = BodyType.Text,
                Content = mimeMessage.TextBody
            },
            ToRecipients = mimeMessage.To.Select(r => new Recipient
            {
                EmailAddress = new EmailAddress
                {
                    Address = r.Name
                }
            }).ToList()
        };

        bool saveToSentItems = true;

        var requestBuilder = graphClient.Users[_emailConfig.Sender];

        await requestBuilder.SendMail.PostAsync(new Microsoft.Graph.Users.Item.SendMail.SendMailPostRequestBody
        {
             Message = message,
             SaveToSentItems = saveToSentItems
        }, cancellationToken: token);
    }
}
