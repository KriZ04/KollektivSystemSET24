using KollektivSystem.ApiService.Models.Transport;

namespace KollektivSystem.ApiService.Repositories
{
    public interface ITranzitLineRepository : IRepository<Route, int>
    {
        Task<bool> DeleteAsync(int id);
    }
}
