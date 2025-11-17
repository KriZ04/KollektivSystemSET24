using KollektivSystem.ApiService.Infrastructure;
using KollektivSystem.ApiService.Models;
using KollektivSystem.ApiService.Repositories.Uow;
using KollektivSystem.ApiService.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace KollektivSystem.ApiService.Services
{
    public class PurchasedTicketService : IPurchasedTicketService
    {
        private readonly IUnitOfWork _uow;
        private readonly ITicketTypeService _ticketTypeService;
        private readonly ILogger<PurchasedTicketService> _logger;

        public PurchasedTicketService(IUnitOfWork uow, ITicketTypeService ticketTypeService, ILogger<PurchasedTicketService> logger)
        {
            _uow = uow;
            _ticketTypeService = ticketTypeService;
            _logger = logger;
        }

        public async Task<PurchasedTicket?> GetByIdAsync(Guid ticketId, CancellationToken ct)
        {
            var ticket = await _uow.PurchasedTickets.FindAsync(ticketId, ct);
            return ticket;
        }

        public async Task<IReadOnlyList<PurchasedTicket>> GetByUserAsync(Guid userId, bool includeInvalid, CancellationToken ct)
        {
            var user = await _uow.Users.FindAsync(userId, ct);
            if (user == null)
                return [];
            var tickets = user.PurchasedTickets;
            if (!includeInvalid)
            {
                return tickets.Where(t => t.IsValid).ToList();
            }

            return tickets.ToList();

        }

        public async Task<PurchasedTicket?> GetForUserByIdAsync(Guid userId, Guid ticketId, CancellationToken ct)
        {
            var tickets = await GetByUserAsync(userId, includeInvalid: true, ct);
            var ticket = tickets.FirstOrDefault(t => t.Id == ticketId);
            return ticket;

        }

        public async Task<PurchasedTicket?> PurchaseAsync(Guid userId, int ticketTypeId, CancellationToken ct)
        {
            var ticketType = await _ticketTypeService.GetByIdAsync(ticketTypeId, ct);
            if (ticketType == null) return null;
            var code = await GenerateUniqueCodeAsync(ct);

            PurchasedTicket ticket = new PurchasedTicket
            {
                Id = Guid.NewGuid(),
                ExpireAt = DateTimeOffset.UtcNow.AddMinutes(ticketType.AliveTime.TotalMinutes),
                TicketTypeId = ticketTypeId,
                UserId = userId,
                ValidationCode = code
            };
            await _uow.PurchasedTickets.AddAsync(ticket, ct);
            await _uow.SaveChangesAsync(ct);

            return ticket;

        }

        public async Task<bool> RevokeAsync(Guid ticketId, CancellationToken ct)
        {
            var ticket = await GetByIdAsync(ticketId, ct);
            if (ticket == null) return false;
            ticket.Revoked = true;
            _uow.PurchasedTickets.Update(ticket);
            await _uow.SaveChangesAsync(ct);
            return true;
        }

        public async Task<(bool isValid, PurchasedTicket? ticket, string? reason)> ValidateAsync(string validationCode, CancellationToken ct)
        {
            var ticket = await _uow.PurchasedTickets.GetByValidationCodeAsync(validationCode, ct);

            if (ticket is null)
                return (false, null, "Ticket not found");

            if (ticket.Revoked)
                return (false, ticket, "Ticket has been revoked");

            if (ticket.IsExpired)
                return (false, ticket, "Ticket has expired");

            return (true, ticket, null);
        }

        private async Task<string> GenerateUniqueCodeAsync(CancellationToken ct)
        {
            const int MaxAttempts = 5;

            for (int i = 0; i < MaxAttempts; i++)
            {
                var code = TicketValidationCodeGenerator.Generate();

                // check for collision
                var exists = await _uow.PurchasedTickets.ValidationCodeExistsAsync(code, ct);

                if (!exists)
                    return code;
            }

            throw new InvalidOperationException("Failed to generate unique validation code after several attempts.");
        }
    }
}
