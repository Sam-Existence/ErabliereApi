using ErabliereApi.Extensions;
using ErabliereApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace ErabliereApi.Controllers;

/// <summary>
/// Contrôler de checkout
/// </summary>
[ApiController]
[Route("/[controller]")]
public class CheckoutController : ControllerBase
{
    private readonly ICheckoutService _checkoutService;
    private readonly IConfiguration _configuration;

    /// <summary>
    /// Constructeur par initialisation
    /// </summary>
    public CheckoutController(
        ICheckoutService checkoutService,
        IConfiguration configuration)
    {
        _checkoutService = checkoutService;
        _configuration = configuration;
    }

    /// <summary>
    /// Create a checkout session
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> Checkout(CancellationToken token)
    {
        if (!_configuration.StripeIsEnabled())
        {
            return NotFound();
        }

        var session = await _checkoutService.CreateSessionAsync(token);

        return Ok(session);
    }

    /// <summary>
    /// Webhook utilisé pour recevoir des appels d'un fournisseur de paiement
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    [Route("[action]")]
    public async Task<IActionResult> Webhook()
    {
        if (!_configuration.StripeIsEnabled())
        {
            return NotFound();
        }

        using var reader = new StreamReader(HttpContext.Request.Body);

        var json = await reader.ReadToEndAsync();
        
        await _checkoutService.Webhook(json);

        return Ok();
    }
}
