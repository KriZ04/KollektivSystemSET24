using KollektivSystem.ApiService.Models;

namespace KollektivSystem.ApiService.Services.Interfaces
{
    public interface IPurchasedTicketService
    {
        Task<PurchasedTicket> PurchaseAsync(Guid userId, int ticketTypeId, CancellationToken ct);
        Task<IReadOnlyList<PurchasedTicket>> GetByUserAsync(Guid userId, bool includeExpired, CancellationToken ct);
        Task<PurchasedTicket?> GetForUserByIdAsync(Guid userId, Guid ticketId, CancellationToken ct);
        Task<(bool isValid, PurchasedTicket? ticket, string? reason)>ValidateAsync(string validationCode, CancellationToken ct);
        Task<bool> RevokeAsync(Guid ticketId, CancellationToken ct);
        Task<PurchasedTicket?> GetByIdAsync(Guid ticketId, CancellationToken ct);
        Task<IReadOnlyList<PurchasedTicket>> GetByUserIdAsync(Guid userId, CancellationToken ct);
    }
}
