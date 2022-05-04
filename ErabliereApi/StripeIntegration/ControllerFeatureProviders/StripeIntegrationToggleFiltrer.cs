using System.Reflection;
using ErabliereApi.Controllers;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace ErabliereApi.StripeIntegration;

/// <summary>
/// Classe de filtre pour le contrôleur de checkout
/// </summary>
public class StripeIntegrationToggleFiltrer : ControllerFeatureProvider
{
    private readonly IConfiguration _config;

    /// <summary>
    /// Build a ControllerFeatureProvider that will filter out Stripe integration controllers
    /// base on if there is an api key in the configuration. The key checked is 'Stripe.ApiKey'.
    /// </summary>
    public StripeIntegrationToggleFiltrer(IConfiguration config)
    {
        _config = config;
    }
    
    /// <inheritdoc />
    protected override bool IsController(TypeInfo typeInfo) 
    {
        if (typeInfo.Name == nameof(CheckoutController)) 
        {
            var stripeEnabled = !string.IsNullOrWhiteSpace(_config["Stripe.ApiKey"]);

            return stripeEnabled;
        }

        return base.IsController(typeInfo);
    }
}