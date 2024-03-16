using System.Reflection;
using ErabliereApi.Controllers;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace ErabliereApi.ControllerFeatureProviders;

/// <summary>
/// Classe de filtre pour le contrôleur de checkout
/// </summary>
public class ErabliereAPIControllerFeatureProvider : ControllerFeatureProvider
{
    private readonly IConfiguration _config;

    /// <summary>
    /// Build a ControllerFeatureProvider that will filter out Stripe integration controllers
    /// base on if there is an api key in the configuration. The key checked is 'Stripe.ApiKey'.
    /// </summary>
    public ErabliereAPIControllerFeatureProvider(IConfiguration config)
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

        if (typeInfo.Name == nameof(ErabliereAIController))
        {
            var aiEnable = !string.IsNullOrWhiteSpace(_config["AzureOpenAIUri"]);

            return aiEnable;
        }

        if (typeInfo.Name == nameof(CallsController))
        {
            var enableCalls = !string.IsNullOrWhiteSpace(_config["agora.localbackend"]);

            return enableCalls;
        }

        if (typeInfo.Name == nameof(ImagesCapteurController))
        {
            var enableImages = !string.IsNullOrWhiteSpace(_config["EmailImageObserverUrl"]);

            return enableImages;
        }

        return base.IsController(typeInfo);
    }
}