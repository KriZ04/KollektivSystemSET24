using KollektivSystem.ApiService.Repositories;
using KollektivSystem.ApiService.Services.Interfaces;
using KollektivSystem.ApiService.Infrastructure.Logging;
using Microsoft.Extensions.Logging;
using KollektivSystem.ApiService.Repositories.Uow;
using KollektivSystem.ApiService.Models;
using KollektivSystem.ApiService.Models.Dtos.Stops;

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

        public async Task<Stop> CreateAsync(CreateStopRequest request, CancellationToken ct = default)
        {
            var stop = new Stop
            {
                Name = request.Name,
                Latitude = request.Latitude,
                Longitude = request.Longitude
            };

            try
            {
                await _uow.Stops.AddAsync(stop, ct);
                await _uow.SaveChangesAsync(ct);

                _logger.LogStopCreated(stop.Id, stop.Name);
                return stop;
            }
            catch (Exception ex)
            {
                _logger.LogStopCreationFailed(ex.Message);
                throw;
            }
        }

        public async Task<IReadOnlyList<Stop>> GetAllAsync(CancellationToken ct = default)
        {
            var stops = await _uow.Stops.GetAllAsync(ct); 
            return stops;
        }

        public async Task<Stop?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            var stop = await _uow.Stops.FindAsync(id, ct);
            if (stop == null)
                _logger.LogStopNotFound(id);

            return stop;
        }

        public async Task<bool> UpdateAsync(int id, Stop updated, CancellationToken ct = default)
        {
            var existing = await _uow.Stops.FindAsync(id, ct);
            if (existing == null)
            {
                _logger.LogStopNotFound(id);
                return false;
            }

            existing.Name = updated.Name;

            try
            {
                _uow.Stops.Update(existing);
                await _uow.SaveChangesAsync(ct);

                _logger.LogStopUpdated(id, existing.Name);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogStopUpdateFailed(id, ex.Message);
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken ct = default)
        {
            var existing = await _uow.Stops.FindAsync(id, ct);
            if (existing == null)
            {
                _logger.LogStopNotFound(id);
                return false;
            }

            _uow.Stops.Remove(existing);
            await _uow.SaveChangesAsync(ct);

            _logger.LogStopDeleted(id);
            return true;
        }

    }
}