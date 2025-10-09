using KollektivSystem.ApiService.Models.Transport;

namespace KollektivSystem.ApiService.Repositories
{
    public interface IRouteRepository : IRepository<Route, int>
    {
        Task<bool> DeleteAsync(int id);
    }
}
