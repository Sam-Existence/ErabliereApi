using ErabliereApi.Donnees;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ErabliereApi.Depot.Sql.EntityConfiguration;

/// <summary>
/// Configuration de l'entité <see cref="AlerteCapteur"/>
/// </summary>
public class AlerteCapteurEntityConfiguration : IEntityTypeConfiguration<AlerteCapteur>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<AlerteCapteur> builder)
    {
        builder.Ignore(e => e.OwnerId);
        builder.Ignore(e => e.Owner);
    }
}
