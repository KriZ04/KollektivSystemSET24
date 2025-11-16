using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KollektivSystem.ApiService.Models.Configs;

public sealed class PurchasedTicketConfiguration : IEntityTypeConfiguration<PurchasedTicket>
{
    public void Configure(EntityTypeBuilder<PurchasedTicket> builder)
    {
        builder.HasKey(t => t.Id);

        builder.Property(t => t.ValidationCode)
            .IsRequired()
            .HasMaxLength(8);

        builder.HasIndex(t => t.ValidationCode)
            .IsUnique();

        builder.Property(t => t.PurchasedAt)
            .HasDefaultValueSql("SYSUTCDATETIME()");

        builder.HasOne(t => t.TicketInfo)
            .WithMany()
            .HasForeignKey(t => t.TicketInfoId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(t => t.User)
            .WithMany()
            .HasForeignKey(t => t.UserId);
    }
}
