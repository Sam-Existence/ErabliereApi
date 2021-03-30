using AutoFixture;
using ErabliereApi.Depot.Sql;
using ErabliereApi.Donnees;

namespace ErabliereApi.Test.Autofixture
{
    internal static class DbContextExtension
    {
        /// <summary>
        /// Permet d'ajouter des données des une instance de <see cref="ErabliereDbContext"/>. 
        /// Une fois les données ajouter, la même instance est retourner par la méthode.
        /// </summary>
        public static ErabliereDbContext PopulatesDbSets(this ErabliereDbContext context, IFixture fixture)
        {
            context.Erabliere.AddRange(fixture.CreateMany<Erabliere>());

            context.SaveChanges();

            return context;
        }
    }
}
