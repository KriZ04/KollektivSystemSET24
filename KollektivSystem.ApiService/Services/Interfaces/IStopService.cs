using KollektivSystem.ApiService.Models;

namespace KollektivSystem.ApiService.Services.Interfaces
{
    public interface IStopService
    {
        Task<Stop> CreateAsync(Stop stop, CancellationToken ct = default);
        Task<List<Stop>> GetAllAsync(CancellationToken ct = default);
        Task<Stop?> GetByIdAsync(int id, CancellationToken ct = default);
        Task<bool> UpdateAsync(int id, Stop updated, CancellationToken ct = default);
        Task<bool> DeleteAsync(int id, CancellationToken ct = default);
    }
}
