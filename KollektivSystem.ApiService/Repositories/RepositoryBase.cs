

using KollektivSystem.ApiService.Models;
using Microsoft.EntityFrameworkCore;

namespace KollektivSystem.ApiService.Repositories
{
    public abstract class RepositoryBase<T, TKey> : IRepository<T, TKey> where T : class
    {
        protected readonly ApplicationDbContext Db;
        protected readonly DbSet<T> Set;

        protected RepositoryBase(ApplicationDbContext db)
        {
            Db = db;
            Set = db.Set<T>();
        }

        public IQueryable<T> Query()
            => Set.AsQueryable();

        public Task<T?> FindAsync(TKey id, CancellationToken ct = default)
            => Set.FindAsync(new object?[] { id }, ct).AsTask();

        public async Task<bool> ExistsAsync(TKey id, CancellationToken ct = default)
            => await Set.FindAsync(new object?[] { id }, ct) is not null;

        public Task AddAsync(T entity, CancellationToken ct = default)
            => Set.AddAsync(entity, ct).AsTask();

        public Task AddRangeAsync(IEnumerable<T> entities, CancellationToken ct = default)
            => Set.AddRangeAsync(entities, ct);

        public void Update(T entity)
            => Set.Update(entity);

        public void Remove(T entity)
            => Set.Remove(entity);

        public void RemoveRange(IEnumerable<T> entities)
            => Set.RemoveRange(entities);

        public Task<List<T>> GetAllAsync(CancellationToken ct = default)
            => Set.ToListAsync(ct);

    }
}
