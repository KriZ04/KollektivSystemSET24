using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KollektivSystem.ApiService.Models.Configs;

public sealed class TicketInfoConfiguration : IEntityTypeConfiguration<TicketInfo>
{
    public void Configure(EntityTypeBuilder<TicketInfo> builder)
    {
        builder.HasKey(t => t.Id);

        builder.Property(t => t.Name)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(t => t.Price)
            .IsRequired();

        builder.Property(t => t.AliveTime)
            .IsRequired();
    }
}
