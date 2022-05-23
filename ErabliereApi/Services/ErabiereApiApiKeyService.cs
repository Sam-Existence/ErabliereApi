using ErabliereApi.Depot.Sql;
using ErabliereApi.Donnees;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Security.Cryptography;

namespace ErabliereApi.Services;

/// <summary>
/// Implémentation de <see cref="IApiKeyService" /> gérant les clé avec la logique interne 
/// au projet ErabliereApi
/// </summary>
public class ErabiereApiApiKeyService : IApiKeyService
{
    private readonly ErabliereDbContext _context;
    private readonly IEmailService _emailService;
    private readonly EmailConfig _emailConfig;

    /// <summary>
    /// Constructeur par initlaisation
    /// </summary>
    /// <param name="context"></param>
    /// <param name="emailService"></param>
    /// <param name="emailConfig"></param>
    public ErabiereApiApiKeyService(ErabliereDbContext context, IEmailService emailService, IOptions<EmailConfig> emailConfig)
    {
        _context = context;
        _emailService = emailService;
        _emailConfig = emailConfig.Value;
    }

    /// <inheritdoc />
    public async Task<ApiKey> CreateApiKeyAsync(string email, CancellationToken token)
    {
        var customer = _context.Customers.FirstOrDefault(x => x.Email == email);

        if (customer == null || customer.Id == null)
        {
            throw new InvalidOperationException("Customer dosen't exist");
        }

        var apiKeyBytes = Guid.NewGuid().ToByteArray();

        var apiKeyObj = new ApiKey
        {
            Key = HashApiKey(apiKeyBytes),
            CustomerId = customer.Id.Value
        };

        await _context.ApiKeys.AddAsync(apiKeyObj, token);

        await SendEmailAsync(email, Convert.ToBase64String(apiKeyBytes), token);

        await _context.SaveChangesAsync(token);

        return apiKeyObj;
    }

    private async Task SendEmailAsync(string email, string originalKey, CancellationToken token)
    {
        if (_emailConfig.IsConfigured)
        {
            var mailMessage = new MimeMessage();
            mailMessage.From.Add(new MailboxAddress("ErabliereAPI - Your API Key", _emailConfig.Sender));
            mailMessage.To.Add(MailboxAddress.Parse(email));
            mailMessage.Subject = $"Your API Key for ErabliereAPI";
            mailMessage.Body = new TextPart("plain")
            {
                Text = $"Your api key is: {originalKey}"
            };

            await _emailService.SendEmailAsync(mailMessage, token);
        }
    }

    /// <inheritdoc />
    public string HashApiKey(byte[] key)
    {
        using var sha = SHA256.Create();

        var hash = sha.ComputeHash(key);

        var hashedContent = Convert.ToBase64String(hash);

        return hashedContent;
    }

    /// <inheritdoc />
    public string HashApiKey(string key)
    {
        return HashApiKey(Convert.FromBase64String(key));
    }

    /// <inheritdoc />
    public async Task SetSubscriptionKeyAsync(
        string customerId, string id, CancellationToken token)
    {
        var customer = await _context.Customers
            .SingleAsync(c => c.StripeId == customerId, token);

        var now = DateTimeOffset.Now;

        var apiKey = await _context.ApiKeys
            .Where(a => a.CustomerId == customer.Id && 
                        a.CreationTime <= now &&
                        a.RevocationTime == null &&
                        a.DeletionTime == null).OrderByDescending(a => a.CreationTime)
            .FirstAsync(token);

        apiKey.SubscriptionId = id;

        var entity = _context.Update(apiKey);

        await _context.SaveChangesAsync(token);
    }
}
