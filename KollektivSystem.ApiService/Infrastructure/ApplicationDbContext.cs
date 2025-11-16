using KollektivSystem.ApiService.Models;
using Microsoft.EntityFrameworkCore;

namespace KollektivSystem.ApiService.Infrastructure
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public virtual DbSet<User> Users => Set<User>();
        public virtual DbSet<TransitLine> TransitLine => Set<TransitLine>();
        public virtual DbSet<Stop> Stops => Set<Stop>();
        public virtual DbSet<TicketInfo> TicketInfos => Set<TicketInfo>();
        public virtual DbSet<PurchasedTicket> PurchasedTickets => Set<PurchasedTicket>();


        public DbSet<RefreshToken> RefreshTokens { get; set; } = default!;


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Optional: apply IEntityTypeConfiguration<T> classes automatically
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }

    }
}
