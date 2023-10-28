using AutoFixture;
using AutoFixture.AutoNSubstitute;
using AutoMapper;
using ErabliereApi.Depot.Sql;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using ErabliereApi.Donnees.Action.Post;
using ErabliereApi.Donnees;
using Microsoft.EntityFrameworkCore;
using ErabliereApi.Donnees.AutoMapper;
using Microsoft.Extensions.Caching.Distributed;

namespace ErabliereApi.Test.Autofixture;
public static class ErabliereFixture
{
    /// <summary>
    /// Create and instance of <see cref="IFixture"/> with ErabliereAPI configuration.
    /// </summary>
    /// <param name="modelOnly">If set to false, DBContext will be register and populate with fixture data</param>
    /// <returns></returns>
    public static IFixture CreerFixture(bool modelOnly = true)
    {
        var fixture = new Fixture();

        fixture.Customize<ActionDescriptor>(c => c.OmitAutoProperties());
        fixture.Customize<ControllerContext>(c => c.OmitAutoProperties());
        fixture.Customize(new AutoNSubstituteCustomization());

        fixture.Customize<PostErabliere>(c => c.With(e => e.IpRules, () => fixture.CreateRandomIPAddress().ToString())
                                               .With(e => e.IsPublic, () => true));
        
        fixture.Customize<Erabliere>(c => c.With(e => e.IpRule, () => fixture.CreateRandomIPAddress().ToString())
                                           .Without(e => e.Donnees)
                                           .Without(e => e.Dompeux)
                                           .Without(e => e.Barils)
                                           .Without(e => e.Documentations)
                                           .Without(e => e.Notes)
                                           .Without(e => e.Alertes));

        fixture.Customize<Capteur>(c => c.Without(cc => cc.Erabliere));

        fixture.Customize<DonneeCapteur>(c => c.Without(cc => cc.Capteur)
                                               .Without(cc => cc.Owner));

        fixture.Customize<AlerteCapteur>(c => c.Without(cc => cc.Capteur)
                                               .Without(cc => cc.Owner)
                                               .Without(cc => cc.Id));

        fixture.Customize<Customer>(c =>
            c.With(c => c.Email, RandomEmail)
             .Without(c => c.ApiKeys)
             .Without(c => c.CustomerErablieres));
        
        if (!modelOnly)
        {
            var builder = GetServicesProvider();

            fixture.Register(() => builder.GetRequiredService<ErabliereDbContext>().PopulatesDbSets(fixture));
            fixture.Register(() => builder.GetRequiredService<IDistributedCache>());
            fixture.Register(() => builder.GetRequiredService<IMapper>());
            fixture.Register(() => fixture.CreateRandomIPAddress());

            fixture.Freeze<ErabliereDbContext>();
            var httpContext = fixture.Freeze<HttpContext>();

            httpContext.RequestServices = builder;
        }

        return fixture;
    }

    private static string RandomEmail()
    {
        var random = new Random();
        var domain = random.Next(0, 2) == 0 ? "gmail.com" : "hotmail.com";
        var name = Guid.NewGuid().ToString();
        var email = $"{name}@{domain}";
        return email;
    }

    private static IServiceProvider GetServicesProvider()
    {
        var services = new ServiceCollection();

        services.AddDbContext<ErabliereDbContext>(options =>
        {
            options.UseInMemoryDatabase(Guid.NewGuid().ToString());
        });

        services.AjouterAutoMapperErabliereApiDonnee();

        services.AddDistributedMemoryCache();

        return services.BuildServiceProvider();
    }
}
