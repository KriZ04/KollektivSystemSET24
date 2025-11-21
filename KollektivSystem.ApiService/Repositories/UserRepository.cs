using KollektivSystem.ApiService.Infrastructure;
using KollektivSystem.ApiService.Models;
using KollektivSystem.ApiService.Models.Enums;
using KollektivSystem.ApiService.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace KollektivSystem.ApiService.Repositories;

public class UserRepository: RepositoryBase<User, Guid>, IUserRepository
{
    public UserRepository(ApplicationDbContext db) : base(db)
    {
    }

    public async Task<User?> GetByProviderSubAsync(AuthProvider provider, string sub, CancellationToken ct = default) 
        => await Set.SingleOrDefaultAsync(u => u.Provider == provider && u.Sub == sub, ct);
}
