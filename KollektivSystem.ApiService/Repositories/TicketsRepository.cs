using KollektivSystem.ApiService.Infrastructure;
using KollektivSystem.ApiService.Models;
using KollektivSystem.ApiService.Repositories;

namespace KollektivSystem.ApiService.Repositories
{
    public class TicketsRepository : RepositoryBase<Tickets, int>, ITicketsRepository
    {
        public TicketsRepository(ApplicationDbContext db) : base(db)
        {
        }
    }
}
