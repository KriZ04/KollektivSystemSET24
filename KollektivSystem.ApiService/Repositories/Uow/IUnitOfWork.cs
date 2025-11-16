namespace KollektivSystem.ApiService.Repositories.Uow
{
    public interface IUnitOfWork
    {
        IUserRepository Users { get; }
        IRefreshTokenRepository RefreshTokens { get; }
        IStopRepository Stops { get; }
        ITicketsRepository Tickets { get; }
        ITransitLineRepository TransitLines { get; }



        /// <summary>
        /// Saves all pending changes to the database.
        /// </summary>
        Task<int> SaveChangesAsync(CancellationToken ct = default);
    }
}
