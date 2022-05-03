using ErabliereApi.Donnees;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ErabliereApi.Depot.Sql.EntityConfiguration;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.HasIndex(e => e.Email)
               .IsUnique();

        builder.HasMany(u => u.ApiKeys)
               .WithOne(a => a.Customer)
               .HasForeignKey(a => a.CustomerId);
    }
}
