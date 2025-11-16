using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KollektivSystem.ApiService.Models.Configs
{
    public class TransitLineConfiguration
    {
        public void Configure(EntityTypeBuilder<TransitLine> builder)
        {
            builder.Property(t => t.Name)
                .HasMaxLength(100)
                .IsRequired();

            // Since TransitLine has many Stops
            builder.HasMany(t => t.Stops)
                .WithOne(s => s.Route)
                .HasForeignKey(s => s.RouteId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

}
