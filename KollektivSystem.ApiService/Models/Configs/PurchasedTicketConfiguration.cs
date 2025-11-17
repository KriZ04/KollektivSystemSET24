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

        builder.HasOne(t => t.TicketType)
            .WithMany()
            .HasForeignKey(t => t.TicketTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(t => t.User)
            .WithMany(u => u.PurchasedTickets)
            .HasForeignKey(t => t.UserId);
    }
}
