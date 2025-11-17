using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KollektivSystem.ApiService.Models.Configs;

public sealed class TicketTypeConfiguration : IEntityTypeConfiguration<TicketType>
{
    public void Configure(EntityTypeBuilder<TicketType> builder)
    {
        builder.HasKey(t => t.Id);

        builder.Property(t => t.Name)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(t => t.Price)
            .IsRequired();

        builder.Property(t => t.AliveTime)
            .IsRequired()
            .HasConversion(
                v => (int)v.TotalMinutes,
                v => TimeSpan.FromMinutes(v))
            .HasColumnType("int");        
    }
}
