using KollektivSystem.ApiService.Models;
using KollektivSystem.ApiService.Models.Dtos.TicketTypes;

namespace KollektivSystem.ApiService.Services.Interfaces;

public interface ITicketTypeService
{
    Task<IReadOnlyList<TicketType>> GetAllActiveAsync(CancellationToken ct);
    Task<IReadOnlyList<TicketType>> GetAllAsync(CancellationToken ct); // staff
    Task<TicketType?> GetActiveByIdAsync(int id, CancellationToken ct);
    Task<TicketType?> GetByIdAsync(int id, CancellationToken ct);      // staff
    Task<TicketType> CreateAsync(CreateTicketTypeRequest request, CancellationToken ct);
    Task<bool> DeactivateAsync(int id, CancellationToken ct);
    Task<bool> ActivateAsync(int id, CancellationToken ct);
}
