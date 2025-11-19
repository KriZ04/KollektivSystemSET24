using KollektivSystem.ApiService.Infrastructure;
using KollektivSystem.ApiService.Models;
using KollektivSystem.ApiService.Repositories.Interfaces;

namespace KollektivSystem.ApiService.Repositories
{
    public class StopRepository : RepositoryBase<Stop, int>, IStopRepository
    {
        public StopRepository(ApplicationDbContext db) : base(db)
        {
        }
    }
}
