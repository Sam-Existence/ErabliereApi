using ErabliereApi.Donnees;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ErabliereApi.Depot.Sql.EntityConfiguration
{
    /// <summary>
    /// Configuration de l'entité Position Graph
    /// </summary>
    public class PositionGraphEntityonfiguration : IEntityTypeConfiguration<PositionGraph>
    {
        /// <inheritdoc />
        public void Configure(EntityTypeBuilder<PositionGraph> builder)
        {
            builder.HasIndex(donnee => donnee.D, "D_Index");
        }
    }
}
