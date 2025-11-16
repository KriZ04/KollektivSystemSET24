using KollektivSystem.ApiService.Repositories;
using KollektivSystem.ApiService.Services.Interfaces;
using KollektivSystem.ApiService.Infrastructure.Logging;
using Microsoft.Extensions.Logging;
using KollektivSystem.ApiService.Repositories.Uow;
using KollektivSystem.ApiService.Models;

namespace KollektivSystem.ApiService.Services
{
    public class StopService : IStopService
    {
        private readonly IUnitOfWork _uow;
        private readonly ILogger<StopService> _logger;

        public StopService(IUnitOfWork uow, ILogger<StopService> logger)
        {
            _uow = uow;
            _logger = logger;
        }

        public async Task<Stop> CreateAsync(Stop stop)
        {
            try
            {
                await _uow.Stops.AddAsync(stop);
                await _uow.SaveChangesAsync();

                _logger.LogStopCreated(stop.Id, stop.Name);
                return stop;
            }
            catch (Exception ex)
            {
                _logger.LogStopCreationFailed(ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<Stop>> GetAllAsync()
        {
            return await _uow.Stops.GetAllAsync();
        }

        public async Task<Stop?> GetByIdAsync(int id)
        {
            var stop = await _uow.Stops.FindAsync(id);
            if (stop == null)
                _logger.LogStopNotFound(id);

            return stop;
        }

        public async Task<bool> UpdateAsync(int id, Stop updated)
        {
            var existing = await _uow.Stops.FindAsync(id);
            if (existing == null)
            {
                _logger.LogStopNotFound(id);
                return false;
            }

            existing.Name = updated.Name;
            existing.Order = updated.Order;
            existing.RouteId = updated.RouteId;

            try
            {
                _uow.Stops.Update(existing);
                await _uow.SaveChangesAsync();

                _logger.LogStopUpdated(id, existing.Name);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogStopUpdateFailed(id, ex.Message);
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _uow.Stops.FindAsync(id);
            if (existing == null)
            {
                _logger.LogStopNotFound(id);
                return false;
            }

            _uow.Stops.Remove(existing);
            await _uow.SaveChangesAsync();

            _logger.LogStopDeleted(id);
            return true;
        }
    }
}
