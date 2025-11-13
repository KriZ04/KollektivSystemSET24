using KollektivSystem.ApiService.Models.Transport;
using KollektivSystem.ApiService.Repositories;
using KollektivSystem.ApiService.Services.Interfaces;
using KollektivSystem.ApiService.Infrastructure.Logging;
using Microsoft.Extensions.Logging;

namespace KollektivSystem.ApiService.Services.Implementations
{
    public class StopService : IStopService
    {
        private readonly IStopRepository _repo;
        private readonly ILogger<StopService> _logger;

        public StopService(IStopRepository repo, ILogger<StopService> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        public async Task<Stop> CreateAsync(Stop stop)
        {
            try
            {
                await _repo.AddAsync(stop);
                await _repo.SaveChangesAsync();

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
            return await _repo.GetAllAsync();
        }

        public async Task<Stop?> GetByIdAsync(int id)
        {
            var stop = await _repo.FindAsync(id);
            if (stop == null)
                _logger.LogStopNotFound(id);

            return stop;
        }

        public async Task<bool> UpdateAsync(int id, Stop updated)
        {
            var existing = await _repo.FindAsync(id);
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
                _repo.Update(existing);
                await _repo.SaveChangesAsync();

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
            var existing = await _repo.FindAsync(id);
            if (existing == null)
            {
                _logger.LogStopNotFound(id);
                return false;
            }

            _repo.Remove(existing);
            await _repo.SaveChangesAsync();

            _logger.LogStopDeleted(id);
            return true;
        }
    }
}
