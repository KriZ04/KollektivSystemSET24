using KollektivSystem.ApiService.Infrastructure;
using KollektivSystem.ApiService.Models;
using KollektivSystem.ApiService.Repositories.Interfaces;

namespace KollektivSystem.ApiService.Repositories;

public class PurchasedTicketRepository : RepositoryBase<PurchasedTicket, Guid>, IPurchasedTicketRepository
{
    public PurchasedTicketRepository(ApplicationDbContext db) : base(db)
    {
    }
}
