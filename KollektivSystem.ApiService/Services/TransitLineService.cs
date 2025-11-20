using KollektivSystem.ApiService.Repositories.Uow;
using KollektivSystem.ApiService.Services.Interfaces;
using KollektivSystem.ApiService.Infrastructure.Logging;
using Microsoft.Extensions.Logging;
using KollektivSystem.ApiService.Models;
using KollektivSystem.ApiService.Models.Dtos.TransitLines;

namespace KollektivSystem.ApiService.Services
{
    public class TransitLineService : ITransitLineService
    {
        private readonly IUnitOfWork _uow;
        private readonly ILogger<TransitLineService> _logger;

        public TransitLineService(IUnitOfWork uow, ILogger<TransitLineService> logger)
        {
            _uow = uow;
            _logger = logger;
        }

        public async Task<TransitLine> CreateAsync(CreateTransitLineRequest request, CancellationToken ct = default)
        {
            var line = new TransitLine
            {
                Name = request.Name
            };

            try
            {
                await _uow.TransitLines.AddAsync(line, ct);
                await _uow.SaveChangesAsync(ct);

                _logger.LogTransitLineCreated(line.Id, line.Name);
                return line;
            }
            catch (Exception ex)
            {
                _logger.LogTransitLineCreationFailed(line.Id, line.Name, ex.Message);
                throw;
            }
        }

        public async Task<IReadOnlyList<TransitLine>> GetAllAsync(CancellationToken ct = default)
        {
            return await _uow.TransitLines.GetAllAsync(ct);
        }

        public async Task<TransitLine?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            var line = await _uow.TransitLines.FindAsync(id, ct);

            if (line == null)
                _logger.LogTransitLineNotFound(id);

            return line;
        }

        public async Task<bool> UpdateAsync(int id, TransitLine updated, CancellationToken ct = default)
        {
            var existing = await _uow.TransitLines.FindAsync(id, ct);
            if (existing == null)
            {
                _logger.LogTransitLineNotFound(id);
                return false;
            }

            existing.Name = updated.Name;
            existing.Stops = updated.Stops;

            try
            {
                _uow.TransitLines.Update(existing);
                await _uow.SaveChangesAsync(ct);

                _logger.LogTransitLineUpdated(id, existing.Name);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogTransitLineUpdateFailed(id, ex.Message);
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken ct = default)
        {
            var existing = await _uow.TransitLines.FindAsync(id, ct);
            if (existing == null)
            {
                _logger.LogTransitLineNotFound(id);
                return false;
            }

            try
            {
                _uow.TransitLines.Remove(existing);
                await _uow.SaveChangesAsync(ct);

                _logger.LogTransitLineDeleted(id, existing.Name);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogTransitLineDeleteFailed(id, ex.Message);
                throw;
            }
        }
    }
}
