using ErabliereApi.Donnees;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ErabliereApi.Depot.Sql.EntityConfiguration
{
    public class CapteurEntityConfiguration : IEntityTypeConfiguration<Capteur>
    {
        public void Configure(EntityTypeBuilder<Capteur> capteur)
        {
            capteur.HasMany(c => c.DonneesCapteur)
                   .WithOne(dc => dc.Capteur)
                   .HasForeignKey(dc => dc.IdCapteur);
        }
    }
}
