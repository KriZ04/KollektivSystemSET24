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
    public async Task<IReadOnlyList<PurchasedTicket>> GetByUserIdAsync(Guid userId, bool includeInvalid, CancellationToken ct = default)
    {
        var query = Set
            .Include(t => t.TicketType)
            .Where(t => t.UserId == userId);

        if (!includeInvalid)
        {
            var now = DateTimeOffset.UtcNow;
            query = query.Where(t => !t.Revoked && t.ExpireAt > now);
        }


        return await query.ToListAsync(ct);
    }
}
