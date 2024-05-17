using ErabliereApi.Donnees;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ErabliereApi.Depot.Sql.EntityConfiguration;

/// <summary>
/// Configuration de l'entité Customer
/// </summary>
public class CustomerErabliereConfiguration : IEntityTypeConfiguration<CustomerErabliere>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<CustomerErabliere> builder)
    {
        builder.HasIndex(c => c.IdCustomer);

        builder.HasIndex(c => c.IdErabliere);

        builder.HasKey(c => new { c.IdCustomer, c.IdErabliere });
    }
}
