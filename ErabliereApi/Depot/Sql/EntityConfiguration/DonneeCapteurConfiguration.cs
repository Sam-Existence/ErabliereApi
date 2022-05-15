using ErabliereApi.Donnees;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ErabliereApi.Depot.Sql.EntityConfiguration;

public class DonneeCapteurConfiguration : IEntityTypeConfiguration<DonneeCapteur>
{
    public void Configure(EntityTypeBuilder<DonneeCapteur> builder)
    {
        builder.HasIndex(donnee => donnee.D, "D_Index");
    }
}
