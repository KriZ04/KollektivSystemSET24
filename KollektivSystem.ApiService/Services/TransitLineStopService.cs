using KollektivSystem.ApiService.Models;
using KollektivSystem.ApiService.Models.Dtos.TransitLineStops;
using KollektivSystem.ApiService.Repositories.Uow;
using KollektivSystem.ApiService.Services.Interfaces;
using KollektivSystem.ApiService.Infrastructure.Logging;
using Microsoft.Extensions.Logging;

namespace KollektivSystem.ApiService.Services
{
    public sealed class TransitLineStopService : ITransitLineStopService
    {
        private readonly IUnitOfWork _uow;
        private readonly ILogger<TransitLineStopService> _logger;

        public TransitLineStopService(IUnitOfWork uow, ILogger<TransitLineStopService> logger)
        {
            _uow = uow;
            _logger = logger;
        }

        public async Task<TransitLineStop?> CreateAsync(CreateTransitLineStopRequest request, CancellationToken ct)
        {
            // Validate unique Order per TransitLine
            var orderExists = await _uow.TransitLineStops.AnyAsync(
                tls => tls.TransitLineId == request.TransitLineId && tls.Order == request.Order,
                ct);

            if (orderExists)
            {
                _logger.LogOrderAlreadyExists(request.TransitLineId, request.Order);
                return null;
            }

            // Validate unique StopId per TransitLine
            var stopExists = await _uow.TransitLineStops.AnyAsync(
                tls => tls.TransitLineId == request.TransitLineId && tls.StopId == request.StopId,
                ct);

            if (stopExists)
            {
                _logger.LogStopAlreadyExists(request.TransitLineId, request.StopId);
                return null;
            }

            var entity = new TransitLineStop
            {
                TransitLineId = request.TransitLineId,
                StopId = request.StopId,
                Order = request.Order
            };

            try
            {
                await _uow.TransitLineStops.AddAsync(entity, ct);
                await _uow.SaveChangesAsync(ct);

                _logger.LogTransitLineStopCreated(entity.Id, entity.TransitLineId, entity.StopId, entity.Order);
                return entity;
            }
            catch (Exception ex)
            {
                _logger.LogTransitLineStopCreationFailed(request.TransitLineId, request.StopId, request.Order, ex.Message);
                throw;
            }
        }

        public async Task<IReadOnlyList<TransitLineStop>> GetAllAsync(CancellationToken ct)
        {
            return await _uow.TransitLineStops.GetAllAsync(ct);
        }

        public async Task<TransitLineStop?> GetByIdAsync(int id, CancellationToken ct)
        {
            var entity = await _uow.TransitLineStops.FindAsync(id, ct);

            if (entity is null)
                _logger.LogTransitLineStopNotFound(id);

            return entity;
        }

        public async Task<bool> UpdateAsync(int id, TransitLineStop updated, CancellationToken ct)
        {
            var existing = await _uow.TransitLineStops.FindAsync(id, ct);
            if (existing is null)
            {
                _logger.LogTransitLineStopNotFoundForUpdate(id);
                return false;
            }

            existing.Order = updated.Order;
            existing.StopId = updated.StopId;
            existing.TransitLineId = updated.TransitLineId;

            try
            {
                await _uow.SaveChangesAsync(ct);
                _logger.LogTransitLineStopUpdated(id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogTransitLineStopUpdateFailed(id, ex.Message);
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken ct)
        {
            var existing = await _uow.TransitLineStops.FindAsync(id, ct);
            if (existing is null)
            {
                _logger.LogTransitLineStopNotFoundForDeletion(id);
                return false;
            }

            try
            {
                _uow.TransitLineStops.Remove(existing);
                await _uow.SaveChangesAsync(ct);

                _logger.LogTransitLineStopDeleted(id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogTransitLineStopDeleteFailed(id, ex.Message);
                throw;
            }
        }
    }
}
