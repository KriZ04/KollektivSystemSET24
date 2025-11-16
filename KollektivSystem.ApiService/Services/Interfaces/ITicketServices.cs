using KollektivSystem.ApiService.Models;

namespace KollektivSystem.ApiService.Services.Interfaces
{
    public interface ITicketService
    {
        Task<Tickets> CreateAsync(Tickets ticket);
        Task<Tickets?> GetByIdAsync(int id);
        Task<IEnumerable<Tickets>> GetAllAsync();
        Task<bool> DeleteAsync(int id);
    }
}
