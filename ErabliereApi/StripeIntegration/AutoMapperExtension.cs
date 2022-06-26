using AutoMapper;

namespace ErabliereApi.StripeIntegration;

/// <summary>
/// Classe contenant les instructions de mapping
/// </summary>
public static class AutoMapperExtension
{
    /// <summary>
    /// Ajout des règles de mapping pour la gestion des utilisateurs
    /// </summary>
    /// <param name="config"></param>
    public static void AddCustomersApiKeyMappings(IMapperConfigurationExpression config)
    {
        config.CreateMap<Stripe.Customer, ErabliereApi.Donnees.Customer>()
              .ForMember(s => s.Email, o => o.MapFrom(e => e.Email))
              .ForMember(s => s.UniqueName, o => o.MapFrom(e => e.Email))
              .ForMember(s => s.Name, o => o.MapFrom(e => e.Name))
              .ForMember(s => s.StripeId, o => o.MapFrom(e => e.Id))
              .ForMember(s => s.Id, o => o.Ignore())
              .AfterMap((stripeCustomer, erabiereApiCustomer) =>
              {
                  erabiereApiCustomer.AccountType = "Stripe.Customer";
              });
    }
}
