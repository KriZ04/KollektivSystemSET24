using KollektivSystem.ApiService.Models;

namespace KollektivSystem.ApiService.Services.Interfaces
{
    public interface ITransitLineStopService
    {
        Task<TransitLineStop> CreateAsync(TransitLineStop lineStop);
        Task<IEnumerable<TransitLineStop>> GetAllAsync();
        Task<TransitLineStop?> GetByIdAsync(int id);
        Task<bool> UpdateAsync(int id, TransitLineStop updated);
        Task<bool> DeleteAsync(int id);
    }
}
