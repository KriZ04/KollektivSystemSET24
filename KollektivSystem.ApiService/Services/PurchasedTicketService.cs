using KollektivSystem.ApiService.Models;
using KollektivSystem.ApiService.Repositories.Uow;
using KollektivSystem.ApiService.Services.Interfaces;

namespace KollektivSystem.ApiService.Services
{
    public class PurchasedTicketService : IPurchasedTicketService
    {
        private readonly IUnitOfWork _uow;
        private readonly ILogger<PurchasedTicketService> _logger;

        public PurchasedTicketService(IUnitOfWork uow, ILogger<PurchasedTicketService> logger)
        {
            _uow = uow;
            _logger = logger;
        }

        public Task<PurchasedTicket?> GetByIdAsync(Guid ticketId, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<PurchasedTicket>> GetByUserAsync(Guid userId, bool includeExpired, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<PurchasedTicket>> GetByUserIdAsync(Guid userId, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public Task<PurchasedTicket?> GetForUserByIdAsync(Guid userId, Guid ticketId, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public Task<PurchasedTicket> PurchaseAsync(Guid userId, int ticketTypeId, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RevokeAsync(Guid ticketId, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public Task<(bool isValid, PurchasedTicket? ticket, string? reason)> ValidateAsync(string validationCode, CancellationToken ct)
        {
            throw new NotImplementedException();
        }
    }
}
