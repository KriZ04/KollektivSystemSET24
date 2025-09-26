using Microsoft.EntityFrameworkCore;

namespace KollektivSystem.ApiService.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
        { 
        
        }

        public DbSet<User> Users { get; set; }

        public string DbPath { get; }

         

    }
}
