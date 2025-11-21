using KollektivSystem.ApiService.Infrastructure;
using KollektivSystem.ApiService.Models;
using KollektivSystem.ApiService.Repositories.Interfaces;

namespace KollektivSystem.ApiService.Repositories;

public class TicketTypeRepository : RepositoryBase<TicketType, int>, ITicketTypeRepository
{
    public TicketTypeRepository(ApplicationDbContext db) : base(db)
    {
    }
}
