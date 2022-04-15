using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Stripe;
using Stripe.Checkout;

namespace ErabliereApi.Controllers;

/// <summary>
/// Contrôler de checkout
/// </summary>
[ApiController]
[Route("/[controller]")]
public class CheckoutController : ControllerBase
{
    private readonly IOptions<StripeOptions> _options;

    /// <summary>
    /// Constructeur par initialisation
    /// </summary>
    public CheckoutController(IOptions<StripeOptions> options)
    {
        _options = options;
    }

    /// <summary>
    /// Create a checkout session
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> Checkout()
    {
        StripeConfiguration.ApiKey = _options.Value.ApiKey;

        var options = new SessionCreateOptions
        {
            SuccessUrl = "https://localhost:4200/success",
            CancelUrl = "https://localhost:4200/cancel",
            LineItems = new List<SessionLineItemOptions>
            {
                new SessionLineItemOptions
                {
                    Price = "price_1KoWXfCADOntGQHJeaOmVHel",
                    Quantity = 1,
                },
            },
            Mode = "subscription",
            PaymentMethodTypes = new List<string>() { "card" },

        };
        var service = new SessionService();
        var session = await service.CreateAsync(options);

        return Ok(session);
    }
}
