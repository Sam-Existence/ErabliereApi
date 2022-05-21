using ErabliereApi.Donnees;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ErabliereApi.Depot.Sql.EntityConfiguration;

/// <summary>
/// Configuration de l'entité DonneeCapteur
/// </summary>
public class DonneeCapteurConfiguration : IEntityTypeConfiguration<DonneeCapteur>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<DonneeCapteur> builder)
    {
        builder.HasIndex(donnee => donnee.D, "D_Index");
    }
}
