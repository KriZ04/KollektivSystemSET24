using KollektivSystem.ApiService.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KollektivSystem.ApiService.Models.Configs;

public class TransitLineStopConfiguration : IEntityTypeConfiguration<TransitLineStop>
{
    public void Configure(EntityTypeBuilder<TransitLineStop> builder)
    {
        builder.ToTable("TransitLineStops");

        builder.HasKey(tls => tls.Id);

        builder.Property(tls => tls.Id)
               .ValueGeneratedOnAdd();

        builder.Property(tls => tls.Order)
               .IsRequired();

        // Relationship: TransitLineStop -> TransitLine (many-to-one)
        builder.HasOne(tls => tls.TransitLine)
               .WithMany(t => t.Stops)
               .HasForeignKey(tls => tls.TransitLineId)
               .OnDelete(DeleteBehavior.Cascade);

        // Relationship: TransitLineStop -> Stop (many-to-one)
        builder.HasOne(tls => tls.Stop)
               .WithMany(s => s.TransitLineStops)
               .HasForeignKey(tls => tls.StopId)
               .OnDelete(DeleteBehavior.Cascade); 

        builder.HasIndex(tls => new { tls.TransitLineId, tls.StopId })
               .IsUnique();

        builder.HasIndex(tls => new { tls.TransitLineId, tls.Order })
               .IsUnique();
    }
}
