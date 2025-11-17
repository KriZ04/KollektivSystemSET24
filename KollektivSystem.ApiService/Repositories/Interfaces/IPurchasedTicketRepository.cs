using KollektivSystem.ApiService.Models;

namespace KollektivSystem.ApiService.Repositories.Interfaces
{
    public interface IPurchasedTicketRepository : IRepository<PurchasedTicket, Guid>
    {
        Task<bool> ValidationCodeExistsAsync(string code, CancellationToken ct = default);
        Task<PurchasedTicket?> GetByValidationCodeAsync(string code, CancellationToken ct = default);
        Task<IReadOnlyList<PurchasedTicket>> GetByUserIdAsync(Guid userId, bool includeInvalid, CancellationToken ct = default);
    }
}
