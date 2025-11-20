using KollektivSystem.ApiService.Models;
using KollektivSystem.ApiService.Models.Dtos.TransitLineStops;

namespace KollektivSystem.ApiService.Services.Interfaces
{
    public interface ITransitLineStopService
    {
        Task<TransitLineStop> CreateAsync(CreateTransitLineStopRequest request, CancellationToken ct);
        Task<IReadOnlyList<TransitLineStop>> GetAllAsync(CancellationToken ct);
        Task<TransitLineStop?> GetByIdAsync(int id, CancellationToken ct);
        Task<bool> UpdateAsync(int id, TransitLineStop updated, CancellationToken ct);
        Task<bool> DeleteAsync(int id, CancellationToken ct);

    }
}
