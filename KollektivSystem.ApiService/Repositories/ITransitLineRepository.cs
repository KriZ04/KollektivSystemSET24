using KollektivSystem.ApiService.Models.Transport;

namespace KollektivSystem.ApiService.Repositories
{
    public interface ITransitLineRepository : IRepository<TransitLine, int>
    {
        //new Task<int> AddAsync(TransitLine transitLine, CancellationToken ct = default);
        //Task<object?> UpdateAsync(int id, TransitLine line);
    }
}
