using KollektivSystem.ApiService.Models.Transport;
using KollektivSystem.ApiService.Repositories;
using KollektivSystem.ApiService.Services.Interfaces;
using KollektivSystem.ApiService.Infrastructure.Logging;
using Microsoft.Extensions.Logging;
using KollektivSystem.ApiService.Repositories.Uow;

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

        public async Task<TransitLine> CreateAsync(TransitLine line)
        {
            _logger.LogInformation("Creating transit line {LineName}", line.Name);

            try
            {
                await _uow.TransitLines.AddAsync(line);
                await _uow.SaveChangesAsync();

                _logger.LogTransitLineCreated(line.Id, line.Name);
                return line;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating transit line {LineName}: {Message}", line.Name, ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<TransitLine>> GetAllAsync()
        {
            _logger.LogDebug("Retrieving all transit lines");

            try
            {
                var lines = await _uow.TransitLines.GetAllAsync();
                _logger.LogInformation("Retrieved {Count} transit lines", lines.Count());
                return lines;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all transit lines");
                throw;
            }
        }

        public async Task<TransitLine?> GetByIdAsync(int id)
        {
            _logger.LogDebug("Fetching transit line with ID {LineId}", id);

            try
            {
                var line = await _uow.TransitLines.FindAsync(id);
                if (line == null)
                {
                    _logger.LogWarning("Transit line with ID {LineId} not found", id);
                }
                else
                {
                    _logger.LogInformation("Retrieved transit line {LineName} (ID: {LineId})", line.Name, id);
                }

                return line;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving transit line with ID {LineId}", id);
                throw;
            }
        }

        public async Task<bool> UpdateAsync(int id, TransitLine updated)
        {
            _logger.LogInformation("Updating transit line with ID {LineId}", id);

            try
            {
                var existing = await _uow.TransitLines.FindAsync(id);
                if (existing == null)
                {
                    _logger.LogWarning("Transit line with ID {LineId} not found for update", id);
                    return false;
                }

                existing.Name = updated.Name;
                existing.Stops = updated.Stops;

                _uow.TransitLines.Update(existing);
                await _uow.SaveChangesAsync();

                _logger.LogInformation("Updated transit line {LineName} (ID: {LineId})", updated.Name, id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating transit line with ID {LineId}", id);
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            _logger.LogInformation("Attempting to delete transit line with ID {LineId}", id);

            try
            {
                var existing = await _uow.TransitLines.FindAsync(id);
                if (existing == null)
                {
                    _logger.LogWarning("Transit line with ID {LineId} not found for deletion", id);
                    return false;
                }

                _uow.TransitLines.Remove(existing);
                await _uow.SaveChangesAsync();

                _logger.LogInformation("Deleted transit line {LineName} (ID: {LineId})", existing.Name, id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting transit line with ID {LineId}", id);
                throw;
            }
        }
    }
}