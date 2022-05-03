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

    /// <summary>
    /// Constructeur par initialisation
    /// </summary>
    public CheckoutController(ICheckoutService checkoutService)
    {
        _checkoutService = checkoutService;
    }

    /// <summary>
    /// Create a checkout session
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> Checkout()
    {
        var session = await _checkoutService.CreateSessionAsync();

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
        using var reader = new StreamReader(HttpContext.Request.Body);

        var json = await reader.ReadToEndAsync();
        
        await _checkoutService.Webhook(json);

        return Ok();
    }
}
