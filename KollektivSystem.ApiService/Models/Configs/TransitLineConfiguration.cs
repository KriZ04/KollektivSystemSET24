using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KollektivSystem.ApiService.Models.Configs;

public class TransitLineConfiguration : IEntityTypeConfiguration<TransitLine>
{
    public void Configure(EntityTypeBuilder<TransitLine> builder)
    {
   

        builder.HasKey(t => t.Id);

        builder.Property(t => t.Id)
               .ValueGeneratedOnAdd(); 

        builder.Property(t => t.Name)
               .IsRequired()
               .HasMaxLength(100);

        builder.HasMany(t => t.Stops)
               .WithOne(tls => tls.TransitLine)
               .HasForeignKey(tls => tls.TransitLineId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
