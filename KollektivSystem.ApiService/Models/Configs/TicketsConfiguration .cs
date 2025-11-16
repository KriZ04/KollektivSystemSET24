using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KollektivSystem.ApiService.Models.Configs
{
    public class TicketsConfiguration : IEntityTypeConfiguration<Tickets>
    {
        public void Configure(EntityTypeBuilder<Tickets> Builder)
        {
            Builder.Property(t => t.Price)
                .HasPrecision(18, 2); // Decimal Precision (Preventing EF warning)

            Builder.Property(t => t.Type)
                .HasMaxLength(50)
                .IsRequired();

        }
    }
}
