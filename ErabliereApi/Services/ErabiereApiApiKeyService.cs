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
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IEmailService _emailService;
    private readonly EmailConfig _emailConfig;
    private readonly ILogger<ErabiereApiApiKeyService> _logger;
    private readonly IConfiguration _config;

    /// <summary>
    /// Constructeur par initlaisation
    /// </summary>
    /// <param name="scopeFactory"></param>
    /// <param name="emailService"></param>
    /// <param name="emailConfig"></param>
    /// <param name="logger"></param>
    /// <param name="config"></param>
    public ErabiereApiApiKeyService(
        IServiceScopeFactory scopeFactory, 
        IEmailService emailService, 
        IOptions<EmailConfig> emailConfig,
        ILogger<ErabiereApiApiKeyService> logger,
        IConfiguration config)
    {
        _scopeFactory = scopeFactory;
        _emailService = emailService;
        _emailConfig = emailConfig.Value;
        _logger = logger;
        _config = config;
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

        using (var scope = _scopeFactory.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<ErabliereDbContext>();

            await context.ApiKeys.AddAsync(apiKeyObj, token);

            await context.SaveChangesAsync(token);
        }        

        await SendEmailAsync(email, Convert.ToBase64String(apiKeyBytes), token);

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

            using (var scope = _scopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ErabliereDbContext>();

                var entity = context.Update(apiKey);

                await context.SaveChangesAsync(token);
            }
        }
        else
        {
            _logger.LogCritical("customer was null inside the SetSubscriptionKeyAsync method");
        }
    }

    private async Task<Customer?> TryGetCustomerAsync(Expression<Func<Customer, bool>> predicat, CancellationToken token)
    {
        var retryCount = _config.GetValue<int>("ErabliereApiKeyService:TryGetCustomer:TryCount");
        var milisecondDelay = _config.GetValue<int>("ErabliereApiKeyService:TryGetCustomer:DelayBetweenTryMilliseconds");

        while (retryCount > 0)
        {
            try
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<ErabliereDbContext>();

                    var customer = await context.Customers.SingleAsync(predicat, token);

                    return customer;
                }
            }
            catch (InvalidOperationException)
            {
                if (retryCount == 0)
                {
                    throw;
                }

                retryCount--;
                _logger.LogWarning($"Customer was not found in the database, wait {milisecondDelay / 1000} seconds and retry. RetryLeft: {retryCount}", retryCount);
                await Task.Delay(
                    millisecondsDelay: milisecondDelay, 
                    cancellationToken: token);
            }
        }

        return null;
    }

    private async Task<ApiKey?> TryGetApiKeyAsync(Expression<Func<ApiKey, bool>> predicat, CancellationToken token)
    {
        var retryCount = _config.GetValue<int>("ErabliereApiKeyService:TryGetApiKey:TryCount");
        var milisecondDelay = _config.GetValue<int>("ErabliereApiKeyService:TryGetApiKey:DelayBetweenTryMilliseconds");

        while (retryCount > 0)
        {
            try
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<ErabliereDbContext>();

                    var apikey = await context.ApiKeys
                        .Where(predicat).OrderByDescending(a => a.CreationTime)
                        .FirstAsync(token);

                    return apikey;
                }
            }
            catch (InvalidOperationException)
            {
                if (retryCount == 0)
                {
                    throw;
                }

                retryCount--;
                _logger.LogWarning($"Customer was not found in the database, wait {milisecondDelay / 1000} seconds and retry. RetryLeft: {retryCount}");
                await Task.Delay(milisecondDelay, token);
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
