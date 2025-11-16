using KollektivSystem.Web.Models;

namespace KollektivSystem.Web.Services;

public interface ITicketApiClient
{
    Task<IReadOnlyList<TicketDto>> GetTicketsAsync(CancellationToken ct = default);
    Task<PurchasedTicketDto?> PurchaseTicketAsync(int ticketId, CancellationToken ct = default);
}
