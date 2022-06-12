using ErabliereApi.Depot.Sql;
using ErabliereApi.Donnees;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Linq.Expressions;
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
    private readonly ILogger<ErabiereApiApiKeyService> _logger;

    /// <summary>
    /// Constructeur par initlaisation
    /// </summary>
    /// <param name="context"></param>
    /// <param name="emailService"></param>
    /// <param name="emailConfig"></param>
    /// <param name="logger"></param>
    public ErabiereApiApiKeyService(
        ErabliereDbContext context, 
        IEmailService emailService, 
        IOptions<EmailConfig> emailConfig,
        ILogger<ErabiereApiApiKeyService> logger)
    {
        _context = context;
        _emailService = emailService;
        _emailConfig = emailConfig.Value;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<ApiKey> CreateApiKeyAsync(string email, CancellationToken token)
    {
        var customer = await TryGetCustomerAsync(x => x.Email == email, token);

        if (customer == null)
        {
            throw new InvalidOperationException("A customer instance is required");
        }

        if (!customer.Id.HasValue)
        {
            throw new InvalidOperationException("customer is supposed to have Id");
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
        Customer? customer = await TryGetCustomerAsync(c => c.StripeId == customerId, token);

        if (customer != null)
        {
            var now = DateTimeOffset.Now;

            var apiKey = await TryGetApiKeyAsync(a => 
                                a.CustomerId == customer.Id &&
                                a.CreationTime <= now &&
                                a.RevocationTime == null &&
                                a.DeletionTime == null, token);

            if (apiKey == null)
            {
                throw new InvalidOperationException("apiKey is required to continue the process");
            }

            apiKey.SubscriptionId = id;

            var entity = _context.Update(apiKey);

            await _context.SaveChangesAsync(token);
        }
        else
        {
            _logger.LogCritical("customer was null inside the SetSubscriptionKeyAsync method");
        }
    }

    private async Task<Customer?> TryGetCustomerAsync(Expression<Func<Customer, bool>> predicat, CancellationToken token)
    {
        var shouldRetry = 10;

        while (shouldRetry > 0)
        {
            try
            {
                var customer = await _context.Customers.SingleAsync(predicat, token);

                return customer;
            }
            catch (InvalidOperationException)
            {
                if (shouldRetry == 0)
                {
                    throw;
                }

                shouldRetry--;
                _logger.LogInformation("Customer was not found in the database, wait 1 seconds and retry. RetryLeft: {shouldRetry}", shouldRetry);
                await Task.Delay(1000, token);
            }
        }

        return null;
    }

    private async Task<ApiKey?> TryGetApiKeyAsync(Expression<Func<ApiKey, bool>> predicat, CancellationToken token)
    {
        var shouldRetry = 10;

        while (shouldRetry > 0)
        {
            try
            {
                var apikey = await _context.ApiKeys
                    .Where(predicat).OrderByDescending(a => a.CreationTime)
                    .FirstAsync(token);

                return apikey;
            }
            catch (InvalidOperationException)
            {
                if (shouldRetry == 0)
                {
                    throw;
                }

                shouldRetry--;
                _logger.LogInformation("Customer was not found in the database, wait 1 seconds and retry. RetryLeft: {shouldRetry}", shouldRetry);
                await Task.Delay(1000, token);
            }
        }

        return null;
    }

    /// <inheritdoc />
    public bool TryHashApiKey(string key, out string? hashApiKey)
    {
        try
        {
            hashApiKey = HashApiKey(key);

            return true;
        }
        catch { }

        hashApiKey = null;

        return false;
    }
}
