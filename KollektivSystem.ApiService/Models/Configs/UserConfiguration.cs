using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KollektivSystem.ApiService.Models.Configs
{
    public class UserConfiguration: IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.DisplayName).HasMaxLength(200).IsRequired();
            builder.Property(x => x.Sub).HasMaxLength(200).IsRequired();
            builder.HasIndex(x => new { x.Provider, x.Sub }).IsUnique();

            builder.Property(x => x.Role)
                .HasConversion<string>()                   
                .HasMaxLength(32);
            builder.HasIndex(x => x.Role);

            builder.Property(x => x.Email).HasMaxLength(320);
            builder.HasIndex(x => x.Email);
        }
    }
}
