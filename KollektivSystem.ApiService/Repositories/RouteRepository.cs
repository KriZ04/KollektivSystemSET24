using KollektivSystem.ApiService.Models.Transport;

namespace KollektivSystem.ApiService.Repositories
{
    public class RouteRepository : RepositoryBase<Route, int>, IRouteRepository
    {
        public RouteRepository(ApplicationDbContext db) : base(db)
        {
        }
    }
}

