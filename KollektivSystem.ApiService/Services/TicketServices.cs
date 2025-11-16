using KollektivSystem.ApiService.Repositories;
using KollektivSystem.ApiService.Services.Interfaces;
using KollektivSystem.ApiService.Infrastructure.Logging;
using Microsoft.Extensions.Logging;
using KollektivSystem.ApiService.Repositories.Uow;
using KollektivSystem.ApiService.Models;

namespace KollektivSystem.ApiService.Services
{
    public class TicketService : ITicketService
    {
        private readonly IUnitOfWork _uow;
        private readonly ILogger<TicketService> _logger;

        public TicketService(IUnitOfWork uow, ILogger<TicketService> logger)
        {
            _uow = uow;
            _logger = logger;
        }

        public async Task<Tickets> CreateAsync(Tickets ticket)
        {
            try
            {
                await _uow.Tickets.AddAsync(ticket);
                await _uow.SaveChangesAsync();

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
            return await _uow.Tickets.GetAllAsync();
        }

        public async Task<Tickets?> GetByIdAsync(int id)
        {
            var ticket = await _uow.Tickets.FindAsync(id);

            if (ticket == null)
                _logger.LogTicketNotFound(id);

            return ticket;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _uow.Tickets.FindAsync(id);
            if (existing == null)
            {
                _logger.LogTicketNotFound(id);
                return false;
            }

            _uow.Tickets.Remove(existing);
            await _uow.SaveChangesAsync();

            _logger.LogTicketDeleted(id);
            return true;

        }

        public async Task<bool> UpdateAsync(int id, Tickets updated)
        {
            try
            {
                
                var existing = await _uow.Tickets.FindAsync(id);
                if (existing == null)
                {
                    _logger.LogTicketNotFound(id);
                    return false;
                }

                
                existing.Type = updated.Type;
                existing.Price = updated.Price;

                _uow.Tickets.Update(existing);
                await _uow.SaveChangesAsync();

                _logger.LogTicketUpdated(id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogTicketUpdateFailed(ex.Message);
                throw;
            }
        }

    }
}
