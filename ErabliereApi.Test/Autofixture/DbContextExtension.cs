using AutoFixture;
using ErabliereApi.Depot.Sql;
using ErabliereApi.Donnees;

namespace ErabliereApi.Test.Autofixture
{
    public static class DbContextExtension
    {
        public static ErabliereDbContext PopulatesDbSets(this ErabliereDbContext context, IFixture fixture)
        {
            context.Erabliere.AddRange(fixture.CreateMany<Erabliere>());

            context.SaveChanges();

            return context;
        }
    }
}
