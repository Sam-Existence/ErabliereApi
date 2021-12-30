using ErabliereApi.Donnees;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ErabliereApi.Depot.Sql.EntityConfiguration
{
    /// <summary>
    /// Configuration de la table <see cref="ErabliereDbContext.Erabliere" />
    /// </summary>
    public class ErabliereEntityConfiguration : IEntityTypeConfiguration<Erabliere>
    {
        /// <inheritdoc />
        public void Configure(EntityTypeBuilder<Erabliere> erabliere)
        {
            erabliere.HasMany(c => c.Capteurs)
                     .WithOne(c => c.Erabliere)
                     .HasForeignKey(e => e.IdErabliere)
                     .OnDelete(DeleteBehavior.Cascade);

            erabliere.HasMany(c => c.Donnees)
                     .WithOne(c => c.Erabliere)
                     .HasForeignKey(e => e.IdErabliere)
                     .OnDelete(DeleteBehavior.Cascade);

            erabliere.HasMany(c => c.Barils)
                     .WithOne(c => c.Erabliere)
                     .HasForeignKey(e => e.IdErabliere)
                     .OnDelete(DeleteBehavior.Cascade);

            erabliere.HasMany(c => c.Dompeux)
                     .WithOne(c => c.Erabliere)
                     .HasForeignKey(e => e.IdErabliere)
                     .OnDelete(DeleteBehavior.Cascade);

            erabliere.HasMany(c => c.Notes)
                     .WithOne(c => c.Erabliere)
                     .HasForeignKey(e => e.IdErabliere)
                     .OnDelete(DeleteBehavior.Cascade);

            erabliere.HasMany(c => c.Documentations)
                     .WithOne(c => c.Erabliere)
                     .HasForeignKey(e => e.IdErabliere)
                     .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
