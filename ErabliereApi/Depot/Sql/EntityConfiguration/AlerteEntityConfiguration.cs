using ErabliereApi.Donnees;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ErabliereApi.Depot.Sql.EntityConfiguration
{
    public class AlerteEntityConfiguration : IEntityTypeConfiguration<Alerte>
    {
        public void Configure(EntityTypeBuilder<Alerte> builder)
        {
            
        }
    }
}
