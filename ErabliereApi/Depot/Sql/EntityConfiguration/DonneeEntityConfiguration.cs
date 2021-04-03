using ErabliereApi.Donnees;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ErabliereApi.Depot.Sql.EntityConfiguration
{
    public class DonneeEntityConfiguration : IEntityTypeConfiguration<Donnee>
    {
        public void Configure(EntityTypeBuilder<Donnee> builder)
        {
            
        }
    }
}
