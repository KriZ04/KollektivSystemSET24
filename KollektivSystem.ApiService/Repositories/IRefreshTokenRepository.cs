using KollektivSystem.ApiService.Models;

namespace KollektivSystem.ApiService.Repositories
{
    public interface IRefreshTokenRepository : IRepository<RefreshToken, Guid>
    {
        Task<RefreshToken?> GetByHashAsync(string tokenHash, CancellationToken ct = default);
    }
}