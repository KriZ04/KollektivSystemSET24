using KollektivSystem.ApiService.Models;
using KollektivSystem.ApiService.Models.Enums;

namespace KollektivSystem.ApiService.Repositories
{
    public interface IUserRepository : IRepository<User, int>
    {
        Task<User?> GetByProviderSubAsync(AuthProvider provider, string sub, CancellationToken ct = default);
    }
}
