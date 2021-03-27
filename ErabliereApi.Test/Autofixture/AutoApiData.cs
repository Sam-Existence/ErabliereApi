using AutoFixture;
using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using AutoMapper;
using ErabliereApi.Depot.Sql;
using ErabliereApi.Donnees;
using ErabliereApi.Donnees.Action.Post;
using ErabliereApi.Donnees.AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net;

namespace ErabliereApi.Test.Autofixture
{
    internal class AutoApiData : AutoDataAttribute
    {
        public AutoApiData() : base(() =>
        {
            var fixture = new Fixture();

            var builder = GetServicesProvider();

            fixture.Customize<ControllerContext>(c => c.OmitAutoProperties());
            fixture.Customize(new AutoNSubstituteCustomization());

            fixture.Customize<PostErabliere>(c => c.With(e => e.IpRules, () => fixture.CreateRandomIPAddress().ToString()));
            fixture.Customize<Erabliere>(c => c.With(e => e.IpRule, () => fixture.CreateRandomIPAddress().ToString()));

            fixture.Register(() => builder.GetRequiredService<ErabliereDbContext>().PopulatesDbSets(fixture));
            fixture.Register(() => builder.GetRequiredService<IMapper>());

            fixture.Freeze<ErabliereDbContext>();

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

            var builder = services.BuildServiceProvider();

            return builder;
        }
    }
}
