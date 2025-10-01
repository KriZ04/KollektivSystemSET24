using KollektivSystem.ApiService.Models;

namespace KollektivSystem.ApiService.Repositories.Uow
{
    public class ApplicationUnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;

        public ApplicationUnitOfWork(ApplicationDbContext db)
        {
            _db = db;
        }

        public Task<int> SaveChangesAsync(CancellationToken ct = default)
            => _db.SaveChangesAsync(ct);
    }
}
