using System.Reflection;
using ErabliereApi.Controllers;
using Microsoft.AspNetCore.Mvc.Controllers;

/// <summary>
/// Classe de filtre pour le contrôleur de checkout
/// </summary>
public class StripeIntegrationToggleFiltrer : ControllerFeatureProvider
{
    private readonly bool _stripeEnabled;

    /// <summary>
    /// Build a ControllerFeatureProvider that will filter out Stripe integration controllers
    /// base on if there is an api key in the configuration. The key checked is 'Stripe.ApiKey'.
    /// </summary>
    public StripeIntegrationToggleFiltrer(IConfiguration config)
    {
        _stripeEnabled = !string.IsNullOrWhiteSpace(config["Stripe.ApiKey"]);
    }
    
    /// <inheritdoc />
    protected override bool IsController(TypeInfo typeInfo) 
    {
        if (typeInfo.Name == nameof(CheckoutController)) 
        {
            return _stripeEnabled;
        }

        return base.IsController(typeInfo);
    }
}