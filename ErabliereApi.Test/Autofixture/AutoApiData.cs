using AutoFixture;
using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using AutoMapper;
using ErabliereApi.Depot.Sql;
using ErabliereApi.Donnees;
using ErabliereApi.Donnees.Action.Post;
using ErabliereApi.Donnees.AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ErabliereApi.Test.Autofixture
{
    internal class AutoApiData : AutoDataAttribute
    {
        public AutoApiData() : base(() =>
        {
            var fixture = new Fixture();

            var builder = GetServicesProvider();

            fixture.Customize<ActionDescriptor>(c => c.OmitAutoProperties());
            fixture.Customize<ControllerContext>(c => c.OmitAutoProperties());
            fixture.Customize(new AutoNSubstituteCustomization());

            fixture.Customize<PostErabliere>(c => c.With(e => e.IpRules, () => fixture.CreateRandomIPAddress().ToString()));
            fixture.Customize<Erabliere>(c => c.With(e => e.IpRule, () => fixture.CreateRandomIPAddress().ToString())
                                               .Without(e => e.Donnees)
                                               .Without(e => e.Dompeux)
                                               .Without(e => e.Barils)
                                               .Without(e => e.Capteurs));

            fixture.Register(() => builder.GetRequiredService<ErabliereDbContext>().PopulatesDbSets(fixture));
            fixture.Register(() => builder.GetRequiredService<IMapper>());
            fixture.Register(() => fixture.CreateRandomIPAddress());

            fixture.Freeze<ErabliereDbContext>();
            var httpContext = fixture.Freeze<HttpContext>();

            httpContext.RequestServices = builder;

            return fixture;
        })
        {

        }

        private static IServiceProvider GetServicesProvider()
        {
            var services = new ServiceCollection();

            services.AddDbContext<ErabliereDbContext>(options =>
            {
                options.UseInMemoryDatabase(Guid.NewGuid().ToString());
            });

            services.AjouterAutoMapperErabliereApiDonnee();

            return services.BuildServiceProvider();
        }
    }
}
