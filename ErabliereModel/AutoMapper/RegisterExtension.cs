using Microsoft.Extensions.DependencyInjection;
using ErabliereApi.Donnees.Action.Get;
using ErabliereApi.Donnees.Action.Post;
using System;

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
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AjouterAutoMapperErabliereApiDonnee(this IServiceCollection services) =>
        services.AddAutoMapper(config =>
        {
            config.CreateMap<Capteur, GetCapteurs>().ReverseMap();
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
                  .ForMember(d => d.File, o => o.MapFrom(p => p.File != null ? Convert.FromBase64String(p.File) : null))
                  .ReverseMap()
                  .ForMember(d => d.File, o => o.MapFrom(p => p.File != null ? Convert.ToBase64String(p.File) : null));
        });
}
