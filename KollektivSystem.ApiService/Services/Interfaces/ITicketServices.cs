using KollektivSystem.ApiService.Models.Transport;

namespace KollektivSystem.ApiService.Services.Interfaces
{
    public interface ITicketService
    {
        Task<Tickets> CreateAsync(Tickets ticket);
        Task<Tickets?> GetByIdAsync(int id);
        Task<IEnumerable<Tickets>> GetAllAsync();
        Task<bool> DeleteAsync(int id);
        Task<bool> UpdateAsync(int id, Tickets updated);
    }
}
