using KollektivSystem.ApiService.Infrastructure;
using KollektivSystem.ApiService.Models;
using KollektivSystem.ApiService.Repositories.Interfaces;

namespace KollektivSystem.ApiService.Repositories
{
    public class TransitLineStopRepository : RepositoryBase<TransitLineStop, int>, ITransitLineStopRepository
    {
        public TransitLineStopRepository(ApplicationDbContext db) : base(db)
        {
        }
    }
}
