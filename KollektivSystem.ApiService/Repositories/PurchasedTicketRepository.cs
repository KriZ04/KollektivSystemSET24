using KollektivSystem.ApiService.Infrastructure;
using KollektivSystem.ApiService.Models;
using KollektivSystem.ApiService.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace KollektivSystem.ApiService.Repositories;

public class PurchasedTicketRepository : RepositoryBase<PurchasedTicket, Guid>, IPurchasedTicketRepository
{
    public PurchasedTicketRepository(ApplicationDbContext db) : base(db)
    {
    }
    public Task<bool> ValidationCodeExistsAsync(string code, CancellationToken ct = default)
        => Set.AnyAsync(t => t.ValidationCode == code, ct);

    public Task<PurchasedTicket?> GetByValidationCodeAsync(string code, CancellationToken ct = default)
        => Set.Include(t => t.TicketType).SingleOrDefaultAsync(t => t.ValidationCode == code, ct);

}
