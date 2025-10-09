using KollektivSystem.ApiService.Models.Transport;

namespace KollektivSystem.ApiService.Repositories
{
    public class TranistLineRepository : RepositoryBase<Route, int>, ITranzitLineRepository
    {
        public TranistLineRepository(ApplicationDbContext db) : base(db)
        {
        }
    }
}

