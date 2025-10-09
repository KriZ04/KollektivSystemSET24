using KollektivSystem.ApiService.Infrastructure;
using KollektivSystem.ApiService.Models;

namespace KollektivSystem.ApiService.Repositories
{
    public class UserRepository: RepositoryBase<User, int>, IUserRepository
    {
        public UserRepository(ApplicationDbContext db) : base(db)
        {
        }
    }
}
