using KollektivSystem.ApiService.Infrastructure;

namespace KollektivSystem.ApiService.Repositories.Uow
{
    public sealed class ApplicationUnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;

        public ApplicationUnitOfWork(ApplicationDbContext db, 
            IUserRepository userRepository, 
            IRefreshTokenRepository refreshTokenRepository, 
            IStopRepository stopRepository,
            ITicketsRepository ticketsRepository,
            ITransitLineRepository transitLineRepository)
        {
            _db = db;
            Users = userRepository;
            RefreshTokens = refreshTokenRepository;
            Stops = stopRepository;
            Tickets = ticketsRepository;
            TransitLines = transitLineRepository;
        }

        public IUserRepository Users { get; }

        public IRefreshTokenRepository RefreshTokens { get; }

        public IStopRepository Stops { get; }

        public ITicketsRepository Tickets { get; }

        public ITransitLineRepository TransitLines { get; }

        public Task<int> SaveChangesAsync(CancellationToken ct = default)
            => _db.SaveChangesAsync(ct);
    }
}
