using ErabliereApi.Donnees;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ErabliereApi.Depot.Sql.EntityConfiguration;

/// <summary>
/// Configuration de l'entité Customer
/// </summary>
public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.HasIndex(e => e.Email)
               .IsUnique();

        builder.HasIndex(e => e.UniqueName)
               .IsUnique();

        builder.HasMany(u => u.ApiKeys)
               .WithOne(a => a.Customer)
               .HasForeignKey(a => a.CustomerId);

        builder.HasMany(u => u.CustomerErablieres)
               .WithOne(a => a.Customer)
               .HasForeignKey(a => a.IdCustomer);
    }
}
