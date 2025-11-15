using KollektivSystem.ApiService.Models.Transport;
using KollektivSystem.ApiService.Repositories;
using KollektivSystem.ApiService.Services.Interfaces;
using KollektivSystem.ApiService.Infrastructure.Logging;
using Microsoft.Extensions.Logging;

namespace KollektivSystem.ApiService.Services.Implementations
{
    public class TicketService : ITicketService
    {
        private readonly ITicketsRepository _repo;
        private readonly ILogger<TicketService> _logger;

        public TicketService(ITicketsRepository repo, ILogger<TicketService> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        public async Task<Tickets> CreateAsync(Tickets ticket)
        {
            try
            {
                await _repo.AddAsync(ticket);
                await _repo.SaveChanges();

                _logger.LogTicketCreated(ticket.Id);
                return ticket;
            }
            catch (Exception ex)
            {
                _logger.LogTicketCreationFailed(ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<Tickets>> GetAllAsync()
        {
            return await _repo.GetAllAsync();
        }

        public async Task<Tickets?> GetByIdAsync(int id)
        {
            var ticket = await _repo.FindAsync(id);

            if (ticket == null)
                _logger.LogTicketNotFound(id);

            return ticket;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _repo.FindAsync(id);
            if (existing == null)
            {
                _logger.LogTicketNotFound(id);
                return false;
            }

            _repo.Remove(existing);
            await _repo.SaveChanges();

            _logger.LogTicketDeleted(id);
            return true;

        }
    }
}
