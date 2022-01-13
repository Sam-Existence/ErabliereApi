using ErabliereApi.Donnees;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace ErabliereApi.Depot.Sql
{
    /// <summary>
    /// Classe DbContext pour interagir avec la base de donnée en utilisant EntityFramework
    /// </summary>
    public class ErabliereDbContext : DbContext
    {
        /// <summary>
        /// Constructeur par initialisation
        /// </summary>
        /// <param name="options"></param>
#pragma warning disable CS8618 // Un champ non-nullable doit contenir une valeur non-null lors de la fermeture du constructeur. Le constructeur de base s'occupe d'initialiser les propriétés.
        public ErabliereDbContext([NotNull] DbContextOptions options) : base(options)
#pragma warning restore CS8618 // Un champ non-nullable doit contenir une valeur non-null lors de la fermeture du constructeur. Le constructeur de base s'occupe d'initialiser les propriétés.
        {

        }

        /// <summary>
        /// Table des alertes
        /// </summary>
        public DbSet<Alerte> Alertes { get; private set; }

        /// <summary>
        /// Table des alertes des capteurs
        /// </summary>
        public DbSet<AlerteCapteur> AlerteCapteurs { get; private set; }

        /// <summary>
        /// Table des barils
        /// </summary>
        public DbSet<Baril> Barils { get; private set; }

        /// <summary>
        /// Table des dompeux
        /// </summary>
        public DbSet<Dompeux> Dompeux { get; private set; }

        /// <summary>
        /// Table des données
        /// </summary>
        public DbSet<Donnee> Donnees { get; private set; }

        /// <summary>
        /// Table des érablières
        /// </summary>
        public DbSet<Erabliere> Erabliere { get; private set; }

        /// <summary>
        /// Table des capteurs
        /// </summary>
        public DbSet<Capteur> Capteurs { get; private set; }

        /// <summary>
        /// Table des données des capteurs
        /// </summary>
        public DbSet<DonneeCapteur> DonneesCapteur { get; private set; }

        /// <summary>
        /// Table des notes
        /// </summary>
        public DbSet<Note> Notes { get; private set; }

        /// <summary>
        /// Table de la docuemntation
        /// </summary>
        public DbSet<Documentation> Documentation { get; private set; }

        /// <inheritdoc />
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ErabliereDbContext).Assembly);
        }
    }
}
