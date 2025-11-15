using KollektivSystem.ApiService.Models.Transport;
using KollektivSystem.ApiService.Repositories;
using KollektivSystem.ApiService.Services.Interfaces;
using KollektivSystem.ApiService.Infrastructure.Logging;
using Microsoft.Extensions.Logging;

namespace KollektivSystem.ApiService.Services.Implementations
{
    public class TransitLineService : ITransitLineService
    {
        private readonly ITransitLineRepository _repo;
        private readonly ILogger<TransitLineService> _logger;

        public TransitLineService(ITransitLineRepository repo, ILogger<TransitLineService> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        public async Task<TransitLine> CreateAsync(TransitLine line)
        {
            _logger.LogInformation("Creating transit line {LineName}", line.Name);

            try
            {
                await _repo.AddAsync(line);
                await _repo.SaveChanges();

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
                var lines = await _repo.GetAllAsync();
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
                var line = await _repo.FindAsync(id);
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
                var existing = await _repo.FindAsync(id);
                if (existing == null)
                {
                    _logger.LogWarning("Transit line with ID {LineId} not found for update", id);
                    return false;
                }

                existing.Name = updated.Name;
                existing.Stops = updated.Stops;

                _repo.Update(existing);
                await _repo.SaveChanges();

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
                var existing = await _repo.FindAsync(id);
                if (existing == null)
                {
                    _logger.LogWarning("Transit line with ID {LineId} not found for deletion", id);
                    return false;
                }

                _repo.Remove(existing);
                await _repo.SaveChanges();

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