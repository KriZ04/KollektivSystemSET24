using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KollektivSystem.ApiService.Models.Configs
{
    public class UserConfiguration: IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(u => u.DisplayName)
               .HasMaxLength(100)
               .IsRequired();

            builder.HasIndex(u => u.Role);
        }
    }
}
