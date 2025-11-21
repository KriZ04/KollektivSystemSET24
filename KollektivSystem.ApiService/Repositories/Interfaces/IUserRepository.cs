using KollektivSystem.ApiService.Models;
using KollektivSystem.ApiService.Models.Enums;

namespace KollektivSystem.ApiService.Repositories.Interfaces;

public interface IUserRepository : IRepository<User, Guid>
{
    Task<User?> GetByProviderSubAsync(AuthProvider provider, string sub, CancellationToken ct = default);
}
