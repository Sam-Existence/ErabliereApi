using ErabliereApi.Services;
using Stripe.Checkout;
using Stripe;
using Microsoft.Extensions.Options;

namespace ErabliereApi.StripeIntegration;


public class StripeCheckoutService : ICheckoutService
{
    private readonly IOptions<StripeOptions> _options;

    public StripeCheckoutService(IOptions<StripeOptions> options)
    {
        _options = options;
    }

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
                    Price = _options.Value.BasePlanPriceId,
                    Quantity = 1,
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
