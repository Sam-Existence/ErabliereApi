using ErabliereApi.Donnees;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ErabliereApi.Depot.Sql.EntityConfiguration;

/// <summary>
/// Classe de configuration entity framework des capteurs
/// </summary>
public class CapteurEntityConfiguration : IEntityTypeConfiguration<Capteur>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<Capteur> capteur)
    {
        capteur.HasMany(c => c.DonneesCapteur)
               .WithOne(dc => dc.Capteur)
               .HasForeignKey(dc => dc.IdCapteur)
               .OnDelete(DeleteBehavior.Cascade);

        capteur.HasMany(c => c.AlertesCapteur)
               .WithOne(dc => dc.Capteur)
               .HasForeignKey(dc => dc.IdCapteur)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
