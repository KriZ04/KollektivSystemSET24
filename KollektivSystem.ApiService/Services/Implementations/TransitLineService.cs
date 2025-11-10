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
            await _repo.AddAsync(line);
            await _repo.SaveChanges();

            _logger.LogTransitLineCreated(line.Id, line.Name);

            return line;
        }

        public async Task<IEnumerable<TransitLine>> GetAllAsync()
        {
            return await _repo.GetAllAsync();
        }

        public async Task<TransitLine?> GetByIdAsync(int id)
        {
            return await _repo.FindAsync(id);
        }

        public async Task<bool> UpdateAsync(int id, TransitLine updated)
        {
            var existing = await _repo.FindAsync(id);
            if (existing == null)
                return false;

            existing.Name = updated.Name;
            existing.Stops = updated.Stops;

            _repo.Update(existing);
            await _repo.SaveChanges();

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _repo.FindAsync(id);
            if (existing == null)
                return false;

            _repo.Remove(existing);
            await _repo.SaveChanges();
            return true;
        }
    }
}
