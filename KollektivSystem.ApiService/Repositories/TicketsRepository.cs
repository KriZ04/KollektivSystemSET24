using KollektivSystem.ApiService.Models;
using KollektivSystem.ApiService.Models.Transport;
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
