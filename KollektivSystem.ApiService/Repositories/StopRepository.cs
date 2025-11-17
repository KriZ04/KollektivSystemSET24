using KollektivSystem.ApiService.Infrastructure;
using KollektivSystem.ApiService.Models;

namespace KollektivSystem.ApiService.Repositories
{
    public class StopRepository : RepositoryBase<TransitLineStop, int>, IStopRepository
    {
        public StopRepository(ApplicationDbContext db) : base(db)
        {
        }
    }
}
