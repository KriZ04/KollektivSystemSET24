using KollektivSystem.ApiService.Infrastructure;
using KollektivSystem.ApiService.Models;
using KollektivSystem.ApiService.Repositories.Interfaces;

namespace KollektivSystem.ApiService.Repositories;

public class TransitLineRepository : RepositoryBase<TransitLine, int>, ITransitLineRepository
{
    public TransitLineRepository(ApplicationDbContext db) : base(db)
    {
    }
}
