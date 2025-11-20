using KollektivSystem.ApiService.Models;
using System.Linq.Expressions;

namespace KollektivSystem.ApiService.Repositories.Interfaces
{
    public interface ITransitLineStopRepository : IRepository<TransitLineStop, int>
    {
        Task<bool> AnyAsync(Expression<Func<TransitLineStop, bool>> predicate, CancellationToken ct);
    }
}
