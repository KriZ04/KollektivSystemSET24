using KollektivSystem.ApiService.Infrastructure;
using KollektivSystem.ApiService.Models;
using KollektivSystem.ApiService.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace KollektivSystem.ApiService.Repositories
{
    public class UserRepository: RepositoryBase<User, int>, IUserRepository
    {
        public UserRepository(ApplicationDbContext db) : base(db)
        {
        }

        public async Task<User?> GetByProviderSubAsync(AuthProvider provider, string sub, CancellationToken ct = default) 
            => await Set.SingleOrDefaultAsync(u => u.Provider == provider && u.Sub == sub, ct);
    }
}
