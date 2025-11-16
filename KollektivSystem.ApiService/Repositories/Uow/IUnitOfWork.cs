using KollektivSystem.ApiService.Repositories.Interfaces;

namespace KollektivSystem.ApiService.Repositories.Uow
{
    public interface IUnitOfWork
    {
        IUserRepository Users { get; }
        IRefreshTokenRepository RefreshTokens { get; }
        IStopRepository Stops { get; }
        ITicketTypeRepository TicketTypes { get; }
        ITransitLineRepository TransitLines { get; }
        IPurchasedTicketRepository PurchasedTickets { get; }



        /// <summary>
        /// Saves all pending changes to the database.
        /// </summary>
        Task<int> SaveChangesAsync(CancellationToken ct = default);
    }
}
