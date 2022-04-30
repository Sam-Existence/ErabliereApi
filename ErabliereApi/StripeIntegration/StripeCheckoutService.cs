using ErabliereApi.Services;
using Stripe.Checkout;
using Stripe;
using Microsoft.Extensions.Options;

namespace ErabliereApi.StripeIntegration;

/// <summary>
/// Implémentation de ICheckoutService permettan d'initialiser une session avec Stripe
/// </summary>
public class StripeCheckoutService : ICheckoutService
{
    private readonly IOptions<StripeOptions> _options;

    /// <summary>
    /// Constructeur
    /// </summary>
    /// <param name="options"></param>
    public StripeCheckoutService(IOptions<StripeOptions> options)
    {
        _options = options;
    }

    /// <summary>
    /// Implémentation de ICheckoutService permettan d'initialiser une session avec Stripe
    /// </summary>
    /// <returns></returns>
    public async Task<object> CreateSessionAsync()
    {
        StripeConfiguration.ApiKey = _options.Value.ApiKey;

        var options = new SessionCreateOptions
        {
            SuccessUrl = _options.Value.SuccessUrl,
            CancelUrl = _options.Value.CancelUrl,
            LineItems = new List<SessionLineItemOptions>
            {
                new SessionLineItemOptions
                {
                    Price = _options.Value.BasePlanPriceId
                }
            },
            Mode = "subscription",
            PaymentMethodTypes = new List<string>() { "card" }
        };

        var service = new SessionService();
        var session = await service.CreateAsync(options);

        return session;
    }
}
