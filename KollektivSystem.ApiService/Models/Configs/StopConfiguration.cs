using KollektivSystem.ApiService.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KollektivSystem.ApiService.Models.Configs;

public class StopConfiguration : IEntityTypeConfiguration<Stop>
{
    public void Configure(EntityTypeBuilder<Stop> builder)
    {
        builder.ToTable("Stops");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.Id)
               .ValueGeneratedOnAdd();


        builder.Property(s => s.Name)
               .IsRequired()
               .HasMaxLength(100);

        builder.Property(s => s.Latitude)
               .IsRequired();

        builder.Property(s => s.Longitude)
               .IsRequired();

        // Relationship: Stop 1 -> TransitLineStop
        builder.HasMany(s => s.TransitLineStops)
               .WithOne(tls => tls.Stop)
               .HasForeignKey(tls => tls.StopId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
