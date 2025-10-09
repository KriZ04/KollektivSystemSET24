using Microsoft.EntityFrameworkCore;
using KollektivSystem.ApiService.Models.Transport;

namespace KollektivSystem.ApiService.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public virtual DbSet<User> Users => Set<User>();
        public virtual DbSet<Route> Routes => Set<Route>();
        public virtual DbSet<Stop> Stops => Set<Stop>();
        public virtual DbSet<Tickets> Tickets => Set<Tickets>();


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Optional: apply IEntityTypeConfiguration<T> classes automatically
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }

    }
}
