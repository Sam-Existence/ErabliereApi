using ErabliereApi.Donnees;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ErabliereApi.Depot.Sql.EntityConfiguration
{
    /// <summary>
    /// Configuration de l'entité Alerte
    /// </summary>
    public class AlerteEntityConfiguration : IEntityTypeConfiguration<Alerte>
    {
        /// <inheritdoc />
        public void Configure(EntityTypeBuilder<Alerte> builder)
        {
            
        }
    }
}
