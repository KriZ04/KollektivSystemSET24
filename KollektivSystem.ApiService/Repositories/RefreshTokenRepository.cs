using KollektivSystem.ApiService.Infrastructure;
using KollektivSystem.ApiService.Models;
using KollektivSystem.ApiService.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace KollektivSystem.ApiService.Repositories;

public class RefreshTokenRepository : RepositoryBase<RefreshToken, Guid>, IRefreshTokenRepository
{
    private readonly ApplicationDbContext _db;

    public RefreshTokenRepository(ApplicationDbContext db) : base(db)
    {
        _db = db;
    }

    public async Task<RefreshToken?> GetByHashAsync(string tokenHash, CancellationToken ct = default)
    {
        return await _db.RefreshTokens.SingleOrDefaultAsync(x => x.TokenHash == tokenHash, ct);
    }
}
