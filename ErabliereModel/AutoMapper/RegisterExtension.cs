using Microsoft.Extensions.DependencyInjection;
using ErabliereApi.Donnees.Action.Get;
using ErabliereApi.Donnees.Action.Post;

namespace ErabliereApi.Donnees.AutoMapper
{
    public static class RegisterExtension
    {
        public static IServiceCollection AjouterAutoMapperErabliereApiDonnee(this IServiceCollection services) =>
            services.AddAutoMapper(config =>
            {
                config.CreateMap<Dompeux, GetDompeux>();

                config.CreateMap<PostErabliere, Erabliere>()
                      .ForMember(e => e.IpRule, a => a.MapFrom(p => p.IpRules))
                      .ReverseMap()
                      .ForMember(p => p.IpRules, a => a.MapFrom(e => e.IpRule));

                config.CreateMap<PostDonnee, Donnee>();
            });
    }
}
