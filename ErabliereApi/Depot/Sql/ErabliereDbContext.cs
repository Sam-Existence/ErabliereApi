using ErabliereApi.Donnees;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace ErabliereApi.Depot.Sql
{
    public class ErabliereDbContext : DbContext
    {
        public ErabliereDbContext([NotNull] DbContextOptions options) : base(options)
        {
        }

        public DbSet<Alerte> Alertes { get; private set; }
        public DbSet<Baril> Barils { get; private set; }
        public DbSet<Dompeux> Dompeux { get; private set; }
        public DbSet<Donnee> Donnees { get; private set; }
        public DbSet<Erabliere> Erabliere { get; private set; }
    }
}
