using KollektivSystem.ApiService.Models;
using KollektivSystem.ApiService.Models.Dtos.TransitLineStops;
using KollektivSystem.ApiService.Repositories.Uow;
using KollektivSystem.ApiService.Services.Interfaces;
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

        public async Task<TransitLineStop> CreateAsync(CreateTransitLineStopRequest request, CancellationToken ct)
        {

            // Validate unique Order per TransitLine
            var orderExists = await _uow.TransitLineStops
                .AnyAsync(
                    tls => tls.TransitLineId == request.TransitLineId &&
                           tls.Order == request.Order,
                    ct);

            if (orderExists)
            {
                _logger.LogWarning(
                    "Cannot create TransitLineStop: Line {LineId} already has a stop at order {Order}",
                    request.TransitLineId,
                    request.Order);

                throw new BadHttpRequestException(
                    $"Transit line {request.TransitLineId} already contains a stop at order {request.Order}.");
            }

            // Validate unique StopId per TransitLine (you already enforce this in DB)
            var stopExists = await _uow.TransitLineStops
                .AnyAsync(
                    tls => tls.TransitLineId == request.TransitLineId &&
                           tls.StopId == request.StopId,
                    ct);

            if (stopExists)
            {
                _logger.LogWarning(
                    "Cannot create TransitLineStop: Line {LineId} already contains Stop {StopId}",
                    request.TransitLineId,
                    request.StopId);

                throw new BadHttpRequestException(
                    $"Transit line {request.TransitLineId} already contains stop {request.StopId}.");
            }

            var entity = new TransitLineStop
            {
                TransitLineId = request.TransitLineId,
                StopId = request.StopId,
                Order = request.Order
            };

            await _uow.TransitLineStops.AddAsync(entity, ct);
            await _uow.SaveChangesAsync(ct);

            _logger.LogInformation(
                "Created TransitLineStop: Id {Id}, TransitLineId {LineId}, StopId {StopId}, Order {Order}",
                entity.Id, entity.TransitLineId, entity.StopId, entity.Order);

            return entity;
        }

        public async Task<IReadOnlyList<TransitLineStop>> GetAllAsync(CancellationToken ct)
        {
            var list = await _uow.TransitLineStops.GetAllAsync(ct);
            return list;
        }

        public async Task<TransitLineStop?> GetByIdAsync(int id, CancellationToken ct)
        {
            var entity = await _uow.TransitLineStops.FindAsync(id, ct);

            if (entity is null)
            {
                _logger.LogWarning("TransitLineStop with ID {Id} not found.", id);
            }

            return entity;
        }

        public async Task<bool> UpdateAsync(int id, TransitLineStop updated, CancellationToken ct)
        {
            var existing = await _uow.TransitLineStops.FindAsync(id, ct);
            if (existing is null)
            {
                _logger.LogWarning("TransitLineStop with ID {Id} not found for update.", id);
                return false;
            }

            existing.Order = updated.Order;
            existing.StopId = updated.StopId;
            existing.TransitLineId = updated.TransitLineId;

            await _uow.SaveChangesAsync(ct);

            _logger.LogInformation("Updated TransitLineStop with ID {Id}.", id);
            return true;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken ct)
        {
            var existing = await _uow.TransitLineStops.FindAsync(id, ct);
            if (existing is null)
            {
                _logger.LogWarning("TransitLineStop with ID {Id} not found for deletion.", id);
                return false;
            }

            _uow.TransitLineStops.Remove(existing);
            await _uow.SaveChangesAsync(ct);

            _logger.LogInformation("Deleted TransitLineStop with ID {Id}.", id);
            return true;
        }
    }
}
