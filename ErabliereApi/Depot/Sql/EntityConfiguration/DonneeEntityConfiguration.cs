using ErabliereApi.Donnees;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ErabliereApi.Depot.Sql.EntityConfiguration;

/// <summary>
/// Configuration de l'entité Donnee
/// </summary>
public class DonneeEntityConfiguration : IEntityTypeConfiguration<Donnee>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<Donnee> builder)
    {
        builder.HasIndex(donnee => donnee.D, "D_index");
    }
}
