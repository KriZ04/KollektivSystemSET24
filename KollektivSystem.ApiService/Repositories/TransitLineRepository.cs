using KollektivSystem.ApiService.Models;
using KollektivSystem.ApiService.Models.Transport;

namespace KollektivSystem.ApiService.Repositories
{
    public class TransitLineRepository : RepositoryBase<TransitLine, int>, ITransitLineRepository
    {
        public TransitLineRepository(ApplicationDbContext db) : base(db)
        {
        }
        
        public Task SaveChanges()
        {
            throw new NotImplementedException();
        }
        

        public async Task SaveChangesAsync(CancellationToken ct = default)
        {
            await Db.SaveChangesAsync(ct);
        }


    }
}
