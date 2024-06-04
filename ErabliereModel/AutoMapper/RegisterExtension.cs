using Microsoft.Extensions.DependencyInjection;
using ErabliereApi.Donnees.Action.Get;
using ErabliereApi.Donnees.Action.Post;
using System;
using ErabliereApi.Donnees.Action.Put;
using AutoMapper;

namespace ErabliereApi.Donnees.AutoMapper;

/// <summary>
/// Classe d'extension pour enregistrer les mapping entre les modèles exposés et les modèles de 
/// la base de données.
/// </summary>
public static class RegisterExtension
{
    /// <summary>
    /// Méthode pour ajouter les mapping entre les modèles exposés et les modèles de la base de données.
    /// </summary>
    /// <param name="services">The ServiceCollection instance to add automapper</param>
    /// <param name="additionnelConfig">An action to add additionnal mapping rule</param>
    /// <returns></returns>
    public static IServiceCollection AjouterAutoMapperErabliereApiDonnee(this IServiceCollection services, Action<IMapperConfigurationExpression>? additionnelConfig = null) =>
        services.AddAutoMapper(config =>
        {
            config.CreateMap<Alerte, GetAlerte>()
                  .ForMember(g => g.Emails, a => a.MapFrom(a => a.EnvoyerA != null ?
                                                                a.EnvoyerA.Split(';', StringSplitOptions.RemoveEmptyEntries) : 
                                                                new string[] { }))
                  .ForMember(g => g.Numeros, a => a.MapFrom(a => a.TexterA != null ?
                                                                a.TexterA.Split(';', StringSplitOptions.RemoveEmptyEntries) :
                                                                new string[] { }))
                  .ReverseMap();
            config.CreateMap<AlerteCapteur, GetAlerteCapteur>()
                  .ForMember(g => g.Emails, a => a.MapFrom(a => a.EnvoyerA != null ?
                                                                a.EnvoyerA.Split(';', StringSplitOptions.RemoveEmptyEntries) :
                                                                new string[] { }))
                  .ForMember(g => g.Numeros, a => a.MapFrom(a => a.TexterA != null ?
                                                                a.TexterA.Split(';', StringSplitOptions.RemoveEmptyEntries) :
                                                                new string[] { }))
                  .ReverseMap();
            config.CreateMap<CapteurImage, PostCapteurImage>().ReverseMap();
            config.CreateMap<CapteurImage, GetCapteurImage>().ReverseMap();
            config.CreateMap<CapteurImage, PutCapteurImage>().ReverseMap();
            config.CreateMap<Capteur, GetCapteur>().ReverseMap();
            config.CreateMap<Customer, GetCustomer>().ReverseMap();
            config.CreateMap<CustomerErabliere, GetCustomerAccess>().ReverseMap();
            config.CreateMap<Erabliere, GetCustomerAccessErabliere>().ReverseMap();
            config.CreateMap<Customer, GetCustomerAccessCustomer>().ReverseMap();
            config.CreateMap<Dompeux, GetDompeux>().ReverseMap();
            config.CreateMap<Donnee, GetDonnee>().ReverseMap();
            config.CreateMap<Baril, GetBaril>().ReverseMap();
            config.CreateMap<GetErabliereDashboard, Erabliere>().ReverseMap();
            config.CreateMap<DonneeCapteur, GetDonneesCapteur>().ReverseMap();

            config.CreateMap<PostErabliere, Erabliere>()
                  .ForMember(e => e.IpRule, a => a.MapFrom(p => p.IpRules))
                  .ReverseMap()
                  .ForMember(p => p.IpRules, a => a.MapFrom(e => e.IpRule));
            config.CreateMap<PostCapteur, Capteur>();
            config.CreateMap<PostDonnee, Donnee>();
            config.CreateMap<PostDonneeCapteur, DonneeCapteur>()
                  .ForMember(d => d.Valeur, o => o.MapFrom(p => p.V));
            config.CreateMap<PostDocumentation, Documentation>()
                  .ForMember(d => d.File, o => o.MapFrom(p => p.File != null ? Convert.FromBase64String(p.File) : null))
                  .ReverseMap()
                  .ForMember(d => d.File, o => o.MapFrom(p => p.File != null ? Convert.ToBase64String(p.File) : null));
            config.CreateMap<PostNote, Note>()
                  .ForMember(d => d.File, o => o.MapFrom(p => p.FileBytes != null ? p.FileBytes : p.File != null ? Convert.FromBase64String(p.File) : null))
                  .ReverseMap()
                  .ForMember(d => d.File, o => o.MapFrom(p => p.File != null ? Convert.ToBase64String(p.File) : null));
            config.CreateMap<Note, PostNoteMultipartResponse>()
                  .ReverseMap();
            config.CreateMap<PostRappel, Rappel>();

            config.CreateMap<PutAlerteCapteur, AlerteCapteur>();

            additionnelConfig?.Invoke(config);
        });
}
