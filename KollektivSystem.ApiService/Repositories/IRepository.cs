namespace KollektivSystem.ApiService.Repositories
{
    public interface IRepository<T, TKey> where T : class
    {
        IQueryable<T> Query();
        Task<T?> FindAsync(TKey id,  CancellationToken ct = default);
        Task<bool> ExistsAsync(TKey id, CancellationToken ct = default);

        Task AddAsync(T entity, CancellationToken ct = default);
        Task AddRangeAsync(IEnumerable<T> entities, CancellationToken ct = default);
        void Update(T entity);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);
        Task<List<T>> GetAllAsync(CancellationToken ct = default);

        Task<bool> RemoveByIdAsync(TKey id, CancellationToken ct = default);
    }
}