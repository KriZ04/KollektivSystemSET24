using KollektivSystem.ApiService.Infrastructure;
using KollektivSystem.ApiService.Models;

namespace KollektivSystem.ApiService.Repositories
{
    public class StopRepository : RepositoryBase<Stop, int>, IStopRepository
    {
        public StopRepository(ApplicationDbContext db) : base(db)
        {
        }
    }
}
