using KollektivSystem.ApiService.Models;
using KollektivSystem.ApiService.Models.Dtos.Stops;

namespace KollektivSystem.ApiService.Services.Interfaces
{
    public interface IStopService
    {
        Task<Stop> CreateAsync(CreateStopRequest request, CancellationToken ct);
        Task<IReadOnlyList<Stop>> GetAllAsync(CancellationToken ct);
        Task<Stop?> GetByIdAsync(int id, CancellationToken ct);
        Task<bool> UpdateAsync(int id, Stop updated, CancellationToken ct);
        Task<bool> DeleteAsync(int id, CancellationToken ct);
    }
}
