namespace KollektivSystem.ApiService.Repositories.Uow
{
    public interface IUnitOfWork
    {
        /// <summary>
        /// Saves all pending changes to the database.
        /// </summary>
        Task<int> SaveChangesAsync(CancellationToken ct = default);
    }
}
