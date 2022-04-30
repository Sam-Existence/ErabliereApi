using ErabliereApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ErabliereApi.Controllers;

/// <summary>
/// Contrôler de checkout
/// </summary>
[ApiController]
[Route("/[controller]")]
[Authorize]
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

    
}
