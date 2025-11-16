using KollektivSystem.ApiService.Infrastructure;
using KollektivSystem.ApiService.Models;

namespace KollektivSystem.ApiService.Repositories
{
    public class TransitLineRepository : RepositoryBase<TransitLine, int>, ITransitLineRepository
    {
        public TransitLineRepository(ApplicationDbContext db) : base(db)
        {
        }
    }
}
