using KollektivSystem.ApiService.Models.Transport;

namespace KollektivSystem.ApiService.Services.Interfaces
{
    public interface IStopService
    {
        Task<Stop> CreateAsync(Stop stop);
        Task<IEnumerable<Stop>> GetAllAsync();
        Task<Stop?> GetByIdAsync(int id);
        Task<bool> UpdateAsync(int id, Stop updated);
        Task<bool> DeleteAsync(int id);
    }
}
