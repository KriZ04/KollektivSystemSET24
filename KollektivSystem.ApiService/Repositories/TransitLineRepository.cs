using KollektivSystem.ApiService.Models.Transport;

namespace KollektivSystem.ApiService.Repositories
{
    public class TransitLineRepository : RepositoryBase<Route, int>, ITransitLineRepository
    {
        public TransitLineRepository(ApplicationDbContext db) : base(db)
        {
        }
    }
}

