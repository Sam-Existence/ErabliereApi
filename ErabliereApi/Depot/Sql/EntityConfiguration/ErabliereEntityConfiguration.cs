using ErabliereApi.Donnees;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ErabliereApi.Depot.Sql.EntityConfiguration
{
    public class ErabliereEntityConfiguration : IEntityTypeConfiguration<Erabliere>
    {
        public void Configure(EntityTypeBuilder<Erabliere> erabliere)
        {
            erabliere.HasMany(c => c.Capteurs)
                     .WithOne(c => c.Erabliere)
                     .HasForeignKey(e => e.IdErabliere)
                     .OnDelete(DeleteBehavior.Cascade);

            erabliere.HasMany(c => c.Donnees)
                     .WithOne(c => c.Erabliere)
                     .HasForeignKey(e => e.IdErabliere)
                     .OnDelete(DeleteBehavior.Cascade);

            erabliere.HasMany(c => c.Barils)
                     .WithOne(c => c.Erabliere)
                     .HasForeignKey(e => e.IdErabliere)
                     .OnDelete(DeleteBehavior.Cascade);

            erabliere.HasMany(c => c.Dompeux)
                     .WithOne(c => c.Erabliere)
                     .HasForeignKey(e => e.IdErabliere)
                     .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
